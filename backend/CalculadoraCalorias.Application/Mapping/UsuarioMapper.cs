namespace CalculadoraCalorias.Application.Mapping
{
    using global::CalculadoraCalorias.Application.DTOs.Requests;
    using global::CalculadoraCalorias.Application.DTOs.Responses;
    using global::CalculadoraCalorias.Core.Domain.Entities;
    using Riok.Mapperly.Abstractions;

    [Mapper]
    public partial class UsuarioMapper
    {
        [MapperIgnoreTarget(nameof(CriarUsuarioResponse.Role))]
        [MapperIgnoreSource(nameof(Usuario.Role))]
        [MapperIgnoreSource(nameof(Usuario.Senha))]
        [MapperIgnoreSource(nameof(Usuario.RegistroFisico))]
        [MapperIgnoreSource(nameof(Usuario.PerfilBiometrico))]
        [MapperIgnoreSource(nameof(Usuario.Ativo))]
        public partial CriarUsuarioResponse CriarUsuarioParaRespose(Usuario usuario);

        [MapperIgnoreTarget(nameof(LoginUsarioResponse.AccessToken))]
        [MapperIgnoreTarget(nameof(LoginUsarioResponse.RefreshToken))]
        [MapperIgnoreSource(nameof(Usuario.Senha))]
        [MapperIgnoreSource(nameof(Usuario.RegistroFisico))]
        [MapperIgnoreSource(nameof(Usuario.PerfilBiometrico))]
        [MapperIgnoreSource(nameof(Usuario.Ativo))]
        public partial LoginUsarioResponse LoginUsuarioParaResponse(Usuario usuario);

        //public partial Usuario RequestParaEntity(LoginRequest request);

    }
}