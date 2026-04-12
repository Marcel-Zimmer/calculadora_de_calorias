using CalculadoraCalorias.Core.Domain.Enums;

namespace CalculadoraCalorias.Core.Domain.InternalDTO
{
    public class ExercicioDTO
    {
        public long Id { get; set; }
        public TipoExercicioEnum TipoExercicio { get; set; }
        public decimal? CaloriasEstimadas {  get; set; }
        public TimeSpan? TempoDeExercicio { get; set; }
        public DateOnly Data { get; set; }
    }
}
