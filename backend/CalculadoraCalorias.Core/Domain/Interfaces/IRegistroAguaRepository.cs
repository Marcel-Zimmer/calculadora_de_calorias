namespace CalculadoraCalorias.Core.Domain.Interfaces
{
    public interface IRegistroAguaRepository : IRepositoryBase<Entities.RegistroAgua>
    {
        Task<List<Entities.RegistroAgua>> ObterPorPeriodo(long usuarioId, DateOnly inicio, DateOnly fim);
        Task<List<Entities.RegistroAgua>> ObterDiariosPorUsuarioId(long usuarioId, DateOnly? data = null);
    }
}
