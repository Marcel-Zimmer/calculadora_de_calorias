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
        public MacrosResponse? Macros { get; set; }
        public VereditoResponse? Veredito { get; set; }
    }

    public class MacrosResponse
    {
        public double CaloriasProteinas { get; set; }
        public double CaloriasCarboidratos { get; set; }
        public double CaloriasGorduras { get; set; }
        public double TotalCalorias { get; set; }
        public double PercentualProteinas { get; set; }
        public double PercentualCarboidratos { get; set; }
        public double PercentualGorduras { get; set; }
    }

    public class VereditoResponse
    {
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Icone { get; set; } = string.Empty;
        public string CorCss { get; set; } = string.Empty;
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
