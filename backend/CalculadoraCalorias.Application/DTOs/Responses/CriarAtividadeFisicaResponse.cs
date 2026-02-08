namespace CalculadoraCalorias.Application.DTOs.Responses
{
    public class CriarAtividadeFisicaResponse
    {   
        public long Id { get; set; }
        public long UsuarioId { get; set; }
        public long TipoAtividadeId { get; set; }
        public decimal PesoSnapshot { get; set; }
        public decimal CaloriasCalculadas { get; set; }
        public double VelocidadeMedia { get; set; }
        public DateTime DataExercicio { get; set; }
        public TimeSpan TempoExercicio { get; set; }
    }
}
