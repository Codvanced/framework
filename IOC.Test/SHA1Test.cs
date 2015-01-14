using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Security.Cryptography;

namespace IOC.Test
{
    [TestFixture]
    public class SHA1Test
    {
        [Test]
        public void DesTest()
        {
            string[] passwords = new string[] { 
                 "@664971318@EstacioLP",               
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
        }
    }
}
