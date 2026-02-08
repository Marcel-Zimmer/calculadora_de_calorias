using CalculadoraCalorias.Application.DTOs.Requests;
using CalculadoraCalorias.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CalculadoraCalorias.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AtividadeFisicaController(IAtividadeFisicaAppService atividadeFisicaAppService) : ApiBaseController
    {
        private readonly IAtividadeFisicaAppService _atividadeFisicaAppService = atividadeFisicaAppService;

        [HttpPost]
        [Route("simular")]
        public async Task<IActionResult> SimularAtividade([FromBody] CriarAtividadeFisicaRequest requisicao)
        {
            return ProcessarResultado(await _atividadeFisicaAppService.Simular(requisicao));
        }

        [HttpPost]
        public async Task<IActionResult> Adicionar([FromBody] CriarAtividadeFisicaRequest requisicao)
        {
            return ProcessarResultado(await _atividadeFisicaAppService.Adicionar(requisicao));
        }
        
        [HttpGet]
        [Route("obter-todos/{idUsuario}")]
        public async Task<IActionResult> ObterTodosPorId([FromRoute] int idUsuario)
        {
            return ProcessarResultado(await _atividadeFisicaAppService.ObterTodosPorId(idUsuario));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> ObterPorId([FromRoute] int id)
        {
            return ProcessarResultado(await _atividadeFisicaAppService.ObterPorID(id));
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Excluir([FromRoute] int id)
        {
            return ProcessarResultado(await _atividadeFisicaAppService.Excluir(id));
        }

        [HttpPut]
        public async Task<IActionResult> Atualizar([FromBody] AtualizarAtividadeFisicaRequest requisicao)
        {
            return ProcessarResultado(await _atividadeFisicaAppService.Atualizar(requisicao));
        }
    }
}
