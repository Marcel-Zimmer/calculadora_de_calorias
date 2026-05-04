using CalculadoraCalorias.Core.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CalculadoraCalorias.Api.Services
{
    public class ContextoHttpService : IContextoHttpService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ContextoHttpService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public long ObterUsuarioId()
        {
            var claimsPrincipal = _httpContextAccessor.HttpContext?.User;
            if (claimsPrincipal == null) return 0;

            var idClaim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);
            if (idClaim != null && long.TryParse(idClaim.Value, out var id))
            {
                return id;
            }

            return 0;
        }

        public string ObterUsuarioEmail()
        {
            var claimsPrincipal = _httpContextAccessor.HttpContext?.User;
            if (claimsPrincipal == null) return string.Empty;

            var emailClaim = claimsPrincipal.FindFirst(ClaimTypes.Email);
            return emailClaim?.Value ?? string.Empty;
        }
    }
}