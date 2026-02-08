using CalculadoraCalorias.Core.Domain.Common;
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
        Task<List<AtividadeFisica>> ObterTodosPorId(int ididUsuario);
        Task<bool> Excluir(int id);
        Task<AtividadeFisica?> ObterPorId(int id);
        Task<AtividadeFisica?> Atualizar(long id, TipoExercicioEnum tipo, int kilometragemPercorrida, TimeSpan tempoDeExercicio);
    }
}
