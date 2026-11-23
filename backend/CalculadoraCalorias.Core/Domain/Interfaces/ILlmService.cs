using CalculadoraCalorias.Application.DTOs.Requests;

namespace CalculadoraCalorias.Core.Domain.Interfaces
{
    public interface ILlmService
    {
        Task<EstimativaFeitaPorLlmDTO?> SimularCaloriasRefeicao(
            byte[] imagemBytes,
            double pesoEmGramas,
            string mimeType = "image/jpeg",
            CancellationToken cancellationToken = default);
    }
}
