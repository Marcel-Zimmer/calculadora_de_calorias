using CalculadoraCalorias.Core.Domain.Interfaces;

namespace CalculadoraCalorias.Application.Features
{
    public abstract class AppServiceBase(IContextoHttpService contextoHttpService)
    {
        private readonly IContextoHttpService _contextoHttpService = contextoHttpService;

        protected long UsuarioId => _contextoHttpService.ObterUsuarioId();
        protected string UsuarioEmail => _contextoHttpService.ObterUsuarioEmail();
    }
}
