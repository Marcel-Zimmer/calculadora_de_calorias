using CalculadoraCalorias.Application.DTOs.Requests;
using CalculadoraCalorias.Core.Domain.Entities;
using CalculadoraCalorias.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CalculadoraCalorias.Core.Domain.Interfaces
{
    public interface ILlmService
    {
        Task<EstimativaFeitaPorLLM?> SimularCaloriasRefeicao(byte[] imagemBase64, int peso);

    }
}
