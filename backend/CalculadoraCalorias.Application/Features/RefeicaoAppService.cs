
using CalculadoraCalorias.Application.DTOs.Records;
using CalculadoraCalorias.Application.DTOs.Requests;
using CalculadoraCalorias.Application.DTOs.Responses;
using CalculadoraCalorias.Application.Filas;
using CalculadoraCalorias.Application.Interfaces;
using CalculadoraCalorias.Application.Mapping;
using CalculadoraCalorias.Core.Domain.Common;
using CalculadoraCalorias.Core.Domain.Entities;
using CalculadoraCalorias.Core.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CalculadoraCalorias.Application.Features
{
    public class RefeicaoAppService(IUsuarioService usuarioService, ILlmService llmService, IRefeicaoService refeicaoService, AtividadeFisicaMapper atividadeFisicaMapper, IUnitOfWork unitOfWork, FilaEstimativaIa filaEstimativaIa) : IRefeicaoAppService
    {
        private readonly ILlmService _llmService = llmService;
        private readonly IRefeicaoService _refeicaoService=refeicaoService;
        private readonly IUsuarioService _usuarioService = usuarioService;
        private readonly AtividadeFisicaMapper _atividadeFisicaMapper = atividadeFisicaMapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly FilaEstimativaIa _filaIa = filaEstimativaIa;

        public async Task<Resultado<Refeicao>> Adicionar(CriarRefeicaoRequest requisicao)
        {
            if(!await _usuarioService.ValidarExistencia(requisicao.UsuarioId)) return Resultado<Refeicao>.Failure(TipoDeErro.SystemFailure, "Id de usuário inválido");

            var caminhoBase = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "refeicoes");
            var extensao = Path.GetExtension(requisicao.Imagem.FileName);
            var guidArquivo = Guid.NewGuid();
            var nomeArquivo = $"{guidArquivo}{extensao}";

            var caminhoFisicoCompleto = Path.Combine(caminhoBase, nomeArquivo);

            using (var stream = new FileStream(caminhoFisicoCompleto, FileMode.Create))
            {
                await requisicao.Imagem.CopyToAsync(stream);
            }

            var refeicao = await _refeicaoService.Adicionar(requisicao.UsuarioId, requisicao.Apelido, requisicao.PesoEmGramas, requisicao.Tipo, requisicao.Data, guidArquivo);

            if (refeicao == null) return Resultado<Refeicao>.Failure(TipoDeErro.SystemFailure, "erro ao solicitar a estimatiza de calorias");

            await _unitOfWork.CommitAsync();

            var requestFila = new EstimativaIaRequest(refeicao.Id, guidArquivo);
            await _filaIa.EnviarParaFilaAsync(requestFila);

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
