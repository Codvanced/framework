using IOC.FW.Validation;
using NUnit.Framework;

namespace IOC.Test
{
    [TestFixture]
    public class DocumentsTest
    {
        public void ValidateCpfTest() 
        {
            bool isValid = CPF.ValidateCpf("33546645847");
            Assert.True(isValid);

            isValid = CPF.ValidateCpf("33646545848");
            Assert.False(isValid);
        }
    }
}
