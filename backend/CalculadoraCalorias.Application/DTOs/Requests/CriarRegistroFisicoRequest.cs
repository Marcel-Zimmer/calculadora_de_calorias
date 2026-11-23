using System.ComponentModel.DataAnnotations;

namespace CalculadoraCalorias.Application.DTOs.Requests
{
    public class CriarRegistroFisicoRequest
    {
        [Required(ErrorMessage = "O peso é obrigatório.")]
        [Range(20, 450, ErrorMessage = "O peso deve estar entre 20kg e 450kg.")]
        public decimal PesoKg { get; set; }

        public decimal? MetaCaloricaDiaria { get; set; }

    }
}
