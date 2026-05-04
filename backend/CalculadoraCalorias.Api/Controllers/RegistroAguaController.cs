using CalculadoraCalorias.Application.DTOs.Requests;
using CalculadoraCalorias.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CalculadoraCalorias.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistroAguaController(
        IRegistroAguaAppService _registroAguaAppService) : ApiBaseController
    {
        [HttpPost]
        [Route("adicionar")]
        public async Task<IActionResult> Adicionar([FromBody] CriarRegistroAguaRequest requisicao)
        {
            return ProcessarResultado(await _registroAguaAppService.Adicionar(requisicao));
        }

        [HttpGet]
        [Route("diario")]
        public async Task<IActionResult> ObterDiario([FromQuery] DateOnly? data)
        {
            return ProcessarResultado(await _registroAguaAppService.ObterDiarios(data));
        }

        [HttpGet]
        [Route("estatisticas/semanal")]
        public async Task<IActionResult> ObterEstatisticasSemanais([FromQuery] DateOnly? data)
        {
            return ProcessarResultado(await _registroAguaAppService.ObterEstatisticasSemanais(data));
        }

        [HttpGet]
        [Route("estatisticas/mensal")]
        public async Task<IActionResult> ObterEstatisticasMensais([FromQuery] DateOnly? data)
        {
            return ProcessarResultado(await _registroAguaAppService.ObterEstatisticasMensais(data));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir([FromRoute] long id)
        {
            return ProcessarResultado(await _registroAguaAppService.Excluir(id));
        }
    }
}
