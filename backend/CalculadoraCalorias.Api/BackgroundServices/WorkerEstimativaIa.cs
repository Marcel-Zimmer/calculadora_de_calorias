using CalculadoraCalorias.Application.Filas;
using CalculadoraCalorias.Core.Domain.Interfaces;
using CalculadoraCalorias.Infrastructure.Repository;
using Microsoft.AspNetCore.SignalR;
using CalculadoraCalorias.Api.Hubs;
using System.Text.Json;

namespace CalculadoraCalorias.Api.BackgroundServices
{
    public class WorkerEstimativaIa : BackgroundService
    {
        private readonly FilaEstimativaIa _fila;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<WorkerEstimativaIa> _logger;
        private readonly IHubContext<NotificacaoHub> _hubContext;

        public WorkerEstimativaIa(
            FilaEstimativaIa fila,
            IServiceScopeFactory scopeFactory,
            ILogger<WorkerEstimativaIa> logger,
            IHubContext<NotificacaoHub> hubContext)
        {
            _fila = fila;
            _scopeFactory = scopeFactory;
            _logger = logger;
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker de Estimativa IA iniciado.");

            await foreach (var request in _fila.LerFilaAsync(stoppingToken))
            {
                int tentativas = 0;
                const int maxTentativas = 3;
                bool processadoComSucesso = false;

                while (tentativas < maxTentativas && !processadoComSucesso)
                {
                    try
                    {
                        tentativas++;
                        _logger.LogInformation($"Processando Refeição ID: {request.RefeicaoId} (Tentativa {tentativas}/{maxTentativas})");

                        using var scope = _scopeFactory.CreateScope();
                        var dbContext = scope.ServiceProvider.GetRequiredService<IRefeicaoRepository>();
                        var llmService = scope.ServiceProvider.GetRequiredService<ILlmService>();
                        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                        var refeicao = await dbContext.ObterPorId(request.RefeicaoId);
                        if (refeicao == null)
                        {
                            _logger.LogWarning($"Refeição {request.RefeicaoId} não encontrada no banco. Abortando item.");
                            processadoComSucesso = true; // Marca como "sucesso" para tirar da fila já que não existe
                            continue;
                        }

                        var caminhoBase = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "refeicoes");
                        var padraoDeBusca = $"{request.GuidArquivo}.*";
                        var arquivosEncontrados = Directory.GetFiles(caminhoBase, padraoDeBusca);

                        if (arquivosEncontrados.Length == 0)
                        {
                            _logger.LogWarning($"Imagem não encontrada no disco para a Refeição ID: {request.RefeicaoId}");
                            processadoComSucesso = true;
                            continue;
                        }

                        var caminhoFisicoCompleto = arquivosEncontrados[0];
                        byte[] imagemBytes = await File.ReadAllBytesAsync(caminhoFisicoCompleto, stoppingToken);

                        // Chamada à LLM
                        var estimativaCalorica = await llmService.SimularCaloriasRefeicao(imagemBytes, refeicao.Peso);

                        refeicao.AtualizarEstimativa(
                            estimativaCalorica.Alimento ?? "alimento não identificado", 
                            estimativaCalorica.Calorias, 
                            estimativaCalorica.Proteinas,
                            estimativaCalorica.Carboidratos, 
                            estimativaCalorica.Gorduras, 
                            estimativaCalorica.Acucares, 
                            estimativaCalorica.Fibras);

                        await unitOfWork.CommitAsync();

                        // Notificar o Frontend
                        await _hubContext.Clients.Group(refeicao.UsuarioId.ToString())
                            .SendAsync("RefeicaoProcessada", refeicao.Id, stoppingToken);

                        // Limpeza
                        try { if (File.Exists(caminhoFisicoCompleto)) File.Delete(caminhoFisicoCompleto); }
                        catch (Exception exFile) { _logger.LogWarning(exFile, "Falha ao deletar imagem órfã."); }

                        processadoComSucesso = true;
                        _logger.LogInformation($"Refeição {request.RefeicaoId} processada com sucesso.");
                        
                        // Pequeno fôlego para a API após sucesso (opcional, mas ajuda na estabilidade)
                        await Task.Delay(2000, stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Erro na tentativa {tentativas} para RefeicaoId: {request.RefeicaoId}");
                        
                        if (tentativas < maxTentativas)
                        {
                            _logger.LogInformation("Aguardando 40 segundos para nova tentativa devido a possível Rate Limit...");
                            await Task.Delay(TimeSpan.FromSeconds(40), stoppingToken);
                        }
                        else
                        {
                            _logger.LogCritical($"Máximo de tentativas atingido para RefeicaoId: {request.RefeicaoId}. O item será descartado para não travar a fila.");
                        }
                    }
                }
            }
        }
    }
}