using CalculadoraCalorias.Application.DTOs.Requests;
using CalculadoraCalorias.Application.DTOs.Responses;
using CalculadoraCalorias.Core.Domain.Common;

namespace CalculadoraCalorias.Application.Interfaces
{
    public interface IRegistroAguaAppService
    {
        Task<Resultado<RegistroAguaResponse>> Adicionar(long usuarioId, CriarRegistroAguaRequest request);
        Task<Resultado<List<RegistroAguaResponse>>> ObterDiarios(long usuarioId, DateOnly? data = null);
        Task<Resultado<EstatisticasAguaResponse>> ObterEstatisticasSemanais(long usuarioId, DateOnly? data = null);
        Task<Resultado<EstatisticasAguaResponse>> ObterEstatisticasMensais(long usuarioId, DateOnly? data = null);
        Task<Resultado<bool>> Excluir(long id);
    }
}
