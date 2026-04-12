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
        public double? Calorias {  get; set; }
        public DateOnly Data { get; set; }
    }
}
