
using CalculadoraCalorias.Core.Domain.Common;
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

    public async Task<List<AtividadeFisica>> ObterTodosPorId(int idUsuario)
    {
        return await atividadeFisicaRepository.ObterTodosPorId(idUsuario);
    }

    public async Task<bool> Excluir(int id)
    {
        return await atividadeFisicaRepository.Excluir(id);
    }

    public async Task<AtividadeFisica?> ObterPorId(int id)
    {
        return await atividadeFisicaRepository.ObterPorId(id);
    }

    public async Task<AtividadeFisica?> Atualizar(long id, TipoExercicioEnum tipo, int kilometragemPercorrida, TimeSpan tempoDeExercicio)
    {
       var atividade = await atividadeFisicaRepository.ObterPorId(id);
       if(atividade == null) return null; 

       atividade.Atualizar((int)tipo, kilometragemPercorrida, tempoDeExercicio);
       return atividade;
    }
}

