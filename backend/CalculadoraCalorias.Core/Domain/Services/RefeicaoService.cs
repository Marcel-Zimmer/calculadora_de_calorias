
using CalculadoraCalorias.Application.DTOs.Requests;
using CalculadoraCalorias.Core.Domain.Common;
using CalculadoraCalorias.Core.Domain.Entities;
using CalculadoraCalorias.Core.Domain.Enums;
using CalculadoraCalorias.Core.Domain.Interfaces;

namespace CalculadoraCalorias.Core.Domain.Services;

public class RefeicaoService() : IRefeicaoService
{
    public async Task<Refeicao> Adicionar(long usuarioId, string? apelido, int peso, TipoRefeicaoEnum tipo, DateOnly data, EstimativaFeitaPorLLM estimativa)
    {
        return new Refeicao(usuarioId, apelido,peso, tipo, data, estimativa.Alimento, estimativa.Calorias, estimativa.Proteinas, estimativa.Carboidratos, estimativa.Gorduras,estimativa.Acucares, estimativa.Fibras, false, null);
    }
}

