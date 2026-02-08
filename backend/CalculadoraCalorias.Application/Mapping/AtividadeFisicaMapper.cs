namespace CalculadoraCalorias.Application.Mapping
{
    using global::CalculadoraCalorias.Application.DTOs.Requests;
    using global::CalculadoraCalorias.Application.DTOs.Responses;
    using global::CalculadoraCalorias.Core.Domain.Entities;
    using Riok.Mapperly.Abstractions;

    [Mapper]
    public partial class AtividadeFisicaMapper

    {
        public partial AtividadeFisicaResponse EntidadeParaResponse(AtividadeFisica usuario);
        public partial List<AtividadeFisicaResponse> EntidadesParaResponse(List<AtividadeFisica> atividades);

        //public partial AtividadeFisica RequestToEntity(CriarPerfilBiometricoRequest request);
    }
}