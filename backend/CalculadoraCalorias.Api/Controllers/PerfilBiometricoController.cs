using CalculadoraCalorias.Application.DTOs.Requests;
using CalculadoraCalorias.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CalculadoraCalorias.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerfilBiometricoController(IPerfilBiometricoAppService perfilBiometricoAppService) : ApiBaseController
    {
        private readonly IPerfilBiometricoAppService _perfilBiometricoAppService = perfilBiometricoAppService;

        [HttpPost]
        public async Task<IActionResult> Adicionar([FromBody] CriarPerfilBiometricoRequest requisicao)
        {
            return ProcessarResultado(await _perfilBiometricoAppService.Adicionar(requisicao));
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> ObterPorUsuarioId(long usuarioId)
        {
            return ProcessarResultado(await _perfilBiometricoAppService.ObterPorUsuarioId(usuarioId));
        }

        [HttpPut("usuario/{usuarioId}")]
        public async Task<IActionResult> Atualizar(long usuarioId, [FromBody] CriarPerfilBiometricoRequest requisicao)
        {
            return ProcessarResultado(await _perfilBiometricoAppService.Atualizar(usuarioId, requisicao));
        }
    }
}
