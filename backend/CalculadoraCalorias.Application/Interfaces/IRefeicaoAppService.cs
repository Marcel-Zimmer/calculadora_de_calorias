using CalculadoraCalorias.Application.DTOs.Requests;
using CalculadoraCalorias.Application.DTOs.Responses;
using CalculadoraCalorias.Core.Domain.Common;
using CalculadoraCalorias.Core.Domain.Entities;

namespace CalculadoraCalorias.Application.Interfaces
{
    public interface IRefeicaoAppService
    {
        public Task<Resultado<Refeicao>> Adicionar(CriarRefeicaoRequest requisicao);
        public Task<Resultado<bool>> Excluir(long id);
        public Task<Resultado<List<RefeicaoModeloResponse>>> ObterModelosFrequentes(long usuarioId);
    }
}
