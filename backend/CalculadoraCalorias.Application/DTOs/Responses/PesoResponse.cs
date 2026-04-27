namespace CalculadoraCalorias.Application.DTOs.Responses
{
    public class PesoPontoResponse
    {
        public long Id { get; set; }
        public string Data { get; set; } = string.Empty;
        public string Legenda { get; set; } = string.Empty;
        public double Peso { get; set; }
    }

    public class EstatisticasPesoResponse
    {
        public double PesoAtual { get; set; }
        public double PesoInicial { get; set; }
        public double MaiorPeso { get; set; }
        public double MenorPeso { get; set; }
        public double VariacaoTotal { get; set; }
        public double ImcAtual { get; set; }
        public double TmbAtual { get; set; }
        public string Objetivo { get; set; } = string.Empty;
        public int ObjetivoId { get; set; }
        public List<PesoPontoResponse> Historico { get; set; } = [];
    }
}
