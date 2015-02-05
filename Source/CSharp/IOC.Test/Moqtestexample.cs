using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Moq;

namespace IOC.Test
{
    [TestFixture]
    public class Moqtestexample
    {
        public void Teste() {
            var mock = Mock.Of<Triangle>();
            
            var a = 3;
            var b = 4;
            var hypo = mock.Hypotenuse(a, b);

            Assert.AreEqual(5, hypo);
        }
    }

    public class Triangle {

        public double Hypotenuse(double a, double b)
        {
            return Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));
        }
    }
}
