using CalculadoraCalorias.Core.Domain.Common;
using CalculadoraCalorias.Core.Domain.Entities;
using CalculadoraCalorias.Core.Domain.Enums;
using CalculadoraCalorias.Core.Domain.Interfaces;
using CalculadoraCalorias.Core.Domain.InternalDTO;
using CalculadoraCalorias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CalculadoraCalorias.Infrastructure.Repository
{
    public class RefreshTokenRepository(AppDbContext context) : RepositoryBase<RefreshToken>(context), IRefreshTokenRepository
    {
        public async Task<RefreshToken?> ObterParaAtualizar(string refreshToken, long usuarioId)
        {
            return await _dbSet
                .AsTracking()
                .Where(x => x.Token == refreshToken && x.UsuarioId == usuarioId)
                .FirstOrDefaultAsync();
        }
    }
}
