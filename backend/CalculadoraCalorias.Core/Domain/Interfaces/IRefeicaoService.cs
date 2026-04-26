using CalculadoraCalorias.Application.DTOs.Requests;
using CalculadoraCalorias.Core.Domain.Entities;
using CalculadoraCalorias.Core.Domain.Enums;
using CalculadoraCalorias.Core.Domain.InternalDTO;

namespace CalculadoraCalorias.Core.Domain.Interfaces
{
    public interface IRefeicaoService
    {
        public Task<Refeicao> Adicionar(long usuarioId, string? apelido, double pesoEmGramas, TipoRefeicaoEnum tipo, DateOnly data, Guid guidArquivo);
        Task<Refeicao> AdicionarManual(long usuarioId, string? apelido, string alimento, int calorias, double proteinas, double carboidratos, double gorduras, double acucares, double fibras, double pesoEmGramas, TipoRefeicaoEnum tipo, DateOnly data);
        Task<Refeicao?> AdicionarBaseadoEmModelo(long usuarioId, long modeloId, double pesoEmGramas, TipoRefeicaoEnum tipo, DateOnly data);
        Task<List<RefeicaoDTO>> ObterDiariasPorUsuarioId(long usuarioId, DateOnly? data = null);
        Task<List<RefeicaoDTO>> ObterPorPeriodo(long usuarioId, DateOnly inicio, DateOnly fim);
        Task<List<RefeicaoModeloDTO>> ObterModelosFrequentes(long usuarioId);
        Task<bool> Excluir(long id);
    }
}
