using CalculadoraCalorias.Core.Domain.Entities;
using CalculadoraCalorias.Core.Domain.Enums;
using CalculadoraCalorias.Core.Domain.InternalDTO;

namespace CalculadoraCalorias.Application.DTOs.Responses
{
    public class RefeicaoGraficoDiarioResponse
    {   
        public decimal MetaCaloricaDiaria { get; set; }
        public double TotalCaloriasConsumidas { get; set; }
        public decimal TotalCaloriasGastas {  get; set; }
        public List<RefeicaoDTO> Refeicoes {  get; set; } = [];
        public List<ExercicioDTO> Exercicios { get; set; } = [];
    }

    public class GraficoPontoResponse
    {
        public string Legenda { get; set; } = string.Empty;
        public double CaloriasConsumidas { get; set; }
        public double CaloriasGastas { get; set; }
        public string Data { get; set; } = string.Empty;
    }

    public class GraficoPeriodoResponse
    {
        public decimal MetaCaloricaDiaria { get; set; }
        public List<GraficoPontoResponse> Pontos { get; set; } = [];
    }

    public class DistribuicaoItemResponse
    {
        public string Nome { get; set; } = string.Empty;
        public double Valor { get; set; }
        public double Percentual { get; set; }
    }

    public class EstatisticasDetalhadasResponse
    {
        public decimal MetaCaloricaDiaria { get; set; }
        public double TotalConsumido { get; set; }
        public double TotalGasto { get; set; }
        public double MediaConsumoDiario { get; set; }
        public double MediaGastoDiario { get; set; }
        public List<GraficoPontoResponse> Pontos { get; set; } = [];
        public List<DistribuicaoItemResponse> DistribuicaoExercicios { get; set; } = [];
        public List<DistribuicaoItemResponse> DistribuicaoRefeicoes { get; set; } = [];
    }
}
