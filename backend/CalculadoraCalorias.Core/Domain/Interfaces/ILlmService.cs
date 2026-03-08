using CalculadoraCalorias.Application.DTOs.Requests;

namespace CalculadoraCalorias.Core.Domain.Interfaces
{
    public interface ILlmService
    {
        Task<EstimativaFeitaPorLLM?> SimularCaloriasRefeicao(byte[] imagemBase64, int pesoEmGramas);

    }
}
