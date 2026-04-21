using System;

namespace CalculadoraCalorias.Core.Domain.Common
{
    public static class FusoHorario
    {
        public static DateTime ObterDataHoraBrasilia()
        {
            try
            {
                var fuso = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");
                return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, fuso);
            }
            catch (TimeZoneNotFoundException)
            {
                var fuso = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
                return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, fuso);
            }
        }

        public static DateOnly ObterDataHojeBrasilia()
        {
            return DateOnly.FromDateTime(ObterDataHoraBrasilia());
        }
    }
}
