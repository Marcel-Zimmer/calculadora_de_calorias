using CalculadoraCalorias.Application.DTOs.Requests;
using CalculadoraCalorias.Application.DTOs.Responses;
using CalculadoraCalorias.Core.Domain.Common;
using CalculadoraCalorias.Core.Domain.Entities;

namespace CalculadoraCalorias.Application.Interfaces
{
    public interface IGraficoAppService
    {
        Task<Resultado<RefeicaoGraficoDiarioResponse>> GraficoDiario(long usuarioId);
        Task<Resultado<GraficoPeriodoResponse>> GraficoMensal(long usuarioId);
        Task<Resultado<GraficoPeriodoResponse>> GraficoSemanal(long usuarioId);
    }
}
