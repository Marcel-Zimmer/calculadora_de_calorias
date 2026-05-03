using CalculadoraCalorias.Core.Domain.Entities;
using CalculadoraCalorias.Core.Domain.Interfaces;
using CalculadoraCalorias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using CalculadoraCalorias.Core.Domain.Common;

namespace CalculadoraCalorias.Infrastructure.Repository
{
    public class RegistroAguaRepository(AppDbContext context) : RepositoryBase<RegistroAgua>(context), IRegistroAguaRepository
    {
        public async Task<List<RegistroAgua>> ObterDiariosPorUsuarioId(long usuarioId, DateOnly? data = null)
        {
            var dataFiltro = data ?? FusoHorario.ObterDataHojeBrasilia();

            return await _dbSet
                .AsNoTracking()
                .Where(x => x.UsuarioId == usuarioId && x.Data == dataFiltro)
                .OrderBy(x => x.Hora)
                .ToListAsync();
        }

        public async Task<List<RegistroAgua>> ObterPorPeriodo(long usuarioId, DateOnly inicio, DateOnly fim)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(x => x.UsuarioId == usuarioId && x.Data >= inicio && x.Data <= fim)
                .OrderBy(x => x.Data)
                .ThenBy(x => x.Hora)
                .ToListAsync();
        }
    }
}
