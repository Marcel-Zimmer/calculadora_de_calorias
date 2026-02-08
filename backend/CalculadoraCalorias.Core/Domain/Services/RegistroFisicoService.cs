
using CalculadoraCalorias.Core.Domain.Entities;
using CalculadoraCalorias.Core.Domain.Enums;
using CalculadoraCalorias.Core.Domain.Interfaces;

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


    public async Task<RegistroFisico?> Adicionar(long usuarioId, decimal pesoKg, decimal? metaCaloricaDiaria)
    {
        var perfilBiometrico = await _perfilBiometricoService.ObterPorCodigoUsuario(usuarioId);
        var imcCalculado = CalcularImc(perfilBiometrico?.AlturaCm, pesoKg);
        var taxaMetabolica = CalcularTaxaMetabolicaBasal(perfilBiometrico, pesoKg);

        if(imcCalculado == 0 || taxaMetabolica == 0) 
        {
            return null;
        }
        var registroFisico = new RegistroFisico(usuarioId, DateTime.UtcNow, pesoKg, imcCalculado, taxaMetabolica);

        if (metaCaloricaDiaria != null && metaCaloricaDiaria != 0m) {
            registroFisico.AdicionarMetaCalorica((decimal)metaCaloricaDiaria);
        }
        await _registroFisicoRepository.Adicionar(registroFisico);
        return registroFisico;
    }

    private static decimal CalcularImc(int? altura, decimal pesoKg)
    {
        if (altura == null || altura <= 0) return 0;

        decimal alturaMetros = (int)altura / 100m;
        return pesoKg / (alturaMetros * alturaMetros);

    }

    private static decimal CalcularTaxaMetabolicaBasal(PerfilBiometrico? perfil, decimal pesoKg)
    {
        if(perfil == null) return 0;

        decimal tmbCalculado;
        int idade = perfil.ObterIdade();

        if (perfil.Genero == GeneroEnum.Masculino)
        {

            tmbCalculado = (10m * pesoKg) + (6.25m * perfil.AlturaCm) - (5m * idade) + 5m;
        }
        else { 
            tmbCalculado = (10 * pesoKg) + ((decimal)6.25 * perfil.AlturaCm) - (5 * idade) - 161;
        }

        return perfil.ObterFatorAtividade() * tmbCalculado;    
    }


}

