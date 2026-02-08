using CalculadoraCalorias.Application.DTOs.Requests;
using CalculadoraCalorias.Application.DTOs.Responses;
using CalculadoraCalorias.Core.Domain.Common;
using CalculadoraCalorias.Core.Domain.Entities;

namespace CalculadoraCalorias.Application.Interfaces
{
    public interface IUsuarioAppService
    {
        public Task<Resultado<CriarUsuarioResponse>> Adicionar(CriarUsuarioRequest requisicao);
    }
}
