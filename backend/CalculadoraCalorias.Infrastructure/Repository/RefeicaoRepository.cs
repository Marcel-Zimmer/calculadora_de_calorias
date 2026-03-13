using CalculadoraCalorias.Core.Domain.Entities;
using CalculadoraCalorias.Core.Domain.Interfaces;
using CalculadoraCalorias.Core.Domain.InternalDTO;
using CalculadoraCalorias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CalculadoraCalorias.Infrastructure.Repository
{
    public class RefeicaoRepository(AppDbContext context) : RepositoryBase<Refeicao>(context), IRefeicaoRepository
    {
        public async Task<List<RefeicaoDTO>> ObterDiariasPorUsuarioId(long usuarioId)
        {
            var dataHoje = DateOnly.FromDateTime(DateTime.Now);

            return await _dbSet
                .AsNoTracking()
                .Where(x => x.UsuarioId == usuarioId && x.Data == dataHoje)
                .Select(x => new RefeicaoDTO{
                    Id = x.Id,
                    Calorias = x.Calorias,
                    TipoRefeicao = x.Tipo
                })
                .ToListAsync();
        }
    }
}
