using CalculadoraCalorias.Core.Domain.Entities;
using CalculadoraCalorias.Core.Domain.Interfaces;
using CalculadoraCalorias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CalculadoraCalorias.Infrastructure.Repository
{
    public class PerfilBiometricoRepository(AppDbContext context) : RepositoryBase<PerfilBiometrico>(context), IPerfilBiometricoRepository
    {
        public async Task<PerfilBiometrico?> ObterPorIdUsuario(long codigoUsuario)
        {
            return await _dbSet
                 .Where(x => x.UsuarioId == codigoUsuario)
                 .FirstOrDefaultAsync();
        }
    }
}
