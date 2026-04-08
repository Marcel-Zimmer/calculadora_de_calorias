using CalculadoraCalorias.Core.Domain.Entities;

namespace CalculadoraCalorias.Core.Domain.Interfaces
{
    public interface ITokenService
    {
        string GerarToken(Usuario usuario);
    }
}
