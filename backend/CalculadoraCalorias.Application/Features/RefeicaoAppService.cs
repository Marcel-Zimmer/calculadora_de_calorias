

using CalculadoraCalorias.Application.DTOs.Records;
using CalculadoraCalorias.Application.DTOs.Requests;
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

            var refeicao = await _refeicaoService.Adicionar(requisicao.UsuarioId, requisicao.Apelido, requisicao.PesoEmGramas, requisicao.Tipo, requisicao.Data, guidArquivo);

            if (refeicao == null) return Resultado<Refeicao>.Failure(TipoDeErro.SystemFailure, "erro ao solicitar a estimatiza de calorias");

            await _unitOfWork.CommitAsync();

            var requestFila = new EstimativaIaRequest(refeicao.Id, guidArquivo);
            await _filaEstimativaIa.EnviarParaFilaAsync(requestFila);

            return Resultado<Refeicao>.Success(refeicao);
        }
    }
}
