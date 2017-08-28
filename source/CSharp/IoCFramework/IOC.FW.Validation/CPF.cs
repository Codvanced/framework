using IOC.FW.Extensions;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace IOC.FW.Validation
{
    public class CPF
    {
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