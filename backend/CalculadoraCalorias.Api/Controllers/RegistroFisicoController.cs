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

        [HttpGet]
        [Route("ultimo")]
        public async Task<IActionResult> ObterUltimo()
        {
            return ProcessarResultado(await _registroFisicoAppService.ObterUltimo());
        }

        [HttpPut]
        public async Task<IActionResult> Atualizar([FromBody] CriarRegistroFisicoRequest requisicao)
        {
            return ProcessarResultado(await _registroFisicoAppService.Atualizar(requisicao));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir(long id)
        {
            return ProcessarResultado(await _registroFisicoAppService.Excluir(id));
        }
    }
}
