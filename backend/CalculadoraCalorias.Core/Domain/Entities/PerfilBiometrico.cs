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

        public long Id { get; set; }
        public long UsuarioId { get; set; }
        public DateTime DataNascimento {  get; set; }
        public GeneroEnum Genero { get; set; }
        public int AlturaCm { get; set; }
        public NivelAtividadeEnum NivelAtividade { get; set; }
        public ObjetivoEnum Objetivo { get; set; }
        public virtual Usuario Usuario { get; set; }


    }



}
