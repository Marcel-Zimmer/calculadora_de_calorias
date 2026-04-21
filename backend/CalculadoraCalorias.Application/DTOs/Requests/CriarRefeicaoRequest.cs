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

        [MaxLength(100, ErrorMessage = "O apelido deve ter no máximo 100 caracteres.")]
        public string? Apelido { get; set; } 

        [Required(ErrorMessage = "O peso em gramas deve ser informado.")]
        [Range(1, 10000, ErrorMessage = "O peso deve estar entre 1g e 10kg.")]
        public int PesoEmGramas { get; set; }

        [Required(ErrorMessage = "O tipo de refeição deve ser informado.")]
        [EnumDataType(typeof(TipoRefeicaoEnum), ErrorMessage = "Tipo de refeição inválido.")]
        public TipoRefeicaoEnum Tipo { get; set; }

        [Required(ErrorMessage = "A data da refeição deve ser informada.")]
        public DateOnly Data { get; set; }

        public IFormFile? Imagem { get; set; }

        public long? CodigoRefeicaoModelo { get; set; }

        public int? CaloriasManuais { get; set; }
        public string? AlimentoManual { get; set; }
    }
}
