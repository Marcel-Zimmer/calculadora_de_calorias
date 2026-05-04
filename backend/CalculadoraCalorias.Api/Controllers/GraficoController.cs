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
        [Route("dashboard-diario")]
        public async Task<IActionResult> GraficoDiario([FromQuery] string? data = null)
        {
            DateOnly? dataFiltro = null;
            if (!string.IsNullOrEmpty(data) && DateOnly.TryParse(data, out var d))
            {
                dataFiltro = d;
            }
            return ProcessarResultado(await _graficoAppService.GraficoDiario(dataFiltro));
        }

        [HttpGet]
        [Route("dashboard-semanal")]
        public async Task<IActionResult> GraficoSemanal([FromQuery] string? data = null)
        {
            DateOnly? dataFiltro = null;
            if (!string.IsNullOrEmpty(data) && DateOnly.TryParse(data, out var d))
            {
                dataFiltro = d;
            }
            return ProcessarResultado(await _graficoAppService.GraficoSemanal(dataFiltro));
        }

        [HttpGet]
        [Route("dashboard-mensal")]
        public async Task<IActionResult> GraficoMensal([FromQuery] string? data = null)
        {
            DateOnly? dataFiltro = null;
            if (!string.IsNullOrEmpty(data) && DateOnly.TryParse(data, out var d))
            {
                dataFiltro = d;
            }
            return ProcessarResultado(await _graficoAppService.GraficoMensal(dataFiltro));
        }

        [HttpGet]
        [Route("estatisticas-semanais")]
        public async Task<IActionResult> EstatisticasSemanais([FromQuery] string? data = null)
        {
            DateOnly? dataFiltro = null;
            if (!string.IsNullOrEmpty(data) && DateOnly.TryParse(data, out var d))
            {
                dataFiltro = d;
            }
            return ProcessarResultado(await _graficoAppService.EstatisticasSemanais(dataFiltro));
        }

        [HttpGet]
        [Route("estatisticas-mensais")]
        public async Task<IActionResult> EstatisticasMensais([FromQuery] string? data = null)
        {
            DateOnly? dataFiltro = null;
            if (!string.IsNullOrEmpty(data) && DateOnly.TryParse(data, out var d))
            {
                dataFiltro = d;
            }
            return ProcessarResultado(await _graficoAppService.EstatisticasMensais(dataFiltro));
        }

        [HttpGet]
        [Route("estatisticas-peso")]
        public async Task<IActionResult> EstatisticasPeso()
        {
            return ProcessarResultado(await _graficoAppService.ObterEstatisticasPeso());
        }
    }
}
