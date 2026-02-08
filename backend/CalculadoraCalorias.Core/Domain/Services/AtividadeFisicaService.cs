
using CalculadoraCalorias.Core.Domain.Entities;
using CalculadoraCalorias.Core.Domain.Enums;
using CalculadoraCalorias.Core.Domain.Interfaces;

namespace CalculadoraCalorias.Core.Domain.Services;

public class AtividadeFisicaService(IUsuarioRepository usuarioRepository, IAtividadeFisicaRepository atividadeFisicaRepository) : IAtividadeFisicaService
{
    public async Task<AtividadeFisica> Simular(long usuarioId, TipoExercicioEnum tipoExercicio, int kilometragemPercorrida, TimeSpan tempoDeExercicio)
    {   
        var usuarioCompleto = await usuarioRepository.ObterCompletoPorId(usuarioId);
        if (usuarioCompleto == null)
        {
            return null;
        }
        var registroFisicoAtual = usuarioCompleto.RegistroFisico.OrderBy(x => x.DataRegistro).First();

        AtividadeFisica teste = new AtividadeFisica(usuarioId, (int)tipoExercicio, registroFisicoAtual.PesoKg, tempoDeExercicio, kilometragemPercorrida);
       
        return teste;
    }

    public async Task<AtividadeFisica?> Adicionar(long usuarioId, TipoExercicioEnum tipoExercicio, int kilometragemPercorrida, TimeSpan tempoDeExercicio)
    {
        var usuarioCompleto = await usuarioRepository.ObterCompletoPorId(usuarioId);
        if (usuarioCompleto == null)
        {
            return null;
        }
        var registroFisicoAtual = usuarioCompleto.RegistroFisico.OrderBy(x => x.DataRegistro).First();

        AtividadeFisica atividade = new AtividadeFisica(usuarioId, (int)tipoExercicio, registroFisicoAtual.PesoKg, tempoDeExercicio, kilometragemPercorrida);

        await atividadeFisicaRepository.Adicionar(atividade);

        return atividade;
    }
}

