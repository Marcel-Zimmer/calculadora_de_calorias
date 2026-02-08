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
        [Route("/simular")]
        public async Task<IActionResult> SimularAtividade([FromBody] SimularGastoCaloricoRequest requisicao)
        {
            return ProcessarResultado(await _atividadeFisicaAppService.Simular(requisicao));
        }

        [HttpPost]
        public async Task<IActionResult> Adicionar([FromBody] SimularGastoCaloricoRequest requisicao)
        {
            return ProcessarResultado(await _atividadeFisicaAppService.Adicionar(requisicao));
        }
    }
}
