using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace IOC.FW.Core.Cripto
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
            // Create a new Stringbuilder to collect the bytes 
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();
            using (MD5 md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash. 
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Loop through each byte of the hashed data  
                // and format each one as a hexadecimal string. 
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
            }

            // Return the hexadecimal string. 
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
            // Hash the input. 
            string hashOfInput = string.Empty;

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            using (MD5 md5Hash = MD5.Create())
            {
                hashOfInput = GetHash(input);
            }

           return comparer.Compare(hashOfInput, hash) == 0;
        }
    }
}
