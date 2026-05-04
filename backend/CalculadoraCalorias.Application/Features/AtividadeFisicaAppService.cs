
using CalculadoraCalorias.Application.DTOs.Requests;
using CalculadoraCalorias.Application.DTOs.Responses;
using CalculadoraCalorias.Application.Interfaces;
using CalculadoraCalorias.Application.Mapping;
using CalculadoraCalorias.Core.Domain.Common;
using CalculadoraCalorias.Core.Domain.Interfaces;
using CalculadoraCalorias.Core.Domain.Services;

namespace CalculadoraCalorias.Application.Features
{
    public class AtividadeFisicaAppService(
        IAtividadeFisicaService _atividadeFisicaService,
        AtividadeFisicaMapper _atividadeFisicaMapper, 
        IUnitOfWork _unitOfWork,
        IContextoHttpService contextoHttpService) : AppServiceBase(contextoHttpService), IAtividadeFisicaAppService
    {


        public async Task<Resultado<AtividadeFisicaResponse>> Simular(CriarEstimativaAtividadeFisicaRequest requisicao)
        {
            var atividade = await _atividadeFisicaService.Simular(UsuarioId,
                                                                        requisicao.Tipo,
                                                                        requisicao.KilometragemPercorrida,
                                                                        requisicao.TempoDeExercicio);

            if (atividade == null) return Resultado<AtividadeFisicaResponse>.Failure(TipoDeErro.SystemFailure, "Erro ao simular atividade");

            return Resultado<AtividadeFisicaResponse>.Success(_atividadeFisicaMapper.EntidadeParaResponse(atividade));
        }

        public async Task<Resultado<AtividadeFisicaResponse>> EstimarGastoCalorico(CriarEstimativaAtividadeFisicaRequest requisicao)
        {
            return Resultado<AtividadeFisicaResponse>.Failure(TipoDeErro.None, "Não implementado");
        }

        public async Task<Resultado<List<AtividadeFisicaResponse>>> ObterTodos()
        {
            var atividades = await _atividadeFisicaService.ObterTodosPorId((int)UsuarioId);
            return Resultado<List<AtividadeFisicaResponse>>.Success(_atividadeFisicaMapper.EntidadesParaResponse(atividades));
        }

        public async Task<Resultado> Excluir(int id)
        {
            if (id == 0) return Resultado.Failure(TipoDeErro.Validation, "id não foi informado");

            var excluido = await _atividadeFisicaService.Excluir(id);

            if (!excluido) return Resultado.Failure(TipoDeErro.Validation, "registro não encontrado");

            await _unitOfWork.CommitAsync();
            return Resultado.Success();
        }

        public async Task<Resultado<AtividadeFisicaResponse>> ObterPorID(int id)
        {
            if (id == 0) return Resultado<AtividadeFisicaResponse>.Failure(TipoDeErro.Validation, "id não foi informado");

            var atividade = await _atividadeFisicaService.ObterPorId(id);
            if (atividade == null) return Resultado<AtividadeFisicaResponse>.Failure(TipoDeErro.NotFound, "Registro não encontrado");

            return Resultado<AtividadeFisicaResponse>.Success(_atividadeFisicaMapper.EntidadeParaResponse(atividade));
        }

        public async Task<Resultado<AtividadeFisicaResponse>> Atualizar(AtualizarAtividadeFisicaRequest requisicao)
        {
            var atividade = await _atividadeFisicaService.Atualizar(requisicao.Id,
                                                                        requisicao.Tipo,
                                                                        requisicao.TempoDeExercicio,
                                                                        requisicao.CaloriasEstimadas);

            if (atividade == null)
            {
                return Resultado<AtividadeFisicaResponse>.Failure(TipoDeErro.NotFound, "erro ao atualizar");
            }

            await _unitOfWork.CommitAsync();
            return Resultado<AtividadeFisicaResponse>.Success(_atividadeFisicaMapper.EntidadeParaResponse(atividade));
        }

        public async Task<Resultado<AtividadeFisicaResponse>> Adicionar(CriarAtividadeFisicaRequest requisicao)
        {
            var atividade = await _atividadeFisicaService.Adicionar(UsuarioId,
                                                                        requisicao.CaloriasEstimadas,
                                                                        requisicao.Tipo,
                                                                        requisicao.TempoDeExercicio,
                                                                        requisicao.DataDoExercicio);

            if (atividade == null)
            {
                return Resultado<AtividadeFisicaResponse>.Failure(TipoDeErro.None, "erro ao criar a atividade fisica");
            }

            await _unitOfWork.CommitAsync();
            return Resultado<AtividadeFisicaResponse>.Success(_atividadeFisicaMapper.EntidadeParaResponse(atividade));
        }
    }
}
