using CalculadoraCalorias.Core.Domain.Common;
using CalculadoraCalorias.Core.Domain.Entities;
using CalculadoraCalorias.Core.Domain.InternalDTO;

namespace CalculadoraCalorias.Core.Domain.Interfaces
{
    public interface IAtividadeFisicaRepository : IRepositoryBase<AtividadeFisica>
    {
        Task<List<ExercicioDTO>> ObterDiariasPorUsuarioId(long usuarioId);
        Task<List<AtividadeFisica>> ObterTodosPorId(int idUsuario);
    }
}
