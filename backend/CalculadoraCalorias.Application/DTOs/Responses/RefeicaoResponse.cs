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
}
