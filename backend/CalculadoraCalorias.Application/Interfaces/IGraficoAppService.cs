using CalculadoraCalorias.Application.DTOs.Requests;
using CalculadoraCalorias.Application.DTOs.Responses;
using CalculadoraCalorias.Core.Domain.Common;
using CalculadoraCalorias.Core.Domain.Entities;

namespace CalculadoraCalorias.Application.Interfaces
{
    public interface IGraficoAppService
    {
        Task<Resultado<RefeicaoGraficoDiarioResponse>> GraficoDiario(DateOnly? data = null);
        Task<Resultado<GraficoPeriodoResponse>> GraficoMensal(DateOnly? data = null);
        Task<Resultado<GraficoPeriodoResponse>> GraficoSemanal(DateOnly? data = null);
        Task<Resultado<EstatisticasDetalhadasResponse>> EstatisticasSemanais(DateOnly? data = null);
        Task<Resultado<EstatisticasDetalhadasResponse>> EstatisticasMensais(DateOnly? data = null);
        Task<Resultado<EstatisticasPesoResponse>> ObterEstatisticasPeso();
    }
}
