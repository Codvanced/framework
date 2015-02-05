using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace IOC.FW.Core.Cripto
{
    /// <summary>
    /// Classe com o propósito de facilitar a utilização de AES
    /// </summary>
    public static class AESUtil
    {
        /// <summary>
        /// Método auxiliar para obter chaves para encriptar e decriptar utilizando AES
        /// </summary>
        /// <returns>Array com chaves de criptografia, index:0 = KEY, index:1 = IV</returns>
        public static IList<byte[]> Keys() 
        {
            IList<byte[]> myKeys = new List<byte[]>();
            using (var aes = Aes.Create())
            {
                myKeys.Add(aes.Key);
                myKeys.Add(aes.IV);
            }

            return myKeys;
        }

        /// <summary>
        /// Método responsável por encriptar textos
        /// </summary>
        /// <param name="plainText">Texto puro</param>
        /// <param name="Key">Chave (key)</param>
        /// <param name="IV">Chave (iv)</param>
        /// <returns>Array de bytes com texto encriptado</returns>
        public static byte[] Encrypt(
            string plainText,
            byte[] Key,
            byte[] IV
        )
        {
            // Check arguments. 
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");
            byte[] encrypted;
            
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(
                    aesAlg.Key,
                    aesAlg.IV
                );

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(
                            msEncrypt,
                            encryptor,
                            CryptoStreamMode.Write
                        )
                    )
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return encrypted;
        }

        /// <summary>
        /// Método responsável por decriptar textos
        /// </summary>
        /// <param name="cipherText">Texto cifrado</param>
        /// <param name="Key">Chave (key)</param>
        /// <param name="IV">Chave (iv)</param>
        /// <returns>String com texto decriptado</returns>
        public static string Decrypt(
            byte[] cipherText, 
            byte[] Key, 
            byte[] IV
        )
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");

            string plaintext = null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(
                    aesAlg.Key,
                    aesAlg.IV
                );

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(
                            msDecrypt,
                            decryptor,
                            CryptoStreamMode.Read
                        )
                    )
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;

        }
    }
}
