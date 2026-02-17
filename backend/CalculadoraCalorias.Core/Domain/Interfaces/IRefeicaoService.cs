using CalculadoraCalorias.Application.DTOs.Requests;
using CalculadoraCalorias.Core.Domain.Entities;
using CalculadoraCalorias.Core.Domain.Enums;

namespace CalculadoraCalorias.Core.Domain.Interfaces
{
    public interface IRefeicaoService
    {
        public Task<Refeicao> Adicionar(long usuarioId, string? apelido, int pesoEmGramas, TipoRefeicaoEnum tipo, DateOnly data, EstimativaFeitaPorLLM estimativa);

    }
}
