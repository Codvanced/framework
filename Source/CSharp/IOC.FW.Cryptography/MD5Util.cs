using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace IOC.FW.Cryptography
{
    /// <summary>
    /// Classe responsável por encriptação utilizando a técnica MD5
    /// </summary>
    public static class MD5Util
    {
        /// <summary>
        /// Método responsável por encriptar e gerar o Hash
        /// </summary>
        /// <param name="input">Texto puro</param>
        /// <returns>String contendo o hash</returns>
        public static string GetHash(string input)  
        {
            StringBuilder sBuilder = new StringBuilder();
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
            }

            return sBuilder.ToString();
        }

        /// <summary>
        /// Método utilizado para validar uma entrada com o hash gerado
        /// </summary>
        /// <param name="input">Texto puro</param>
        /// <param name="hash">Texto com hash</param>
        /// <returns>Validação da comparação de hash com string</returns>
        public static bool VerifyHash(string input, string hash)
        {
            string hashOfInput = string.Empty;

            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            using (MD5 md5Hash = MD5.Create())
            {
                hashOfInput = GetHash(input);
            }

           return comparer.Compare(hashOfInput, hash) == 0;
        }
    }
}
