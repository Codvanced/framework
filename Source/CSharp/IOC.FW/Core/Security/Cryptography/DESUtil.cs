using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace IOC.FW.Core.Security.Cryptography
{
    /// <summary>
    /// Classe responsável por encriptação e decriptação utilizando a técnica DES
    /// </summary>
    public static class DESUtil
    {

        /// <summary>
        /// Método responsável por criar e devolver as chaves utilizadas para os processos
        /// </summary>
        /// <returns>Listagem de chaves (Key como index: 0 e IV como index: 1)</returns>
        public static IList<byte[]> Keys()
        {
            IList<byte[]> keys = new List<byte[]>(2);
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                keys.Add(des.Key);
                keys.Add(des.IV);
            }

            return keys;
        }

        /// <summary>
        /// Método responsável por encriptar textos
        /// </summary>
        /// <param name="plainText">Texto puro</param>
        /// <param name="Key">Chave (key)</param>
        /// <param name="IV">Chave (iv)</param>
        /// <returns>Array de bytes com texto encriptado</returns>
        public static byte[] Encrypt(string plainText, byte[] Key, byte[] IV)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");
            byte[] encrypted;
            
            using (DESCryptoServiceProvider tdsAlg = new DESCryptoServiceProvider())
            {
                tdsAlg.Key = Key;
                tdsAlg.IV = IV;

                ICryptoTransform encryptor = tdsAlg.CreateEncryptor(tdsAlg.Key, tdsAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
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
        public static string Decrypt(byte[] cipherText, byte[] Key, byte[] IV)
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");

            string plaintext = null;

            using (DESCryptoServiceProvider tdsAlg = new DESCryptoServiceProvider())
            {
                tdsAlg.Key = Key;
                tdsAlg.IV = IV;

                ICryptoTransform decryptor = tdsAlg.CreateDecryptor(tdsAlg.Key, tdsAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
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
