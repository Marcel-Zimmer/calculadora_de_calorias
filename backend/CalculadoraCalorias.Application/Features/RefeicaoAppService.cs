
using CalculadoraCalorias.Application.DTOs.Requests;
using CalculadoraCalorias.Application.DTOs.Responses;
using CalculadoraCalorias.Application.Interfaces;
using CalculadoraCalorias.Application.Mapping;
using CalculadoraCalorias.Core.Domain.Common;
using CalculadoraCalorias.Core.Domain.Entities;
using CalculadoraCalorias.Core.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CalculadoraCalorias.Application.Features
{
    public class RefeicaoAppService(ILlmService llmService, IRefeicaoService refeicaoService, AtividadeFisicaMapper atividadeFisicaMapper, IUnitOfWork unitOfWork) : IRefeicaoAppService
    {
        private readonly ILlmService _llmService = llmService;
        private readonly IRefeicaoService _refeicaoService=refeicaoService;
        private readonly AtividadeFisicaMapper _atividadeFisicaMapper = atividadeFisicaMapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Resultado<Refeicao>> Simular(CriarRefeicaoRequest requisicao)
        {
            var imagemEmBytes = await ConverterFileEmArrayByte(requisicao.Imagem);

            var estimativaCalorica = await _llmService.SimularCaloriasRefeicao(imagemEmBytes, requisicao.Peso);

            if (estimativaCalorica == null) { return Resultado<Refeicao>.Failure(TipoDeErro.SystemFailure, "erro ao solicitar ao gemini as calorias"); }

            var refeicao = await _refeicaoService.Adicionar(requisicao.UsuarioId, requisicao.Peso, requisicao.Tipo, requisicao.Data, estimativaCalorica);
            
            return Resultado<Refeicao>.Success(refeicao);
        }

        private static async Task<byte[]> ConverterFileEmArrayByte(IFormFile imagem) 
        {
            using var ms = new MemoryStream();
            await imagem.CopyToAsync(ms);
            return ms.ToArray();
        }
    }
}
