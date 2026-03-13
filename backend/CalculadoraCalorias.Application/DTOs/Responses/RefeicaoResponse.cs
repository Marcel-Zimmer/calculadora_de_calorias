using CalculadoraCalorias.Core.Domain.Entities;
using CalculadoraCalorias.Core.Domain.Enums;
using CalculadoraCalorias.Core.Domain.InternalDTO;

namespace CalculadoraCalorias.Application.DTOs.Responses
{
    public class RefeicaoGraficoDiarioResponse
    {   
        public decimal MetaCaloricaDiaria { get; set; }
        public double TotalCaloriasConsumidas { get; set; }
        public List<RefeicaoDTO> Refeicoes {  get; set; } = [];
    }

}
