using CalculadoraCalorias.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CalculadoraCalorias.Core.Domain.InternalDTO
{
    public class RefeicaoDTO
    {
        public long Id { get; set; }
        public TipoRefeicaoEnum TipoRefeicao { get; set; }
        public int? Calorias {  get; set; }
        public DateOnly Data { get; set; }

        public double? Proteinas { get; set; }
        public double? Carboidratos { get; set; }
        public double? Gorduras { get; set; }
        public double? Acucares { get; set; }
        public double? Fibras { get; set; }
    }
}
