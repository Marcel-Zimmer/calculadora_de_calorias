
using CalculadoraCalorias.Application.DTOs.Requests;
using CalculadoraCalorias.Core.Domain.Common;
using CalculadoraCalorias.Core.Domain.Entities;
using CalculadoraCalorias.Core.Domain.Enums;
using CalculadoraCalorias.Core.Domain.Interfaces;

namespace CalculadoraCalorias.Core.Domain.Services;

public class RefeicaoService(IUsuarioRepository usuarioRepository, IAtividadeFisicaRepository atividadeFisicaRepository) : IRefeicaoService
{
    public async Task<Refeicao> Adicionar(long usuarioId, int peso, TipoRefeicaoEnum tipo, DateOnly data, EstimativaFeitaPorLLM estimativa)
    {
        return new Refeicao(usuarioId, peso, tipo, data, estimativa.Alimento, estimativa.Calorias, estimativa.Proteinas, estimativa.Carboidratos, estimativa.Gorduras);
    }
}

