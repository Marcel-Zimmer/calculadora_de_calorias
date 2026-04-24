using CalculadoraCalorias.Application.DTOs.Records;
using CalculadoraCalorias.Application.DTOs.Requests;
using CalculadoraCalorias.Application.DTOs.Responses;
using CalculadoraCalorias.Application.Filas;
using CalculadoraCalorias.Application.Interfaces;
using CalculadoraCalorias.Core.Domain.Common;
using CalculadoraCalorias.Core.Domain.Entities;
using CalculadoraCalorias.Core.Domain.Interfaces;

namespace CalculadoraCalorias.Application.Features
{
    public class RefeicaoAppService(
        IUsuarioService _usuarioService, 
        IRefeicaoService _refeicaoService, 
        IUnitOfWork _unitOfWork, 
        FilaEstimativaIa _filaEstimativaIa) : IRefeicaoAppService
    {
        public async Task<Resultado<Refeicao>> Adicionar(CriarRefeicaoRequest requisicao)
        {
            if(!await _usuarioService.ValidarExistencia(requisicao.UsuarioId)) return Resultado<Refeicao>.Failure(TipoDeErro.SystemFailure, "Id de usuário inválido");

            Refeicao? refeicao;

            if (requisicao.CodigoRefeicaoModelo.HasValue)
            {
                refeicao = await _refeicaoService.AdicionarBaseadoEmModelo(
                    requisicao.UsuarioId, 
                    requisicao.CodigoRefeicaoModelo.Value, 
                    requisicao.PesoEmGramas, 
                    requisicao.Tipo, 
                    requisicao.Data);
            }
            else if (requisicao.CaloriasManuais.HasValue && !string.IsNullOrWhiteSpace(requisicao.AlimentoManual))
            {
                refeicao = await _refeicaoService.AdicionarManual(
                    requisicao.UsuarioId,
                    requisicao.Apelido,
                    requisicao.AlimentoManual,
                    requisicao.CaloriasManuais.Value,
                    requisicao.ProteinasManuais ?? 0,
                    requisicao.CarboidratosManuais ?? 0,
                    requisicao.GordurasManuais ?? 0,
                    requisicao.AcucaresManuais ?? 0,
                    requisicao.FibrasManuais ?? 0,
                    requisicao.PesoEmGramas,
                    requisicao.Tipo,
                    requisicao.Data
                );
            }
            else
            {
                if (requisicao.Imagem == null) return Resultado<Refeicao>.Failure(TipoDeErro.Validation, "Imagem é obrigatória quando não se utiliza um modelo ou adição manual");

                var caminhoBase = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "refeicoes");
                if (!Directory.Exists(caminhoBase))
                {
                    Directory.CreateDirectory(caminhoBase);
                }
                var extensao = Path.GetExtension(requisicao.Imagem.FileName);
                var guidArquivo = Guid.NewGuid();
                var nomeArquivo = $"{guidArquivo}{extensao}";

                var caminhoFisicoCompleto = Path.Combine(caminhoBase, nomeArquivo);

                using (var stream = new FileStream(caminhoFisicoCompleto, FileMode.Create))
                {
                    await requisicao.Imagem.CopyToAsync(stream);
                }

                refeicao = await _refeicaoService.Adicionar(requisicao.UsuarioId, requisicao.Apelido, requisicao.PesoEmGramas, requisicao.Tipo, requisicao.Data, guidArquivo);
            }

            if (refeicao == null) return Resultado<Refeicao>.Failure(TipoDeErro.SystemFailure, "erro ao processar a refeição");

            await _unitOfWork.CommitAsync();

            if (!requisicao.CodigoRefeicaoModelo.HasValue && !requisicao.CaloriasManuais.HasValue)
            {
                var requestFila = new EstimativaIaRequest(refeicao.Id, refeicao.GuidArquivo);
                await _filaEstimativaIa.EnviarParaFilaAsync(requestFila);
            }

            return Resultado<Refeicao>.Success(refeicao);
        }

        public async Task<Resultado<bool>> Excluir(long id)
        {
            var deletado = await _refeicaoService.Excluir(id);
            if (!deletado) return Resultado<bool>.Failure(TipoDeErro.NotFound, "Refeição não encontrada");

            await _unitOfWork.CommitAsync();
            return Resultado<bool>.Success(true);
        }

        public async Task<Resultado<List<RefeicaoModeloResponse>>> ObterModelosFrequentes(long usuarioId)
        {
            var modelos = await _refeicaoService.ObterModelosFrequentes(usuarioId);
            var response = modelos.Select(x => new RefeicaoModeloResponse
            {
                Id = x.Id,
                Apelido = x.Apelido,
                Calorias = x.Calorias,
                Proteinas = x.Proteinas,
                Carboidratos = x.Carboidratos,
                Gorduras = x.Gorduras,
                PesoOriginal = x.PesoOriginal
            }).ToList();

            return Resultado<List<RefeicaoModeloResponse>>.Success(response);
        }
    }
}
