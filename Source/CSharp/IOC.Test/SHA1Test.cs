using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Security.Cryptography;
using IOC.FW.Core.Cripto;

namespace IOC.Test
{
    [TestFixture]
    public class SHA1Test
    {
        [Test]
        public void DesTest()
        {
            string[] passwords = new string[] { 
                 "teste123",               
            };

            for (int i = 0; i < passwords.Length; i++)
            {
                passwords[i] = BitConverter.ToString(
                    SHA1.Create().ComputeHash(
                        Encoding.UTF8.GetBytes(
                            passwords[i]
                        )
                    )
                )
                .Replace("-", string.Empty);
            }

            string a = SHA1Util.GenerateSHA1("teste123");
        }
    }
}
