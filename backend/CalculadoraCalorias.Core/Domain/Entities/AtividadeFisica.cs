using System;
using System.Collections.Generic;
using System.Text;

namespace CalculadoraCalorias.Core.Domain.Entities
{
    public class AtividadeFisica
    {
        protected AtividadeFisica() { }

        public AtividadeFisica(long usuarioId, int tipoAtividadeId, decimal pesoSnapshot, TimeSpan tempoExercicio, int kilometragem)
        {
            UsuarioId = usuarioId;
            TipoAtividadeId = tipoAtividadeId;
            PesoSnapshot = pesoSnapshot;
            TempoExercicio = tempoExercicio;
            DataExercicio = DateTime.UtcNow;
            VelocidadeMedia = CalcularVelocidade(kilometragem, tempoExercicio);
            CaloriasCalculadas = CalcularCalorias();
        }

        public int Id {  get; private set; }
        public long UsuarioId {  get; private set; }
        public int TipoAtividadeId {  get; private set; }
        public decimal PesoSnapshot {  get; private set; }
        public decimal CaloriasCalculadas {  get; private set; }
        public double VelocidadeMedia { get; private set; }
        public DateTime DataExercicio {  get; private set; }
        public TimeSpan TempoExercicio {  get; private set; }
        public virtual Usuario? Usuario {  get; private set; }


        private static TimeSpan CalcularTempoDeExercicio(TimeOnly horaInicio, TimeOnly horaFim)
        {
            return horaFim - horaInicio;
        }
        private static double CalcularVelocidade(int kilometragem, TimeSpan tempoDeExercicio)
        {
            var tempoEmHoras = tempoDeExercicio.TotalHours;
            return (double)kilometragem / tempoEmHoras;
        }

        private decimal ObterMetPelaVelocidade()
        {
            return VelocidadeMedia switch
            {
                < 16.0 => 4.0m,  // Lazer, devagar (< 10mph)
                < 19.0 => 6.8m,  // Leve (10-12mph)
                < 22.5 => 8.0m,  // Moderado (12-14mph)
                < 25.5 => 10.0m, // Vigoroso (14-16mph)
                < 30.0 => 12.0m, // Muito Vigoroso (16-19mph)
                _ => 15.8m       // Corrida (> 20mph)
            };
        }

        private decimal CalcularCalorias()
        {
            decimal metCalculado = ObterMetPelaVelocidade();
            return metCalculado * PesoSnapshot * (decimal)TempoExercicio.TotalHours;
        }
    }
    

}
