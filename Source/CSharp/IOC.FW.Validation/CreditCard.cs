using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IOC.FW.Validation
{
    public class CreditCard
    {
        /// <summary>
        /// Método responsável por gerar digito verificador de cartão de crédito (luhn digit)
        /// </summary>
        /// <param name="number">Número para calcular o digito</param>
        /// <returns>Digito verificador</returns>
        public static int LuhnGenerate(string number)
        {
            Regex reg = new Regex(@"[\D\s]+");
            number = reg.Replace(number, string.Empty);
            bool test = false;
            int sum = 0;

            for (int i = 0; i < number.Length; i++)
            {
                int digit = Convert.ToInt32(number[i].ToString());
                if (test)
                {
                    digit *= 2;

                    if (digit > 9)
                        digit -= 9;
                }

                sum += digit;
                test = !test;
            }

            return ((sum * 9) % 10);
        }

        /// <summary>
        /// Método responsável por validar um digito verificador (Luhn digit)
        /// </summary>
        /// <param name="number">Numero com digito</param>
        /// <returns>Valor booleando indicando se o numero é valido</returns>
        public static bool CompareLuhnDigit(string number)
        {
            return LuhnGenerate(number) == 0;
        }
    }
}