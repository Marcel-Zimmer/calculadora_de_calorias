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
        [Route("diario/{usuarioId}")]
        public async Task<IActionResult> ObterNutrientesDiario(long usuarioId)
        {
            return ProcessarResultado(await _nutrientesAppService.ObterNutrientesDiario(usuarioId));
        }

        [HttpGet]
        [Route("semanal/{usuarioId}")]
        public async Task<IActionResult> ObterNutrientesSemanal(long usuarioId)
        {
            return ProcessarResultado(await _nutrientesAppService.ObterNutrientesSemanal(usuarioId));
        }

        [HttpGet]
        [Route("mensal/{usuarioId}")]
        public async Task<IActionResult> ObterNutrientesMensal(long usuarioId)
        {
            return ProcessarResultado(await _nutrientesAppService.ObterNutrientesMensal(usuarioId));
        }
    }
}
