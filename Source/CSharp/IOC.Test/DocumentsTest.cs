using NUnit.Framework;

namespace IOC.Test
{
    [TestFixture]
    public class DocumentsTest
    {
        public void ValidateCpfTest() 
        {
            bool isValid = FW.Core.Helper.Documents.Validation.ValidateCpf("33546645847");
            Assert.True(isValid);

            isValid = FW.Core.Helper.Documents.Validation.ValidateCpf("33646545848");
            Assert.False(isValid);
        }
    }
}
