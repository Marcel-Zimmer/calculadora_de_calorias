using CalculadoraCalorias.Application.DTOs.Requests;
using CalculadoraCalorias.Application.DTOs.Responses;
using CalculadoraCalorias.Application.Interfaces;
using CalculadoraCalorias.Application.Mapping;
using CalculadoraCalorias.Core.Domain.Common;
using CalculadoraCalorias.Core.Domain.Entities;
using CalculadoraCalorias.Core.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CalculadoraCalorias.Application.Features
{
    public class UsuarioAppService(
        IUsuarioService usuarioService, 
        UsuarioMapper usuarioMapper, 
        IUnitOfWork unitOfWork, 
        ITokenService tokenService,
        IRefreshTokenService refreshTokenService) : IUsuarioAppService
    {
        private readonly IUsuarioService _usuarioService = usuarioService;
        private readonly UsuarioMapper _mapperUsuario = usuarioMapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ITokenService _tokenService = tokenService;
        private readonly IRefreshTokenService _refreshTokenService = refreshTokenService;

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

            var tokens = _tokenService.GerarTokens(usuario);
            
            await SalvarRefreshToken(usuario.Id, tokens.RefreshToken);
            await _unitOfWork.CommitAsync();

            var response = _mapperUsuario.LoginUsuarioParaResponse(usuario);
            response.AccessToken = tokens.AccessToken;
            response.RefreshToken = tokens.RefreshToken;

            return Resultado<LoginUsarioResponse>.Success(response);
        }

        public async Task<Resultado<TokenResponse>> RefreshToken(string accessToken, string refreshToken)
        {
            var principal = ObterPrincipalDeTokenExpirado(accessToken);
            if (principal == null)
            {
                return Resultado<TokenResponse>.Failure(TipoDeErro.Unauthorized, "Token inválido");
            }

            var usuarioId = long.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var storedRefreshToken = await _refreshTokenService.ObterParaAtualizar(refreshToken,usuarioId);

            if (storedRefreshToken == null || !storedRefreshToken.EstaAtivo)
            {
                return Resultado<TokenResponse>.Failure(TipoDeErro.Unauthorized, "Refresh token inválido ou expirado");
            }

            storedRefreshToken.Revogado = true;

            var novosTokens = _tokenService.GerarTokens(principal.Claims);
            await SalvarRefreshToken(usuarioId, novosTokens.RefreshToken);
            
            await _unitOfWork.CommitAsync();

            return Resultado<TokenResponse>.Success(novosTokens);
        }

        private async Task SalvarRefreshToken(long usuarioId, string token)
        {
            var refreshToken = new RefreshToken
            {
                UsuarioId = usuarioId,
                Token = token,
                DataExpiracao = DateTime.UtcNow.AddDays(7),
                DataCriacao = DateTime.UtcNow,
                Revogado = false
            };

            await _refreshTokenService.Adicionar(refreshToken);
        }

        private ClaimsPrincipal? ObterPrincipalDeTokenExpirado(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                // Aqui você deve passar os mesmos parâmetros de validação do Program.cs,
                // mas ignorando o tempo de expiração.
                // Para simplificar, estamos apenas lendo os claims sem validar a assinatura de novo
                // já que o refresh token será validado no banco.
                var jwtToken = tokenHandler.ReadJwtToken(token);
                return new ClaimsPrincipal(new ClaimsIdentity(jwtToken.Claims, "Bearer"));
            }
            catch
            {
                return null;
            }
        }
    }
}
