using CalculadoraCalorias.Application.DTOs.Requests;
using CalculadoraCalorias.Application.DTOs.Responses;
using CalculadoraCalorias.Core.Domain.Common;
using CalculadoraCalorias.Core.Domain.Entities;

namespace CalculadoraCalorias.Application.Interfaces
{
    public interface IRegistroFisicoAppService
    {
        public Task<Resultado<CriarRegistroFisicoResponse>> Adicionar(CriarRegistroFisicoRequest requisicao);
        public Task<Resultado<CriarRegistroFisicoResponse>> ObterUltimoPorUsuarioId(long usuarioId);
        public Task<Resultado<CriarRegistroFisicoResponse>> Atualizar(long usuarioId, CriarRegistroFisicoRequest requisicao);
    }
}
