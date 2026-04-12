using CalculadoraCalorias.Core.Domain.Entities;
using CalculadoraCalorias.Core.Domain.Enums;

namespace CalculadoraCalorias.Core.Domain.Interfaces
{
    public interface IRegistroFisicoService
    {
        public Task<RegistroFisico?> Adicionar(long usuarioId,
                                            decimal pesoKg,
                                            decimal? MetaCaloricaDiaria,
                                            PerfilBiometrico? perfil = null);
        Task<RegistroFisico?> ObterPorIdUsuario(long idUsuario);
        Task<RegistroFisico?> Atualizar(long usuarioId,
                                            decimal pesoKg,
                                            decimal? MetaCaloricaDiaria);
    }
}
