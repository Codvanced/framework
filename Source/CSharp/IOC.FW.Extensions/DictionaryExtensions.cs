using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOC.FW.Extensions
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Transforma um dictionary em uma query string
        /// </summary>
        /// <param name="dict">Dictionary com as associações</param>
        /// <returns>O dictionary convertido ou uma string vazia</returns>
        public static string ToQueryString<TValue>(this IDictionary<string, TValue> dict)
        {
            if (dict.Count == 0) return string.Empty;

            var buffer = new StringBuilder();
            int count = 0;
            bool end = false;

            foreach (var key in dict.Keys)
            {
                if (count == dict.Count - 1) end = true;

                if (end)
                    buffer.AppendFormat("{0}={1}", key, dict[key]);
                else
                    buffer.AppendFormat("{0}={1}&", key, dict[key]);

                count++;
            }

            return buffer.ToString();
        }
    }
}
