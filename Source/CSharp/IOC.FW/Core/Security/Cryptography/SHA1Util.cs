using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Diagnostics.Contracts;

namespace IOC.FW.Core.Security.Cryptography
{
    /// <summary>
    /// Classe responsável por encriptação utilizando a técnica SHA1
    /// </summary>
    public static class SHA1Util
    {
        /// <summary>
        /// Método responsável por encriptar 
        /// </summary>
        /// <param name="plainTextString">Texto puro</param>
        /// <returns>Texto encriptado</returns>
        [Pure]
        public static string GenerateSHA1(string plainTextString)
        {
            return GenerateSHA1(plainTextString, null);
        }

        /// <summary>
        /// Método responsável por encriptar 
        /// </summary>
        /// <param name="plainTextString">Texto puro</param>
        /// <param name="salt">Array de bytes que representa chave de criptografia</param>
        /// <returns>Texto encriptado</returns>
        [Pure]
        public static string GenerateSHA1(string plainTextString, byte[] salt)
        {
            if (string.IsNullOrWhiteSpace(plainTextString))
            {
                throw new ArgumentNullException("plainTextString", "the string to be hashed needs to be different then null");
            }

            salt = salt == null 
                ? new byte[0] 
                : salt;

            HashAlgorithm algorithm = new SHA1Managed();
            var plainTextBytes = Encoding.UTF8.GetBytes(plainTextString);

            var plainTextWithSaltBytes = AppendByteArray(plainTextBytes, salt);
            var saltedSHA1 = BitConverter.ToString(
                SHA1.Create().ComputeHash(
                    plainTextWithSaltBytes
                )
            )
            .Replace("-", string.Empty); ;

            return saltedSHA1;
        }
        
        /// <summary>
        /// Método responsável por criar o array de bytes utilizado para encriptação
        /// </summary>
        /// <param name="saltSize">Quantidade de itens no array de bytes</param>
        /// <returns>Array de bytes preenchido com valores randomizados que representa chave de criptografia</returns>
        public static byte[] GenerateSalt(int saltSize)
        {
            var rng = new RNGCryptoServiceProvider();
            var buff = new byte[saltSize];
            rng.GetBytes(buff);
            return buff;
        }
        
        /// <summary>
        /// Método responsável por unificar dois byte arrays
        /// </summary>
        /// <param name="byteArray1">Primeiro array</param>
        /// <param name="byteArray2">Segundo array</param>
        /// <returns>Array unificado</returns>
        private static byte[] AppendByteArray(byte[] byteArray1, byte[] byteArray2)
        {
            var byteArrayResult = new byte[byteArray1.Length + byteArray2.Length];

            for (var i = 0; i < byteArray1.Length; i++)
                byteArrayResult[i] = byteArray1[i];
            for (var i = 0; i < byteArray2.Length; i++)
                byteArrayResult[byteArray1.Length + i] = byteArray2[i];

            return byteArrayResult;
        }
        
        /// <summary>
        /// Método responsável por verificar se uma entrada bate com um conteúdo encriptado
        /// </summary>
        /// <param name="input">Entrada em texto puro</param>
        /// <param name="hash">Texto criptografado a fim de comparação</param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static bool VerifyHash(string input, string hash, byte[] salt)
        {
            string hashOfInput = string.Empty;

            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            string hashToCompare = GenerateSHA1(input, salt);

            return comparer.Compare(hashToCompare, hash) == 0;
        }

        /// <summary>
        /// Método responsável por verificar se uma entrada bate com um conteúdo encriptado
        /// </summary>
        /// <param name="input">Entrada em texto puro</param>
        /// <param name="hash">Texto criptografado a fim de comparação</param>
        /// <returns></returns>
        public static bool VerifyHash(string input, string hash)
        {
            string hashOfInput = string.Empty;

            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            string hashToCompare = GenerateSHA1(input);

            return comparer.Compare(hashToCompare, hash) == 0;
        }
    }
}
