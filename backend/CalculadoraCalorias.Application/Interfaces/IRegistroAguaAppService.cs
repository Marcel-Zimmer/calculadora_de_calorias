using CalculadoraCalorias.Application.DTOs.Requests;
using CalculadoraCalorias.Application.DTOs.Responses;
using CalculadoraCalorias.Core.Domain.Common;

namespace CalculadoraCalorias.Application.Interfaces
{
    public interface IRegistroAguaAppService
    {
        Task<Resultado<RegistroAguaResponse>> Adicionar(CriarRegistroAguaRequest request);
        Task<Resultado<List<RegistroAguaResponse>>> ObterDiarios(DateOnly? data = null);
        Task<Resultado<EstatisticasAguaResponse>> ObterEstatisticasSemanais(DateOnly? data = null);
        Task<Resultado<EstatisticasAguaResponse>> ObterEstatisticasMensais(DateOnly? data = null);
        Task<Resultado<bool>> Excluir(long id);
    }
}
