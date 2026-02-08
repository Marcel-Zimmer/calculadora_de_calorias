using CalculadoraCalorias.Core.Domain.Entities;
using CalculadoraCalorias.Core.Domain.Enums;

namespace CalculadoraCalorias.Core.Domain.Interfaces
{
    public interface IAtividadeFisicaService
    {
        Task<AtividadeFisica> Simular(long usuarioId, 
                                        TipoExercicioEnum tipo, 
                                        int kilometragemPercorrida,
                                        TimeSpan tempoDeExercicio);

        Task<AtividadeFisica?> Adicionar(long usuarioId,
                                        TipoExercicioEnum tipo,
                                        int kilometragemPercorrida,
                                        TimeSpan tempoDeExercicio);
    }
}
