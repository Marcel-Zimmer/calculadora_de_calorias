using CalculadoraCalorias.Core.Domain.Enums;

namespace CalculadoraCalorias.Core.Domain.Entities
{
    public class Refeicao
    {
        protected Refeicao() { }

        public Refeicao(long usuarioId, int peso, TipoRefeicaoEnum tipo, DateOnly data, string alimento, double calorias, double proteinas, double carboidratos, double gorduras)
        {
            UsuarioId = usuarioId;
            Peso = peso;
            Tipo = tipo;
            Data = data;
            Alimento = alimento;
            Calorias = calorias;
            Proteinas = proteinas;
            Carboidratos = carboidratos;
            Gorduras = gorduras;
        }

        public long Id { get; private set; }
        public long UsuarioId { get; private set; }
        public int Peso {  get; private set; }
        public TipoRefeicaoEnum Tipo {  get; private set; }
        public DateOnly Data {  get; private set; }
        public string Alimento { get; private set; } = string.Empty;
        public double Calorias { get; private set; }
        public double Proteinas { get; private set; }
        public double Carboidratos { get; private set; }
        public double Gorduras { get; private set; }

    }


}
