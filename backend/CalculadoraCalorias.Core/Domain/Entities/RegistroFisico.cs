using System;
using System.Collections.Generic;
using System.Text;

namespace CalculadoraCalorias.Core.Domain.Entities
{
    public class RegistroFisico
    {
        protected RegistroFisico() { }
        public RegistroFisico(long usuarioId, 
                                long perfilBiometricoID,
                                DateTime dataRegistro,
                                decimal pesoKg,
                                decimal imcCalculado,
                                decimal taxaMetabolicaBasal)
        {
            UsuarioId = usuarioId;
            PerfilBiometricoId = perfilBiometricoID;
            DataRegistro = dataRegistro;
            PesoKg = pesoKg;
            ImcCalculado = imcCalculado;
            TaxaMetabolicaBasal = taxaMetabolicaBasal;

        }

        public long Id { get; private set; }
        public long UsuarioId { get; private set; }
        public long PerfilBiometricoId { get; private set; }
        public DateTime DataRegistro {  get; private set; }
        public decimal PesoKg { get; private set; }
        public decimal ImcCalculado { get; private set; }
        public decimal TaxaMetabolicaBasal {  get; private set; }
        public decimal? MetaCaloricaDiaria { get; private set; }
        public virtual Usuario? Usuario { get; private set; }
        public virtual PerfilBiometrico? PerfilBiometrico { get; private set; }

        public void AdicionarMetaCalorica(decimal meta) { 
            MetaCaloricaDiaria = meta;
        }
    }
}
