using CalculadoraCalorias.Core.Domain.Entities;
using CalculadoraCalorias.Core.Domain.Enums;

namespace CalculadoraCalorias.Core.Domain.Interfaces
{
    public interface IPerfilBiometricoService
    {
        public Task<PerfilBiometrico> Adicionar(long usuarioId, 
                                            DateTime dataNascimento, 
                                            GeneroEnum genero, 
                                            int alturaCm, 
                                            NivelAtividadeEnum nivelAtividade, 
                                            ObjetivoEnum objetivo);

        public Task<PerfilBiometrico?> ObterPorCodigoUsuario(long codigoUsuario);
    }
}
