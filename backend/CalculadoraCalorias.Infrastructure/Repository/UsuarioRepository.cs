using CalculadoraCalorias.Core.Domain.Entities;
using CalculadoraCalorias.Core.Domain.Interfaces;
using CalculadoraCalorias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CalculadoraCalorias.Infrastructure.Repository
{
    public class UsuarioRepository(AppDbContext context) : RepositoryBase<Usuario>(context), IUsuarioRepository
    {
        public async Task<bool> VerficarSeEmailEstaEmUso(string email)
        {
            return await _dbSet
                .AnyAsync(x => x.Email == email);
        }
    }
}
