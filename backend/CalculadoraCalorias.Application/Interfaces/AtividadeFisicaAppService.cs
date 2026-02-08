using CalculadoraCalorias.Application.DTOs.Requests;
using CalculadoraCalorias.Application.DTOs.Responses;
using CalculadoraCalorias.Core.Domain.Common;
using CalculadoraCalorias.Core.Domain.Entities;

namespace CalculadoraCalorias.Application.Interfaces
{
    public interface IAtividadeFisicaAppService
    {
        public Task<Resultado<CriarAtividadeFisicaResponse>> Simular(SimularGastoCaloricoRequest requisicao);

        public Task<Resultado<CriarAtividadeFisicaResponse>> Adicionar(SimularGastoCaloricoRequest requisicao);
    }
}
