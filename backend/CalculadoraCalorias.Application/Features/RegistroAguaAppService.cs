using CalculadoraCalorias.Application.DTOs.Requests;
using CalculadoraCalorias.Application.DTOs.Responses;
using CalculadoraCalorias.Application.Interfaces;
using CalculadoraCalorias.Core.Domain.Common;
using CalculadoraCalorias.Core.Domain.Interfaces;
using System.Globalization;

namespace CalculadoraCalorias.Application.Features
{
    public class RegistroAguaAppService(
        IRegistroAguaService _service,
        IRegistroFisicoService _registroFisicoService,
        IContextoHttpService contextoHttpService) : AppServiceBase(contextoHttpService), IRegistroAguaAppService
    {
        public async Task<Resultado<RegistroAguaResponse>> Adicionar(CriarRegistroAguaRequest request)
        {
            var data = request.Data ?? FusoHorario.ObterDataHojeBrasilia();
            var hora = request.Hora ?? TimeOnly.FromDateTime(FusoHorario.ObterDataHoraBrasilia());

            var registro = await _service.Adicionar(UsuarioId, request.QuantidadeMl, data, hora);

            return Resultado<RegistroAguaResponse>.Success(new RegistroAguaResponse
            {
                Id = registro.Id,
                QuantidadeMl = registro.QuantidadeMl,
                Data = registro.Data.ToString("yyyy-MM-dd"),
                Hora = registro.Hora.ToString("HH:mm")
            });
        }

        public async Task<Resultado<List<RegistroAguaResponse>>> ObterDiarios(DateOnly? data = null)
        {
            var registros = await _service.ObterDiariosPorUsuarioId(UsuarioId, data);

            var response = registros.Select(r => new RegistroAguaResponse
            {
                Id = r.Id,
                QuantidadeMl = r.QuantidadeMl,
                Data = r.Data.ToString("yyyy-MM-dd"),
                Hora = r.Hora.ToString("HH:mm")
            }).ToList();

            return Resultado<List<RegistroAguaResponse>>.Success(response);
        }

        public async Task<Resultado<EstatisticasAguaResponse>> ObterEstatisticasSemanais(DateOnly? data = null)
        {
            var dataRef = data?.ToDateTime(TimeOnly.MinValue) ?? DateTime.Today;
            int diff = (7 + (dataRef.DayOfWeek - DayOfWeek.Monday)) % 7;
            var inicioSemana = DateOnly.FromDateTime(dataRef.AddDays(-1 * diff));
            var fimSemana = inicioSemana.AddDays(6);

            var stats = await ObterDadosPorPeriodo(inicioSemana, fimSemana, true);
            return Resultado<EstatisticasAguaResponse>.Success(stats);
        }

        public async Task<Resultado<EstatisticasAguaResponse>> ObterEstatisticasMensais(DateOnly? data = null)
        {
            var dataRef = data ?? DateOnly.FromDateTime(DateTime.Today);
            var inicioMes = new DateOnly(dataRef.Year, dataRef.Month, 1);
            var fimMes = inicioMes.AddMonths(1).AddDays(-1);

            var stats = await ObterDadosPorPeriodo(inicioMes, fimMes, false);
            return Resultado<EstatisticasAguaResponse>.Success(stats);
        }

        private async Task<EstatisticasAguaResponse> ObterDadosPorPeriodo(DateOnly inicio, DateOnly fim, bool usarNomeDia)
        {
            var registroFisico = await _registroFisicoService.ObterPorIdUsuario(UsuarioId);
            var peso = registroFisico?.PesoKg ?? 70m; // Padrão 70kg se não houver registro
            var metaAgua = (int)(peso * 35); // 35ml por kg

            var registros = await _service.ObterPorPeriodo(UsuarioId, inicio, fim);

            var pontos = new List<AguaPontoResponse>();
            var cultura = new CultureInfo("pt-BR");
            int diasComDados = 0;

            for (var data = inicio; data <= fim; data = data.AddDays(1))
            {
                var legenda = usarNomeDia 
                    ? cultura.DateTimeFormat.GetDayName(data.ToDateTime(TimeOnly.MinValue).DayOfWeek)
                    : data.Day.ToString();

                var consumidoDia = registros.Where(r => r.Data == data).Sum(r => r.QuantidadeMl);

                if (consumidoDia > 0) diasComDados++;

                pontos.Add(new AguaPontoResponse
                {
                    Data = data.ToString("yyyy-MM-dd"),
                    Legenda = legenda,
                    QuantidadeMl = consumidoDia
                });
            }

            var totalConsumido = registros.Sum(r => r.QuantidadeMl);
            var mediaDiaria = diasComDados > 0 ? totalConsumido / diasComDados : 0;
            var diasNaMeta = pontos.Count(p => p.QuantidadeMl >= metaAgua);

            var maiorConsumo = pontos.Count > 0 ? pontos.Max(p => p.QuantidadeMl) : 0;
            var diaMaiorConsumo = maiorConsumo > 0 ? pontos.First(p => p.QuantidadeMl == maiorConsumo).Legenda : "-";

            return new EstatisticasAguaResponse
            {
                MetaAguaDiaria = metaAgua,
                TotalConsumido = totalConsumido,
                MediaDiaria = mediaDiaria,
                DiasNaMeta = diasNaMeta,
                TotalDias = pontos.Count,
                MaiorConsumo = maiorConsumo,
                DiaMaiorConsumo = diaMaiorConsumo,
                Pontos = pontos
            };
        }

        public async Task<Resultado<bool>> Excluir(long id)
        {
            var sucesso = await _service.Excluir(id);
            return Resultado<bool>.Success(sucesso);
        }
    }
}
