using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using IOC.FW.Core.Cripto;

namespace IOC.Test
{
    [TestFixture]
    public class DESUtilTest
    {
        [Test]
        public void DesTest()
        {
            var keys = DESUtil.Keys();
            Assert.IsNotNull(keys);

            string textToEncrypt = "texto a encriptar";
            
            var encrypted = DESUtil.Encrypt(textToEncrypt, keys[0], keys[1]);
            Assert.IsNotNullOrEmpty(textToEncrypt);

            var decrypted = DESUtil.Decrypt(encrypted, keys[0], keys[1]);
            Assert.IsNotNullOrEmpty(decrypted);
            Assert.AreEqual(textToEncrypt, decrypted);
        }

    }
}
