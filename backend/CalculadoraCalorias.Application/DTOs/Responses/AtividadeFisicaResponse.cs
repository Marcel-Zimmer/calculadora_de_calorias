namespace CalculadoraCalorias.Application.DTOs.Responses
{
    public class AtividadeFisicaResponse
    {   
        public long? Id { get; set; }
        public long? UsuarioId { get; set; }
        public long? TipoAtividadeId { get; set; }
        public decimal? CaloriasEstimadas { get; set; }
        public DateOnly DataExercicio { get; set; }
        public TimeSpan TempoExercicio { get; set; }
    }
}
