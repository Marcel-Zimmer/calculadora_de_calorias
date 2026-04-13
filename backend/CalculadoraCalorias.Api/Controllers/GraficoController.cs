using CalculadoraCalorias.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CalculadoraCalorias.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GraficoController : ApiBaseController
    {
        private readonly IGraficoAppService _graficoAppService;

        public GraficoController(IGraficoAppService graficoAppService)
        {
            _graficoAppService = graficoAppService;
        }

        [HttpGet]
        [Route("dashboard-diario/{usuarioId}")]
        public async Task<IActionResult> GraficoDiario(long usuarioId)
        {
            return ProcessarResultado(await _graficoAppService.GraficoDiario(usuarioId));
        }

        [HttpGet]
        [Route("dashboard-semanal/{usuarioId}")]
        public async Task<IActionResult> GraficoSemanal(long usuarioId)
        {
            return ProcessarResultado(await _graficoAppService.GraficoSemanal(usuarioId));
        }

        [HttpGet]
        [Route("dashboard-mensal/{usuarioId}")]
        public async Task<IActionResult> GraficoMensal(long usuarioId)
        {
            return ProcessarResultado(await _graficoAppService.GraficoMensal(usuarioId));
        }

        [HttpGet]
        [Route("estatisticas-semanais/{usuarioId}")]
        public async Task<IActionResult> EstatisticasSemanais(long usuarioId)
        {
            return ProcessarResultado(await _graficoAppService.EstatisticasSemanais(usuarioId));
        }

        [HttpGet]
        [Route("estatisticas-mensais/{usuarioId}")]
        public async Task<IActionResult> EstatisticasMensais(long usuarioId)
        {
            return ProcessarResultado(await _graficoAppService.EstatisticasMensais(usuarioId));
        }
    }
}
