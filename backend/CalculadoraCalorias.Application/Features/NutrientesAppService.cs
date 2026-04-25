using System;
using System.Linq;
using System.Threading.Tasks;
using CalculadoraCalorias.Application.DTOs.Responses;
using CalculadoraCalorias.Application.Interfaces;
using CalculadoraCalorias.Core.Domain.Common;
using CalculadoraCalorias.Core.Domain.Interfaces;

namespace CalculadoraCalorias.Application.Features
{
    public class NutrientesAppService(
        IRefeicaoService _refeicaoService, 
        IRegistroFisicoService _registroFisicoService) : INutrientesAppService
    {
        public async Task<Resultado<NutrientesResponse>> ObterNutrientesDiario(long usuarioId, DateTime? data = null)
        {
            var dataReferencia = data ?? DateTime.Today;
            var dataFormatada = DateOnly.FromDateTime(dataReferencia);
            return await CalcularRelatorioNutrientes(usuarioId, dataFormatada, dataFormatada, "diario");
        }

        public async Task<Resultado<NutrientesResponse>> ObterNutrientesSemanal(long usuarioId, DateTime? data = null)
        {
            var dataReferencia = data ?? DateTime.Today;
            int diff = (7 + (dataReferencia.DayOfWeek - DayOfWeek.Monday)) % 7;
            var inicioSemana = DateOnly.FromDateTime(dataReferencia.AddDays(-1 * diff));
            var fimSemana = inicioSemana.AddDays(6);

            return await CalcularRelatorioNutrientes(usuarioId, inicioSemana, fimSemana, "semanal");
        }

        public async Task<Resultado<NutrientesResponse>> ObterNutrientesMensal(long usuarioId, DateTime? data = null)
        {
            var dataReferencia = data ?? DateTime.Today;
            var inicioMes = new DateOnly(dataReferencia.Year, dataReferencia.Month, 1);
            var fimMes = inicioMes.AddMonths(1).AddDays(-1);

            return await CalcularRelatorioNutrientes(usuarioId, inicioMes, fimMes, "mensal");
        }

        private async Task<Resultado<NutrientesResponse>> CalcularRelatorioNutrientes(long usuarioId, DateOnly inicio, DateOnly fim, string periodo)
        {
            if (usuarioId == 0) return Resultado<NutrientesResponse>.Failure(TipoDeErro.SystemFailure, "Id de usuário inválido");

            var registroFisico = await _registroFisicoService.ObterPorIdUsuario(usuarioId);
            if (registroFisico == null) return Resultado<NutrientesResponse>.Failure(TipoDeErro.SystemFailure, "Registro fisico null");

            var refeicoes = await _refeicaoService.ObterPorPeriodo(usuarioId, inicio, fim);
            
            var metaCaloricaBase = (double)(registroFisico.MetaCaloricaDiaria ?? 0);

            // Cálculos básicos (Meta diária)
            var metaProteinasDiaria = (metaCaloricaBase * 0.30) / 4.0;
            var metaCarboDiaria = (metaCaloricaBase * 0.50) / 4.0;
            var metaGorduraDiaria = (metaCaloricaBase * 0.20) / 9.0;
            var metaFibraDiaria = 25.0; // Padrão geral de mercado
            var limiteAcucarDiario = (metaCaloricaBase * 0.10) / 4.0; // 10% máximo da caloria diária
            
            double totalConsumoProteinas = refeicoes.Sum(r => r.Proteinas ?? 0);
            double totalConsumoCarbo = refeicoes.Sum(r => r.Carboidratos ?? 0);
            double totalConsumoGordura = refeicoes.Sum(r => r.Gorduras ?? 0);
            double totalConsumoFibras = refeicoes.Sum(r => r.Fibras ?? 0);
            double totalConsumoAcucares = refeicoes.Sum(r => r.Acucares ?? 0);

            var diasComRefeicao = refeicoes.Select(r => r.Data).Distinct().Count();
            
            // Para as médias de consumo e a meta diária (quando semanal/mensal for analisado usando médias)
            var mediaConsumoProteinas = diasComRefeicao > 0 ? (totalConsumoProteinas / diasComRefeicao) : 0;
            var mediaConsumoCarbo = diasComRefeicao > 0 ? (totalConsumoCarbo / diasComRefeicao) : 0;
            var mediaConsumoGordura = diasComRefeicao > 0 ? (totalConsumoGordura / diasComRefeicao) : 0;
            var mediaConsumoFibras = diasComRefeicao > 0 ? (totalConsumoFibras / diasComRefeicao) : 0;
            var mediaConsumoAcucares = diasComRefeicao > 0 ? (totalConsumoAcucares / diasComRefeicao) : 0;

            var response = new NutrientesResponse
            {
                Periodo = periodo,
                MetaProteinas = Math.Round(metaProteinasDiaria, 1),
                MetaCarboidratos = Math.Round(metaCarboDiaria, 1),
                MetaGorduras = Math.Round(metaGorduraDiaria, 1),
                MetaFibras = Math.Round(metaFibraDiaria, 1),
                LimiteAcucares = Math.Round(limiteAcucarDiario, 1),

                // Para visualização clara em todos os quadros (Diário, Semanal, Mensal), usaremos a MÉDIA diária
                // Isso padroniza com a mesma decisão tomada para o total de consumo/gastos nos outros relatórios
                ConsumoProteinas = periodo == "diario" ? Math.Round(totalConsumoProteinas, 1) : Math.Round(mediaConsumoProteinas, 1),
                ConsumoCarboidratos = periodo == "diario" ? Math.Round(totalConsumoCarbo, 1) : Math.Round(mediaConsumoCarbo, 1),
                ConsumoGorduras = periodo == "diario" ? Math.Round(totalConsumoGordura, 1) : Math.Round(mediaConsumoGordura, 1),
                ConsumoFibras = periodo == "diario" ? Math.Round(totalConsumoFibras, 1) : Math.Round(mediaConsumoFibras, 1),
                ConsumoAcucares = periodo == "diario" ? Math.Round(totalConsumoAcucares, 1) : Math.Round(mediaConsumoAcucares, 1),
            };

            return Resultado<NutrientesResponse>.Success(response);
        }
    }
}
