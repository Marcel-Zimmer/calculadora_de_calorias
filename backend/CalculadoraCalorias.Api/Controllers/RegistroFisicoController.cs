using CalculadoraCalorias.Application.DTOs.Requests;
using CalculadoraCalorias.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CalculadoraCalorias.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistroFisicoController(IRegistroFisicoAppService registroFisicoAppService) : ControllerBase
    {
        private readonly IRegistroFisicoAppService _registroFisicoAppService = registroFisicoAppService;

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarRegistroFisicoRequest requisicao)
        {
            var perfil = await _registroFisicoAppService.Criar(requisicao);
            return Ok(perfil);
        }
    }
}
