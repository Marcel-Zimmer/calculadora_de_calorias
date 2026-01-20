using CalculadoraCalorias.Core.Domain.Enums;
using System.Runtime.Intrinsics.X86;

namespace CalculadoraCalorias.Core.Domain.Entities
{
    public class PerfilBiometrico
    {
        protected PerfilBiometrico() { }
        public PerfilBiometrico(long usuarioId,
                                DateTime dataNascimento, 
                                GeneroEnum genero, 
                                int alturaCm, 
                                NivelAtividadeEnum nivelAtividade, 
                                ObjetivoEnum objetivo)
        {
            UsuarioId = usuarioId;
            DataNascimento = DateTime.SpecifyKind(dataNascimento, DateTimeKind.Utc);
            Genero = genero;
            AlturaCm = alturaCm;
            NivelAtividade = nivelAtividade;
            Objetivo = objetivo;
        }

        public long Id { get; private set; }
        public long UsuarioId { get; private set; }
        public DateTime DataNascimento {  get; private set; }
        public GeneroEnum Genero { get; private set; }
        public int AlturaCm { get; private set; }
        public NivelAtividadeEnum NivelAtividade { get; private set; }
        public ObjetivoEnum Objetivo { get; private set; }
        public virtual Usuario? Usuario { get; private set; }

        public int ObterIdade() {

            var dataAtual = DateTime.UtcNow;
            int idadeAnos = dataAtual.Year - DataNascimento.Year;

            if (dataAtual.Month < DataNascimento.Month ||
                (dataAtual.Month == DataNascimento.Month && dataAtual.Day < DataNascimento.Day))
            {
                idadeAnos--;
            }

            return idadeAnos;
        }
        public decimal ObterFatorAtividade()
        {
            return NivelAtividade switch
            {
                NivelAtividadeEnum.Sedentario => 1.2m,
                NivelAtividadeEnum.LevementeAtivo => 1.375m,
                NivelAtividadeEnum.ModeradamenteAtivo => 1.55m,
                NivelAtividadeEnum.MuitoAtivo => 1.725m,
                NivelAtividadeEnum.ExtremamenteAtivo => 1.9m,
                _ => 1.0m
            };
        }
    }



}
