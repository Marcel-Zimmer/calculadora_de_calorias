
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
    public class PerfilBiometricoAppService(IPerfilBiometricoService perfilBiometricoService, PerfilBiometricoMapper perfilBiometrico) : IPerfilBiometricoAppService
    {
        private readonly IPerfilBiometricoService _perfilBiometricoService = perfilBiometricoService;
        private readonly PerfilBiometricoMapper _mapperPerfilBiometrico = perfilBiometrico;

        public async Task<Resultado<CriarPerfilBiometricoResponse>> Criar(CriarPerfilBiometricoRequest requisicao)
        {
            var perfil = await _perfilBiometricoService.Criar(requisicao.UsuarioId, 
                                                             requisicao.DataNascimento, 
                                                             requisicao.Genero, 
                                                             requisicao.AlturaCm, 
                                                             requisicao.NivelAtividade, 
                                                             requisicao.Objetivo); 

            return Resultado<CriarPerfilBiometricoResponse>.Success(_mapperPerfilBiometrico.EntidadeParaResponse(perfil));
        }
    }
}
