using CalculadoraCalorias.Core.Domain.Entities;
using CalculadoraCalorias.Core.Domain.Interfaces;
using CalculadoraCalorias.Core.Domain.Enums;
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
                    Apelido = x.Apelido,
                    Alimento = x.Alimento,
                    Calorias = (int?)x.Calorias,
                    TipoRefeicao = x.Tipo,
                    Peso = x.Peso,
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
                    Apelido = x.Apelido,
                    Alimento = x.Alimento,
                    Calorias = (int?)x.Calorias,
                    TipoRefeicao = x.Tipo,
                    Peso = x.Peso,
                    Data = x.Data,
                    Proteinas = (double?)x.Proteinas,
                    Carboidratos = (double?)x.Carboidratos,
                    Gorduras = (double?)x.Gorduras,
                    Acucares = (double?)x.Acucares,
                    Fibras = (double?)x.Fibras
                })
                .ToListAsync();
        }

        public async Task<List<RefeicaoModeloDTO>> ObterModelosFrequentes(long usuarioId, TipoRefeicaoEnum? tipo = null)
        {
            var query = _dbSet
                .AsNoTracking()
                .Where(x => x.UsuarioId == usuarioId && x.Apelido != null && x.Calorias != null);

            if (tipo.HasValue)
            {
                query = query.Where(x => x.Tipo == tipo.Value);
            }

            var agrupado = await query
                .GroupBy(x => x.Apelido)
                .Select(g => new
                {
                    Apelido = g.Key,
                    Count = g.Count(),
                    UltimaRefeicao = g.OrderByDescending(x => x.Data).ThenByDescending(x => x.Id).FirstOrDefault(),
                    UltimaData = g.Max(x => x.Data),
                    UltimoId = g.Max(x => x.Id)
                })
                .OrderByDescending(x => x.Count)
                .ThenByDescending(x => x.UltimaData)
                .ThenByDescending(x => x.UltimoId)
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
