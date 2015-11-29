using IOC.FW.Validation;
using NUnit.Framework;
using System;
using System.Collections;
using System.Linq.Expressions;


//NAMING CONVENTION FOR UNIT TESTS: "MethodName_ExpectedBehavior_StateUnderTest" (Every method rename, the unit test need to be changed too!)
namespace IOC.Test
{
    [TestFixture]
    public class CheckTests
    {
        [Test]
        public void IfNull_ThrowArgumentNullException_IfObjectIsNull()
        {
            var objectToTest = (int?)null;

            Assert.Throws(
                typeof(ArgumentNullException),
                () => Check.IfNull(
                    () => objectToTest
                )
            );
        }

        [Test]
        public void IfNull_ThrowArgumentNullException_IfStringIsNull()
        {
            var objectToTest = (string)null;

            Assert.Throws(
                typeof(ArgumentNullException),
                () => Check.IfNullOrEmpty(
                    () => objectToTest
                )
            );
        }

        [Test]
        public void IfNull_ThrowArgumentNullException_IfEnumerableIsNull()
        {
            var objectToTest = (IEnumerable)null;

            Assert.Throws(
                typeof(ArgumentNullException),
                () => Check.IfNullOrEmpty(
                    () => objectToTest
                )
            );
        }

        [Test]
        public void IfNull_ThrowArgumentNullException_IfOneObjectIsNullInArray()
        {
            var objectToTest1 = 1;
            var objectToTest2 = 2;
            var objectToTest3 = (object)null;

            Assert.Throws(
                typeof(ArgumentNullException),
                () => Check.IfNull(new Expression<Func<object>>[] {
                    () => objectToTest1,
                    () => objectToTest2,
                    () => objectToTest3
                })
            );
        }

        [Test]
        public void IsNullOrEmpty_ThrowArgumentNullException_IfOneStringIsNullOrEmptyInArray()
        {
            var objectToTest1 = "a";
            var objectToTest2 = "b";
            var objectToTest3 = (string)null;

            Assert.Throws(
                typeof(ArgumentNullException),
                () => Check.IfNullOrEmpty(new Expression<Func<string>>[] {
                    () => objectToTest1,
                    () => objectToTest2,
                    () => objectToTest3
                })
            );
        }

        [Test]
        public void IsNullOrEmpty_ThrowArgumentNullException_IfOneEnumerableIsNullOrEmptyInArray()
        {
            var objectToTest1 = new int[] { 1, 2, 3 };
            var objectToTest2 = new char[] { 'h', 'a', 'c', 'k', 'e', 'r' };
            var objectToTest3 = (int[])null;

            Assert.Throws(
                typeof(ArgumentNullException),
                () => Check.IfNullOrEmpty(new Expression<Func<IEnumerable>>[] {
                    () => objectToTest1,
                    () => objectToTest2,
                    () => objectToTest3
                })
            );
        }
    }
}
