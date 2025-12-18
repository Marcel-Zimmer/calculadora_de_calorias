using System.Runtime.Intrinsics.X86;

namespace CalculadoraCalorias.Core.Domain.Entities
{
    public class Massa
    {
        protected Massa() { }
        public Massa(long codigoUsuario, DateOnly data, float peso, float imc, float taxaMetabolicaBasal, float custoCaloricoParaManutencaoCorporal)
        {
            CodigoUsuario = codigoUsuario;
            Data = data;
            Peso = peso;
            Imc = imc;
            TaxaMetabolicaBasal = taxaMetabolicaBasal;
            CustoCaloricoParaManutencaoCorporal = custoCaloricoParaManutencaoCorporal;
        }

        public long Codigo { get; set; }
        public long CodigoUsuario { get; set; }
        public DateOnly Data {  get; set; }
        public float Peso { get; set; }
        public float Imc { get; set; }
        public float TaxaMetabolicaBasal { get; set; }
        public float CustoCaloricoParaManutencaoCorporal { get; set; }

    }
    


}
