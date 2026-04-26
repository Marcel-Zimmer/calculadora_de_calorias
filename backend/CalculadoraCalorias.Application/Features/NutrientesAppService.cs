using CalculadoraCalorias.Application.DTOs.Responses;
using CalculadoraCalorias.Application.Interfaces;
using CalculadoraCalorias.Core.Domain.Common;
using CalculadoraCalorias.Core.Domain.Enums;
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

            // Cálculos de Metas usando NutrientesEnum como referência conceitual
            var metas = new Dictionary<NutrientesEnum, double>
            {
                { NutrientesEnum.Proteina, (metaCaloricaBase * 0.30) / 4.0 },
                { NutrientesEnum.Carboidrato, (metaCaloricaBase * 0.50) / 4.0 },
                { NutrientesEnum.Gordura, (metaCaloricaBase * 0.20) / 9.0 },
                { NutrientesEnum.Fibra, 25.0 },
                { NutrientesEnum.Acucar, (metaCaloricaBase * 0.10) / 4.0 }
            };
            
            var totais = new Dictionary<NutrientesEnum, double>
            {
                { NutrientesEnum.Proteina, refeicoes.Sum(r => r.Proteinas ?? 0) },
                { NutrientesEnum.Carboidrato, refeicoes.Sum(r => r.Carboidratos ?? 0) },
                { NutrientesEnum.Gordura, refeicoes.Sum(r => r.Gorduras ?? 0) },
                { NutrientesEnum.Fibra, refeicoes.Sum(r => r.Fibras ?? 0) },
                { NutrientesEnum.Acucar, refeicoes.Sum(r => r.Acucares ?? 0) }
            };

            var diasComRefeicao = refeicoes.Select(r => r.Data).Distinct().Count();
            
            // Função auxiliar para calcular consumo final (valor ou média)
            double CalcularValorFinal(NutrientesEnum tipo) 
                => periodo == "diario" ? totais[tipo] : (diasComRefeicao > 0 ? totais[tipo] / diasComRefeicao : 0);

            var response = new NutrientesResponse
            {
                Periodo = periodo,
                MetaProteinas = Math.Round(metas[NutrientesEnum.Proteina], 1),
                MetaCarboidratos = Math.Round(metas[NutrientesEnum.Carboidrato], 1),
                MetaGorduras = Math.Round(metas[NutrientesEnum.Gordura], 1),
                MetaFibras = Math.Round(metas[NutrientesEnum.Fibra], 1),
                LimiteAcucares = Math.Round(metas[NutrientesEnum.Acucar], 1),

                ConsumoProteinas = Math.Round(CalcularValorFinal(NutrientesEnum.Proteina), 1),
                ConsumoCarboidratos = Math.Round(CalcularValorFinal(NutrientesEnum.Carboidrato), 1),
                ConsumoGorduras = Math.Round(CalcularValorFinal(NutrientesEnum.Gordura), 1),
                ConsumoFibras = Math.Round(CalcularValorFinal(NutrientesEnum.Fibra), 1),
                ConsumoAcucares = Math.Round(CalcularValorFinal(NutrientesEnum.Acucar), 1),
            };

            // Preenche a lista detalhada usando a Enum
            foreach (NutrientesEnum nutriente in Enum.GetValues(typeof(NutrientesEnum)))
            {
                response.Detalhes.Add(new NutrienteDetalheResponse
                {
                    Tipo = nutriente,
                    Nome = nutriente.ToString(),
                    Meta = Math.Round(metas[nutriente], 1),
                    Valor = Math.Round(CalcularValorFinal(nutriente), 1),
                    IsLimite = nutriente == NutrientesEnum.Acucar
                });
            }

            // Cálculos de Macros (Usa totais brutos para o IF para evitar problemas com arredondamento)
            var bProt = CalcularValorFinal(NutrientesEnum.Proteina);
            var bCarb = CalcularValorFinal(NutrientesEnum.Carboidrato);
            var bGord = CalcularValorFinal(NutrientesEnum.Gordura);
            
            var calProt = bProt * 4.0;
            var calCarb = bCarb * 4.0;
            var calGord = bGord * 9.0;
            var totalCalMacros = calProt + calCarb + calGord;

            // Se houver qualquer consumo de macronutrientes, gera o objeto de Macros e Veredito
            if (totalCalMacros > 0.1)
            {
                response.Macros = new MacrosResponse
                {
                    CaloriasProteinas = Math.Round(calProt, 1),
                    CaloriasCarboidratos = Math.Round(calCarb, 1),
                    CaloriasGorduras = Math.Round(calGord, 1),
                    TotalCalorias = Math.Round(totalCalMacros, 0),
                    PercentualProteinas = Math.Round((calProt / totalCalMacros) * 100, 1),
                    PercentualCarboidratos = Math.Round((calCarb / totalCalMacros) * 100, 1),
                    PercentualGorduras = Math.Round((calGord / totalCalMacros) * 100, 1)
                };

                // Cálculo do Veredito baseado nos percentuais calculados
                var pProt = response.Macros.PercentualProteinas;
                var pCarb = response.Macros.PercentualCarboidratos;
                var pGord = response.Macros.PercentualGorduras;

                if (pProt > 25)
                    response.Veredito = new VereditoResponse { Titulo = "Perfil Anabólico", Descricao = "Sua dieta está priorizando a regeneração e construção muscular.", CorCss = "text-rose-600 bg-rose-50", Icone = "💪" };
                else if (pGord > 35)
                    response.Veredito = new VereditoResponse { Titulo = "Perfil Lipídico", Descricao = "Consumo elevado de gorduras. Atente-se à qualidade das fontes.", CorCss = "text-orange-600 bg-orange-50", Icone = "🥑" };
                else if (pCarb > 60)
                    response.Veredito = new VereditoResponse { Titulo = "Perfil Energético", Descricao = "Foco alto em carboidratos, ideal para dias de treinos intensos.", CorCss = "text-amber-600 bg-amber-50", Icone = "⚡" };
                else
                    response.Veredito = new VereditoResponse { Titulo = "Perfil Equilibrado", Descricao = "Excelente harmonia entre proteínas, carbos e gorduras.", CorCss = "text-emerald-600 bg-emerald-50", Icone = "⚖️" };
            }
            else if (periodo == "diario")
            {
                // Se for diário e não houver dados, podemos retornar um objeto vazio de veredito para não quebrar o layout se preferível
                // Mas o frontend já trata com *ngIf, então manter null está correto.
            }

            return Resultado<NutrientesResponse>.Success(response);
        }
    }
}
