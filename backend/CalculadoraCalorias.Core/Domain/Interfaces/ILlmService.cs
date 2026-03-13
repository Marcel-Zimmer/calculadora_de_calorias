using CalculadoraCalorias.Application.DTOs.Requests;

namespace CalculadoraCalorias.Core.Domain.Interfaces
{
    public interface ILlmService
    {
        Task<EstimativaFeitaPorLlmDTO?> SimularCaloriasRefeicao(byte[] imagemBase64, int pesoEmGramas);

    }
}
