using CalculadoraCalorias.Application.DTOs.Requests;
using CalculadoraCalorias.Application.DTOs.Responses;
using CalculadoraCalorias.Core.Domain.Common;
using CalculadoraCalorias.Core.Domain.Interfaces;

namespace CalculadoraCalorias.Application.Interfaces
{
    public interface IUsuarioAppService
    {
        public Task<Resultado<CriarUsuarioResponse>> Registrar(RegistroUsuarioRequest requisicao);
        Task<Resultado<LoginUsarioResponse>> Login(LoginUsuarioRequest requisicao);
        Task<Resultado<TokenResponse>> RefreshToken(string accessToken, string refreshToken);
    }
}
