
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
        IUnitOfWork _unitOfWork) : IAtividadeFisicaAppService
    {


        public async Task<Resultado<AtividadeFisicaResponse>> Simular(CriarEstimativaAtividadeFisicaRequest requisicao)
        {
            var atividade = await _atividadeFisicaService.Simular(requisicao.UsuarioId,
                                                                        requisicao.Tipo,
                                                                        requisicao.KilometragemPercorrida,
                                                                        requisicao.TempoDeExercicio);

            return Resultado<AtividadeFisicaResponse>.Success(_atividadeFisicaMapper.EntidadeParaResponse(atividade));
        }

        public async Task<Resultado<AtividadeFisicaResponse>> EstimarGastoCalorico(CriarEstimativaAtividadeFisicaRequest requisicao)
        {
            //var atividade = await _atividadeFisicaService.Adicionar(requisicao.UsuarioId,
            //                                                            requisicao.Tipo,
            //                                                            requisicao.KilometragemPercorrida,
            //                                                            requisicao.TempoDeExercicio);

            //if (atividade == null)
            //{
            //    return Resultado<AtividadeFisicaResponse>.Failure(TipoDeErro.None, "erro ao criar");
            //}

            //await _unitOfWork.CommitAsync();
            return Resultado<AtividadeFisicaResponse>.Success(_atividadeFisicaMapper.EntidadeParaResponse(null));
        }

        public async Task<Resultado<List<AtividadeFisicaResponse>>> ObterTodosPorId(int idUsuario)
        {
            if (idUsuario == 0)
            {
                return Resultado<List<AtividadeFisicaResponse>>.Failure(TipoDeErro.Validation, "id do usuario não foi informado");
            }
            return Resultado<List<AtividadeFisicaResponse>>.Success(
                                                                    _atividadeFisicaMapper.EntidadesParaResponse(
                                                                    await _atividadeFisicaService.ObterTodosPorId(idUsuario)));
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
            if (atividade == null) Resultado<AtividadeFisicaResponse>.Failure(TipoDeErro.NotFound, "Registro não encontrado");

            return Resultado<AtividadeFisicaResponse>.Success(_atividadeFisicaMapper.EntidadeParaResponse(atividade));
        }

        public async Task<Resultado<object>> Atualizar(AtualizarAtividadeFisicaRequest requisicao)
        {
            var atividade = await _atividadeFisicaService.Atualizar(requisicao.Id,
                                                                        requisicao.Tipo,
                                                                        requisicao.KilometragemPercorrida,
                                                                        requisicao.TempoDeExercicio);

            if (atividade == null)
            {
                return Resultado<AtividadeFisicaResponse>.Failure(TipoDeErro.NotFound, "erro ao atualizar");
            }

            await _unitOfWork.CommitAsync();
            return Resultado<AtividadeFisicaResponse>.Success(_atividadeFisicaMapper.EntidadeParaResponse(atividade));
        }

        public async Task<Resultado<AtividadeFisicaResponse>> Adicionar(CriarAtividadeFisicaRequest requisicao)
        {
            var atividade = await _atividadeFisicaService.Adicionar(requisicao.UsuarioId,
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
