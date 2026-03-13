
using CalculadoraCalorias.Application.DTOs.Records;
using CalculadoraCalorias.Application.DTOs.Requests;
using CalculadoraCalorias.Application.DTOs.Responses;
using CalculadoraCalorias.Application.Filas;
using CalculadoraCalorias.Application.Interfaces;
using CalculadoraCalorias.Application.Mapping;
using CalculadoraCalorias.Core.Domain.Common;
using CalculadoraCalorias.Core.Domain.Entities;
using CalculadoraCalorias.Core.Domain.Interfaces;
using CalculadoraCalorias.Core.Domain.InternalDTO;
using Microsoft.AspNetCore.Http;

namespace CalculadoraCalorias.Application.Features
{
    public class RefeicaoAppService(
        IUsuarioService _usuarioService, 
        IRefeicaoService _refeicaoService, 
        IRegistroFisicoService _registroFisicoService,
        IUnitOfWork _unitOfWork, 
        FilaEstimativaIa _filaEstimativaIa) : IRefeicaoAppService
    {
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
            await _filaEstimativaIa.EnviarParaFilaAsync(requestFila);

            return Resultado<Refeicao>.Success(refeicao);
        }

        public async Task<Resultado<RefeicaoGraficoDiarioResponse>> GraficoDiario(long usuarioId)
        {
            if(usuarioId == 0) return Resultado<RefeicaoGraficoDiarioResponse>.Failure(TipoDeErro.SystemFailure, "Id de usuário inválido");

            var registroFisico = await _registroFisicoService.ObterPorIdUsuario(usuarioId);
            if (registroFisico == null) return Resultado<RefeicaoGraficoDiarioResponse>.Failure(TipoDeErro.SystemFailure, "Registro fisico null");

            var refeicoes = await _refeicaoService.ObterDiariasPorUsuarioId(usuarioId);

            var informacoesDiarias = new RefeicaoGraficoDiarioResponse
            {
                MetaCaloricaDiaria = registroFisico.MetaCaloricaDiaria ?? 0,
                TotalCaloriasConsumidas = refeicoes?.Sum(x => x.Calorias) ?? 0,
                Refeicoes = refeicoes ?? [], 
            };

            return Resultado<RefeicaoGraficoDiarioResponse>.Success(informacoesDiarias);
        }

        public Task<Resultado<object>> GraficoMensal(long idUsuario)
        {
            throw new NotImplementedException();
        }

        public Task<Resultado<object>> GraficoSemanal(long idUsuario)
        {
            throw new NotImplementedException();
        }
    }
}
