using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq.Expressions;

namespace IOC.FW.Core
{
    /// <summary>
    /// Classe destinada a armazenar os Extensions Methods do Framework
    /// </summary>
    public static class Extensions
    {
        #region Extensões para Double
        /// <summary>
        /// Método auxiliar para truncar decimais
        /// </summary>
        /// <param name="value">Tipo de objeto a extender</param>
        /// <param name="decimalPlaces">Quantidade desejada de casas decimais</param>
        /// <returns>Valor truncado com a quantidade de decimais fornecida</returns>
        public static double Truncate(
            this double value,
            int decimalPlaces
        )
        {
            double integralValue = Math.Truncate(value);
            double fraction = value - integralValue;
            int factor = (int)Math.Pow(10, decimalPlaces);
            double truncatedFraction = Math.Truncate(fraction * factor) / factor;
            double result = integralValue + truncatedFraction;
            return result;
        }
        #endregion

        #region Extensões para String
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

        #endregion

        #region Extensões para Listas
        /// <summary>
        /// Extension method destinado a facilitar a validação de Lists
        /// </summary>
        /// <typeparam name="T">Type da lista</typeparam>
        /// <param name="list">Lista a comparar</param>
        /// <returns>Retorna se existem elementos</returns>
        public static bool HasElements<T>(this IEnumerable<T> list)
        {
            return
                list != null
                && list.Any();
        }

        /// <summary>
        /// Extension method destinado a "randomizar" listas usando o algoritmo de Fisher-Yates
        /// </summary>
        /// <typeparam name="T">Type da lista</typeparam>
        /// <param name="list">Lista a randomizar os itens</param>
        /// <returns>Lista randomizada</returns>
        public static IList<T> FisherYates<T>(this IList<T> list)
        {
            List<T> shuffledList = new List<T>(list.Count);

            if (list != null)
            {
                int count = list.Count;
                int jumpSize = 200;
                int index = 0;

                while (count > 0)
                {
                    if (count < jumpSize)
                        jumpSize = count;

                    shuffledList.AddRange(FisherYates(list, index, jumpSize));

                    count -= jumpSize;
                    index += jumpSize;
                }
            }

            return shuffledList;
        }

        /// <summary>
        /// Extension method destinado a "randomizar" listas usando o algoritmo de Fisher-Yates
        /// </summary>
        /// <typeparam name="T">Type da lista</typeparam>
        /// <param name="list">Lista a randomizar os iten</param>
        /// <param name="startIndex">Index inicial para randomizar</param>
        /// <param name="count">Quantidade de itens à randomizar</param>
        /// <returns>Lista com quantidade definida de itens randomizados</returns>
        public static IList<T> FisherYates<T>(this IList<T> list, int startIndex, int count)
        {
            IList<T> listTemp = new List<T>();

            if (list != null)
            {
                listTemp = list
                            .Skip(startIndex)
                            .Take(count)
                            .ToList();

                listTemp.Shuffle();
            }

            return listTemp;
        }

        /// <summary>
        /// Extension method destinado a "randomizar" listas usando o algoritmo de Fisher-Yates
        /// </summary>
        /// <typeparam name="T">Type da lista</typeparam>
        /// <param name="list">Lista a randomizar os iten</param>
        public static void Shuffle<T>(this IList<T> list)
        {
            // Provider de random numbers generator
            var randomGenerator = new RNGCryptoServiceProvider();

            // Contador para a quantidade de itens no list
            var itemIndex = list.Count;
            // Enquanto ainda tiver itens não randomizados na lista...
            while (itemIndex > 1)
            {

                var box = new byte[1];
                // Fará enquanto o valor randomizado
                do
                {
                    randomGenerator.GetBytes(box);
                }
                while (box[0] > itemIndex * (Byte.MaxValue / itemIndex));


                var randomIndex = (box[0] % itemIndex);
                itemIndex--;

                // Recupera item com index de destino.
                T value = list[randomIndex];
                // Faz a troca para o index encontrado de forma randomica
                list[randomIndex] = list[itemIndex];
                // Retorna o item recuperado para sua nova posição no vetor
                list[itemIndex] = value;
            }
        }
        #endregion

        #region Extensões para Dictionaries

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


        #endregion

        #region Extensões para Stream
        /// <summary>
        /// Extension method destinado a facilitar a validação do stream
        /// </summary>
        /// <param name="stream">Stream a validar</param>
        /// <returns>Retorna se o stream passado é imagem</returns>
        public static bool IsImage(this Stream stream)
        {
            if (stream == null)
            {
                return false;
            }

            if (!stream.CanRead)
            {
                return false;
            }

            try
            {
                return stream.Length > 0 && Image.FromStream(stream) != null;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Extensões para Image
        /// <summary>
        /// Método responsável por localizar mimetype de imagens.
        /// </summary>
        /// <param name="imageFormat">Objeto para extender a classe ImageFormat</param>
        /// <returns>String com o mimetype da imagem</returns>
        public static string GetMimeType(this ImageFormat imageFormat)
        {
            IDictionary<Guid, string> mimeTypes = new Dictionary<Guid, string> { 
                { new Guid(0xb96b3caa, 0x0728, 0x11d3, 0x9d, 0x7b, 0x00, 0x00, 0xf8, 0x1e, 0xf3, 0x2e), "image/bmp" },
                { new Guid(0xb96b3cab, 0x0728, 0x11d3, 0x9d, 0x7b, 0x00, 0x00, 0xf8, 0x1e, 0xf3, 0x2e), "image/bmp" },
                { new Guid(0xb96b3cac, 0x0728, 0x11d3, 0x9d, 0x7b, 0x00, 0x00, 0xf8, 0x1e, 0xf3, 0x2e), "application/emf" },
                { new Guid(0xb96b3cad, 0x0728, 0x11d3, 0x9d, 0x7b, 0x00, 0x00, 0xf8, 0x1e, 0xf3, 0x2e), "application/wmf" },
                { new Guid(0xb96b3cae, 0x0728, 0x11d3, 0x9d, 0x7b, 0x00, 0x00, 0xf8, 0x1e, 0xf3, 0x2e), "image/jpeg" },
                { new Guid(0xb96b3caf, 0x0728, 0x11d3, 0x9d, 0x7b, 0x00, 0x00, 0xf8, 0x1e, 0xf3, 0x2e) , "image/png" },
                { new Guid(0xb96b3cb0, 0x0728, 0x11d3, 0x9d, 0x7b, 0x00, 0x00, 0xf8, 0x1e, 0xf3, 0x2e), "image/gif" },
                { new Guid(0xb96b3cb1, 0x0728, 0x11d3, 0x9d, 0x7b, 0x00, 0x00, 0xf8, 0x1e, 0xf3, 0x2e), "image/tiff" }
            };

            if (mimeTypes.ContainsKey(imageFormat.Guid))
            {
                return mimeTypes[imageFormat.Guid];
            }

            return string.Empty;
        }
        #endregion

        #region Extensões para Random
        /// <summary>
        /// Método responsável por randomizar valores em um range informado 
        /// </summary>
        /// <param name="randomizer">Objeto para extender a classe Random</param>
        /// <param name="minValue">Valor mínimo</param>
        /// <param name="maxValue">Valor máximo</param>
        /// <returns>Número aleatorio entre o minimo e máximo</returns>
        public static int RandomNext(this Random randomizer, int minValue, int maxValue)
        {
            if (minValue > maxValue)
                throw new ArgumentOutOfRangeException("minValue");

            if (minValue == maxValue)
                return minValue;

            long diff = maxValue - minValue;
            var _rng = RNGCryptoServiceProvider.Create();
            byte[] _uint32Buffer = new byte[4];

            while (true)
            {
                _rng.GetBytes(_uint32Buffer);
                uint rand = BitConverter.ToUInt32(_uint32Buffer, 0);

                long max = (1 + (long)uint.MaxValue);
                long remainder = max % diff;
                if (rand < max - remainder)
                {
                    return (int)(minValue + (rand % diff));
                }
            }
        } 
        #endregion

        #region Extensões para Expression<T>

            /// <summary>
            /// Metodo responsavel por realizar uma operação unaria em uma expressão
            /// </summary>
            /// <typeparam name="T">Tipo</typeparam>
            /// <param name="first">Expressão base</param>
            /// <param name="merge">UnaryExpression que sera aplicada</param>
            /// <returns>Retorna Expressão com UnaryExpression Aplicada</returns>
            public static Expression<T> Compose<T>(this Expression<T> first, Func<Expression, Expression> merge)
            {
                return Expression.Lambda<T>(Expression.Not(first.Body), first.Parameters);
            }

            /// <summary>
            /// Metodo responsavel por realizar uma operção binaria em uma expressão
            /// </summary>
            /// <typeparam name="T">Tipo</typeparam>
            /// <param name="first">Expressão da esquerda</param>
            /// <param name="second">Expressão da direito</param>
            /// <param name="merge">BinaryExpression que sera aplicada</param>
            /// <returns>Retorna Expressão com BinaryExpression Aplicada</returns>
            public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
            {
                // build parameter map (from parameters of second to parameters of first)
                var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);

                // replace parameters in the second lambda expression with parameters from the first
                var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);


                // apply composition of lambda expression bodies to parameters from the first expression 
                return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
            }
            
            /// <summary>
            /// Metodo responsavel por realizar uma operação "E" entre duas expressoes
            /// </summary>
            /// <typeparam name="T">Tipo</typeparam>
            /// <param name="expr">Espressão base</param>
            /// <param name="ands">Espressões que serão aplicadas no operador "E"</param>
            /// <returns>Retorna expressão com operador "E" aplicado</returns>
            public static Expression<T> And<T>(this Expression<T> expr, params Expression<T>[] ands)
            {
                foreach (var item in ands)
                    expr = expr.Compose(item, Expression.AndAlso);

                return expr;
            }

            /// <summary>
            /// Metodo responsavel por realizar uma operação "Ou" entre duas expressoes
            /// </summary>
            /// <typeparam name="T">Tipo</typeparam>
            /// <param name="expr">Espressão base</param>
            /// <param name="ands">Espressões que serão aplicadas no operador "Ou"</param>
            /// <returns>Retorna expressão com operador "Ou" aplicado</returns>
            public static Expression<T> Or<T>(this Expression<T> expr, params Expression<T>[] ands)
            {
                foreach (var item in ands)
                    expr = expr.Compose(item, Expression.OrElse);


                return expr;
            }
            
            /// <summary>
            /// Metodo responsavel por realizar uma operação "Not" na expressão
            /// </summary>
            /// <typeparam name="T">Tipo</typeparam>
            /// <param name="expr">Expressão base</param>
            /// <returns>Reotorna expressão negada</returns>
            public static Expression<T> Not<T>(this Expression<T> expr)
            {
                return expr.Compose(Expression.Not);
            }

            /// <summary>
            /// Metodo responsavel por realizar uma operação "E" negando as expressões a direita
            /// </summary>
            /// <typeparam name="T">Tipo</typeparam>
            /// <param name="expr">Espressão base</param>
            /// <param name="ands">Espressões que serão aplicadas no operador "E"</param>
            /// <returns>Retorna expressão com operador "E", porem negando as expressões aplicadas</returns>
            public static Expression<T> AndNot<T>(this Expression<T> expr, params Expression<T>[] ands)
            {
                foreach (var item in ands)
                    expr = expr.Compose(item.Not(), Expression.AndAlso);

                return expr;
            }

            /// <summary>
            /// Metodo responsavel por realizar uma operação "Ou" negando as expressões a direita
            /// </summary>
            /// <typeparam name="T">Tipo</typeparam>
            /// <param name="expr">Espressão base</param>
            /// <param name="ands">Espressões que serão aplicadas no operador "Ou"</param>
            /// <returns>Retorna expressão com operador "Ou", porem negando as expressões aplicadas</returns>
            public static Expression<T> OrNot<T>(this Expression<T> expr, params Expression<T>[] ands)
            {
                foreach (var item in ands)
                    expr = expr.Compose(item.Not(), Expression.OrElse);

                return expr;
            }
        #endregion

    }
}
