using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;
using System.Collections;
using System.Data.SqlClient;
using System.IO;

namespace PerformanceTest
{
    class Program
    {

        private static readonly string connectionString = @"Data Source=.\SQLEXPRESS; Initial Catalog=Correios; User ID=sa; Password=1234; Persist Security Info=True;";

        static void Main(string[] args)
        {
            Ticket();
        }

        private static void Ticket()
        {
            int id = 0;
            Dictionary<string, string> dict = new Dictionary<string, string>(1000000);
            var provider = MD5.Create();
            Random rnd = new Random(100);

            while (id < 1000000000)
            {
                long timestamp = Stopwatch.GetTimestamp();
                string message = string.Format(
                    "{0}{1}{2}",
                    id,
                    timestamp,
                    rnd.Next()
                );

                string hash = BitConverter.ToString(
                    provider.ComputeHash(
                        Encoding.UTF8.GetBytes(message)
                    )
                ).Replace("-", string.Empty);

                try
                {
                    hash = hash.Substring(0, 12);
                    dict.Add(hash, null);
                }
                catch
                {
                    id--;
                    Console.WriteLine("colision in: {0}", hash);
                }

                if (id > 0 && id % 10000000 == 0)
                {
                    GenInsert(dict);
                }

                id++;
            }
            Console.Read();
        }

        private static void Md5Test()
        {
            StreamReader reader = new StreamReader(@"C:\projetos\dev\ticket.txt");
            string aaa = reader.ReadLine();

            int maxTickets = 100000000;
            byte[] rndBytes = new byte[100];
            Random rnd = new Random(10);
            SHA1 sha1Provider = SHA1.Create();

            Stopwatch watch = new Stopwatch();
            string[] arrStr = new string[maxTickets];

            Dictionary<string, string> dict = new Dictionary<string, string>(100000);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var comm = conn.CreateCommand();
                comm.Parameters.Add(new SqlParameter("@ticket", string.Empty));

                //var trans = conn.BeginTransaction();

                watch.Start();
                for (int i = 0; i < maxTickets; i++)
                {

                    string stringToHash = string.Format(
                        "{0}{1}{2}",
                        UnixTimestampFromDateTime(DateTime.Now),
                        i,
                        rnd.Next()
                    );

                    var hash = BitConverter.ToString(
                        sha1Provider.ComputeHash(
                            Encoding.UTF8.GetBytes(
                                stringToHash
                            )
                        )
                    ).Replace("-", string.Empty);

                    if (!string.IsNullOrEmpty(hash)
                        && hash.Length >= 12)
                    {
                        string hashDigits = hash.Substring(0, 12);

                        try
                        {
                            dict.Add(hashDigits, null);
                        }
                        catch (Exception ex)
                        {
                            i--;
                            Console.WriteLine("{0}={1}", stringToHash, hashDigits);
                        }

                    }


                    if (i > 0 && i % 5000000 == 0)
                    {
                        GenInsert(dict);
                    }
                }

                //trans.Commit();

            }
            watch.Stop();

            Console.WriteLine(
                "elapsed: {0}",
                watch.Elapsed.ToString()
            );
            Console.Read();
        }

        private static long UnixTimestampFromDateTime(DateTime date)
        {
            long unixTimestamp = date.Ticks - new DateTime(1970, 1, 1).Ticks;
            unixTimestamp /= TimeSpan.TicksPerSecond;
            return unixTimestamp;
        }

        private static void GenInsert(Dictionary<string, string> dct)
        {
            StreamWriter writer = new StreamWriter(
                @"C:\projetos\dev\ticket.txt",
                true
            );

            int contRemove = (int)(dct.Count * 0.2);
            var list = dct.ToList();
            for (int i = 0; i < contRemove; i++)
            {
                writer.WriteLine(list[i].Key);
            }
            list.RemoveRange(0, contRemove);

            dct.Clear();
            dct = list.ToDictionary(
                p => p.Key, 
                p => p.Value
            );

            writer.Flush();
            writer.Close();
        }
    }

}