namespace CalculadoraCalorias.Application.DTOs.Responses
{
    public class RefeicaoModeloResponse
    {
        public long Id { get; set; }
        public string? Apelido { get; set; }
        public double? Calorias { get; set; }
        public double? Proteinas { get; set; }
        public double? Carboidratos { get; set; }
        public double? Gorduras { get; set; }
        public int PesoOriginal { get; set; }
    }
}
