namespace CalculadoraCalorias.Core.Domain.ExcecoesPersonalizadas
{
    public class InformacaoDuplicada(string mensagem) : Exception(mensagem)
    {
    }
}
