
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

    public async Task<List<RefeicaoDTO>> ObterDiariasPorUsuarioId(long usuarioId)
    {
        return await _refeicaoRepository.ObterDiariasPorUsuarioId(usuarioId);
    }
}

