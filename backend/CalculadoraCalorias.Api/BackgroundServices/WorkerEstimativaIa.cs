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
            await foreach (var request in _fila.LerFilaAsync(stoppingToken))
            {
                try
                {
                    Console.WriteLine($"Iniciando processamento da Refeição ID: {request.RefeicaoId}");
                    using var scope = _scopeFactory.CreateScope();

                    var dbContext = scope.ServiceProvider.GetRequiredService<IRefeicaoRepository>();

                    var llmService = scope.ServiceProvider.GetRequiredService<ILlmService>();

                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    var refeicao = await dbContext.ObterPorId(request.RefeicaoId);

                    if (refeicao == null)
                    {
                        Console.WriteLine($"Refeição {request.RefeicaoId} não encontrada no banco.");
                        continue;
                    }

                    var caminhoBase = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "refeicoes");
                    var padraoDeBusca = $"{request.GuidArquivo}.*";
                    var arquivosEncontrados = Directory.GetFiles(caminhoBase, padraoDeBusca);

                    if (arquivosEncontrados.Length == 0)
                    {
                        Console.WriteLine($"Imagem não encontrada no disco para a Refeição ID: {request.RefeicaoId}");
                        continue;
                    }

                    var caminhoFisicoCompleto = arquivosEncontrados[0];
                    byte[] imagemBytes = await File.ReadAllBytesAsync(caminhoFisicoCompleto, stoppingToken);

                    var estimativaCalorica = await llmService.SimularCaloriasRefeicao(imagemBytes, refeicao.Peso);

                    refeicao.AtualizarEstimativa(
                        estimativaCalorica.Alimento ?? "alimento não identificado", 
                        estimativaCalorica.Calorias, 
                        estimativaCalorica.Proteinas,
                        estimativaCalorica.Carboidratos, 
                        estimativaCalorica.Gorduras, 
                        estimativaCalorica.Acucares, 
                        estimativaCalorica.Fibras);


                    var json = JsonSerializer.Serialize(estimativaCalorica, new JsonSerializerOptions { WriteIndented = true });
                    Console.WriteLine(json);

                    await unitOfWork.CommitAsync();

                    // Notificar o Frontend em Tempo Real
                    await _hubContext.Clients.Group(refeicao.UsuarioId.ToString())
                        .SendAsync("RefeicaoProcessada", refeicao.Id, stoppingToken);

                    try
                    {
                        if (File.Exists(caminhoFisicoCompleto))
                        {
                            File.Delete(caminhoFisicoCompleto);
                            _logger.LogInformation($"Imagem deletada com sucesso: {caminhoFisicoCompleto}");
                        }
                    }
                    catch (Exception exFile)
                    {
                        _logger.LogWarning(exFile, $"Falha ao deletar a imagem no disco (ela ficará órfã): {caminhoFisicoCompleto}");
                    }


                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Erro ao processar estimativa da IA para RefeicaoId: {request.RefeicaoId}");
                    await Task.Delay(TimeSpan.FromMinutes(2));
                    await _fila.EnviarParaFilaAsync(request);
                }
            }
        }
    }
}