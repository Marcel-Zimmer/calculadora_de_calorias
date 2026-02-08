
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
    public class PerfilBiometricoAppService(IPerfilBiometricoService perfilBiometricoService, PerfilBiometricoMapper perfilBiometrico, IUnitOfWork unitOfWork) : IPerfilBiometricoAppService
    {
        private readonly IPerfilBiometricoService _perfilBiometricoService = perfilBiometricoService;
        private readonly PerfilBiometricoMapper _mapperPerfilBiometrico = perfilBiometrico;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Resultado<CriarPerfilBiometricoResponse>> Adicionar(CriarPerfilBiometricoRequest requisicao)
        {
            var perfil = await _perfilBiometricoService.Adicionar(requisicao.UsuarioId, 
                                                                     requisicao.DataNascimento, 
                                                                     requisicao.Genero, 
                                                                     requisicao.AlturaCm, 
                                                                     requisicao.NivelAtividade, 
                                                                     requisicao.Objetivo);

            await _unitOfWork.CommitAsync();
            return Resultado<CriarPerfilBiometricoResponse>.Success(_mapperPerfilBiometrico.EntidadeParaResponse(perfil));
        }
    }
}
