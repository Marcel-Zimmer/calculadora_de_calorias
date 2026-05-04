using CalculadoraCalorias.Application.DTOs.Responses;
using CalculadoraCalorias.Application.Interfaces;
using CalculadoraCalorias.Core.Domain.Common;
using CalculadoraCalorias.Core.Domain.Enums;
using CalculadoraCalorias.Core.Domain.Interfaces;
using System.Globalization;

namespace CalculadoraCalorias.Application.Features
{
    public class GraficoAppService(
        IRefeicaoService _refeicaoService, 
        IAtividadeFisicaService _atividadeFisicaService,
        IRegistroFisicoService _registroFisicoService,
        IPerfilBiometricoService _perfilBiometricoService,
        IRegistroAguaService _registroAguaService,
        IContextoHttpService contextoHttpService) : AppServiceBase(contextoHttpService), IGraficoAppService
    {
       
        public async Task<Resultado<RefeicaoGraficoDiarioResponse>> GraficoDiario(DateOnly? data = null)
        {
            var registroFisico = await _registroFisicoService.ObterPorIdUsuario(UsuarioId);
            if (registroFisico == null) return Resultado<RefeicaoGraficoDiarioResponse>.Failure(TipoDeErro.SystemFailure, "Registro fisico null");

            var dataFiltro = data ?? FusoHorario.ObterDataHojeBrasilia();
            var refeicoes = await _refeicaoService.ObterDiariasPorUsuarioId(UsuarioId, data);
            var atividades = await _atividadeFisicaService.ObterDiariasPorUsuarioId(UsuarioId, data);
            var registrosAgua = await _registroAguaService.ObterDiariosPorUsuarioId(UsuarioId, data);

            var totalConsumido = refeicoes?.Sum(x => x.Calorias ?? 0) ?? 0;
            var totalGasto = atividades?.Sum(y => y.CaloriasEstimadas ?? 0) ?? 0;
            var totalAgua = registrosAgua?.Sum(a => a.QuantidadeMl) ?? 0;

            var informacoesDiarias = new RefeicaoGraficoDiarioResponse
            {
                MetaCaloricaDiaria = registroFisico.MetaCaloricaDiaria ?? 0,
                TotalCaloriasConsumidas = totalConsumido,
                TotalCaloriasGastas = totalGasto,
                CaloriasCalculadas = Math.Max(0, totalConsumido - totalGasto),
                TotalAguaMl = totalAgua,
                Refeicoes = refeicoes ?? [], 
                Exercicios = atividades ?? [],
                RegistrosAgua = registrosAgua?.Select(a => new RegistroAguaResponse
                {
                    Id = a.Id,
                    QuantidadeMl = a.QuantidadeMl,
                    Data = a.Data.ToString("yyyy-MM-dd"),
                    Hora = a.Hora.ToString("HH:mm")
                }).ToList() ?? []
            };

            return Resultado<RefeicaoGraficoDiarioResponse>.Success(informacoesDiarias);
        }

        public async Task<Resultado<GraficoPeriodoResponse>> GraficoSemanal(DateOnly? data = null)
        {
            var dataRef = data?.ToDateTime(TimeOnly.MinValue) ?? DateTime.Today;
            int diff = (7 + (dataRef.DayOfWeek - DayOfWeek.Monday)) % 7;
            var inicioSemana = DateOnly.FromDateTime(dataRef.AddDays(-1 * diff));
            var fimSemana = inicioSemana.AddDays(6);

            var dados = await ObterDadosPorPeriodo(inicioSemana, fimSemana, true);
            var insights = CalcularInsights(dados.Pontos, dados.MetaCaloricaDiaria, dados.TaxaMetabolicaBasal);

            var totalAgua = dados.Pontos.Sum(p => p.AguaMl);
            var mediaAgua = dados.DiasComDados > 0 ? totalAgua / dados.DiasComDados : 0;

            return Resultado<GraficoPeriodoResponse>.Success(new GraficoPeriodoResponse 
            { 
                MetaCaloricaDiaria = dados.MetaCaloricaDiaria, 
                TaxaMetabolicaBasal = dados.TaxaMetabolicaBasal,
                TotalCaloriasConsumidas = dados.MediaConsumoDiario,
                TotalCaloriasGastas = dados.MediaGastoDiario,
                CaloriasCalculadas = Math.Max(0, dados.MediaConsumoDiario - dados.MediaGastoDiario),
                TotalAguaMl = totalAgua,
                MediaAguaDiaria = mediaAgua,
                Pontos = dados.Pontos,
                Insights = insights
            });
        }

        public async Task<Resultado<GraficoPeriodoResponse>> GraficoMensal(DateOnly? data = null)
        {
            var dataRef = data ?? DateOnly.FromDateTime(DateTime.Today);
            var inicioMes = new DateOnly(dataRef.Year, dataRef.Month, 1);
            var fimMes = inicioMes.AddMonths(1).AddDays(-1);

            var dados = await ObterDadosPorPeriodo(inicioMes, fimMes, false);
            var insights = CalcularInsights(dados.Pontos, dados.MetaCaloricaDiaria, dados.TaxaMetabolicaBasal);

            var totalAgua = dados.Pontos.Sum(p => p.AguaMl);
            var mediaAgua = dados.DiasComDados > 0 ? totalAgua / dados.DiasComDados : 0;

            return Resultado<GraficoPeriodoResponse>.Success(new GraficoPeriodoResponse 
            { 
                MetaCaloricaDiaria = dados.MetaCaloricaDiaria, 
                TaxaMetabolicaBasal = dados.TaxaMetabolicaBasal,
                TotalCaloriasConsumidas = dados.MediaConsumoDiario,
                TotalCaloriasGastas = dados.MediaGastoDiario,
                CaloriasCalculadas = Math.Max(0, dados.MediaConsumoDiario - dados.MediaGastoDiario),
                TotalAguaMl = totalAgua,
                MediaAguaDiaria = mediaAgua,
                Pontos = dados.Pontos,
                Insights = insights
            });
        }

        private DashboardInsightsResponse CalcularInsights(List<GraficoPontoResponse> pontos, decimal meta, decimal tmb)
        {
            var pontosComRegistro = pontos.Where(p => p.CaloriasConsumidas > 0 || p.CaloriasGastas > 0).ToList();
            if (pontosComRegistro.Count == 0) return new DashboardInsightsResponse();

            var diasNaMeta = pontosComRegistro.Count(p => (p.CaloriasConsumidas - p.CaloriasGastas) <= (int)meta);
            var saldoTotal = pontosComRegistro.Sum(p => p.CaloriasConsumidas - p.CaloriasGastas);
            
            // Impacto e Equilíbrio baseados na TMB (Gasto Energético Total Estimado)
            var manutencaoTotal = (int)tmb * pontosComRegistro.Count;
            var diferencaAbsoluta = manutencaoTotal - saldoTotal;
            var impactoPeso = (double)diferencaAbsoluta / 7700;

            return new DashboardInsightsResponse
            {
                DiasNaMeta = diasNaMeta,
                TotalDias = pontos.Count,
                SaldoTotal = saldoTotal,
                DiferencaAbsoluta = diferencaAbsoluta,
                ImpactoPeso = Math.Round(impactoPeso, 3)
            };
        }

        public async Task<Resultado<EstatisticasDetalhadasResponse>> EstatisticasSemanais(DateOnly? data = null)
        {
            var dataRef = data?.ToDateTime(TimeOnly.MinValue) ?? DateTime.Today;
            int diff = (7 + (dataRef.DayOfWeek - DayOfWeek.Monday)) % 7;
            var inicioSemana = DateOnly.FromDateTime(dataRef.AddDays(-1 * diff));
            var fimSemana = inicioSemana.AddDays(6);

            var dados = await ObterDadosPorPeriodo(inicioSemana, fimSemana, true);
            dados.Insights = CalcularInsights(dados.Pontos, dados.MetaCaloricaDiaria, dados.TaxaMetabolicaBasal);
            return Resultado<EstatisticasDetalhadasResponse>.Success(dados);
        }

        public async Task<Resultado<EstatisticasDetalhadasResponse>> EstatisticasMensais(DateOnly? data = null)
        {
            var dataRef = data ?? DateOnly.FromDateTime(DateTime.Today);
            var inicioMes = new DateOnly(dataRef.Year, dataRef.Month, 1);
            var fimMes = inicioMes.AddMonths(1).AddDays(-1);

            var dados = await ObterDadosPorPeriodo(inicioMes, fimMes, false);
            dados.Insights = CalcularInsights(dados.Pontos, dados.MetaCaloricaDiaria, dados.TaxaMetabolicaBasal);
            
            // Novos Insights de Consumo Mensal
            var pontosConsumo = dados.Pontos.Where(p => p.CaloriasConsumidas > 0).ToList();
            if (pontosConsumo.Count > 0)
            {
                var maiorIngestao = pontosConsumo.Max(p => p.CaloriasConsumidas);
                var diaMaiorIngestao = pontosConsumo.First(p => p.CaloriasConsumidas == maiorIngestao).Legenda;

                var topPicos = pontosConsumo
                    .OrderByDescending(p => p.CaloriasConsumidas)
                    .Take(5)
                    .ToList();

                var mediasSemanais = new List<SemanaMediaResponse>();
                for (int i = 0; i < dados.Pontos.Count; i += 7)
                {
                    var lote = dados.Pontos.Skip(i).Take(7).ToList();
                    var media = lote.Average(p => p.CaloriasConsumidas);
                    mediasSemanais.Add(new SemanaMediaResponse { Nome = $"Semana {Math.Floor((double)i / 7) + 1}", Valor = media });
                }

                dados.ConsumoInsights = new ConsumoMensalInsightsResponse
                {
                    MaiorIngestao = maiorIngestao,
                    DiaMaiorIngestao = diaMaiorIngestao,
                    TopPicos = topPicos,
                    MediasSemanais = mediasSemanais
                };
            }

            // Novos Insights de Gasto Mensal
            var pontosGasto = dados.Pontos.Where(p => p.CaloriasGastas > 0).ToList();
            if (pontosGasto.Count > 0)
            {
                var maiorGasto = pontosGasto.Max(p => p.CaloriasGastas);
                var diaMaiorGasto = pontosGasto.First(p => p.CaloriasGastas == maiorGasto).Legenda;

                var topGasto = pontosGasto
                    .OrderByDescending(p => p.CaloriasGastas)
                    .Take(5)
                    .ToList();

                var mediasSemanaisGasto = new List<SemanaMediaResponse>();
                for (int i = 0; i < dados.Pontos.Count; i += 7)
                {
                    var lote = dados.Pontos.Skip(i).Take(7).ToList();
                    var soma = lote.Sum(p => p.CaloriasGastas);
                    mediasSemanaisGasto.Add(new SemanaMediaResponse { Nome = $"Sem {Math.Floor((double)i / 7) + 1}", Valor = soma });
                }

                var exercicioFavorito = dados.DistribuicaoExercicios
                    .OrderByDescending(x => x.Valor)
                    .FirstOrDefault();

                dados.GastoInsights = new GastoMensalInsightsResponse
                {
                    MaiorGasto = maiorGasto,
                    DiaMaiorGasto = diaMaiorGasto,
                    ExercicioPrincipal = exercicioFavorito?.Nome ?? "Nenhum",
                    TopGasto = topGasto,
                    MediasSemanais = mediasSemanaisGasto
                };
            }

            return Resultado<EstatisticasDetalhadasResponse>.Success(dados);
        }

        public async Task<Resultado<EstatisticasPesoResponse>> ObterEstatisticasPeso()
        {
            var historico = await _registroFisicoService.ObterHistorico(UsuarioId);
            if (historico == null || historico.Count == 0)
                return Resultado<EstatisticasPesoResponse>.Failure(TipoDeErro.NotFound, "Nenhum registro de peso encontrado.");

            var perfil = await _perfilBiometricoService.ObterPorIdUsuario(UsuarioId);

            var historicoAgrupado = historico
                .GroupBy(h => h.DataRegistro.Date)
                .Select(g => g.OrderByDescending(x => x.DataRegistro).First())
                .OrderBy(x => x.DataRegistro)
                .ToList();

            var pontos = historicoAgrupado.Select(h => new PesoPontoResponse
            {
                Id = h.Id,
                Data = h.DataRegistro.ToString("yyyy-MM-dd"),
                Legenda = h.DataRegistro.ToString("dd/MM"),
                Peso = (double)h.PesoKg
            }).ToList();

            var pesoAtual = (double)historicoAgrupado.Last().PesoKg;
            var pesoInicial = (double)historicoAgrupado.First().PesoKg;
            var maiorPeso = (double)historicoAgrupado.Max(h => h.PesoKg);
            var menorPeso = (double)historicoAgrupado.Min(h => h.PesoKg);
            var imcAtual = (double)historico.Last().ImcCalculado;
            var tmbAtual = (double)historico.Last().TaxaMetabolicaBasal;

            var objetivoDescricao = perfil?.Objetivo switch
            {
                ObjetivoEnum.PerdaPesoAgressiva => "Perda de Peso (Foco)",
                ObjetivoEnum.PerdaPesoLeve => "Perda de Peso Leve",
                ObjetivoEnum.ManterPeso => "Manutenção",
                _ => "Não Definido"
            };

            return Resultado<EstatisticasPesoResponse>.Success(new EstatisticasPesoResponse
            {
                PesoAtual = pesoAtual,
                PesoInicial = pesoInicial,
                MaiorPeso = maiorPeso,
                MenorPeso = menorPeso,
                VariacaoTotal = Math.Round(pesoAtual - pesoInicial, 2),
                ImcAtual = Math.Round(imcAtual, 2),
                TmbAtual = Math.Round(tmbAtual, 2),
                Objetivo = objetivoDescricao,
                ObjetivoId = (int)(perfil?.Objetivo ?? 0),
                Historico = pontos
            });
        }

        private async Task<EstatisticasDetalhadasResponse> ObterDadosPorPeriodo(DateOnly inicio, DateOnly fim, bool usarNomeDia)
        {
            var registroFisico = await _registroFisicoService.ObterPorIdUsuario(UsuarioId);
            var refeicoes = await _refeicaoService.ObterPorPeriodo(UsuarioId, inicio, fim);
            var atividades = await _atividadeFisicaService.ObterPorPeriodo(UsuarioId, inicio, fim);
            var registrosAgua = await _registroAguaService.ObterPorPeriodo(UsuarioId, inicio, fim);

            var pontos = new List<GraficoPontoResponse>();
            var cultura = new CultureInfo("pt-BR");
            int diasComDados = 0;

            for (var data = inicio; data <= fim; data = data.AddDays(1))
            {
                var legenda = usarNomeDia 
                    ? cultura.DateTimeFormat.GetDayName(data.ToDateTime(TimeOnly.MinValue).DayOfWeek)
                    : data.Day.ToString();

                var consumidoDia = refeicoes.Where(r => r.Data == data).Sum(r => r.Calorias ?? 0);
                var gastoDia = (double)atividades.Where(a => a.Data == data).Sum(a => a.CaloriasEstimadas ?? 0);
                var aguaDia = registrosAgua.Where(a => a.Data == data).Sum(a => a.QuantidadeMl);

                if (consumidoDia > 0 || gastoDia > 0 || aguaDia > 0) diasComDados++;

                pontos.Add(new GraficoPontoResponse
                {
                    Data = data.ToString("yyyy-MM-dd"),
                    Legenda = legenda,
                    CaloriasConsumidas = (int)consumidoDia,
                    CaloriasGastas = (int)gastoDia,
                    SaldoCalorico = (int)(consumidoDia - gastoDia),
                    AguaMl = aguaDia
                });
            }

            var totalConsumido = refeicoes.Sum(r => r.Calorias ?? 0);
            var totalGasto = (double)atividades.Sum(a => a.CaloriasEstimadas ?? 0);
            int diasComRefeicao = refeicoes.Select(r => r.Data).Distinct().Count();
            
            var hoje = DateOnly.FromDateTime(DateTime.Today);
            var fimCalculo = hoje < fim ? hoje : fim;
            int diasCorridos = (fimCalculo.DayNumber - inicio.DayNumber) + 1;
            if (diasCorridos <= 0) diasCorridos = 1;

            int totalDiasPeriodo = (fim.DayNumber - inicio.DayNumber) + 1;

            // Distribuição de Exercícios
            var distExercicios = atividades
                .GroupBy(a => a.TipoExercicio)
                .Select(g => new DistribuicaoItemResponse
                {
                    Nome = ((int)g.Key).ToString(),
                    Valor = (int)g.Sum(x => x.CaloriasEstimadas ?? 0),
                    Percentual = totalGasto > 0 ? (double)g.Sum(x => x.CaloriasEstimadas ?? 0) / totalGasto * 100 : 0
                }).ToList();

            // Distribuição de Refeições (Garante todos os tipos)
            var distRefeicoes = Enum.GetValues(typeof(TipoRefeicaoEnum))
                .Cast<TipoRefeicaoEnum>()
                .Select(tipo => {
                    var soma = refeicoes.Where(r => r.TipoRefeicao == tipo).Sum(x => x.Calorias ?? 0);
                    return new DistribuicaoItemResponse
                    {
                        Nome = ((int)tipo).ToString(),
                        Valor = (int)soma,
                        Media = diasComDados > 0 ? (int)(soma / diasComDados) : 0,
                        Percentual = totalConsumido > 0 ? (soma / (double)totalConsumido) * 100 : 0
                    };
                }).ToList();

            return new EstatisticasDetalhadasResponse
            {
                MetaCaloricaDiaria = registroFisico?.MetaCaloricaDiaria ?? 0,
                TaxaMetabolicaBasal = registroFisico?.TaxaMetabolicaBasal ?? 0,
                TotalConsumido = (int)totalConsumido,
                TotalGasto = (int)totalGasto,
                MediaConsumoDiario = diasComRefeicao > 0 ? (int)(totalConsumido / diasComRefeicao) : 0,
                MediaGastoDiario = (int)(totalGasto / diasCorridos),
                DiasComDados = diasComDados,
                Pontos = pontos,
                DistribuicaoExercicios = distExercicios,
                DistribuicaoRefeicoes = distRefeicoes
            };
        }
    }
}
