using System;

namespace IOC.FW.Extensions
{
    /// <summary>
    /// Classe destinada a armazenar os Extensions Methods do Framework
    /// </summary>
    public static class DoubleExtensions
    {
        /// <summary>
        /// Método auxiliar para truncar decimais
        /// </summary>
        /// <param name="value">Tipo de objeto a extender</param>
        /// <param name="decimalPlaces">Quantidade desejada de casas decimais</param>
        /// <returns>Valor truncado com a quantidade de decimais fornecida</returns>
        public static double Truncate(
            this double value,
            int decimalPlaces
        )
        {
            double integralValue = Math.Truncate(value);
            double fraction = value - integralValue;
            int factor = (int)Math.Pow(10, decimalPlaces);
            double truncatedFraction = Math.Truncate(fraction * factor) / factor;
            double result = integralValue + truncatedFraction;
            return result;
        }
    }
}