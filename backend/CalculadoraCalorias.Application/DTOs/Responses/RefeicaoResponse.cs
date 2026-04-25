using CalculadoraCalorias.Core.Domain.Entities;
using CalculadoraCalorias.Core.Domain.Enums;
using CalculadoraCalorias.Core.Domain.InternalDTO;

namespace CalculadoraCalorias.Application.DTOs.Responses
{
    public class RefeicaoGraficoDiarioResponse
    {   
        public decimal MetaCaloricaDiaria { get; set; }
        public int TotalCaloriasConsumidas { get; set; }
        public int TotalCaloriasGastas {  get; set; }
        public int CaloriasCalculadas { get; set; }
        public List<RefeicaoDTO> Refeicoes {  get; set; } = [];
        public List<ExercicioDTO> Exercicios { get; set; } = [];
    }

    public class GraficoPontoResponse
    {
        public string Legenda { get; set; } = string.Empty;
        public int CaloriasConsumidas { get; set; }
        public int CaloriasGastas { get; set; }
        public int SaldoCalorico { get; set; }
        public string Data { get; set; } = string.Empty;
    }

    public class DashboardInsightsResponse
    {
        public int DiasNaMeta { get; set; }
        public int TotalDias { get; set; }
        public int SaldoTotal { get; set; }
        public double ImpactoPeso { get; set; }
        public int DiferencaAbsoluta { get; set; }
    }

    public class GraficoPeriodoResponse
    {
        public decimal MetaCaloricaDiaria { get; set; }
        public int TotalCaloriasConsumidas { get; set; }
        public int TotalCaloriasGastas { get; set; }
        public int CaloriasCalculadas { get; set; }
        public List<GraficoPontoResponse> Pontos { get; set; } = [];
        public DashboardInsightsResponse? Insights { get; set; }
    }

    public class DistribuicaoItemResponse
    {
        public string Nome { get; set; } = string.Empty;
        public int Valor { get; set; }
        public double Percentual { get; set; }
    }

    public class EstatisticasDetalhadasResponse
    {
        public decimal MetaCaloricaDiaria { get; set; }
        public int TotalConsumido { get; set; }
        public int TotalGasto { get; set; }
        public int MediaConsumoDiario { get; set; }
        public int MediaGastoDiario { get; set; }
        public int DiasComDados { get; set; }
        public List<GraficoPontoResponse> Pontos { get; set; } = [];
        public List<DistribuicaoItemResponse> DistribuicaoExercicios { get; set; } = [];
        public List<DistribuicaoItemResponse> DistribuicaoRefeicoes { get; set; } = [];
        public DashboardInsightsResponse? Insights { get; set; }
    }
}
