using CalculadoraCalorias.Core.Domain.Common;
using CalculadoraCalorias.Core.Domain.Entities;
using CalculadoraCalorias.Core.Domain.InternalDTO;

namespace CalculadoraCalorias.Core.Domain.Interfaces
{
    public interface IRefeicaoRepository : IRepositoryBase<Refeicao>
    {
        Task<List<RefeicaoDTO>> ObterDiariasPorUsuarioId(long usuarioId);
        Task<List<RefeicaoDTO>> ObterPorPeriodo(long usuarioId, DateOnly inicio, DateOnly fim);
        Task<List<RefeicaoModeloDTO>> ObterModelosFrequentes(long usuarioId);
    }
}
