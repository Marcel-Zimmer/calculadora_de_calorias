using System.Net;

namespace CalculadoraCalorias.Core.Domain.ExcecoesPersonalizadas
{
    public class LlmApiException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public bool EhTransitorio { get; }

        public LlmApiException(string message, HttpStatusCode statusCode, bool ehTransitorio)
            : base(message)
        {
            StatusCode = statusCode;
            EhTransitorio = ehTransitorio;
        }
    }
}
