using CalculadoraCalorias.Core.Domain.Entities;

namespace CalculadoraCalorias.Core.Domain.Interfaces
{
    public interface IRegistroAguaService
    {
        Task<RegistroAgua> Adicionar(long usuarioId, int quantidadeMl, DateOnly data, TimeOnly hora);
        Task<List<RegistroAgua>> ObterPorPeriodo(long usuarioId, DateOnly inicio, DateOnly fim);
        Task<List<RegistroAgua>> ObterDiariosPorUsuarioId(long usuarioId, DateOnly? data = null);
        Task<bool> Excluir(long id);
    }
}
