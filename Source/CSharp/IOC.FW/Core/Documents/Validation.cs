﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
<<<<<<< HEAD
<<<<<<< HEAD
using IOC.FW.Core;
=======
using IOC.FW.Core.Extensions;
>>>>>>> 77fa2ca... Implementação de extensions de operações entre expressions
=======
using IOC.FW.Core;
>>>>>>> f898fdb... remanejamento de pasta

namespace IOC.FW.Core.Documents
{
    /// <summary>
    /// Classe responsável por validar documentos
    /// </summary>
    public static class Validation
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
        
        /// <summary>
        /// Método responsável por gerar os digitos verificadores de CPF
        /// </summary>
        /// <param name="cpf">Número de cpf sem os digitos</param>
        /// <returns>CPF com os digitos</returns>
        public static string CpfDigits(string cpf)
        {
            Regex reg = new Regex(@"[\D\s]+");
            cpf = reg.Replace(cpf, string.Empty);

            if (!string.IsNullOrEmpty(cpf)
                && cpf.Length == 9)
            {
                int multiplier = 10;
                int sum = 0;
                int times = 2;
                int tempCalc = 0;

                while (times > 0)
                {
                    for (int i = 0; i < cpf.Length; i++)
                    {
                        int cpfDigit = 0;
                        int.TryParse(cpf[i].ToString(), out cpfDigit);
                        sum += (cpfDigit * multiplier--);
                    }

                    if (multiplier <= 2)
                    {
                        tempCalc = (sum % 11);
                        cpf += tempCalc < 2
                            ? "0"
                            : (11 - tempCalc).ToString();

                        multiplier = 11;
                        times--;
                        sum = 0;
                    }
                }
            }

            return cpf;
        }

        /// <summary>
        /// Método responsável por validar CPF
        /// </summary>
        /// <param name="cpf">Número de cpf sem os digitos</param>
        /// <returns>Indica se o CPF é válido</returns>
        public static bool ValidateCpf(string cpf)
        {
            bool isValid = false;
            Regex reg = new Regex(@"[\D\s]+");
            cpf = reg.Replace(cpf, string.Empty);

            string[] blackList = new string[] { 
                "00000000000",
                "11111111111",
                "22222222222",
                "33333333333",
                "44444444444",
                "55555555555",
                "66666666666",
                "77777777777",
                "88888888888",
                "99999999999",
            };

            isValid = !blackList.Contains(cpf);

            if (isValid 
                && !string.IsNullOrEmpty(cpf)
                && cpf.Length == 11)
            {
                string realCpf = CpfDigits(
                    cpf.Remove(cpf.Length - 2, 2)
                );

                isValid = !string.IsNullOrEmpty(realCpf)
                    && cpf.Equals(realCpf);
            }

            return isValid;
        }

        /// <summary>
        /// Método responsável por gerar CPF para testes
        /// </summary>
        /// <returns>Número válido de CPF</returns>
        public static string GenerateCpf()
        {
            var rand = new Random();
            
            string cpf = string.Empty;

            for (int i = 0; i < 9; i++)
            {
                int randomNumber = rand.RandomNext(0, 9);
                cpf = string.Format("{0}{1}", cpf, randomNumber.ToString()); 
            }

            cpf = CpfDigits(cpf);
            return cpf;
        }
    }
}