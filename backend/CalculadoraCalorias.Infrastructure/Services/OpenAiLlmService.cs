using CalculadoraCalorias.Application.DTOs.Requests;
using CalculadoraCalorias.Core.Domain.ExcecoesPersonalizadas;
using CalculadoraCalorias.Core.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CalculadoraCalorias.Infrastructure.Services
{
    public class OpenAiLlmService : ILlmService
    {
        private const string EndpointChatCompletions = "https://api.openai.com/v1/chat/completions";

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private static readonly object JsonSchemaEstimativa = new
        {
            type = "object",
            additionalProperties = false,
            properties = new
            {
                itens = new
                {
                    type = "array",
                    items = new
                    {
                        type = "object",
                        additionalProperties = false,
                        properties = new
                        {
                            nome = new { type = "string" },
                            peso_estimado_g = new { type = "number" },
                            calorias = new { type = "number" },
                            proteinas = new { type = "number" },
                            carboidratos = new { type = "number" },
                            gorduras = new { type = "number" }
                        },
                        required = new[] { "nome", "peso_estimado_g", "calorias", "proteinas", "carboidratos", "gorduras" }
                    }
                },
                alimento = new { type = "string" },
                calorias = new { type = "number" },
                proteinas = new { type = "number" },
                carboidratos = new { type = "number" },
                gorduras = new { type = "number" },
                fibras = new { type = "number" },
                acucares = new { type = "number" }
            },
            required = new[] { "itens", "alimento", "calorias", "proteinas", "carboidratos", "gorduras", "fibras", "acucares" }
        };

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<OpenAiLlmService> _logger;

        public OpenAiLlmService(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<OpenAiLlmService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<EstimativaFeitaPorLlmDTO?> SimularCaloriasRefeicao(
            byte[] imagemBytes,
            double pesoEmGramas,
            string mimeType = "image/jpeg",
            CancellationToken cancellationToken = default)
        {
            var apiKey = _configuration["OpenAI:ApiKey"];
            if (string.IsNullOrWhiteSpace(apiKey) || apiKey.StartsWith("INSIRA_SUA_CHAVE", StringComparison.OrdinalIgnoreCase))
            {
                apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            }

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new InvalidOperationException("A chave de API da OpenAI não foi configurada (OpenAI:ApiKey ou OPENAI_API_KEY).");
            }

            var model = _configuration["OpenAI:Model"];
            if (string.IsNullOrWhiteSpace(model)) model = "gpt-6.1-sol";

            var reasoningEffort = _configuration["OpenAI:ReasoningEffort"];
            if (string.IsNullOrWhiteSpace(reasoningEffort)) reasoningEffort = "high";

            var imagemBase64 = Convert.ToBase64String(imagemBytes);
            var dataUrl = $"data:{mimeType};base64,{imagemBase64}";

            var payload = new
            {
                model,
                reasoning_effort = reasoningEffort,
                messages = new object[]
                {
                    new
                    {
                        role = "user",
                        content = new object[]
                        {
                            new { type = "text", text = ObterPromptEstimativaCalorias(pesoEmGramas) },
                            new
                            {
                                type = "image_url",
                                image_url = new { url = dataUrl, detail = "high" }
                            }
                        }
                    }
                },
                response_format = new
                {
                    type = "json_schema",
                    json_schema = new
                    {
                        name = "estimativa_refeicao",
                        strict = true,
                        schema = JsonSchemaEstimativa
                    }
                }
            };

            using var request = new HttpRequestMessage(HttpMethod.Post, EndpointChatCompletions);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            request.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            using var response = await _httpClient.SendAsync(request, cancellationToken);
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var ehTransitorio = response.StatusCode == HttpStatusCode.TooManyRequests
                    || response.StatusCode == HttpStatusCode.RequestTimeout
                    || (int)response.StatusCode >= 500;

                var trechoErro = responseBody.Length > 400 ? responseBody[..400] : responseBody;
                throw new LlmApiException(
                    $"Erro na API da OpenAI (HTTP {(int)response.StatusCode}): {trechoErro}",
                    response.StatusCode,
                    ehTransitorio);
            }

            using var doc = JsonDocument.Parse(responseBody);
            var root = doc.RootElement;

            if (root.TryGetProperty("usage", out var usage))
            {
                var promptTokens = usage.TryGetProperty("prompt_tokens", out var pt) ? pt.GetInt32() : 0;
                var completionTokens = usage.TryGetProperty("completion_tokens", out var ct) ? ct.GetInt32() : 0;
                _logger.LogInformation(
                    "OpenAI ({Model}/{Effort}) respondeu com sucesso. Tokens: in={InputTokens}, out={OutputTokens}",
                    model, reasoningEffort, promptTokens, completionTokens);
            }

            if (!root.TryGetProperty("choices", out var choices) || choices.GetArrayLength() == 0)
            {
                return null;
            }

            var contentText = choices[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            if (string.IsNullOrWhiteSpace(contentText))
            {
                return null;
            }

            return JsonSerializer.Deserialize<EstimativaFeitaPorLlmDTO>(contentText, JsonOptions);
        }

        private static string ObterPromptEstimativaCalorias(double pesoEmGramas)
        {
            var pesoFormatado = pesoEmGramas.ToString("0.0", CultureInfo.InvariantCulture);

            return $"""
                Você é um nutricionista especialista em análise visual de refeições.
                Analise a imagem deste prato. O peso líquido total dos alimentos exibidos (desconsiderando o prato/recipiente) é exatamente {pesoFormatado} gramas.

                Regras obrigatórias:
                1. Identifique cada alimento visível no prato (incluindo molhos, azeite/óleo aparente, queijos e acompanhamentos).
                2. Distribua os {pesoFormatado}g proporcionalmente entre os itens identificados no campo "itens", de modo que a soma de "peso_estimado_g" seja igual a {pesoFormatado}g.
                3. Utilize valores nutricionais de referência (USDA/TACO) para alimentos prontos para consumo / cozidos, considerando o modo de preparo aparente (grelhado, frito, refogado, cozido, empanado, com molho).
                4. Calcule os totais somando os valores de todos os itens. Garanta coerência calórica aproximada (4 * proteinas + 4 * carboidratos + 9 * gorduras ~= calorias) e que fibras e acucares não ultrapassem os carboidratos totais.
                5. No campo "alimento", escreva um resumo curto dos itens identificados em português.
                6. Se não houver comida na imagem, retorne "itens" vazio, "alimento": "Nenhum alimento identificado" e todos os valores numéricos zerados.
                """;
        }
    }
}
