using CalculadoraCalorias.Core.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace CalculadoraCalorias.Api.Controllers
{
    public class ApiBaseController : ControllerBase
    {
        protected IActionResult ProcessarResultado<T>(Resultado<T> result)
        {
            if (result.Sucesso)
            {
                return Ok(result.Data);
            }

            return ConverterErroParaHttp(result);
        }

        protected IActionResult ProcessarResultado(Resultado result)
        {
            if (result.Sucesso)
            {
                return NoContent();
            }

            return ConverterErroParaHttp(result);
        }

        private IActionResult ConverterErroParaHttp(Resultado result)
        {
            return result.TipoErro switch
            {
                TipoDeErro.NotFound => NotFound(result.MensagemErro),
                TipoDeErro.Validation => BadRequest(result.MensagemErro),
                TipoDeErro.Unauthorized => Unauthorized(result.MensagemErro),
                TipoDeErro.Conflict => Conflict(result.MensagemErro),
                _ => StatusCode(500, "Erro desconhecido")
            };
        }
    }
}
