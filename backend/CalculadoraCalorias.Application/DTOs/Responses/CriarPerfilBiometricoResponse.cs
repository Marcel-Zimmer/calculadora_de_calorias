using CalculadoraCalorias.Core.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CalculadoraCalorias.Application.DTOs.Responses
{
    public class CriarPerfilBiometricoResponse
    {   
        public long Id { get; set; }
        public long UsuarioId { get; set; }
        public DateTime DataNascimento { get; set; }
        public GeneroEnum Genero { get; set; }
        public int AlturaCm {  get; set; }
        public NivelAtividadeEnum NivelAtividade { get; set; }
        public ObjetivoEnum Objetivo { get; set; }
    }
}
