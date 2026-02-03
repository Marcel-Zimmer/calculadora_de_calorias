
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
    public class RegistroFisicoAppService(IRegistroFisicoService registroFisicoService, RegistroFisicoMapper registroFisicoMapper) : IRegistroFisicoAppService
    {
        private readonly IRegistroFisicoService _registroFisicoService = registroFisicoService;
        private readonly RegistroFisicoMapper _mapperRegistroFisico = registroFisicoMapper;

        public async Task<Resultado<CriarRegistroFisicoResponse>> Criar(CriarRegistroFisicoRequest requisicao)
        {
            var registroFisico = await _registroFisicoService.Criar(requisicao.UsuarioId,
                                                           requisicao.PerfilBiometricoId,
                                                           requisicao.PesoKg,
                                                           requisicao.MetaCaloricaDiaria);

            return Resultado<CriarRegistroFisicoResponse>.Success(_mapperRegistroFisico.EntidadeParaResponse(registroFisico));
        }


    }
}
