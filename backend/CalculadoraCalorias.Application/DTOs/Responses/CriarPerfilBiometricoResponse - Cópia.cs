using CalculadoraCalorias.Core.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CalculadoraCalorias.Application.DTOs.Responses
{
    public class CriarRegistroFisicoResponse
    {   
        public long Id { get; set; }
        public long UsuarioId { get; set; }
        public long PerfilBiometricoId { get; set; }
        public decimal ImcCalculado { get; set; }
        public decimal TaxaMetabolicaBasal {  get; set; }

    }
}
