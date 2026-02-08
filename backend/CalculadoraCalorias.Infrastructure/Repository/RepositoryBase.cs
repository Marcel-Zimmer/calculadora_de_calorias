using CalculadoraCalorias.Infrastructure.Data;
using CalculadoraCalorias.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CalculadoraCalorias.Infrastructure.Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public RepositoryBase(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T> Adicionar(T entidade)
        {
            await _dbSet.AddAsync(entidade);
            return entidade;
        }

        public async Task AdicionarLote(IEnumerable<T> entidades)
        {
            await _dbSet.AddRangeAsync(entidades);
        }

        public async Task<T?> ObterPorId(long id)
        {
            return await _dbSet.FindAsync(id);
        }
    }
}
