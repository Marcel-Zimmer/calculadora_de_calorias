using CalculadoraCalorias.Application.DTOs.Requests;
using CalculadoraCalorias.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CalculadoraCalorias.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController(IUsuarioAppService usuarioAppService) : ControllerBase
    {
        private readonly IUsuarioAppService _usuarioAppService = usuarioAppService;

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarUsuarioRequest requisicao)
        {
            var usuario = await _usuarioAppService.CriarUsuario(requisicao);
            return Ok(usuario);
        }
    }
}
