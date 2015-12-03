using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Drawing.Imaging;

namespace IOC.FW.ImageTransformation
{
    /// <summary>
    /// Classe responsável por transformações em imagens
    /// </summary>
    public class Transform
    {

        /// <summary>
        /// Método responsável por croppar imagens
        /// </summary>
        /// <param name="img">Objeto com a imagem para o crop</param>
        /// <param name="rectCrop">Objeto de retangulo informando X, Y, width e height para cropar na imagem</param>
        /// <returns>Nova imagem com o resultado do crop</returns>
        public static Image Crop(
            Image img,
            Rectangle rectCrop
        )
        {
            if (img == null)
            {
                throw new ArgumentNullException("img");
            }

            if (rectCrop == null)
            {
                throw new ArgumentNullException("rectCrop");
            }

            Bitmap finalThumb = new Bitmap(rectCrop.Width, rectCrop.Height);
            using (Graphics g = Graphics.FromImage(finalThumb))
            {
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.Clear(Color.White);
                g.DrawImage(img, 0, 0, rectCrop, GraphicsUnit.Pixel);
            }

            return finalThumb;
        }

        /// <summary>
        /// Método responsável por resize em imagens
        /// </summary>
        /// <param name="img">Objeto com a imagem para o crop</param>
        /// <param name="size">Objeto com proporções para o resize </param>
        /// <returns>Nova imagem com o resultado do resize</returns>
        public static Image Resize(Image img, Size size)
        {
            var image = new Bitmap(img);
            int oldWidth = image.Width;
            int oldHeight = image.Height;
            int newHeight = (oldHeight * size.Width / oldWidth);

            var newImage = new Bitmap(size.Width, newHeight);
            using (var graphic = Graphics.FromImage(newImage))
            {
                graphic.SmoothingMode = SmoothingMode.AntiAlias;
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphic.DrawImage(image, new Rectangle(0, 0, size.Width, newHeight));
            }

            return newImage;
        }

        /// <summary>
        /// Método responsável por transformar objetos de image em memory stream
        /// </summary>
        /// <param name="image">Objeto com a imagem para a conversão</param>
        /// <returns>Objeto de memory stream</returns>
        public static MemoryStream Convert(Image image)
        {
            MemoryStream memoryStream = new MemoryStream();
            image.Save(memoryStream, ImageFormat.Png);
            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}
