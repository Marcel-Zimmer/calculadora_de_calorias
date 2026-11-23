
using CalculadoraCalorias.Core.Domain.Entities;
using CalculadoraCalorias.Core.Domain.Enums;
using CalculadoraCalorias.Core.Domain.Interfaces;
using CalculadoraCalorias.Core.Domain.Common;

namespace CalculadoraCalorias.Core.Domain.Services;
public class RegistroFisicoService : IRegistroFisicoService
{
    private readonly IRegistroFisicoRepository _registroFisicoRepository;
    private readonly IPerfilBiometricoService _perfilBiometricoService;
    public RegistroFisicoService(IRegistroFisicoRepository registroFisicoRepository,
                                 IPerfilBiometricoService perfilBiometricoService)
    {
        _registroFisicoRepository = registroFisicoRepository;
        _perfilBiometricoService = perfilBiometricoService;
    }


    public async Task<RegistroFisico?> Adicionar(long usuarioId, decimal pesoKg, decimal? metaCaloricaDiaria, PerfilBiometrico? perfil = null)
    {
        var perfilBiometrico = perfil ?? await _perfilBiometricoService.ObterPorIdUsuario(usuarioId);
        var imcCalculado = CalcularImc(perfilBiometrico?.AlturaCm, pesoKg);
        var taxaMetabolica = CalcularTaxaMetabolicaBasal(perfilBiometrico, pesoKg);
        var gastoEnergeticoTotal = CalcularGastoEnergeticoTotal(perfilBiometrico, taxaMetabolica);

        if (imcCalculado <= 0 || taxaMetabolica <= 0) 
        {
            return null;
        }
        var registroFisico = new RegistroFisico(usuarioId, DateTime.UtcNow, pesoKg, imcCalculado, taxaMetabolica, gastoEnergeticoTotal);

        if (metaCaloricaDiaria != null && metaCaloricaDiaria != 0m) {
            registroFisico.AdicionarMetaCalorica((decimal)metaCaloricaDiaria);
        }
        await _registroFisicoRepository.Adicionar(registroFisico);
        return registroFisico;
    }

    private static decimal CalcularImc(int? altura, decimal pesoKg)
    {
        if (altura == null || altura <= 0 || pesoKg <= 0) return 0;

        decimal alturaMetros = (int)altura / 100m;
        return pesoKg / (alturaMetros * alturaMetros);
    }

    // Fórmula de Mifflin-St Jeor (TMB pura, sem fator de atividade)
    private static decimal CalcularTaxaMetabolicaBasal(PerfilBiometrico? perfil, decimal pesoKg)
    {
        if (perfil == null || pesoKg <= 0 || perfil.AlturaCm <= 0) return 0;

        int idade = perfil.ObterIdade();
        decimal tmbCalculado = perfil.Genero == GeneroEnum.Masculino
            ? (10m * pesoKg) + (6.25m * perfil.AlturaCm) - (5m * idade) + 5m
            : (10m * pesoKg) + (6.25m * perfil.AlturaCm) - (5m * idade) - 161m;

        return Math.Max(0, tmbCalculado);
    }

    // Gasto Energético Total estimado (GET = TMB × Fator de Atividade)
    private static decimal CalcularGastoEnergeticoTotal(PerfilBiometrico? perfil, decimal tmb)
    {
        if (perfil == null || tmb <= 0) return 0;
        return tmb * perfil.ObterFatorAtividade();
    }

    public async Task<RegistroFisico?> ObterPorIdUsuario(long idUsuario)
    {
        return await _registroFisicoRepository.ObterPorIdUsuario(idUsuario);
    }

    public async Task<List<RegistroFisico>> ObterHistorico(long usuarioId)
    {
        return await _registroFisicoRepository.ObterHistoricoPorUsuario(usuarioId);
    }

    public async Task<RegistroFisico?> Atualizar(long usuarioId, decimal pesoKg, decimal? metaCaloricaDiaria)
    {
        var registro = await _registroFisicoRepository.ObterPorIdUsuario(usuarioId);
        if (registro == null) return null;

        var perfilBiometrico = await _perfilBiometricoService.ObterPorIdUsuario(usuarioId);
        var imcCalculado = CalcularImc(perfilBiometrico?.AlturaCm, pesoKg);
        var taxaMetabolica = CalcularTaxaMetabolicaBasal(perfilBiometrico, pesoKg);
        var gastoEnergeticoTotal = CalcularGastoEnergeticoTotal(perfilBiometrico, taxaMetabolica);

        registro.Atualizar(pesoKg, imcCalculado, taxaMetabolica, metaCaloricaDiaria, gastoEnergeticoTotal);
        return registro;
    }

    public async Task RecalcularUltimoRegistroPorPerfil(long usuarioId, PerfilBiometrico perfil)
    {
        var registro = await _registroFisicoRepository.ObterPorIdUsuario(usuarioId);
        if (registro == null || registro.PesoKg <= 0) return;

        var imcCalculado = CalcularImc(perfil.AlturaCm, registro.PesoKg);
        var taxaMetabolica = CalcularTaxaMetabolicaBasal(perfil, registro.PesoKg);
        var gastoEnergeticoTotal = CalcularGastoEnergeticoTotal(perfil, taxaMetabolica);

        if (imcCalculado > 0 && taxaMetabolica > 0)
        {
            registro.RecalcularMetricas(imcCalculado, taxaMetabolica, gastoEnergeticoTotal);
        }
    }

    public async Task<bool> Excluir(long id)
    {
        return await _registroFisicoRepository.Excluir(id);
    }
}

