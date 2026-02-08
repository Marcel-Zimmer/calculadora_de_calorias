using CalculadoraCalorias.Application.DTOs.Requests;
using CalculadoraCalorias.Application.DTOs.Responses;
using CalculadoraCalorias.Core.Domain.Common;
using CalculadoraCalorias.Core.Domain.Entities;

namespace CalculadoraCalorias.Application.Interfaces
{
    public interface IPerfilBiometricoAppService
    {
        public Task<Resultado<CriarPerfilBiometricoResponse>> Criar(CriarPerfilBiometricoRequest requisicao);
    }
}
