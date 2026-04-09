using CalculadoraCalorias.Core.Domain.Entities;
using System.Security.Claims;

namespace CalculadoraCalorias.Core.Domain.Interfaces
{
    public interface IRefreshTokenService 
    {
        Task<RefreshToken> Adicionar(RefreshToken refreshToken);
        Task<RefreshToken?> ObterParaAtualizar(string refreshToken, long usuarioId);
    }
}
