using CalculadoraCalorias.Core.Domain.Entities;
using CalculadoraCalorias.Core.Domain.Interfaces;
using CalculadoraCalorias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CalculadoraCalorias.Infrastructure.Repository
{
    public class PerfilBiometricoRepository(AppDbContext context) : IPerfilBiometricoRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<PerfilBiometrico> Adicionar(PerfilBiometrico perfilBiometrico)
        {
            await _context.PerfilBiometrico.AddAsync(perfilBiometrico);
            await _context.SaveChangesAsync();
            return perfilBiometrico;
        }

        public async Task<PerfilBiometrico> ObterPorCodigoUsuario(long codigoUsuario)
        {
            return await _context.PerfilBiometrico
                .AsNoTracking()
                .Include(x => x.Usuario)
                .Where(x => x.UsuarioId == codigoUsuario)
                .FirstOrDefaultAsync();
        }
    }
}
