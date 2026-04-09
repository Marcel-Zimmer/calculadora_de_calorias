using CalculadoraCalorias.Core.Domain.Entities;
using System.Security.Claims;

namespace CalculadoraCalorias.Core.Domain.Interfaces
{
    public interface ITokenService 
    {
        TokenResponse GerarTokens(Usuario usuario);
        TokenResponse GerarTokens(IEnumerable<Claim> claims);
        string GerarRefreshToken();
    }
}
