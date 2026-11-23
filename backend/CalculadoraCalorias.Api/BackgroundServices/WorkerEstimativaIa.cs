using CalculadoraCalorias.Api.Hubs;
using CalculadoraCalorias.Application.DTOs.Records;
using CalculadoraCalorias.Application.Filas;
using CalculadoraCalorias.Core.Domain.Enums;
using CalculadoraCalorias.Core.Domain.ExcecoesPersonalizadas;
using CalculadoraCalorias.Core.Domain.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace CalculadoraCalorias.Api.BackgroundServices
{
    public class WorkerEstimativaIa : BackgroundService
    {
        private const int MaxTentativas = 3;
        private static readonly TimeSpan[] AtrasosBackoff =
        [
            TimeSpan.FromSeconds(5),
            TimeSpan.FromSeconds(15),
            TimeSpan.FromSeconds(30)
        ];

        private readonly FilaEstimativaIa _fila;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<WorkerEstimativaIa> _logger;
        private readonly IHubContext<NotificacaoHub> _hubContext;
        private readonly IConfiguration _configuration;

        public WorkerEstimativaIa(
            FilaEstimativaIa fila,
            IServiceScopeFactory scopeFactory,
            ILogger<WorkerEstimativaIa> logger,
            IHubContext<NotificacaoHub> hubContext,
            IConfiguration configuration)
        {
            _fila = fila;
            _scopeFactory = scopeFactory;
            _logger = logger;
            _hubContext = hubContext;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker de Estimativa IA iniciado.");

            await RecuperarPendentesNaInicializacaoAsync(stoppingToken);

            await foreach (var request in _fila.LerFilaAsync(stoppingToken))
            {
                await ProcessarItemAsync(request, stoppingToken);
            }
        }

        private async Task RecuperarPendentesNaInicializacaoAsync(CancellationToken stoppingToken)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var refeicaoRepo = scope.ServiceProvider.GetRequiredService<IRefeicaoRepository>();
                var pendentes = await refeicaoRepo.ObterPendentesProcessamentoAsync();

                if (pendentes.Count == 0) return;

                _logger.LogInformation("Recuperando {Quantidade} refeição(ões) pendente(s) na fila após reinicialização.", pendentes.Count);

                foreach (var refeicao in pendentes)
                {
                    if (stoppingToken.IsCancellationRequested) break;
                    await _fila.EnviarParaFilaAsync(new EstimativaIaRequest(refeicao.Id, refeicao.GuidArquivo), stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao recuperar refeições pendentes na inicialização do WorkerEstimativaIa.");
            }
        }

        private async Task ProcessarItemAsync(EstimativaIaRequest request, CancellationToken stoppingToken)
        {
            var timeoutSegundos = _configuration.GetValue<int?>("OpenAI:TimeoutSeconds") ?? 120;
            if (timeoutSegundos < 15) timeoutSegundos = 120;

            string? caminhoFisicoCompleto = null;

            for (int tentativa = 1; tentativa <= MaxTentativas; tentativa++)
            {
                if (stoppingToken.IsCancellationRequested) return;

                try
                {
                    _logger.LogInformation(
                        "Processando Refeição ID: {RefeicaoId} (Tentativa {Tentativa}/{MaxTentativas})",
                        request.RefeicaoId, tentativa, MaxTentativas);

                    using var scope = _scopeFactory.CreateScope();
                    var refeicaoRepo = scope.ServiceProvider.GetRequiredService<IRefeicaoRepository>();
                    var llmService = scope.ServiceProvider.GetRequiredService<ILlmService>();
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    var refeicao = await refeicaoRepo.ObterPorId(request.RefeicaoId);
                    if (refeicao == null)
                    {
                        _logger.LogWarning("Refeição {RefeicaoId} não encontrada no banco. Abortando item.", request.RefeicaoId);
                        return;
                    }

                    if (refeicao.StatusProcessamento != StatusProcessamentoEnum.EnviadoParaFila)
                    {
                        _logger.LogInformation(
                            "Refeição {RefeicaoId} já possui status {Status}. Ignorando item duplicado.",
                            request.RefeicaoId, refeicao.StatusProcessamento);
                        return;
                    }

                    var caminhoBase = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "refeicoes");
                    var arquivosEncontrados = Directory.Exists(caminhoBase)
                        ? Directory.GetFiles(caminhoBase, $"{request.GuidArquivo}.*")
                        : [];

                    if (arquivosEncontrados.Length == 0)
                    {
                        _logger.LogWarning("Imagem não encontrada no disco para a Refeição ID: {RefeicaoId}. Marcando como erro.", request.RefeicaoId);
                        await MarcarErroENotificarAsync(refeicao.Id, stoppingToken);
                        return;
                    }

                    caminhoFisicoCompleto = arquivosEncontrados[0];
                    var mimeType = ObterMimeType(caminhoFisicoCompleto);
                    var imagemBytes = await File.ReadAllBytesAsync(caminhoFisicoCompleto, stoppingToken);

                    using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
                    timeoutCts.CancelAfter(TimeSpan.FromSeconds(timeoutSegundos));

                    var estimativaCalorica = await llmService.SimularCaloriasRefeicao(
                        imagemBytes,
                        refeicao.Peso,
                        mimeType,
                        timeoutCts.Token);

                    if (estimativaCalorica == null)
                    {
                        throw new InvalidOperationException($"A LLM retornou resposta vazia/nula para a Refeição ID: {request.RefeicaoId}.");
                    }

                    refeicao.AtualizarEstimativa(
                        string.IsNullOrWhiteSpace(estimativaCalorica.Alimento) ? "alimento não identificado" : estimativaCalorica.Alimento,
                        estimativaCalorica.Calorias,
                        estimativaCalorica.Proteinas,
                        estimativaCalorica.Carboidratos,
                        estimativaCalorica.Gorduras,
                        estimativaCalorica.Acucares,
                        estimativaCalorica.Fibras);

                    await unitOfWork.CommitAsync();

                    await NotificarSeguramenteAsync(refeicao.UsuarioId, "RefeicaoProcessada", refeicao.Id, stoppingToken);

                    RemoverImagemTemporaria(caminhoFisicoCompleto);
                    _logger.LogInformation("Refeição {RefeicaoId} processada com sucesso.", request.RefeicaoId);
                    return;
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("WorkerEstimativaIa cancelado durante o processamento da Refeição {RefeicaoId}.", request.RefeicaoId);
                    return;
                }
                catch (LlmApiException ex) when (!ex.EhTransitorio)
                {
                    _logger.LogError(ex, "Erro definitivo da API de IA para RefeicaoId {RefeicaoId} (HTTP {StatusCode}). Sem retentativa.",
                        request.RefeicaoId, (int)ex.StatusCode);
                    await MarcarErroENotificarAsync(request.RefeicaoId, stoppingToken);
                    RemoverImagemTemporaria(caminhoFisicoCompleto);
                    return;
                }
                catch (Exception ex)
                {
                    var ehTimeout = ex is OperationCanceledException && !stoppingToken.IsCancellationRequested;
                    if (ehTimeout)
                    {
                        _logger.LogError(ex,
                            "Timeout ({Timeout}s) na tentativa {Tentativa}/{MaxTentativas} para RefeicaoId: {RefeicaoId}",
                            timeoutSegundos, tentativa, MaxTentativas, request.RefeicaoId);
                    }
                    else
                    {
                        _logger.LogError(ex,
                            "Erro na tentativa {Tentativa}/{MaxTentativas} para RefeicaoId: {RefeicaoId}",
                            tentativa, MaxTentativas, request.RefeicaoId);
                    }

                    if (tentativa < MaxTentativas)
                    {
                        var atraso = AtrasosBackoff[Math.Min(tentativa - 1, AtrasosBackoff.Length - 1)];
                        _logger.LogInformation("Aguardando {AtrasoSegundos}s (backoff) antes da próxima tentativa para RefeicaoId {RefeicaoId}...",
                            atraso.TotalSeconds, request.RefeicaoId);
                        await Task.Delay(atraso, stoppingToken);
                    }
                    else
                    {
                        _logger.LogCritical("Máximo de tentativas ({MaxTentativas}) atingido para RefeicaoId: {RefeicaoId}. Marcando como erro.",
                            MaxTentativas, request.RefeicaoId);
                        await MarcarErroENotificarAsync(request.RefeicaoId, stoppingToken);
                        RemoverImagemTemporaria(caminhoFisicoCompleto);
                    }
                }
            }
        }

        private async Task MarcarErroENotificarAsync(long refeicaoId, CancellationToken stoppingToken)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var refeicaoRepo = scope.ServiceProvider.GetRequiredService<IRefeicaoRepository>();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                var refeicao = await refeicaoRepo.ObterPorId(refeicaoId);
                if (refeicao == null) return;

                refeicao.MarcarComoErro();
                await unitOfWork.CommitAsync();

                await NotificarSeguramenteAsync(refeicao.UsuarioId, "RefeicaoErro", refeicao.Id, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao marcar status de erro para a Refeição ID: {RefeicaoId}.", refeicaoId);
            }
        }

        private async Task NotificarSeguramenteAsync(long usuarioId, string evento, long refeicaoId, CancellationToken stoppingToken)
        {
            try
            {
                await _hubContext.Clients.Group(usuarioId.ToString())
                    .SendAsync(evento, refeicaoId, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Falha ao enviar notificação SignalR '{Evento}' para usuário {UsuarioId}.", evento, usuarioId);
            }
        }

        private void RemoverImagemTemporaria(string? caminhoFisico)
        {
            if (string.IsNullOrWhiteSpace(caminhoFisico)) return;
            try
            {
                if (File.Exists(caminhoFisico)) File.Delete(caminhoFisico);
            }
            catch (Exception exFile)
            {
                _logger.LogWarning(exFile, "Falha ao deletar imagem temporária: {Caminho}.", caminhoFisico);
            }
        }

        private static string ObterMimeType(string caminhoArquivo)
        {
            return Path.GetExtension(caminhoArquivo).ToLowerInvariant() switch
            {
                ".png" => "image/png",
                ".webp" => "image/webp",
                ".gif" => "image/gif",
                _ => "image/jpeg"
            };
        }
    }
}
