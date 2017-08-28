using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace IOC.FW.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Extension method para remover caracteres especiais de uma string.
        /// </summary>
        /// <param name="dirtyString">string com o conteúdo a avaliar </param>
        /// <returns>string limpa de caracteres especiais</returns>
        public static string RemoveSpecialChars(this string dirtyString)
        {
            StringBuilder sbuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(dirtyString))
            {
                string acceptableChars = "áàâêäéèêëíìîïóòõôöúùûüÁÀÃÂÄÉÈÊËÍÌÎÏÓÒÔÕÖÚÙÛÜ";
                sbuilder = new StringBuilder(dirtyString.Length);

                for (int i = 0; i < dirtyString.Length; i++)
                {
                    char c = dirtyString[i];

                    if (c.Equals(' ')
                        || (c >= '0' && c <= '9')
                        || (c >= 'A' && c <= 'Z')
                        || (c >= 'a' && c <= 'z')
                        || acceptableChars.Contains(c))
                    {
                        sbuilder.Append(c);
                    }
                }
            }

            return sbuilder.ToString();
        }

        /// <summary>
        /// Extension method para trocar caracteres especiais em simples. (ex: á para a)
        /// </summary>
        /// <param name="dirtyString">string com o conteúdo a avaliar </param>
        /// <returns>string limpa de caracteres especiais</returns>
        public static string ReplaceSpecialChars(this string dirtyString)
        {
            Regex objRex = null;
            Dictionary<string, string> ht = new Dictionary<string, string>();
            string sOut = string.Empty;

            try
            {
                sOut = dirtyString;

                ht.Add("A", @"Á|À|Â|Ã|Ä|Å|Æ|æ");
                ht.Add("a", @"á|à|â|ã|ª");
                ht.Add("E", @"É|È|Ê|Ë");
                ht.Add("e", @"é|è|ê|ë");
                ht.Add("I", @"Í|Ì|Î|Ï");
                ht.Add("i", @"í|ì|î|ï");
                ht.Add("O", @"Ó|Ò|Ô|Õ");
                ht.Add("o", @"ó|ò|ô|õ|º|ö");
                ht.Add("U", @"Ú|Ù|Û|Ü");
                ht.Add("u", @"ú|ù|û|ü");
                ht.Add("C", @"Ç");
                ht.Add("c", @"ç");
                ht.Add(string.Empty, @",|\.|-|/|&|'|´");

                foreach (string Key in ht.Keys)
                {
                    objRex = new System.Text.RegularExpressions.Regex(ht[Key]);
                    sOut = objRex.Replace(sOut, Key);
                }

                return sOut;
            }
            finally
            {
                objRex = null;
                ht = null;
            }
        }

        /// <summary>
        /// Extension method destinado a criar strings de LIKE para utilizar em LINQ com regex
        /// </summary>
        /// <param name="text">Texto contendo string para like</param>
        /// <param name="charSplitter">Caracter separador</param>
        /// <param name="likeCommand">Comando de like</param>
        /// <returns>Pattern para utilizar com LINQ e regex</returns>
        public static string PreparePattern(
            this string text,
            char charSplitter,
            string likeCommand
        )
        {
            StringBuilder pattern = new StringBuilder();

            if (!string.IsNullOrEmpty(text))
            {
                string[] splittedText = text.Split(new char[] { charSplitter }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < splittedText.Length; i++)
                {
                    pattern.AppendFormat("{0}{1}", likeCommand, splittedText[i]);
                }

                pattern.Append(likeCommand);
            }

            return pattern.ToString();
        }

        /// <summary>
        /// Extension method destinado a encontrar itens com patterns de REGEX
        /// </summary>
        /// <param name="text">Texto contendo string para like</param>
        /// <param name="pattern">Pattern utilizado no like</param>
        /// <param name="options">Opções para o REGEX</param>
        /// <returns>Retorna se existe itens para a condição de REGEX</returns>
        public static bool Like(
            this string text,
            string pattern,
            RegexOptions options = RegexOptions.IgnoreCase
        )
        {
            return Regex.IsMatch(text, pattern, options);
        }

        /// <summary>
        /// Método responspavel por identificar se o texto informado remete a um arquivo de imagem
        /// </summary>
        /// <param name="image">Objeto para extender classe string</param>
        /// <returns>Booleano informando se é uma imagem</returns>
        public static bool IsImage(this string image)
        {
            if (File.Exists(image))
            {
                var stream = File.OpenRead(image);
                return stream.IsImage();
            }

            return false;
        }

        public static string ValidateIpAddress(this string clientIp, string serverVariable)
        {
            string ipGrabbed = string.Empty;

            ipGrabbed = (string.IsNullOrEmpty(clientIp) || clientIp.ToLower() == "unknown")
                    ? serverVariable
                    : clientIp;

            //Se contem multiplos IPs na serverVariable, deve-se retirar a do Cliente conforme a logica abaixo.
            if (!string.IsNullOrEmpty(ipGrabbed)
                && ipGrabbed.Contains(","))
            {
                string[] splittedIp = ipGrabbed.Split(',');

                if (splittedIp != null && splittedIp.Length > 0)
                {
                    // o primeiro IP quando vem de HTTP_X_CLUSTER_CLIENT_IP ou de HTTP_X_FORWARDED_FOR refere-se ao Cliente
                    //Fontes: http://en.wikipedia.org/wiki/Talk%3AX-Forwarded-For#HTTP_X_FORWARDED_FOR
                    // e      http://en.wikipedia.org/wiki/X-Forwarded-For
                    ipGrabbed = splittedIp[0].Trim();
                }
            }

            //se o IP estiver como ::1 é o 127.0.0.1 do IPv6.
            return ipGrabbed;
        }

        /// <summary>
        /// Remove tags HTML de uma string
        /// </summary>
        public static string StripHtmlTags(this string source)
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }

        /// <summary>
        /// Remove todas as tags e atributos de html exceto os que forem passados como parametro
        /// </summary>
        /// <param name="source">O html sujo</param>
        /// <param name="allowedTags">Lista de tags permitidas</param>
        /// <param name="allowedAttrs">Lista de atributos permitidos</param>
        /// <returns>O html limpo</returns>
        public static string CleanHtml(this string source, IList<String> allowedTags, IList<String> allowedAttrs)
        {
            if (allowedTags == null)
            {
                throw new ArgumentNullException("allowedTags");
            }

            if (allowedAttrs == null)
            {
                throw new ArgumentNullException("allowedAttrs");
            }

            if (String.IsNullOrEmpty(source))
            {
                return source;
            }

            allowedTags = EscapeItems(allowedTags);

            // (<(?!(\/?tag1\s?|\/?tag2\s?|\/?tag3\s?|\/?a\s?))[^>]*>)
            var tagsRegex = String.Format(@"(<(?!(/?{0}[\s>]))[^>]*>)", allowedTags.Count == 0 ? "0" : String.Join(@"[\s>]|/?", allowedTags));

            // (</?[^\s>]+\s?)(.*?)(/?>)
            var attrsRegex = @"(</?[^\s>]+\s?)(.*?)(/?>)";

            var options = RegexOptions.IgnoreCase | RegexOptions.Singleline;

            source = Regex.Replace(source, tagsRegex, String.Empty, options);

            var attrsMatches = Regex.Matches(source, attrsRegex, options);

            if (attrsMatches.Count > 0)
            {
                var attrs = new List<String>(attrsMatches.Count);

                var separator = "%--ATTRS--%";

                source = Regex.Replace(source, attrsRegex, String.Concat("$1", separator, "$3"), options);

                var newSource = source.Split(new string[] { separator }, StringSplitOptions.None);

                if (newSource.Length > 0)
                {
                    var builder = new StringBuilder();

                    builder.Append(newSource[0]);

                    allowedAttrs = EscapeItems(allowedAttrs);

                    var validAttrsRegex = String.Format("({0})=([\"'].*?[\"'])", String.Join("|", allowedAttrs));

                    for (int i = 0; i < attrsMatches.Count; i++)
                    {
                        var matchedAttrs = attrsMatches[i].Groups[2];
                        if (!String.IsNullOrEmpty(matchedAttrs.Value))
                        {
                            var validAttrs = Regex.Matches(matchedAttrs.Value, validAttrsRegex);
                            List<String> singleAttrs = null;

                            if (validAttrs.Count > 0)
                            {
                                singleAttrs = new List<String>(validAttrs.Count);

                                for (int j = 0; j < validAttrs.Count; j++)
                                {
                                    singleAttrs.Add(validAttrs[j].Value);
                                }

                                builder.Append(String.Join(" ", singleAttrs));
                            }
                        }
                        builder.Append(newSource[i + 1]);
                    }

                    source = builder.ToString().Replace("javascript:", String.Empty);
                    source = Regex.Replace(source, @"\s+", " ", options);
                }
            }

            return source;
        }

        /// <summary>
        /// Método responsável por modificar caracteres especiais pelos relativos escapes
        /// </summary>
        /// <param name="items">Lista de itens a aplicar escapes</param>
        /// <returns>Lista com os escapes aplicados</returns>
        private static IList<String> EscapeItems(IList<String> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException();
            }

            items = new List<String>(items);

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] == null || (items[i] = items[i].Trim()).Length == 0)
                {
                    items.RemoveAt(i--);
                }
                else
                {
                    items[i] = Regex.Escape(items[i]);
                }
            }

            return items;
        }
    }
}