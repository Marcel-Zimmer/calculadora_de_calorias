using CalculadoraCalorias.Core.Domain.Entities;

namespace CalculadoraCalorias.Core.Domain.Interfaces
{
    public interface IPerfilBiometricoRepository : IRepositoryBase<PerfilBiometrico>
    {
        Task<PerfilBiometrico?> ObterPorIdUsuario(long codigoUsuario);
    }
}
