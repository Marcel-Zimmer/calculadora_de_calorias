using CalculadoraCalorias.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CalculadoraCalorias.Core.Domain.Entities
{
    public class AtividadeFisica
    {
        protected AtividadeFisica() { }
        public AtividadeFisica(long usuarioId, int tipoAtividadeId, TimeSpan tempoExercicio, DateOnly dataExercicio, decimal caloriasEstimadas)
        {
            UsuarioId = usuarioId;
            TipoAtividadeId = tipoAtividadeId;
            TempoExercicio = tempoExercicio;
            DataExercicio = dataExercicio;
            CaloriasEstimadas = caloriasEstimadas;
        }
        public AtividadeFisica(long usuarioId, int tipoAtividadeId, decimal pesoSnapshot, TimeSpan tempoExercicio, int kilometragem)
        {
            UsuarioId = usuarioId;
            TipoAtividadeId = tipoAtividadeId;
            PesoSnapshot = pesoSnapshot;
            TempoExercicio = tempoExercicio;
            VelocidadeMedia = CalcularVelocidade(kilometragem, tempoExercicio);
            CaloriasEstimadas = CalcularCalorias();
        }



        public int Id {  get; private set; }
        public long UsuarioId {  get; private set; }
        public int TipoAtividadeId {  get; private set; }
        public decimal? PesoSnapshot {  get; private set; }
        public decimal CaloriasEstimadas {  get; private set; }
        public double? VelocidadeMedia { get; private set; }
        public DateOnly DataExercicio {  get; private set; }
        public TimeSpan TempoExercicio {  get; private set; }
        public virtual Usuario? Usuario {  get; private set; }


        private static TimeSpan CalcularTempoDeExercicio(TimeOnly horaInicio, TimeOnly horaFim)
        {
            return horaFim - horaInicio;
        }
        private static double CalcularVelocidade(int kilometragem, TimeSpan tempoDeExercicio)
        {
            var tempoEmHoras = tempoDeExercicio.TotalHours;
            if (tempoEmHoras <= 0 || kilometragem <= 0) return 0;
            return kilometragem / tempoEmHoras;
        }

        private decimal ObterMetPelaVelocidade()
        {
            var tipo = (TipoExercicioEnum)TipoAtividadeId;
            return tipo switch
            {
                TipoExercicioEnum.Ciclismo => VelocidadeMedia switch
                {
                    < 16.0 => 4.0m,  // Lazer, devagar (< 10mph)
                    < 19.0 => 6.8m,  // Leve (10-12mph)
                    < 22.5 => 8.0m,  // Moderado (12-14mph)
                    < 25.5 => 10.0m, // Vigoroso (14-16mph)
                    < 30.0 => 12.0m, // Muito Vigoroso (16-19mph)
                    _ => 15.8m       // Corrida (> 19mph)
                },
                TipoExercicioEnum.Caminhada => VelocidadeMedia switch
                {
                    < 4.0 => 2.8m,
                    < 5.5 => 3.5m,
                    < 6.5 => 4.3m,
                    _ => 5.0m
                },
                TipoExercicioEnum.Corrida => VelocidadeMedia switch
                {
                    < 8.0 => 8.3m,
                    < 10.0 => 9.8m,
                    < 12.0 => 11.5m,
                    < 14.5 => 12.8m,
                    _ => 14.5m
                },
                TipoExercicioEnum.Boxe => 7.8m,
                TipoExercicioEnum.Musculacao => 5.0m,
                _ => 4.0m
            };
        }

        private decimal CalcularCalorias()
        {
            if (TempoExercicio <= TimeSpan.Zero || (PesoSnapshot ?? 0) <= 0) return 0;
            decimal metCalculado = ObterMetPelaVelocidade();
            return metCalculado * (PesoSnapshot ?? 0) * (decimal)TempoExercicio.TotalHours;
        }

        public void Atualizar(int tipo, TimeSpan tempoDeExercicio, decimal caloriasEstimadas)
        {
            TipoAtividadeId = tipo;
            TempoExercicio = tempoDeExercicio;
            CaloriasEstimadas = caloriasEstimadas;
        }
    }
}
