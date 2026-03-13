using CalculadoraCalorias.Core.Domain.Common;
using CalculadoraCalorias.Core.Domain.Entities;
using CalculadoraCalorias.Core.Domain.InternalDTO;

namespace CalculadoraCalorias.Core.Domain.Interfaces
{
    public interface IRefeicaoRepository : IRepositoryBase<Refeicao>
    {
        Task<List<RefeicaoDTO>> ObterDiariasPorUsuarioId(long usuarioId);
    }
}
