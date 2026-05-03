namespace CalculadoraCalorias.Application.DTOs.Responses
{
    public class RegistroAguaResponse
    {
        public long Id { get; set; }
        public int QuantidadeMl { get; set; }
        public string Data { get; set; } = string.Empty;
        public string Hora { get; set; } = string.Empty;
    }

    public class AguaPontoResponse
    {
        public string Legenda { get; set; } = string.Empty;
        public int QuantidadeMl { get; set; }
        public string Data { get; set; } = string.Empty;
    }

    public class EstatisticasAguaResponse
    {
        public int MetaAguaDiaria { get; set; }
        public int TotalConsumido { get; set; }
        public int MediaDiaria { get; set; }
        public int DiasNaMeta { get; set; }
        public int TotalDias { get; set; }
        public int MaiorConsumo { get; set; }
        public string DiaMaiorConsumo { get; set; } = string.Empty;
        public List<AguaPontoResponse> Pontos { get; set; } = [];
    }
}
