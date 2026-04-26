using CalculadoraCalorias.Core.Domain.Enums;
using System;
using System.Collections.Generic;

namespace CalculadoraCalorias.Application.DTOs.Responses
{
    public class NutrientesResponse
    {
        public string Periodo { get; set; } = string.Empty;

        public double MetaProteinas { get; set; }
        public double ConsumoProteinas { get; set; }

        public double MetaCarboidratos { get; set; }
        public double ConsumoCarboidratos { get; set; }

        public double MetaGorduras { get; set; }
        public double ConsumoGorduras { get; set; }

        public double MetaFibras { get; set; }
        public double ConsumoFibras { get; set; }

        public double LimiteAcucares { get; set; }
        public double ConsumoAcucares { get; set; }

        public List<NutrienteDetalheResponse> Detalhes { get; set; } = new();
    }

    public class NutrienteDetalheResponse
    {
        public NutrientesEnum Tipo { get; set; }
        public string Nome { get; set; } = string.Empty;
        public double Valor { get; set; }
        public double Meta { get; set; }
        public bool IsLimite { get; set; }
    }
}
