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
        IRegistroFisicoService _registroFisicoService) : IGraficoAppService
    {
       
        public async Task<Resultado<RefeicaoGraficoDiarioResponse>> GraficoDiario(long usuarioId, DateOnly? data = null)
        {
            if(usuarioId == 0) return Resultado<RefeicaoGraficoDiarioResponse>.Failure(TipoDeErro.SystemFailure, "Id de usuário inválido");

            var registroFisico = await _registroFisicoService.ObterPorIdUsuario(usuarioId);
            if (registroFisico == null) return Resultado<RefeicaoGraficoDiarioResponse>.Failure(TipoDeErro.SystemFailure, "Registro fisico null");

            var refeicoes = await _refeicaoService.ObterDiariasPorUsuarioId(usuarioId, data);
            var atividades = await _atividadeFisicaService.ObterDiariasPorUsuarioId(usuarioId, data);

            var totalConsumido = refeicoes?.Sum(x => x.Calorias ?? 0) ?? 0;
            var totalGasto = atividades?.Sum(y => y.CaloriasEstimadas ?? 0) ?? 0;

            var informacoesDiarias = new RefeicaoGraficoDiarioResponse
            {
                MetaCaloricaDiaria = registroFisico.MetaCaloricaDiaria ?? 0,
                TotalCaloriasConsumidas = totalConsumido,
                TotalCaloriasGastas = totalGasto,
                CaloriasCalculadas = Math.Max(0, totalConsumido - totalGasto),
                Refeicoes = refeicoes ?? [], 
                Exercicios = atividades ?? []
            };

            return Resultado<RefeicaoGraficoDiarioResponse>.Success(informacoesDiarias);
        }

        public async Task<Resultado<GraficoPeriodoResponse>> GraficoSemanal(long idUsuario, DateOnly? data = null)
        {
            var dataRef = data?.ToDateTime(TimeOnly.MinValue) ?? DateTime.Today;
            int diff = (7 + (dataRef.DayOfWeek - DayOfWeek.Monday)) % 7;
            var inicioSemana = DateOnly.FromDateTime(dataRef.AddDays(-1 * diff));
            var fimSemana = inicioSemana.AddDays(6);

            var dados = await ObterDadosPorPeriodo(idUsuario, inicioSemana, fimSemana, true);
            var insights = CalcularInsights(dados.Pontos, dados.MetaCaloricaDiaria);

            return Resultado<GraficoPeriodoResponse>.Success(new GraficoPeriodoResponse 
            { 
                MetaCaloricaDiaria = dados.MetaCaloricaDiaria, 
                TotalCaloriasConsumidas = dados.MediaConsumoDiario,
                TotalCaloriasGastas = dados.MediaGastoDiario,
                CaloriasCalculadas = Math.Max(0, dados.MediaConsumoDiario - dados.MediaGastoDiario),
                Pontos = dados.Pontos,
                Insights = insights
            });
        }

        public async Task<Resultado<GraficoPeriodoResponse>> GraficoMensal(long idUsuario, DateOnly? data = null)
        {
            var dataRef = data ?? DateOnly.FromDateTime(DateTime.Today);
            var inicioMes = new DateOnly(dataRef.Year, dataRef.Month, 1);
            var fimMes = inicioMes.AddMonths(1).AddDays(-1);

            var dados = await ObterDadosPorPeriodo(idUsuario, inicioMes, fimMes, false);
            var insights = CalcularInsights(dados.Pontos, dados.MetaCaloricaDiaria);

            return Resultado<GraficoPeriodoResponse>.Success(new GraficoPeriodoResponse 
            { 
                MetaCaloricaDiaria = dados.MetaCaloricaDiaria, 
                TotalCaloriasConsumidas = dados.MediaConsumoDiario,
                TotalCaloriasGastas = dados.MediaGastoDiario,
                CaloriasCalculadas = Math.Max(0, dados.MediaConsumoDiario - dados.MediaGastoDiario),
                Pontos = dados.Pontos,
                Insights = insights
            });
        }

        private DashboardInsightsResponse CalcularInsights(List<GraficoPontoResponse> pontos, decimal meta)
        {
            var pontosComRegistro = pontos.Where(p => p.CaloriasConsumidas > 0 || p.CaloriasGastas > 0).ToList();
            if (pontosComRegistro.Count == 0) return new DashboardInsightsResponse();

            var diasNaMeta = pontosComRegistro.Count(p => (p.CaloriasConsumidas - p.CaloriasGastas) <= (int)meta);
            var saldoTotal = pontosComRegistro.Sum(p => p.CaloriasConsumidas - p.CaloriasGastas);
            var metaTotal = (int)meta * pontosComRegistro.Count;
            var diferencaAbsoluta = metaTotal - saldoTotal;
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

        public async Task<Resultado<EstatisticasDetalhadasResponse>> EstatisticasSemanais(long usuarioId, DateOnly? data = null)
        {
            var dataRef = data?.ToDateTime(TimeOnly.MinValue) ?? DateTime.Today;
            int diff = (7 + (dataRef.DayOfWeek - DayOfWeek.Monday)) % 7;
            var inicioSemana = DateOnly.FromDateTime(dataRef.AddDays(-1 * diff));
            var fimSemana = inicioSemana.AddDays(6);

            var dados = await ObterDadosPorPeriodo(usuarioId, inicioSemana, fimSemana, true);
            dados.Insights = CalcularInsights(dados.Pontos, dados.MetaCaloricaDiaria);
            return Resultado<EstatisticasDetalhadasResponse>.Success(dados);
        }

        public async Task<Resultado<EstatisticasDetalhadasResponse>> EstatisticasMensais(long usuarioId, DateOnly? data = null)
        {
            var dataRef = data ?? DateOnly.FromDateTime(DateTime.Today);
            var inicioMes = new DateOnly(dataRef.Year, dataRef.Month, 1);
            var fimMes = inicioMes.AddMonths(1).AddDays(-1);

            var dados = await ObterDadosPorPeriodo(usuarioId, inicioMes, fimMes, false);
            dados.Insights = CalcularInsights(dados.Pontos, dados.MetaCaloricaDiaria);
            return Resultado<EstatisticasDetalhadasResponse>.Success(dados);
        }

        private async Task<EstatisticasDetalhadasResponse> ObterDadosPorPeriodo(long usuarioId, DateOnly inicio, DateOnly fim, bool usarNomeDia)
        {
            var registroFisico = await _registroFisicoService.ObterPorIdUsuario(usuarioId);
            var refeicoes = await _refeicaoService.ObterPorPeriodo(usuarioId, inicio, fim);
            var atividades = await _atividadeFisicaService.ObterPorPeriodo(usuarioId, inicio, fim);

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

                if (consumidoDia > 0 || gastoDia > 0) diasComDados++;

                pontos.Add(new GraficoPontoResponse
                {
                    Data = data.ToString("yyyy-MM-dd"),
                    Legenda = legenda,
                    CaloriasConsumidas = (int)consumidoDia,
                    CaloriasGastas = (int)gastoDia,
                    SaldoCalorico = (int)(consumidoDia - gastoDia)
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
