namespace CalculadoraCalorias.Application.DTOs.Requests
{
    public record CriarRegistroAguaRequest(int QuantidadeMl, DateOnly? Data, TimeOnly? Hora);
}
