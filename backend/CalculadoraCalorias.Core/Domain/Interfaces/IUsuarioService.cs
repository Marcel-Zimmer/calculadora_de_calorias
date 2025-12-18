using CalculadoraCalorias.Core.Domain.Entities;
using CalculadoraCalorias.Core.Domain.Enums;

namespace CalculadoraCalorias.Core.Domain.Interfaces
{
    public interface IUsuarioService
    {
        public Task<bool> VerificarSeEmailEstaEmUso(string email);

        public Task<Usuario> CriarUsuario(string nome, string email, string senha, RoleEnum? role);
    }
}
