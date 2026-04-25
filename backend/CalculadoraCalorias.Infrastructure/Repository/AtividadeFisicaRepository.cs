using CalculadoraCalorias.Core.Domain.Common;
using CalculadoraCalorias.Core.Domain.Entities;
using CalculadoraCalorias.Core.Domain.Enums;
using CalculadoraCalorias.Core.Domain.Interfaces;
using CalculadoraCalorias.Core.Domain.InternalDTO;
using CalculadoraCalorias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CalculadoraCalorias.Infrastructure.Repository
{
    public class AtividadeFisicaRepository(AppDbContext context) : RepositoryBase<AtividadeFisica>(context), IAtividadeFisicaRepository
    {
        public async Task<List<ExercicioDTO>> ObterDiariasPorUsuarioId(long usuarioId, DateOnly? data = null)
        {
            var dataFiltro = data ?? FusoHorario.ObterDataHojeBrasilia();
            return await _dbSet
                .AsNoTracking()
                .Where(x => x.UsuarioId == usuarioId && x.DataExercicio == dataFiltro)
                .Select(x => new ExercicioDTO
                {
                    Id = x.Id,
                    TipoExercicio = (TipoExercicioEnum)x.TipoAtividadeId,
                    CaloriasEstimadas = (int)x.CaloriasEstimadas,
                    TempoDeExercicio = x.TempoExercicio,
                    Data = x.DataExercicio
                })
                .ToListAsync();
                
        }

        public async Task<List<ExercicioDTO>> ObterPorPeriodo(long usuarioId, DateOnly inicio, DateOnly fim)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(x => x.UsuarioId == usuarioId && x.DataExercicio >= inicio && x.DataExercicio <= fim)
                .Select(x => new ExercicioDTO
                {
                    Id = x.Id,
                    TipoExercicio = (TipoExercicioEnum)x.TipoAtividadeId,
                    CaloriasEstimadas = (int)x.CaloriasEstimadas,
                    TempoDeExercicio = x.TempoExercicio,
                    Data = x.DataExercicio
                })
                .ToListAsync();
        }

        public async Task<List<AtividadeFisica>> ObterTodosPorId(int idUsuario)
        {
            return await _dbSet
                .Where(a => a.UsuarioId == idUsuario)
                .ToListAsync();

        }
    }
}
