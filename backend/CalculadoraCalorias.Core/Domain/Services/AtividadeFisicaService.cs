
using CalculadoraCalorias.Core.Domain.Entities;
using CalculadoraCalorias.Core.Domain.Enums;
using CalculadoraCalorias.Core.Domain.Interfaces;
using CalculadoraCalorias.Core.Domain.InternalDTO;

namespace CalculadoraCalorias.Core.Domain.Services;

public class AtividadeFisicaService(
        IUsuarioRepository _usuarioRepository, 
        IAtividadeFisicaRepository _atividadeFisicaRepository) : IAtividadeFisicaService
{
    public async Task<AtividadeFisica?> Simular(long usuarioId, TipoExercicioEnum tipoExercicio, int kilometragemPercorrida, TimeSpan tempoDeExercicio)
    {   
        var usuarioCompleto = await _usuarioRepository.ObterCompletoPorId(usuarioId);
        if (usuarioCompleto == null)
        {
            return null;
        }
        var registroFisicoAtual = usuarioCompleto.RegistroFisico.OrderBy(x => x.DataRegistro).First();

        AtividadeFisica teste = new AtividadeFisica(usuarioId, (int)tipoExercicio, registroFisicoAtual.PesoKg, tempoDeExercicio, kilometragemPercorrida);
       
        return teste;
    }

    public async Task<AtividadeFisica?> Adicionar(long usuarioId, decimal caloriasEstimadas, TipoExercicioEnum tipo, TimeSpan tempoDeExercicio, DateOnly dataDoExercicio)
    {
        var usuarioCompleto = await _usuarioRepository.ObterCompletoPorId(usuarioId);
        if (usuarioCompleto == null)
        {
            return null;
        }

        AtividadeFisica atividade = new(usuarioId,(int) tipo, tempoDeExercicio, dataDoExercicio, caloriasEstimadas);

        await _atividadeFisicaRepository.Adicionar(atividade);

        return atividade;
    }

    public async Task<List<AtividadeFisica>> ObterTodosPorId(int idUsuario)
    {
        return await _atividadeFisicaRepository.ObterTodosPorId(idUsuario);
    }

    public async Task<bool> Excluir(int id)
    {
        return await _atividadeFisicaRepository.Excluir(id);
    }

    public async Task<AtividadeFisica?> ObterPorId(int id)
    {
        return await _atividadeFisicaRepository.ObterPorId(id);
    }

    public async Task<AtividadeFisica?> Atualizar(long id, TipoExercicioEnum tipo, int kilometragemPercorrida, TimeSpan tempoDeExercicio)
    {
       var atividade = await _atividadeFisicaRepository.ObterPorId(id);
       if(atividade == null) return null; 

       atividade.Atualizar((int)tipo, kilometragemPercorrida, tempoDeExercicio);
       return atividade;
    }

    public async Task<List<ExercicioDTO>> ObterDiariasPorUsuarioId(long usuarioId)
    {
        return await _atividadeFisicaRepository.ObterDiariasPorUsuarioId(usuarioId);
    }
}

