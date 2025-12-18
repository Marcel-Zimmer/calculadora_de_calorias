namespace CalculadoraCalorias.Application.DTOs.Responses
{
    public class CriarUsuarioResponse
    {   
        public long Id { get; set; }
        public string? Nome {  get; set; }
        public string? Email { get; set; }
        public int? Role { get; set; }
    }
}
