using CalculadoraCalorias.Core.Domain.Entities;

namespace CalculadoraCalorias.Core.Domain.Interfaces
{
    public interface IRegistroFisicoRepository : IRepositoryBase<RegistroFisico>
    {
        Task<RegistroFisico?> ObterPorIdUsuario(long idUsuario);
        Task<List<RegistroFisico>> ObterHistoricoPorUsuario(long idUsuario);
    }
}
