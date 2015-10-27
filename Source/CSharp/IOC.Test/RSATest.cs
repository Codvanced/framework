using System;
using System.Text;
using NUnit.Framework;
using IOC.FW.Cryptography;

namespace IOC.Test
{
    [TestFixture]
    public class RSATest
    {
        [SetUp]
        public void Should_Setup_All_Tests_And_Create_()
        {
            var publicKey = new System.IO.FileInfo(@"C:\Keys\PublicKey.key");
            var privateKey = new System.IO.FileInfo(@"C:\Keys\PrivateKey.key");
            var encryptText = new System.IO.FileInfo(@"C:\Keys\Encriptado\texto.txt");
            var decryptText = new System.IO.FileInfo(@"C:\Keys\Decriptado\texto.txt");
            var plainText = new System.IO.FileInfo(string.Format(@"C:\Keys\texto{0}.txt", Guid.NewGuid().ToString()));

            if (!publicKey.Directory.Exists)
                publicKey.Directory.Create();

            if (!encryptText.Directory.Exists)
                encryptText.Directory.Create();

            if (!decryptText.Directory.Exists)
                decryptText.Directory.Create();
        }

        [Test]
        public void RsaTestsSecondOverload()
        {
            UnicodeEncoding encoder = new UnicodeEncoding();

            string texto = "TESTE 123";

            var keyPair = RSAUtil.Keys();
            Assert.IsNotEmpty(keyPair);

            byte[] encriptado = RSAUtil.Encrypt(keyPair[0], encoder.GetBytes(texto));
            Assert.IsNotEmpty(encriptado);
            Assert.AreNotEqual(encoder.GetBytes(texto), encriptado);

            byte[] decriptado = RSAUtil.Decrypt(keyPair[1], encriptado);
            Assert.IsNotEmpty(decriptado);
            Assert.AreEqual(encoder.GetBytes(texto), decriptado);
        }

        [Test]
        public void RsaTestsThirdOverload()
        {
            var publicKey = new System.IO.FileInfo(@"C:\Keys\PublicKey.key");
            var privateKey = new System.IO.FileInfo(@"C:\Keys\PrivateKey.key");
            var encryptText = new System.IO.FileInfo(@"C:\Keys\Encriptado\texto.txt");
            var decryptText = new System.IO.FileInfo(@"C:\Keys\Decriptado\texto.txt");
            var plainText = new System.IO.FileInfo(string.Format(@"C:\Keys\texto{0}.txt", Guid.NewGuid().ToString()));

            if (!plainText.Exists)
            {
                var writter = plainText.AppendText();
                writter.Write("teste123123123123");
                writter.Close();
            }

            RSAUtil.Keys(publicKey.FullName, privateKey.FullName);
            Assert.True(System.IO.File.Exists(publicKey.FullName));
            Assert.True(System.IO.File.Exists(privateKey.FullName));

            RSAUtil.Encrypt(publicKey.FullName, plainText.FullName, encryptText.FullName);
            Assert.True(System.IO.File.Exists(encryptText.FullName));

            RSAUtil.Decrypt(privateKey.FullName, encryptText.FullName, decryptText.FullName);
            Assert.True(System.IO.File.Exists(decryptText.FullName));

            Assert.AreEqual(
                System.IO.File.ReadAllText(decryptText.FullName, Encoding.Unicode),
                System.IO.File.ReadAllText(plainText.FullName, Encoding.Unicode)
            );

            publicKey.Delete();
            privateKey.Delete();
            encryptText.Delete();
            decryptText.Delete();
            plainText.Delete();

            encryptText.Directory.Delete();
            decryptText.Directory.Delete();
            publicKey.Directory.Delete();
        }

    }
}
