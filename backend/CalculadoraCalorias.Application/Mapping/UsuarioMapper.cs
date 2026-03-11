namespace CalculadoraCalorias.Application.Mapping
{
    using global::CalculadoraCalorias.Application.DTOs.Requests;
    using global::CalculadoraCalorias.Application.DTOs.Responses;
    using global::CalculadoraCalorias.Core.Domain.Entities;
    using Riok.Mapperly.Abstractions;

    [Mapper]
    public partial class UsuarioMapper
    {
        public partial CriarUsuarioResponse CriarUsuarioParaRespose(Usuario usuario);
        public partial LoginUsarioResponse LoginUsuarioParaResponse(Usuario usuario);

        //public partial Usuario RequestParaEntity(LoginRequest request);

    }
}