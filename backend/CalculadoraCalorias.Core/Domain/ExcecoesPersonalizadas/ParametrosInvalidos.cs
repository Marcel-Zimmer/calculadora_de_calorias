namespace CalculadoraCalorias.Core.Domain.ExcecoesPersonalizadas
{
    public class ParametrosInvalidos(string mensagem) : Exception(mensagem)
    {
    }
}
