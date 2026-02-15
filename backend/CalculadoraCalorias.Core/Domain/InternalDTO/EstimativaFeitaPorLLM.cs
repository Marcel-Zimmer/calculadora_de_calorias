
namespace CalculadoraCalorias.Application.DTOs.Requests
{
    public class EstimativaFeitaPorLLM
    {
        public string Alimento { get; set; } = string.Empty;
        public double Calorias { get; set; }
        public double Proteinas { get; set; }
        public double Carboidratos { get; set; }
        public double Gorduras { get; set; }
    }
}
