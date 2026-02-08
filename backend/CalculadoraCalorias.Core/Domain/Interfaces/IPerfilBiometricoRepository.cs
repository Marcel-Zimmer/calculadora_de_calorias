using CalculadoraCalorias.Core.Domain.Entities;

namespace CalculadoraCalorias.Core.Domain.Interfaces
{
    public interface IPerfilBiometricoRepository
    {
        Task <PerfilBiometrico> Adicionar(PerfilBiometrico usuario);
    }
}
