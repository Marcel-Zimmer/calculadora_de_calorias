using CalculadoraCalorias.Core.Domain.Common;
using CalculadoraCalorias.Core.Domain.Entities;
using CalculadoraCalorias.Core.Domain.InternalDTO;

namespace CalculadoraCalorias.Core.Domain.Interfaces
{
    public interface IAtividadeFisicaRepository : IRepositoryBase<AtividadeFisica>
    {
        Task<List<ExercicioDTO>> ObterDiariasPorUsuarioId(long usuarioId, DateOnly? data = null);
        Task<List<ExercicioDTO>> ObterPorPeriodo(long usuarioId, DateOnly inicio, DateOnly fim);
        Task<List<AtividadeFisica>> ObterTodosPorId(int idUsuario);
    }
}
