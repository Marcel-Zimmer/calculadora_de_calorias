
namespace CalculadoraCalorias.Core.Domain.Entities
{
    public class RegistroFisico
    {
        protected RegistroFisico() { }
        public RegistroFisico(long usuarioId, 
                                DateTime dataRegistro,
                                decimal pesoKg,
                                decimal imcCalculado,
                                decimal taxaMetabolicaBasal)
        {
            UsuarioId = usuarioId;
            DataRegistro = dataRegistro;
            PesoKg = pesoKg;
            ImcCalculado = imcCalculado;
            TaxaMetabolicaBasal = taxaMetabolicaBasal;

        }

        public long Id { get; private set; }
        public long UsuarioId { get; private set; }
        public DateTime DataRegistro {  get; private set; }
        public decimal PesoKg { get; private set; }
        public decimal ImcCalculado { get; private set; }
        public decimal TaxaMetabolicaBasal {  get; private set; }
        public decimal? MetaCaloricaDiaria { get; private set; }
        public virtual Usuario? Usuario { get; private set; }

        public void AdicionarMetaCalorica(decimal meta) { 
            MetaCaloricaDiaria = meta;
        }
    }
}
