using CalculadoraCalorias.Core.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CalculadoraCalorias.Application.DTOs.Requests
{
    public class CriarEstimativaAtividadeFisicaRequest
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

    public class CriarAtividadeFisicaRequest
    {
        [Required(ErrorMessage = "O ID do usuário é obrigatório.")]
        [Range(1, long.MaxValue, ErrorMessage = "O ID do usuário deve ser um valor positivo.")]
        public long UsuarioId { get; set; }

        [Required(ErrorMessage = "A quantidade de calorias é obrigatória.")]
        [Range(0.1, double.MaxValue, ErrorMessage = "A quantidade de calorias deve ser maior que zero.")]
        public decimal CaloriasEstimadas { get; set; }

        [Required(ErrorMessage = "O tipo de exercício deve ser informado.")]
        [EnumDataType(typeof(TipoExercicioEnum), ErrorMessage = "O tipo de exercício selecionado é inválido.")]
        public TipoExercicioEnum Tipo { get; set; }

        [Required(ErrorMessage = "O tempo de exercício é obrigatório.")]
        public TimeSpan TempoDeExercicio { get; set; }

        [Required(ErrorMessage = "A data do exercício é obrigatória.")]
        public DateOnly DataDoExercicio { get; set; }
    }
}
