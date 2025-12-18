namespace CalculadoraCalorias.Application.Mapping
{
    using global::CalculadoraCalorias.Application.DTOs.Requests;
    using global::CalculadoraCalorias.Application.DTOs.Responses;
    using global::CalculadoraCalorias.Core.Domain.Entities;
    using Riok.Mapperly.Abstractions;

    [Mapper]
    public partial class UsuarioMapper
    {
        public partial CriarUsuarioResponse EntidadeParaResponse(Usuario usuario);

        public partial Usuario RequestToEntity(CriarUsuarioRequest request);

    }
}