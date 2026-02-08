
using CalculadoraCalorias.Core.Domain.Entities;
using CalculadoraCalorias.Core.Domain.Enums;
using CalculadoraCalorias.Core.Domain.Interfaces;

namespace CalculadoraCalorias.Core.Domain.Services;
public class UsuarioService(IUsuarioRepository usuarioRepository) : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository = usuarioRepository;

    public async Task<Usuario> CriarUsuario(string nome, string email, string senha, RoleEnum? role)
    {
        var usuario = await _usuarioRepository.Adicionar(new Usuario(nome, email, null, senha));

        return usuario;
    }

    public async Task<bool> VerificarSeEmailEstaEmUso(string email)
    {
        return await _usuarioRepository.VerficarSeEmailEstaEmUso(email);
    }
}

