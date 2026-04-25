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
        public async Task<IActionResult> GraficoDiario(long usuarioId, [FromQuery] string? data = null)
        {
            DateOnly? dataFiltro = null;
            if (!string.IsNullOrEmpty(data) && DateOnly.TryParse(data, out var d))
            {
                dataFiltro = d;
            }
            return ProcessarResultado(await _graficoAppService.GraficoDiario(usuarioId, dataFiltro));
        }

        [HttpGet]
        [Route("dashboard-semanal/{usuarioId}")]
        public async Task<IActionResult> GraficoSemanal(long usuarioId, [FromQuery] string? data = null)
        {
            DateOnly? dataFiltro = null;
            if (!string.IsNullOrEmpty(data) && DateOnly.TryParse(data, out var d))
            {
                dataFiltro = d;
            }
            return ProcessarResultado(await _graficoAppService.GraficoSemanal(usuarioId, dataFiltro));
        }

        [HttpGet]
        [Route("dashboard-mensal/{usuarioId}")]
        public async Task<IActionResult> GraficoMensal(long usuarioId, [FromQuery] string? data = null)
        {
            DateOnly? dataFiltro = null;
            if (!string.IsNullOrEmpty(data) && DateOnly.TryParse(data, out var d))
            {
                dataFiltro = d;
            }
            return ProcessarResultado(await _graficoAppService.GraficoMensal(usuarioId, dataFiltro));
        }

        [HttpGet]
        [Route("estatisticas-semanais/{usuarioId}")]
        public async Task<IActionResult> EstatisticasSemanais(long usuarioId, [FromQuery] string? data = null)
        {
            DateOnly? dataFiltro = null;
            if (!string.IsNullOrEmpty(data) && DateOnly.TryParse(data, out var d))
            {
                dataFiltro = d;
            }
            return ProcessarResultado(await _graficoAppService.EstatisticasSemanais(usuarioId, dataFiltro));
        }

        [HttpGet]
        [Route("estatisticas-mensais/{usuarioId}")]
        public async Task<IActionResult> EstatisticasMensais(long usuarioId, [FromQuery] string? data = null)
        {
            DateOnly? dataFiltro = null;
            if (!string.IsNullOrEmpty(data) && DateOnly.TryParse(data, out var d))
            {
                dataFiltro = d;
            }
            return ProcessarResultado(await _graficoAppService.EstatisticasMensais(usuarioId, dataFiltro));
        }
    }
}
