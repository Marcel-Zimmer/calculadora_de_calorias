using CalculadoraCalorias.Core.Domain.Common;
using CalculadoraCalorias.Core.Domain.Entities;

namespace CalculadoraCalorias.Core.Domain.Interfaces
{
    public interface IAtividadeFisicaRepository : IRepositoryBase<AtividadeFisica>
    {
        Task<List<AtividadeFisica>> ObterTodosPorId(int idUsuario);
    }
}
