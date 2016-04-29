using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace IOC.FW.Cryptography
{
    /// <summary>
    /// Classe com o propósito de facilitar a utilização de RSA
    /// </summary>
    public static class RSAUtil
    {
        /// <summary>
        /// Método auxiliar para obter chaves para encriptar e decriptar utilizando RSA
        /// </summary>
        /// <returns>Array com chaves de criptografia, index:0 = publica, index:1 = privada</returns>
        public static RSAParameters[] Keys()
        {
            RSAParameters[] parameters = new RSAParameters[2];

            CspParameters cspParams = new CspParameters();
            cspParams.ProviderType = 1;
            cspParams.Flags = CspProviderFlags.UseArchivableKey;
            cspParams.KeyNumber = (int)KeyNumber.Exchange;

            RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(cspParams);
            parameters[0] = rsaProvider.ExportParameters(false);
            parameters[1] = rsaProvider.ExportParameters(true);

            return parameters;
        }

        /// <summary>
        /// Método auxiliar para obter chaves para encriptar e decriptar utilizando RSA
        /// </summary>
        /// <param name="publicKeyFileName">Caminho para gerar um arquivo contendo a chave publica</param>
        /// <param name="privateKeyFileName">Caminho para gerar um arquivo contendo a chave privada</param>
        public static void Keys(
            string publicKeyFileName,
            string privateKeyFileName
        )
        {
            CspParameters cspParams = new CspParameters();
            cspParams.ProviderType = 1;
            cspParams.Flags = CspProviderFlags.UseArchivableKey;
            cspParams.KeyNumber = (int)KeyNumber.Exchange;

            RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(cspParams);
            string publicKey = rsaProvider.ToXmlString(false);
            string privateKey = rsaProvider.ToXmlString(true);

            using (StreamWriter publicKeyFile = File.CreateText(publicKeyFileName))
                publicKeyFile.Write(publicKey);

            using (StreamWriter privateKeyFile = File.CreateText(privateKeyFileName))
                privateKeyFile.Write(privateKey);
        }

        /// <summary>
        /// Método auxiliar para encriptar uma mensagem utilizando RSA
        /// </summary>
        /// <param name="publicKeyFileName">Caminho contendo o arquivo com a chave publica</param>
        /// <param name="plainFileName">Caminho contendo o arquivo com o texto puro</param>
        /// <param name="encryptedFileName">Caminho contendo o arquivo com o texto encriptado</param>
        public static void Encrypt(
            string publicKeyFileName,
            string plainFileName,
            string encryptedFileName
        )
        {
            CspParameters cspParams = new CspParameters();
            cspParams.ProviderType = 1;

            RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(cspParams);
            string publicKeyText = "";

            using (StreamReader publicKeyFile = File.OpenText(publicKeyFileName))
                publicKeyText = publicKeyFile.ReadToEnd();

            rsaProvider.FromXmlString(publicKeyText);

            string plainText = "";
            using (StreamReader plainFile = File.OpenText(plainFileName))
                plainText = plainFile.ReadToEnd();

            byte[] plainBytes = Encoding.Unicode.GetBytes(plainText);
            byte[] encryptedBytes = rsaProvider.Encrypt(plainBytes, false);

            using (FileStream encryptedFile = File.Create(encryptedFileName))
                encryptedFile.Write(encryptedBytes, 0, encryptedBytes.Length);
        }

        /// <summary>
        /// Método auxiliar para encriptar uma mensagem utilizando RSA
        /// </summary>
        /// <param name="publicKey">Parametro de RSA contendo a chave publica</param>
        /// <param name="plainBytes">Bytes contendo o texto puro</param>
        /// <returns>Bytes com o texto encriptado</returns>
        public static byte[] Encrypt(
            RSAParameters publicKey,
            byte[] plainBytes
        )
        {
            CspParameters cspParams = new CspParameters();
            cspParams.ProviderType = 1;

            RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(cspParams);
            rsaProvider.ImportParameters(publicKey);

            return rsaProvider.Encrypt(
                plainBytes,
                false
            );
        }

        /// <summary>
        /// Método auxiliar para decriptar uma mensagem utilizando RSA
        /// </summary>
        /// <param name="privateKeyFileName">Caminho contendo o arquivo com a chave privada</param>
        /// <param name="encryptedFileName">Caminho contendo o arquivo com o texto encriptado</param>
        /// <param name="plainFileName">Caminho contendo o arquivo com o texto puro</param>
        public static void Decrypt(
            string privateKeyFileName,
            string encryptedFileName,
            string plainFileName
        )
        {
            CspParameters cspParams = new CspParameters();
            cspParams.ProviderType = 1;

            RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(cspParams);
            string privateKeyText = "";

            using (StreamReader privateKeyFile = File.OpenText(privateKeyFileName))
                privateKeyText = privateKeyFile.ReadToEnd();

            rsaProvider.FromXmlString(privateKeyText);

            byte[] encryptedBytes = null;
            using (FileStream encryptedFile = File.OpenRead(encryptedFileName))
            {
                encryptedBytes = new byte[encryptedFile.Length];
                encryptedFile.Read(encryptedBytes, 0, (int)encryptedFile.Length);
            }

            byte[] plainBytes = rsaProvider.Decrypt(encryptedBytes, false);

            string plainText = "";
            using (StreamWriter plainFile = File.CreateText(plainFileName))
            {
                plainText = Encoding.Unicode.GetString(plainBytes);
                plainFile.Write(plainText);
            }
        }

        /// <summary>
        /// Método auxiliar para decriptar uma mensagem utilizando RSA
        /// </summary>
        /// <param name="privateKey">Parametro de RSA contendo a chave privada</param>
        /// <param name="encryptedBytes">Bytes contendo o texto encriptado</param>
        /// <returns>Bytes com o texto puro</returns>
        public static byte[] Decrypt(
            RSAParameters privateKey,
            byte[] encryptedBytes
        )
        {
            CspParameters cspParams = new CspParameters();
            cspParams.ProviderType = 1;

            RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(cspParams);
            rsaProvider.ImportParameters(privateKey);

            return rsaProvider.Decrypt(
                encryptedBytes,
                false
            );
        }
    }
}
