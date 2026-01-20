namespace CalculadoraCalorias.Application.Mapping
{
    using global::CalculadoraCalorias.Application.DTOs.Requests;
    using global::CalculadoraCalorias.Application.DTOs.Responses;
    using global::CalculadoraCalorias.Core.Domain.Entities;
    using Riok.Mapperly.Abstractions;

    [Mapper]
    public partial class RegistroFisicoMapper
    {
        public partial CriarRegistroFisicoResponse EntidadeParaResponse(RegistroFisico registro);

       // public partial PerfilBiometrico RequestToEntity(CriarPerfilBiometricoRequest request);
    }
}