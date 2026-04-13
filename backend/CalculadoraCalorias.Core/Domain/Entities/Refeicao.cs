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
                        Guid guidArquivo)
        {
            UsuarioId = usuarioId;
            Apelido = apelido;
            Peso = peso;
            Tipo = tipo;
            Data = data;
            GuidArquivo = guidArquivo;

            StatusProcessamento = StatusProcessamentoEnum.EnviadoParaFila;
        }

        public long Id { get; private set; }
        public long UsuarioId { get; private set; }
        public string? Apelido { get; private set; }
        public int Peso {  get; private set; }
        public TipoRefeicaoEnum Tipo {  get; private set; }
        public DateOnly Data {  get; private set; }
        public Guid GuidArquivo { get; private set; }
        public StatusProcessamentoEnum StatusProcessamento { get; private set; }    
        public string? Alimento { get; private set; }
        public double? Calorias { get; private set; }
        public double? Proteinas { get; private set; }
        public double? Carboidratos { get; private set; }
        public double? Gorduras { get; private set; }
        public double? Acucares { get; private set; }
        public double? Fibras { get; private set; }

        public bool? UtilizadoRefeicaoModelo { get; private set; } = false;
        public long? CodigoRefeicaoModelo { get; private set; }

        public virtual Usuario? Usuario { get; private set; }
        public virtual Refeicao? RefeicaoModelo { get; private set; }

        public void AtualizarEstimativa(string? alimento,
                                            double calorias,
                                            double proteinas,
                                            double carboidratos,
                                            double gorduras,
                                            double acucares,
                                            double fibras)
        { 
            Alimento = alimento;
            Calorias = calorias;
            Proteinas = proteinas;
            Carboidratos = carboidratos;
            Gorduras = gorduras;
            Acucares = acucares;
            Fibras = fibras;
            StatusProcessamento = StatusProcessamentoEnum.Concluido;

        }

        public void MarcarComoBaseadoEmModelo(long modeloId)
        {
            UtilizadoRefeicaoModelo = true;
            CodigoRefeicaoModelo = modeloId;
            StatusProcessamento = StatusProcessamentoEnum.Concluido;
        }
    }



}
