using CalculadoraCalorias.Core.Domain.Enums;

namespace CalculadoraCalorias.Core.Domain.Entities
{
    public class Usuario
    {
        protected Usuario() {
            Nome = string.Empty;
            Email = string.Empty;
            Senha = string.Empty;
        }
        public Usuario(string nome, string email, RoleEnum? role, string senha)
        {
            Nome = nome;
            Email = email;
            Role = role ?? RoleEnum.Usuario;
            Senha = senha;
            Ativo = true;
        }

        public long Id { get; private set; }
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public RoleEnum Role { get; private set; }
        public string Senha { get; private set; }
        public Boolean Ativo { private get; set; }

        protected void Inativar() {
            Ativo = false;
        }
        protected void Reativar() {
            Ativo = true;
        }
        protected void AlterarSenha(string senha) { 
            Senha = senha;
        }

        private void AlterarRole(RoleEnum role) {
            Role = role;
        }

    }

    
    
}
