using CalculadoraCalorias.Application.DTOs.Requests;
using CalculadoraCalorias.Application.DTOs.Responses;
using CalculadoraCalorias.Core.Domain.Common;
using CalculadoraCalorias.Core.Domain.Entities;

namespace CalculadoraCalorias.Application.Interfaces
{
    public interface IGraficoAppService
    {
        Task<Resultado<RefeicaoGraficoDiarioResponse>> GraficoDiario(long usuarioId, DateOnly? data = null);
        Task<Resultado<GraficoPeriodoResponse>> GraficoMensal(long usuarioId, DateOnly? data = null);
        Task<Resultado<GraficoPeriodoResponse>> GraficoSemanal(long usuarioId, DateOnly? data = null);
        Task<Resultado<EstatisticasDetalhadasResponse>> EstatisticasSemanais(long usuarioId, DateOnly? data = null);
        Task<Resultado<EstatisticasDetalhadasResponse>> EstatisticasMensais(long usuarioId, DateOnly? data = null);
    }
}
