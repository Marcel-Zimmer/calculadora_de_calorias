using CalculadoraCalorias.Core.Domain.Entities;

namespace CalculadoraCalorias.Core.Domain.Interfaces
{
    public interface IUsuarioRepository : IRepositoryBase<Usuario>
    {
        Task<Usuario?> ObterCompletoPorId(long usuarioId);
        Task<bool> VerficarSeEmailEstaEmUso(string email);
    }
}
