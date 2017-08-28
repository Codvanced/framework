using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace IOC.FW.Extensions
{
    public static class ListExtensions
    {
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
    }
}