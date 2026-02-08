namespace CalculadoraCalorias.Application.Mapping
{
    using global::CalculadoraCalorias.Application.DTOs.Requests;
    using global::CalculadoraCalorias.Application.DTOs.Responses;
    using global::CalculadoraCalorias.Core.Domain.Entities;
    using Riok.Mapperly.Abstractions;

    [Mapper]
    public partial class PerfilBiometricoMapper
    {
        public partial CriarPerfilBiometricoResponse EntidadeParaResponse(PerfilBiometrico usuario);

        public partial PerfilBiometrico RequestToEntity(CriarPerfilBiometricoRequest request);
    }
}