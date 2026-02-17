using CalculadoraCalorias.Core.Domain.Enums;

namespace CalculadoraCalorias.Core.Domain.Entities
{
    public class Refeicao
    {
        protected Refeicao() { }

        public Refeicao(long usuarioId, 
                        string? apelido, 
                        int peso, 
                        TipoRefeicaoEnum tipo, 
                        DateOnly data, 
                        string alimento, 
                        double calorias, 
                        double proteinas, 
                        double carboidratos, 
                        double gorduras, 
                        double acucares, 
                        double fibras, 
                        bool? utilizadoRefeicaoModelo, 
                        long? codigoRefeicaoModelo)
        {
            UsuarioId = usuarioId;
            Apelido = apelido;
            Peso = peso;
            Tipo = tipo;
            Data = data;
            Alimento = alimento;
            Calorias = calorias;
            Proteinas = proteinas;
            Carboidratos = carboidratos;
            Gorduras = gorduras;
            Acucares = acucares;
            Fibras = fibras;
            UtilizadoRefeicaoModelo = utilizadoRefeicaoModelo;
            CodigoRefeicaoModelo = codigoRefeicaoModelo;
        }

        public long Id { get; private set; }
        public long UsuarioId { get; private set; }
        public string? Apelido { get; private set; } = string.Empty;
        public int Peso {  get; private set; }
        public TipoRefeicaoEnum Tipo {  get; private set; }
        public DateOnly Data {  get; private set; }
        public string Alimento { get; private set; } = string.Empty;
        public double Calorias { get; private set; }
        public double Proteinas { get; private set; }
        public double Carboidratos { get; private set; }
        public double Gorduras { get; private set; }
        public double Acucares { get; private set; }
        public double Fibras { get; private set; }

        public bool? UtilizadoRefeicaoModelo { get; private set; } = false;
        public long? CodigoRefeicaoModelo { get; private set; }

        public virtual Usuario? Usuario { get; private set; }
        public virtual Refeicao? RefeicaoModelo { get; private set; }
    }


}
