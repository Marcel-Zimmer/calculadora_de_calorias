
using CalculadoraCalorias.Application.DTOs.Requests;
using CalculadoraCalorias.Application.DTOs.Responses;
using CalculadoraCalorias.Application.Interfaces;
using CalculadoraCalorias.Application.Mapping;
using CalculadoraCalorias.Core.Domain.Common;
using CalculadoraCalorias.Core.Domain.Entities;
using CalculadoraCalorias.Core.Domain.ExcecoesPersonalizadas;
using CalculadoraCalorias.Core.Domain.Interfaces;

namespace CalculadoraCalorias.Application.Features
{
    public class UsuarioAppService(IUsuarioService usuarioService, UsuarioMapper usuarioMapper) : IUsuarioAppService
    {
        private readonly IUsuarioService _usuarioService = usuarioService;
        private readonly UsuarioMapper _mapperUsuario = usuarioMapper;

        public async Task<Resultado<CriarUsuarioResponse>> CriarUsuario(CriarUsuarioRequest requisicao)
        {
            if (await _usuarioService.VerificarSeEmailEstaEmUso(requisicao.Email)) {
                return Resultado<CriarUsuarioResponse>.Failure(TipoDeErro.Conflict, "Email informado já está em uso");
            }

            var senhaHash = BCrypt.Net.BCrypt.HashPassword(requisicao.Senha);
            var usuario = await _usuarioService.CriarUsuario(requisicao.Nome, requisicao.Email, senhaHash, requisicao.Role);
            return Resultado<CriarUsuarioResponse>.Success(_mapperUsuario.EntidadeParaResponse(usuario));
        }
    }
}
