using CalculadoraCalorias.Application.DTOs.Requests;
using CalculadoraCalorias.Core.Domain.Interfaces;
using Google.GenAI;
using Google.GenAI.Types;
using System.Text.Json;

namespace CalculadoraCalorias.Core.Domain.Services
{
    public  class LlmService : ILlmService
    {
        public async Task<EstimativaFeitaPorLLM?> SimularCaloriasRefeicao(byte[] imagemBase64, int peso)
        {

            var client = new Client( );
            var config = new GenerationConfig { ResponseMimeType = "application/json" };

            var response = await client.Models.GenerateContentAsync(
                    model: "gemini-3-flash-preview",
                    contents: new List<Content>
                    {
            new Content
            {
                Parts = new List<Part>
                {
                    new Part { Text = $"Estime a quantidade de calorias com base na imagem, peso da comida: {peso}. Retorne apenas um JSON com os campos: alimento, calorias, proteinas, carboidratos e gorduras." },

                    new Part
                    {
                        InlineData = new Blob
                        {
                            MimeType = "image/jpeg",
                            Data = imagemBase64
                        }
                    }
                }
            }
                    }
                );

            var jsonResposta = response?.Candidates?[0]?.Content?.Parts?[0].Text;

            if (string.IsNullOrEmpty(jsonResposta))
            {
                return null;
            }

            if (jsonResposta.StartsWith("```"))
            {
                jsonResposta = jsonResposta.Replace("```json", "").Replace("```", "").Trim();
            }

            var opcoes = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var refeicao = JsonSerializer.Deserialize<EstimativaFeitaPorLLM>(jsonResposta, opcoes);

            return refeicao;
        }
    }
}
