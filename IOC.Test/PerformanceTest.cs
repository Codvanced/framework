using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Diagnostics;

namespace IOC.Test
{
    [TestFixture]
    public class PerformanceTest
    {
        [Test]
        public void Compare()
        {
            var time = Stopwatch.StartNew();
            int maxValue = 9000000;

            List<string> array = new List<string>();
            for (int i = 0; i < maxValue; i++)
            {
                array.Add(i.ToString());
            }
            time.Stop();
            Console.WriteLine(time.ElapsedMilliseconds);

            //----------------------------------------

            time = Stopwatch.StartNew();
            List<string> array2 = new List<string>(maxValue);
            for (int i = 0; i < maxValue; i++)
            {
                array2.Add(i.ToString());
            }
            time.Stop();
            Console.WriteLine(time.ElapsedMilliseconds);

            //----------------------------------------

            time = Stopwatch.StartNew();
            string[] simpleArray = new string[maxValue];
            for (int i = 0; i < maxValue; i++)
            {
                simpleArray[i] = i.ToString();
            }
            time.Stop();
            Console.WriteLine(time.ElapsedMilliseconds);

        }
    }
}
