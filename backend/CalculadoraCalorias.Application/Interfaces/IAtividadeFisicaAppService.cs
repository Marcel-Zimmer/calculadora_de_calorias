using CalculadoraCalorias.Application.DTOs.Requests;
using CalculadoraCalorias.Application.DTOs.Responses;
using CalculadoraCalorias.Core.Domain.Common;
using CalculadoraCalorias.Core.Domain.Entities;

namespace CalculadoraCalorias.Application.Interfaces
{
    public interface IAtividadeFisicaAppService
    {
        public Task<Resultado<AtividadeFisicaResponse>> Simular(CriarEstimativaAtividadeFisicaRequest requisicao);
        Task<Resultado<AtividadeFisicaResponse>> EstimarGastoCalorico(CriarEstimativaAtividadeFisicaRequest requisicao);
        Task<Resultado<List<AtividadeFisicaResponse>>> ObterTodosPorId(int idUsuario);
        Task<Resultado> Excluir(int id);
        Task<Resultado<AtividadeFisicaResponse>> ObterPorID(int id);
        Task<Resultado<object>> Atualizar(AtualizarAtividadeFisicaRequest requisicao);
        Task<Resultado<AtividadeFisicaResponse>> Adicionar(CriarAtividadeFisicaRequest requisicao);
    }
}
