using CalculadoraCalorias.Core.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CalculadoraCalorias.Application.DTOs.Requests
{
    public class RegistroUsuarioRequest
    {
        // Usuario
        [Required(ErrorMessage = "O nome é obrigatório.")]
        public required string Nome { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
        public required string Email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
        public required string Senha { get; set; }

        // Perfil Biometrico
        [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "O gênero deve ser informado.")]
        public GeneroEnum Genero { get; set; }

        [Required(ErrorMessage = "A altura é obrigatória.")]
        [Range(50, 250, ErrorMessage = "A altura deve estar entre 50cm e 250cm.")]
        public int AlturaCm { get; set; }

        [Required(ErrorMessage = "O nível de atividade é obrigatório.")]
        public NivelAtividadeEnum NivelAtividade { get; set; }

        [Required(ErrorMessage = "O objetivo é obrigatório.")]
        public ObjetivoEnum Objetivo { get; set; }

        // Registro Fisico
        [Required(ErrorMessage = "O peso é obrigatório.")]
        [Range(20, 450, ErrorMessage = "O peso deve estar entre 20kg e 450kg.")]
        public decimal PesoKg { get; set; }

        public decimal? MetaCaloricaDiaria { get; set; }
    }
}
