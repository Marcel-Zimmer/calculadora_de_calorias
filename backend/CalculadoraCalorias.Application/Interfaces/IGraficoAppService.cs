using CalculadoraCalorias.Application.DTOs.Requests;
using CalculadoraCalorias.Application.DTOs.Responses;
using CalculadoraCalorias.Core.Domain.Common;
using CalculadoraCalorias.Core.Domain.Entities;

namespace CalculadoraCalorias.Application.Interfaces
{
    public interface IGraficoAppService
    {
        Task<Resultado<RefeicaoGraficoDiarioResponse>> GraficoDiario(long usuarioId);
        Task<Resultado<object>> GraficoMensal(long usuarioId);
        Task<Resultado<object>> GraficoSemanal(long usuarioId);
    }
}
