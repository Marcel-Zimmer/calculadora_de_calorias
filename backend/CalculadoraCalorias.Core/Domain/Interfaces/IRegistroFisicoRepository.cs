using CalculadoraCalorias.Core.Domain.Entities;

namespace CalculadoraCalorias.Core.Domain.Interfaces
{
    public interface IRegistroFisicoRepository
    {
        Task <RegistroFisico> Adicionar(RegistroFisico registroFisico);
    }
}
