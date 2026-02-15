using CalculadoraCalorias.Core.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CalculadoraCalorias.Application.DTOs.Requests
{
    public class CriarRefeicaoRequest
    {
        [Required(ErrorMessage = "O ID do usuário é obrigatório.")]
        [Range(1, long.MaxValue, ErrorMessage = "ID do usuário inválido.")]
        public long UsuarioId { get; set; }

        public int Peso { get; set; }

        [Required(ErrorMessage = "O tipo de refeicao deve ser informado.")]
        [EnumDataType(typeof(TipoRefeicaoEnum), ErrorMessage = "Tipo inválido.")]
        public TipoRefeicaoEnum Tipo { get; set; }

        [Required(ErrorMessage = "A Imagem deve ser informada.")]
        public required IFormFile Imagem { get; set; }
        public required DateOnly Data { get; set; }
    }
}
