﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using IOC.FW.Core.Cripto;

namespace IOC.Test
{
    [TestFixture]
    public class AESTest
    {
        [Test]
        public void AesTest()
        {
            string original = "String antes de encriptar!";

            var keys = AESUtil.Keys();
            Assert.IsNotNull(keys);

            byte[] encrypted = AESUtil.Encrypt(
                original,
                keys[0],
                keys[1]
            );
            Assert.IsNotEmpty(encrypted);

            string roundtrip = AESUtil.Decrypt(
                encrypted,
                keys[0],
                keys[1]
            );
            Assert.IsNotEmpty(roundtrip);
            Assert.AreEqual(original, roundtrip);
        }
    }
}
