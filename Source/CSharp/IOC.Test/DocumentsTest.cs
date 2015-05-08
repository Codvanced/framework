using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using IOC.FW.Core;

namespace IOC.Test
{
    [TestFixture]
    public class DocumentsTest
    {
        public void ValidateCpfTest() 
        {
            bool isValid = IOC.FW.Core.Documents.Validation.ValidateCpf("33546645847");
            Assert.True(isValid);

            isValid = IOC.FW.Core.Documents.Validation.ValidateCpf("33646545848");
            Assert.False(isValid);
        }
    }
}
