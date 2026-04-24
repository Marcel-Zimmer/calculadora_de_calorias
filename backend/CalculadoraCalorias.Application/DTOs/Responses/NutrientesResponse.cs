using System;

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
    }
}
