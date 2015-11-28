using System.Drawing;
using System.IO;

namespace IOC.FW.Extensions
{
    public static class StreamExtensions
    {
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
    }
}
