namespace CalculadoraCalorias.Core.Domain.Interfaces
{
    public interface IContextoHttpService
    {
        long ObterUsuarioId();
        string ObterUsuarioEmail();
    }
}
