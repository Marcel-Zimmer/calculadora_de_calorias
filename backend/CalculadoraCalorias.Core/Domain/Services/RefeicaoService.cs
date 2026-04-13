
using CalculadoraCalorias.Application.DTOs.Requests;
using CalculadoraCalorias.Core.Domain.Common;
using CalculadoraCalorias.Core.Domain.Entities;
using CalculadoraCalorias.Core.Domain.Enums;
using CalculadoraCalorias.Core.Domain.Interfaces;
using CalculadoraCalorias.Core.Domain.InternalDTO;

namespace CalculadoraCalorias.Core.Domain.Services;

public class RefeicaoService(IRefeicaoRepository refeicaoRepository) : IRefeicaoService
{
    private readonly IRefeicaoRepository _refeicaoRepository = refeicaoRepository;

    public async Task<Refeicao> Adicionar(long usuarioId, string? apelido, int peso, TipoRefeicaoEnum tipo, DateOnly data, Guid guidArquivo)
    {
        var refeicao =  new Refeicao(usuarioId, apelido,peso, tipo, data, guidArquivo);
        await _refeicaoRepository.Adicionar(refeicao);
        return refeicao;

    }

    public async Task<Refeicao?> AdicionarBaseadoEmModelo(long usuarioId, long modeloId, int pesoEmGramas, TipoRefeicaoEnum tipo, DateOnly data)
    {
        var modelo = await _refeicaoRepository.ObterPorId(modeloId);
        if (modelo == null) return null;

        double fator = (double)pesoEmGramas / modelo.Peso;

        var novaRefeicao = new Refeicao(usuarioId, modelo.Apelido, pesoEmGramas, tipo, data, Guid.Empty);
        
        novaRefeicao.AtualizarEstimativa(
            modelo.Alimento,
            (modelo.Calorias ?? 0) * fator,
            (modelo.Proteinas ?? 0) * fator,
            (modelo.Carboidratos ?? 0) * fator,
            (modelo.Gorduras ?? 0) * fator,
            (modelo.Acucares ?? 0) * fator,
            (modelo.Fibras ?? 0) * fator
        );

        novaRefeicao.MarcarComoBaseadoEmModelo(modeloId);

        await _refeicaoRepository.Adicionar(novaRefeicao);
        return novaRefeicao;
    }

    public async Task<List<RefeicaoDTO>> ObterDiariasPorUsuarioId(long usuarioId)
    {
        return await _refeicaoRepository.ObterDiariasPorUsuarioId(usuarioId);
    }

    public async Task<List<RefeicaoDTO>> ObterPorPeriodo(long usuarioId, DateOnly inicio, DateOnly fim)
    {
        return await _refeicaoRepository.ObterPorPeriodo(usuarioId, inicio, fim);
    }

    public async Task<List<RefeicaoModeloDTO>> ObterModelosFrequentes(long usuarioId)
    {
        return await _refeicaoRepository.ObterModelosFrequentes(usuarioId);
    }

    public async Task<bool> Excluir(long id)
    {
        return await _refeicaoRepository.Excluir(id);
    }
}

