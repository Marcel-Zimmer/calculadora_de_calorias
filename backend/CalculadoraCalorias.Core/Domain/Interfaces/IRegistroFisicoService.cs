using CalculadoraCalorias.Core.Domain.Entities;
using CalculadoraCalorias.Core.Domain.Enums;

namespace CalculadoraCalorias.Core.Domain.Interfaces
{
    public interface IRegistroFisicoService
    {
        public Task<RegistroFisico?> Adicionar(long usuarioId,
                                            long perfilBiometricoId,
                                            decimal pesoKg,
                                            decimal? MetaCaloricaDiaria);

    }
}
