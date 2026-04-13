using CalculadoraCalorias.Application.DTOs.Requests;
using CalculadoraCalorias.Core.Domain.Entities;
using CalculadoraCalorias.Core.Domain.Enums;
using CalculadoraCalorias.Core.Domain.InternalDTO;

namespace CalculadoraCalorias.Core.Domain.Interfaces
{
    public interface IRefeicaoService
    {
        public Task<Refeicao> Adicionar(long usuarioId, string? apelido, int pesoEmGramas, TipoRefeicaoEnum tipo, DateOnly data, Guid guidArquivo);
        Task<Refeicao?> AdicionarBaseadoEmModelo(long usuarioId, long modeloId, int pesoEmGramas, TipoRefeicaoEnum tipo, DateOnly data);
        Task<List<RefeicaoDTO>> ObterDiariasPorUsuarioId(long usuarioId);
        Task<List<RefeicaoDTO>> ObterPorPeriodo(long usuarioId, DateOnly inicio, DateOnly fim);
        Task<List<RefeicaoModeloDTO>> ObterModelosFrequentes(long usuarioId);
        Task<bool> Excluir(long id);
    }
}
