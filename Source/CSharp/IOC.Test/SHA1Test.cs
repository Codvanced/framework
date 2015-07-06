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
        public void Should_Hash_A_String_With_Sha1_When_Has_Salt()
        {
            string[] passwords = new string[] { 
                 "teste123",
                 "teste321",
                 "teste"
            };

            var salt = SHA1Util.GenerateSalt(5);

            for (int i = 0; i < passwords.Length; i++)
            {
                string sha1 = string.Empty;
                string old = string.Empty;
                passwords[i] = sha1 = SHA1Util.GenerateSHA1(old = passwords[i], salt);
                bool check = SHA1Util.VerifyHash(old, sha1, salt);

                Assert.IsTrue(check);
            }
        }

        [Test]
        public void Should_Hash_A_String_With_Sha1_When_Dont_Have_Salt()
        {
            string[] passwords = new string[] { 
                 "teste123",
                 "teste321",
                 "teste"
            };

            for (int i = 0; i < passwords.Length; i++)
            {
                string sha1 = string.Empty;
                string old = string.Empty;
                passwords[i] = sha1 = SHA1Util.GenerateSHA1(old = passwords[i]);
                bool check = SHA1Util.VerifyHash(old, sha1);

                Assert.IsTrue(check);
            }
        }

        [Test]
        public void Should_Fail_The_Hashing_When_Dont_Pass_A_String()
        {
            Assert.Throws(typeof(ArgumentNullException), () => {
                SHA1Util.GenerateSHA1(null);
            });
            
        }
    }
}