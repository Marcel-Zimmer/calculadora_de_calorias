using CalculadoraCalorias.Application.DTOs.Responses;
using CalculadoraCalorias.Application.Interfaces;
using CalculadoraCalorias.Core.Domain.Common;
using CalculadoraCalorias.Core.Domain.Interfaces;

namespace CalculadoraCalorias.Application.Features
{
    public class GraficoAppService(
        IRefeicaoService _refeicaoService, 
        IAtividadeFisicaService _atividadeFisicaService,
        IRegistroFisicoService _registroFisicoService) : IGraficoAppService
    {
       
        public async Task<Resultado<RefeicaoGraficoDiarioResponse>> GraficoDiario(long usuarioId)
        {
            if(usuarioId == 0) return Resultado<RefeicaoGraficoDiarioResponse>.Failure(TipoDeErro.SystemFailure, "Id de usuário inválido");

            var registroFisico = await _registroFisicoService.ObterPorIdUsuario(usuarioId);
            if (registroFisico == null) return Resultado<RefeicaoGraficoDiarioResponse>.Failure(TipoDeErro.SystemFailure, "Registro fisico null");

            var refeicoes = await _refeicaoService.ObterDiariasPorUsuarioId(usuarioId);
            var atividades = await _atividadeFisicaService.ObterDiariasPorUsuarioId(usuarioId);

            var informacoesDiarias = new RefeicaoGraficoDiarioResponse
            {
                MetaCaloricaDiaria = registroFisico.MetaCaloricaDiaria ?? 0,
                TotalCaloriasConsumidas = refeicoes?.Sum(x => x.Calorias) ?? 0,
                TotalCaloriasGastas = atividades?.Sum(y => y.CaloriasEstimadas) ?? 0,
                Refeicoes = refeicoes ?? [], 
                Exercicios = atividades ?? []
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
