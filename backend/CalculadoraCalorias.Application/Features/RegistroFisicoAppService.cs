
using CalculadoraCalorias.Application.DTOs.Requests;
using CalculadoraCalorias.Application.DTOs.Responses;
using CalculadoraCalorias.Application.Interfaces;
using CalculadoraCalorias.Application.Mapping;
using CalculadoraCalorias.Core.Domain.Common;
using CalculadoraCalorias.Core.Domain.Entities;
using CalculadoraCalorias.Core.Domain.ExcecoesPersonalizadas;
using CalculadoraCalorias.Core.Domain.Interfaces;

namespace CalculadoraCalorias.Application.Features
{
    public class RegistroFisicoAppService(IRegistroFisicoService registroFisicoService, RegistroFisicoMapper registroFisicoMapper, IUnitOfWork unitOfWork) : IRegistroFisicoAppService
    {
        private readonly IRegistroFisicoService _registroFisicoService = registroFisicoService;
        private readonly RegistroFisicoMapper _mapperRegistroFisico = registroFisicoMapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Resultado<CriarRegistroFisicoResponse>> Adicionar(CriarRegistroFisicoRequest requisicao)
        {
            var registroFisico = await _registroFisicoService.Adicionar(requisicao.UsuarioId,
                                                                       requisicao.PesoKg,
                                                                       requisicao.MetaCaloricaDiaria);

            if (registroFisico == null)
            {
                return Resultado<CriarRegistroFisicoResponse>.Failure(TipoDeErro.NotFound, "Dados não encontrados");
            }

            await _unitOfWork.CommitAsync();
            return Resultado<CriarRegistroFisicoResponse>.Success(_mapperRegistroFisico.EntidadeParaResponse(registroFisico));
        }


    }
}
