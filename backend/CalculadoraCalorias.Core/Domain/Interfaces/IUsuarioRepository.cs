using CalculadoraCalorias.Core.Domain.Entities;

namespace CalculadoraCalorias.Core.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task <Usuario> Adicionar(Usuario usuario);
        Task<bool> VerficarSeEmailEstaEmUso(string email);
    }
}
