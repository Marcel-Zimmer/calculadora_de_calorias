using CalculadoraCalorias.Application.DTOs.Requests;
using CalculadoraCalorias.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CalculadoraCalorias.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController(IUsuarioAppService usuarioAppService) : ApiBaseController
    {
        private readonly IUsuarioAppService _usuarioAppService = usuarioAppService;

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarUsuarioRequest requisicao)
        {
            return ProcessarResultado(await _usuarioAppService.CriarUsuario(requisicao));
        }
    }
}
