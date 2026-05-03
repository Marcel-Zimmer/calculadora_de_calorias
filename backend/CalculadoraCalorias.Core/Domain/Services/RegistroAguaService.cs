using CalculadoraCalorias.Core.Domain.Entities;
using CalculadoraCalorias.Core.Domain.Interfaces;

namespace CalculadoraCalorias.Core.Domain.Services
{
    public class RegistroAguaService(IRegistroAguaRepository _repository, IUnitOfWork _uow) : IRegistroAguaService
    {
        public async Task<RegistroAgua> Adicionar(long usuarioId, int quantidadeMl, DateOnly data, TimeOnly hora)
        {
            var registro = new RegistroAgua(usuarioId, quantidadeMl, data, hora);
            await _repository.Adicionar(registro);
            await _uow.CommitAsync();
            return registro;
        }

        public async Task<List<RegistroAgua>> ObterPorPeriodo(long usuarioId, DateOnly inicio, DateOnly fim)
        {
            return await _repository.ObterPorPeriodo(usuarioId, inicio, fim);
        }

        public async Task<List<RegistroAgua>> ObterDiariosPorUsuarioId(long usuarioId, DateOnly? data = null)
        {
            return await _repository.ObterDiariosPorUsuarioId(usuarioId, data);
        }

        public async Task<bool> Excluir(long id)
        {
            var excluiu = await _repository.Excluir(id);
            if (excluiu)
            {
                await _uow.CommitAsync();
            }
            return excluiu;
        }
    }
}
