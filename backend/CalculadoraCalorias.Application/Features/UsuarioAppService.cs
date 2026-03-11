
using CalculadoraCalorias.Application.DTOs.Requests;
using CalculadoraCalorias.Application.DTOs.Responses;
using CalculadoraCalorias.Application.Interfaces;
using CalculadoraCalorias.Application.Mapping;
using CalculadoraCalorias.Core.Domain.Common;
using CalculadoraCalorias.Core.Domain.Entities;
using CalculadoraCalorias.Core.Domain.Interfaces;

namespace CalculadoraCalorias.Application.Features
{
    public class UsuarioAppService(IUsuarioService usuarioService, UsuarioMapper usuarioMapper, IUnitOfWork unitOfWork) : IUsuarioAppService
    {
        private readonly IUsuarioService _usuarioService = usuarioService;
        private readonly UsuarioMapper _mapperUsuario = usuarioMapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<Resultado<CriarUsuarioResponse>> Adicionar(CriarUsuarioRequest requisicao)
        {
            if (await _usuarioService.VerificarSeEmailEstaEmUso(requisicao.Email))
            {
                return Resultado<CriarUsuarioResponse>.Failure(TipoDeErro.Conflict, "Email informado já está em uso");
            }

            var senhaHash = BCrypt.Net.BCrypt.HashPassword(requisicao.Senha);
            var usuario = await _usuarioService.CriarUsuario(requisicao.Nome, requisicao.Email, senhaHash, requisicao.Role);

            await _unitOfWork.CommitAsync();
            return Resultado<CriarUsuarioResponse>.Success(_mapperUsuario.CriarUsuarioParaRespose(usuario));
        }

        public async Task<Resultado<LoginUsarioResponse>> Login(LoginUsuarioRequest requisicao)
        {
            var usuario = await _usuarioService.ObterPorEmail(requisicao.Email);
            if (usuario == null) {
                return Resultado<LoginUsarioResponse>.Failure(TipoDeErro.Unauthorized, "login ou senha incorreto");
            }
            var senhasIguais = BCrypt.Net.BCrypt.Verify(requisicao.Senha, usuario.Senha);
            if (!senhasIguais) {
                return Resultado<LoginUsarioResponse>.Failure(TipoDeErro.Unauthorized, "login ou senha incorreto");
            }

            return Resultado<LoginUsarioResponse>.Success(_mapperUsuario.LoginUsuarioParaResponse(usuario));

        }
    }
}
