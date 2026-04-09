
using CalculadoraCalorias.Core.Domain.Entities;
using CalculadoraCalorias.Core.Domain.Interfaces;

namespace CalculadoraCalorias.Core.Domain.Services;
public class RefreshTokenService(IRefreshTokenRepository refreshTokenRepository) : IRefreshTokenService
{
    private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;

    public async Task<RefreshToken> Adicionar(RefreshToken refreshToken)
    {
        return await _refreshTokenRepository.Adicionar(refreshToken);
    }

    public async Task<RefreshToken?> ObterParaAtualizar(string refreshToken, long usuarioId)
    {
       return await _refreshTokenRepository.ObterParaAtualizar(refreshToken, usuarioId);
    }

    
}

