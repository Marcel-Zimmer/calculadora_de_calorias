using CalculadoraCalorias.Application.DTOs.Requests;
using CalculadoraCalorias.Application.DTOs.Responses;
using CalculadoraCalorias.Application.Interfaces;
using CalculadoraCalorias.Core.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CalculadoraCalorias.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController(IUsuarioAppService usuarioAppService) : ApiBaseController
    {
        private readonly IUsuarioAppService _usuarioAppService = usuarioAppService;

        [HttpPost]
        [Route("registrar")]
        [AllowAnonymous]
        public async Task<IActionResult> Registrar([FromBody] RegistroUsuarioRequest requisicao)
        {
            return ProcessarResultado(await _usuarioAppService.Registrar(requisicao));
        }

        [HttpPost]
        [Route ("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginUsuarioRequest requisicao) { 

            return ProcessarResultado(await _usuarioAppService.Login(requisicao));
        
        }

        [HttpPost]
        [Route("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] TokenResponse tokens)
        {
            return ProcessarResultado(await _usuarioAppService.RefreshToken(tokens.AccessToken, tokens.RefreshToken));
        }

        [HttpPut("atualizar-senha")]
        public async Task<IActionResult> AtualizarSenha([FromBody] string novaSenha)
        {
            return ProcessarResultado(await _usuarioAppService.AtualizarSenha(UsuarioId, novaSenha));
        }
    }
}
