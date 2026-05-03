using CalculadoraCalorias.Core.Domain.Entities;

namespace CalculadoraCalorias.Core.Domain.Entities
{
    public class RegistroAgua
    {
        protected RegistroAgua() { }

        public RegistroAgua(long usuarioId, int quantidadeMl, DateOnly data, TimeOnly hora)
        {
            UsuarioId = usuarioId;
            QuantidadeMl = quantidadeMl;
            Data = data;
            Hora = hora;
            DataCriacao = DateTime.UtcNow;
        }

        public long Id { get; private set; }
        public long UsuarioId { get; private set; }
        public int QuantidadeMl { get; private set; }
        public DateOnly Data { get; private set; }
        public TimeOnly Hora { get; private set; }
        public DateTime DataCriacao { get; private set; }

        public virtual Usuario? Usuario { get; private set; }
    }
}
