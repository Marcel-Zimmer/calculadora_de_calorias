
using CalculadoraCalorias.Core.Domain.Entities;
using CalculadoraCalorias.Core.Domain.Enums;
using CalculadoraCalorias.Core.Domain.Interfaces;

namespace CalculadoraCalorias.Core.Domain.Services;
public class PerfilBiometricoService(IPerfilBiometricoRepository perfilBiometricoRepository) : IPerfilBiometricoService
{
    private readonly IPerfilBiometricoRepository perfilBiometricoRepository = perfilBiometricoRepository;

    public async Task<PerfilBiometrico> Adicionar(long usuarioId,
                                                DateTime dataNascimento,
                                                GeneroEnum genero,
                                                int alturaCm,
                                                NivelAtividadeEnum nivelAtividade,
                                                ObjetivoEnum objetivo)
    {
        return await perfilBiometricoRepository.Adicionar(new PerfilBiometrico(usuarioId, dataNascimento, genero, alturaCm, nivelAtividade, objetivo));
    }

    public async Task<PerfilBiometrico?> ObterPorIdUsuario(long codigoUsuario)
    {
        return await perfilBiometricoRepository.ObterPorIdUsuario(codigoUsuario);
    }

    public async Task<PerfilBiometrico?> Atualizar(long usuarioId, DateTime dataNascimento, GeneroEnum genero, int alturaCm, NivelAtividadeEnum nivelAtividade, ObjetivoEnum objetivo)
    {
        var perfil = await perfilBiometricoRepository.ObterPorIdUsuario(usuarioId);
        if (perfil == null) return null;

        perfil.Atualizar(dataNascimento, genero, alturaCm, nivelAtividade, objetivo);
        return perfil;
    }
}

