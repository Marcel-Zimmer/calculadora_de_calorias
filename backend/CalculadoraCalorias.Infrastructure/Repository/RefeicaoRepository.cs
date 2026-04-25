using CalculadoraCalorias.Core.Domain.Entities;
using CalculadoraCalorias.Core.Domain.Interfaces;
using CalculadoraCalorias.Core.Domain.InternalDTO;
using CalculadoraCalorias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using CalculadoraCalorias.Core.Domain.Common;

namespace CalculadoraCalorias.Infrastructure.Repository
{
    public class RefeicaoRepository(AppDbContext context) : RepositoryBase<Refeicao>(context), IRefeicaoRepository
    {
        public async Task<List<RefeicaoDTO>> ObterDiariasPorUsuarioId(long usuarioId, DateOnly? data = null)
        {
            var dataFiltro = data ?? FusoHorario.ObterDataHojeBrasilia();

            return await _dbSet
                .AsNoTracking()
                .Where(x => x.UsuarioId == usuarioId && x.Data == dataFiltro)
                .Select(x => new RefeicaoDTO{
                    Id = x.Id,
                    Calorias = (int?)x.Calorias,
                    TipoRefeicao = x.Tipo,
                    Data = x.Data,
                    Proteinas = (double?)x.Proteinas,
                    Carboidratos = (double?)x.Carboidratos,
                    Gorduras = (double?)x.Gorduras,
                    Acucares = (double?)x.Acucares,
                    Fibras = (double?)x.Fibras
                })
                .ToListAsync();
        }

        public async Task<List<RefeicaoDTO>> ObterPorPeriodo(long usuarioId, DateOnly inicio, DateOnly fim)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(x => x.UsuarioId == usuarioId && x.Data >= inicio && x.Data <= fim)
                .Select(x => new RefeicaoDTO{
                    Id = x.Id,
                    Calorias = (int?)x.Calorias,
                    TipoRefeicao = x.Tipo,
                    Data = x.Data,
                    Proteinas = (double?)x.Proteinas,
                    Carboidratos = (double?)x.Carboidratos,
                    Gorduras = (double?)x.Gorduras,
                    Acucares = (double?)x.Acucares,
                    Fibras = (double?)x.Fibras
                })
                .ToListAsync();
        }

        public async Task<List<RefeicaoModeloDTO>> ObterModelosFrequentes(long usuarioId)
        {
            var agrupado = await _dbSet
                .AsNoTracking()
                .Where(x => x.UsuarioId == usuarioId && x.Apelido != null && x.Calorias != null)
                .GroupBy(x => x.Apelido)
                .Select(g => new
                {
                    Apelido = g.Key,
                    Count = g.Count(),
                    UltimaRefeicao = g.OrderByDescending(x => x.Data).ThenByDescending(x => x.Id).FirstOrDefault()
                })
                .OrderByDescending(x => x.Count)
                .Take(10)
                .ToListAsync();

            return agrupado.Select(x => new RefeicaoModeloDTO
            {
                Id = x.UltimaRefeicao!.Id,
                Apelido = x.Apelido,
                Calorias = (int?)x.UltimaRefeicao.Calorias,
                Proteinas = x.UltimaRefeicao.Proteinas,
                Carboidratos = x.UltimaRefeicao.Carboidratos,
                Gorduras = x.UltimaRefeicao.Gorduras,
                PesoOriginal = x.UltimaRefeicao.Peso
            }).ToList();
        }
    }
}
