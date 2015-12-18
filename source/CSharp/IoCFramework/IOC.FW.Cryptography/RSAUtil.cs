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
            CspParameters cspParams = null;
            RSACryptoServiceProvider rsaProvider = null;
            RSAParameters[] parameters = new RSAParameters[2];

            try
            {
                cspParams = new CspParameters();
                cspParams.ProviderType = 1; 
                cspParams.Flags = CspProviderFlags.UseArchivableKey;
                cspParams.KeyNumber = (int)KeyNumber.Exchange;
                rsaProvider = new RSACryptoServiceProvider(cspParams);

                parameters[0] = rsaProvider.ExportParameters(false);
                parameters[1] = rsaProvider.ExportParameters(true);
            }
            catch (Exception ex)
            { }

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
            CspParameters cspParams = null;
            RSACryptoServiceProvider rsaProvider = null;
            StreamWriter publicKeyFile = null;
            StreamWriter privateKeyFile = null;
            string publicKey = "";
            string privateKey = "";

            try
            {
                cspParams = new CspParameters();
                cspParams.ProviderType = 1; 
                cspParams.Flags = CspProviderFlags.UseArchivableKey;
                cspParams.KeyNumber = (int)KeyNumber.Exchange;
                rsaProvider = new RSACryptoServiceProvider(cspParams);

                publicKey = rsaProvider.ToXmlString(false);
                publicKeyFile = File.CreateText(publicKeyFileName);
                publicKeyFile.Write(publicKey);
                privateKey = rsaProvider.ToXmlString(true);
                privateKeyFile = File.CreateText(privateKeyFileName);
                privateKeyFile.Write(privateKey);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (publicKeyFile != null)
                {
                    publicKeyFile.Close();
                }
                if (privateKeyFile != null)
                {
                    privateKeyFile.Close();
                }
            }
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
            CspParameters cspParams = null;
            RSACryptoServiceProvider rsaProvider = null;
            StreamReader publicKeyFile = null;
            StreamReader plainFile = null;
            FileStream encryptedFile = null;
            string publicKeyText = "";
            string plainText = "";
            byte[] plainBytes = null;
            byte[] encryptedBytes = null;

            try
            {
                cspParams = new CspParameters();
                cspParams.ProviderType = 1;
                rsaProvider = new RSACryptoServiceProvider(cspParams);

                publicKeyFile = File.OpenText(publicKeyFileName);
                publicKeyText = publicKeyFile.ReadToEnd();
                rsaProvider.FromXmlString(publicKeyText);

                plainFile = File.OpenText(plainFileName);
                plainText = plainFile.ReadToEnd();
                plainBytes = Encoding.Unicode.GetBytes(plainText);
                encryptedBytes = rsaProvider.Encrypt(plainBytes, false);

                encryptedFile = File.Create(encryptedFileName);
                encryptedFile.Write(encryptedBytes, 0, encryptedBytes.Length);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (publicKeyFile != null)
                {
                    publicKeyFile.Close();
                }
                if (plainFile != null)
                {
                    plainFile.Close();
                }
                if (encryptedFile != null)
                {
                    encryptedFile.Close();
                }
            }
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
            UnicodeEncoding ByteConverter = new UnicodeEncoding();
            CspParameters cspParams = null;
            RSACryptoServiceProvider rsaProvider = null;
            byte[] encryptedBytes = null;

            try
            {
                cspParams = new CspParameters();
                cspParams.ProviderType = 1;
                rsaProvider = new RSACryptoServiceProvider(cspParams);
                rsaProvider.ImportParameters(publicKey);
                encryptedBytes = rsaProvider.Encrypt(plainBytes, false);
            }
            catch (Exception ex)
            {
            }

            return encryptedBytes;
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
            CspParameters cspParams = null;
            RSACryptoServiceProvider rsaProvider = null;
            StreamReader privateKeyFile = null;
            FileStream encryptedFile = null;
            StreamWriter plainFile = null;
            string privateKeyText = "";
            string plainText = "";
            byte[] encryptedBytes = null;
            byte[] plainBytes = null;

            try
            {
                cspParams = new CspParameters();
                cspParams.ProviderType = 1;
                rsaProvider = new RSACryptoServiceProvider(cspParams);

                privateKeyFile = File.OpenText(privateKeyFileName);
                privateKeyText = privateKeyFile.ReadToEnd();
                
                rsaProvider.FromXmlString(privateKeyText);

                encryptedFile = File.OpenRead(encryptedFileName);
                encryptedBytes = new byte[encryptedFile.Length];
                encryptedFile.Read(encryptedBytes, 0, (int)encryptedFile.Length);

                plainBytes = rsaProvider.Decrypt(encryptedBytes, false);
                plainFile = File.CreateText(plainFileName);
                plainText = Encoding.Unicode.GetString(plainBytes);
                plainFile.Write(plainText);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (privateKeyFile != null)
                {
                    privateKeyFile.Close();
                }
                if (encryptedFile != null)
                {
                    encryptedFile.Close();
                }
                if (plainFile != null)
                {
                    plainFile.Close();
                }
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
            UnicodeEncoding ByteConverter = new UnicodeEncoding();
            CspParameters cspParams = null;
            RSACryptoServiceProvider rsaProvider = null;
            byte[] plainBytes = null;

            try
            {
                cspParams = new CspParameters();
                cspParams.ProviderType = 1;
                rsaProvider = new RSACryptoServiceProvider(cspParams);
                rsaProvider.ImportParameters(privateKey);
                plainBytes = rsaProvider.Decrypt(encryptedBytes, false);
            }
            catch (Exception ex)
            {
            }

            return plainBytes;
        }
    }
}
