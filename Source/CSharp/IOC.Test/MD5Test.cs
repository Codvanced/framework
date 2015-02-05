using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Security.Cryptography;
using System.Threading;

namespace IOC.Test
{
    [TestFixture]
    public class MD5Test
    {
        [Test]
        public void Md5Test()
        {
            int maxTickets = 100000;
            var rngProvider = RNGCryptoServiceProvider.Create();
            byte[] rndBytes = new byte[maxTickets];
            
            Dictionary<string, string> dict = new Dictionary<string, string>(maxTickets);
            for (int i = 0; i < maxTickets; i++)
            {
                rngProvider.GetBytes(rndBytes);
                string stringToHash = string.Format(
                    "{0}{1}{2}",
                    UnixTimestampFromDateTime(DateTime.Now),
                    i,
                    ASCIIEncoding.ASCII.GetString(rndBytes)
                );

                var hash = BitConverter.ToString(
                    SHA1.Create().ComputeHash(
                        Encoding.UTF8.GetBytes(
                            stringToHash
                        )
                    )
                ).Replace("-", string.Empty);

                if (!string.IsNullOrEmpty(hash) 
                    && hash.Length >= 15)
                {
                    string hashDigits = hash.Substring(0, 5);

                    if (!dict.ContainsKey(hashDigits))
                    {
                        dict.Add(hashDigits, hashDigits);
                    }
                    else
                    {
                        i--;
                        Console.WriteLine("{0}={1}", stringToHash, hashDigits);
                    }
                }

                Thread.Sleep(50);
            }



        }

        public long UnixTimestampFromDateTime(DateTime date)
        {
            long unixTimestamp = date.Ticks - new DateTime(1970, 1, 1).Ticks;
            unixTimestamp /= TimeSpan.TicksPerSecond;
            return unixTimestamp;
        }

    }
}
