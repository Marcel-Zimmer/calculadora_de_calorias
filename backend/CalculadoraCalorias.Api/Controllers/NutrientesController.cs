using CalculadoraCalorias.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CalculadoraCalorias.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NutrientesController : ApiBaseController
    {
        private readonly INutrientesAppService _nutrientesAppService;

        public NutrientesController(INutrientesAppService nutrientesAppService)
        {
            _nutrientesAppService = nutrientesAppService;
        }

        [HttpGet]
        [Route("diario")]
        public async Task<IActionResult> ObterNutrientesDiario([FromQuery] System.DateTime? data = null)
        {
            return ProcessarResultado(await _nutrientesAppService.ObterNutrientesDiario(data));
        }

        [HttpGet]
        [Route("semanal")]
        public async Task<IActionResult> ObterNutrientesSemanal([FromQuery] System.DateTime? data = null)
        {
            return ProcessarResultado(await _nutrientesAppService.ObterNutrientesSemanal(data));
        }

        [HttpGet]
        [Route("mensal")]
        public async Task<IActionResult> ObterNutrientesMensal([FromQuery] System.DateTime? data = null)
        {
            return ProcessarResultado(await _nutrientesAppService.ObterNutrientesMensal(data));
        }
    }
}
