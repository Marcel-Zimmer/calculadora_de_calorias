using CalculadoraCalorias.Core.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CalculadoraCalorias.Application.DTOs.Requests
{
    public class CriarAtividadeFisicaRequest
    {
        [Required(ErrorMessage = "O ID do usuário é obrigatório.")]
        [Range(1, long.MaxValue, ErrorMessage = "ID do usuário inválido.")]
        public long UsuarioId { get; set; }

        public int KilometragemPercorrida { get; set; }

        [Required(ErrorMessage = "O tipo de exercicio deve ser informado.")]
        [EnumDataType(typeof(TipoExercicioEnum), ErrorMessage = "Tipo inválido.")]
        public TipoExercicioEnum Tipo { get; set; }

        public TimeSpan TempoDeExercicio { get; set; }
    }
}
