using System.Threading.Tasks;
using CalculadoraCalorias.Application.DTOs.Responses;
using CalculadoraCalorias.Core.Domain.Common;

namespace CalculadoraCalorias.Application.Interfaces
{
    public interface INutrientesAppService
    {
        Task<Resultado<NutrientesResponse>> ObterNutrientesDiario(long usuarioId, System.DateTime? data = null);
        Task<Resultado<NutrientesResponse>> ObterNutrientesSemanal(long usuarioId, System.DateTime? data = null);
        Task<Resultado<NutrientesResponse>> ObterNutrientesMensal(long usuarioId, System.DateTime? data = null);
    }
}
