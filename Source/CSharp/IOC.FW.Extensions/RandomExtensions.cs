using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IOC.FW.Extensions
{
    public static class RandomExtensions
    {
        /// <summary>
        /// Método responsável por randomizar valores em um range informado 
        /// </summary>
        /// <param name="randomizer">Objeto para extender a classe Random</param>
        /// <param name="minValue">Valor mínimo</param>
        /// <param name="maxValue">Valor máximo</param>
        /// <returns>Número aleatorio entre o minimo e máximo</returns>
        public static int RandomNext(this Random randomizer, int minValue, int maxValue)
        {
            if (minValue > maxValue)
                throw new ArgumentOutOfRangeException("minValue");

            if (minValue == maxValue)
                return minValue;

            long diff = maxValue - minValue;
            var _rng = RNGCryptoServiceProvider.Create();
            byte[] _uint32Buffer = new byte[4];

            while (true)
            {
                _rng.GetBytes(_uint32Buffer);
                uint rand = BitConverter.ToUInt32(_uint32Buffer, 0);

                long max = (1 + (long)uint.MaxValue);
                long remainder = max % diff;
                if (rand < max - remainder)
                {
                    return (int)(minValue + (rand % diff));
                }
            }
        }
    }
}