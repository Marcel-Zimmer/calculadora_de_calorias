using CalculadoraCalorias.Core.Domain.ExcecoesPersonalizadas;
using System.Net;

namespace CalculadoraCalorias.Api.Middlewares
{
    public class ExecaoMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await ResponderComExcecao(context, ex);
            }
        }
        private static Task ResponderComExcecao(HttpContext context, Exception exception)
        {
            var code = exception switch
            {
                InformacaoDuplicada => HttpStatusCode.Conflict,  // 409
                ArgumentException => HttpStatusCode.BadRequest,  // 400
                _ => HttpStatusCode.InternalServerError          // 500
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            var response = new
            {
                mensagem = exception.Message,
                statusCode = context.Response.StatusCode
            };

            return context.Response.WriteAsJsonAsync(response);
        }
    }
}

