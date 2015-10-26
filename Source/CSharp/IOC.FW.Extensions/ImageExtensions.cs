using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOC.FW.Extensions
{
    public static class ImageExtensions
    {
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
    }
}
