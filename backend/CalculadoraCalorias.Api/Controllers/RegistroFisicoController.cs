using CalculadoraCalorias.Application.DTOs.Requests;
using CalculadoraCalorias.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CalculadoraCalorias.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistroFisicoController(IRegistroFisicoAppService registroFisicoAppService) : ApiBaseController
    {
        private readonly IRegistroFisicoAppService _registroFisicoAppService = registroFisicoAppService;


        [HttpPost]
        public async Task<IActionResult> Adicionar([FromBody] CriarRegistroFisicoRequest requisicao)
        {
            return ProcessarResultado(await _registroFisicoAppService.Adicionar(requisicao));
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> ObterUltimoPorUsuarioId(long usuarioId)
        {
            return ProcessarResultado(await _registroFisicoAppService.ObterUltimoPorUsuarioId(usuarioId));
        }

        [HttpPut("usuario/{usuarioId}")]
        public async Task<IActionResult> Atualizar(long usuarioId, [FromBody] CriarRegistroFisicoRequest requisicao)
        {
            return ProcessarResultado(await _registroFisicoAppService.Atualizar(usuarioId, requisicao));
        }
    }
}
