using CalculadoraCalorias.Core.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CalculadoraCalorias.Application.DTOs.Requests
{
    public class CriarPerfilBiometricoRequest
    {
        [Required(ErrorMessage = "O ID do usuário é obrigatório.")]
        [Range(1, long.MaxValue, ErrorMessage = "ID do usuário inválido.")]
        public long UsuarioId { get; set; }

        [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "O gênero deve ser informado.")]
        [EnumDataType(typeof(GeneroEnum), ErrorMessage = "Gênero inválido.")]
        public GeneroEnum Genero { get; set; }

        [Required(ErrorMessage = "A altura é obrigatória.")]
        [Range(50, 250, ErrorMessage = "A altura deve estar entre 50cm e 250cm.")]
        public int AlturaCm { get; set; }

        [Required(ErrorMessage = "O nível de atividade é obrigatório.")]
        [EnumDataType(typeof(NivelAtividadeEnum), ErrorMessage = "Nível de atividade inválido.")]
        public NivelAtividadeEnum NivelAtividade { get; set; }

        [Required(ErrorMessage = "O objetivo é obrigatório.")]
        [EnumDataType(typeof(ObjetivoEnum), ErrorMessage = "Objetivo inválido.")]
        public ObjetivoEnum Objetivo { get; set; }
    }
}
