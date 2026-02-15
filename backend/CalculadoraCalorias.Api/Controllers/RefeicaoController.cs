using CalculadoraCalorias.Application.DTOs.Requests;
using CalculadoraCalorias.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CalculadoraCalorias.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefeicaoController(IRefeicaoAppService refeicaoAppService) : ApiBaseController
    {
        private readonly IRefeicaoAppService _refeicaoAppService = refeicaoAppService;

        [HttpPost]
        [Route("simular")]
        public async Task<IActionResult> Simular([FromForm] CriarRefeicaoRequest requisicao)
        {
            return ProcessarResultado(await _refeicaoAppService.Simular(requisicao));
        }


    }
}
