using CalculadoraCalorias.Core.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CalculadoraCalorias.Application.DTOs.Requests
{
    public class AtualizarAtividadeFisicaRequest
    {
        [Required(ErrorMessage = "O ID é obrigatório.")]
        [Range(1, long.MaxValue, ErrorMessage = "ID inválido.")]
        public long Id { get; set; }

        public int KilometragemPercorrida { get; set; }

        [Required(ErrorMessage = "O tipo de exercicio deve ser informado.")]
        [EnumDataType(typeof(TipoExercicioEnum), ErrorMessage = "Tipo inválido.")]
        public TipoExercicioEnum Tipo { get; set; }

        public TimeSpan TempoDeExercicio { get; set; }
    }
}
