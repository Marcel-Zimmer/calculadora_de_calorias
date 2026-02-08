
using CalculadoraCalorias.Application.DTOs.Requests;
using CalculadoraCalorias.Application.DTOs.Responses;
using CalculadoraCalorias.Application.Interfaces;
using CalculadoraCalorias.Application.Mapping;
using CalculadoraCalorias.Core.Domain.Common;
using CalculadoraCalorias.Core.Domain.Interfaces;

namespace CalculadoraCalorias.Application.Features
{
    public class AtividadeFisicaAppService(IAtividadeFisicaService atividadeFisicaService, AtividadeFisicaMapper atividadeFisicaMapper, IUnitOfWork unitOfWork) : IAtividadeFisicaAppService
    {
        private readonly IAtividadeFisicaService _atividadeFisicaService = atividadeFisicaService;
        private readonly AtividadeFisicaMapper _atividadeFisicaMapper = atividadeFisicaMapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Resultado<CriarAtividadeFisicaResponse>> Simular(SimularGastoCaloricoRequest requisicao)
        {
            var atividade = await _atividadeFisicaService.Simular(requisicao.UsuarioId,
                                                                        requisicao.Tipo, 
                                                                        requisicao.KilometragemPercorrida, 
                                                                        requisicao.TempoDeExercicio);

            return Resultado<CriarAtividadeFisicaResponse>.Success(_atividadeFisicaMapper.EntidadeParaResponse(atividade));
        }

        public async Task<Resultado<CriarAtividadeFisicaResponse>> Adicionar(SimularGastoCaloricoRequest requisicao)
        {
            var atividade = await _atividadeFisicaService.Adicionar(requisicao.UsuarioId,
                                                                        requisicao.Tipo,
                                                                        requisicao.KilometragemPercorrida,
                                                                        requisicao.TempoDeExercicio);

            if (atividade == null)
            {
                return Resultado<CriarAtividadeFisicaResponse>.Failure(TipoDeErro.None,"erro ao criar");
            }

            await _unitOfWork.CommitAsync();
            return Resultado<CriarAtividadeFisicaResponse>.Success(_atividadeFisicaMapper.EntidadeParaResponse(atividade));
        }
    }
}
