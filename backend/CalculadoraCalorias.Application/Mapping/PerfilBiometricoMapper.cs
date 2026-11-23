namespace CalculadoraCalorias.Application.Mapping
{
    using global::CalculadoraCalorias.Application.DTOs.Requests;
    using global::CalculadoraCalorias.Application.DTOs.Responses;
    using global::CalculadoraCalorias.Core.Domain.Entities;
    using Riok.Mapperly.Abstractions;

    [Mapper]
    public partial class PerfilBiometricoMapper
    {
        [MapperIgnoreSource(nameof(PerfilBiometrico.Usuario))]
        public partial CriarPerfilBiometricoResponse EntidadeParaResponse(PerfilBiometrico usuario);
    }
}