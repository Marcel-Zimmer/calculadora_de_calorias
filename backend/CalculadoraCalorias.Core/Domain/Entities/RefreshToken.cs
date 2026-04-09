using CalculadoraCalorias.Core.Domain.Entities;

namespace CalculadoraCalorias.Core.Domain.Entities
{
    public class RefreshToken
    {
        public long Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime DataExpiracao { get; set; }
        public bool Revogado { get; set; }
        public DateTime DataCriacao { get; set; }
        public long UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        public bool EstaExpirado => DateTime.UtcNow >= DataExpiracao;
        public bool EstaAtivo => !Revogado && !EstaExpirado;
    }
}
