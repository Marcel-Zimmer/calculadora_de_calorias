namespace CalculadoraCalorias.Core.Domain.InternalDTO
{
    public class RefeicaoModeloDTO
    {
        public long Id { get; set; }
        public string? Apelido { get; set; }
        public int? Calorias { get; set; }
        public double? Proteinas { get; set; }
        public double? Carboidratos { get; set; }
        public double? Gorduras { get; set; }
        public int PesoOriginal { get; set; }
    }
}
