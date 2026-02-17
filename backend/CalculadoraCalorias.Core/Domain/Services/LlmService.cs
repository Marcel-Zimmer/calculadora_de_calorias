using CalculadoraCalorias.Application.DTOs.Requests;
using CalculadoraCalorias.Core.Domain.Interfaces;
using Google.GenAI;
using Google.GenAI.Types;
using System.Text.Json;

namespace CalculadoraCalorias.Core.Domain.Services
{
    public  class LlmService : ILlmService
    {
        public async Task<EstimativaFeitaPorLLM?> SimularCaloriasRefeicao(byte[] imagemBase64, int pesoEmGramas)
        {

            var client = new Client(apiKey:"");

            var response = await client.Models.GenerateContentAsync(
                model: "gemini-3-flash-preview",
                contents: ObterContentsParaLLM(imagemBase64, pesoEmGramas)
            );

            var jsonResposta = response?.Candidates?[0]?.Content?.Parts?[0].Text;

            if (string.IsNullOrEmpty(jsonResposta)) return null;

            if (jsonResposta.StartsWith("```"))
            {
                jsonResposta = jsonResposta.Replace("```json", "").Replace("```", "").Trim();
            }

            var opcoes = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<EstimativaFeitaPorLLM>(jsonResposta, opcoes);

        }

        private static List<Content> ObterContentsParaLLM(byte[] imagemBase64, int pesoEmGramas)
        {
            return
                [
                    new Content
                    {
                        Parts =
                        [
                            new Part { Text = ObterPromptEstimativaCalorias(pesoEmGramas) },

                            new Part
                            {
                                InlineData = new Blob
                                {
                                    MimeType = "image/jpeg",
                                    Data = imagemBase64
                                }
                            }
                        ]
                    }
                ];
        }

        private static string ObterPromptEstimativaCalorias(int pesoEmGramas)
        {
            return $$"""
                Analise a imagem deste prato de comida que pesa exatamente {{pesoEmGramas}} gramas.

                Tarefa: Estime a composição nutricional total para este peso informado.
                Formato de Saída: Retorne estritamente um JSON (sem Markdown ou explicações) com este esquema:
                {
                    "alimento": "string contendo a descrição dos itens identificados",
                    "calorias": number,
                    "proteinas": number,
                    "carboidratos": number,
                    "gorduras": number,
                    "fibras": number,
                    "acucares": number
                }

                Regras Importantes:
                1. Use gramas (g) para macros e kcal para calorias.
                2. Retorne apenas números, sem unidades (ex: 250 em vez de '250 kcal').
                3. Se não houver comida na imagem, retorne um JSON com todos os valores zerados.
                """;
        }
    }
}
