using CalculadoraCalorias.Core.Domain.Common;
using CalculadoraCalorias.Core.Domain.Entities;
using CalculadoraCalorias.Core.Domain.InternalDTO;

namespace CalculadoraCalorias.Core.Domain.Interfaces
{
    public interface IRefreshTokenRepository : IRepositoryBase<RefreshToken>
    {
        Task<RefreshToken?> ObterParaAtualizar(string refreshToken, long usuarioId);
    }
}
