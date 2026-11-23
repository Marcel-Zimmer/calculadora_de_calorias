using System.Threading.Tasks;
using CalculadoraCalorias.Application.DTOs.Responses;
using CalculadoraCalorias.Core.Domain.Common;

namespace CalculadoraCalorias.Application.Interfaces
{
    public interface INutrientesAppService
    {
        Task<Resultado<NutrientesResponse>> ObterNutrientesDiario(System.DateTime? data = null);
        Task<Resultado<NutrientesResponse>> ObterNutrientesSemanal(System.DateTime? data = null);
        Task<Resultado<NutrientesResponse>> ObterNutrientesMensal(System.DateTime? data = null);
    }
}
