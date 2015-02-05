using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using IOC.FW.Configuration;

namespace IOC.FW.Core.Images
{
    /// <summary>
    /// 
    /// </summary>
    public static class Thumbnail
    {
        /// <summary>
        /// Método responsável por criar thumbnail
        /// </summary>
        /// <param name="img">Objeto de image para o crop</param>
        /// <param name="cropArea">Área informada para realizar o crop</param>
        /// <param name="refPoint">Ponto de referência para manter a proporção</param>
        /// <param name="fitInside"></param>
        /// <returns></returns>
        public static Image Create(
            Image img, 
            Size cropArea, 
            Enumerators.ReferencePoint refPoint = Enumerators.ReferencePoint.MiddleCenter, 
            bool fitInside = false
        )
        {
            Bitmap finalThumb = new Bitmap(cropArea.Width, cropArea.Height);
            Size newSize = GetProportionalSize(img.Size, finalThumb.Size, fitInside);
            Bitmap thumb = new Bitmap(newSize.Width, newSize.Height);
            Point p = GetStartPosition(finalThumb.Size, newSize, refPoint);

            using (Graphics g = Graphics.FromImage(finalThumb))
            {
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.Clear(Color.White);

                g.DrawImage(img, -(p.X), -(p.Y), thumb.Width, thumb.Height);
            }

            return finalThumb;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requested"></param>
        /// <param name="proportional"></param>
        /// <param name="refPoint"></param>
        /// <returns></returns>
        public static Point GetStartPosition(
            Size requested, 
            Size proportional,
            Enumerators.ReferencePoint refPoint
        )
        {
            Point result;

            switch (refPoint)
            {
                case Enumerators.ReferencePoint.TopLeft:
                    result = GetStartPositionOnTopLeft(requested, proportional);
                    break;
                case Enumerators.ReferencePoint.TopCenter:
                    result = GetStartPositionOnTopCenter(requested, proportional);
                    break;
                case Enumerators.ReferencePoint.TopRight:
                    result = GetStartPositionOnTopRight(requested, proportional);
                    break;
                case Enumerators.ReferencePoint.MiddleLeft:
                    result = GetStartPositionOnMiddleLeft(requested, proportional);
                    break;
                case Enumerators.ReferencePoint.MiddleCenter:
                    result = GetStartPositionOnMiddleCenter(requested, proportional);
                    break;
                case Enumerators.ReferencePoint.MiddleRight:
                    result = GetStartPositionOnMiddleRight(requested, proportional);
                    break;
                case Enumerators.ReferencePoint.BottomLeft:
                    result = GetStartPositionOnBottomLeft(requested, proportional);
                    break;
                case Enumerators.ReferencePoint.BottomCenter:
                    result = GetStartPositionOnBottomCenter(requested, proportional);
                    break;
                case Enumerators.ReferencePoint.BottomRight:
                    result = GetStartPositionOnBottomRight(requested, proportional);
                    break;
                default:
                    result = new Point(0, 0);
                    break;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requested"></param>
        /// <param name="proportional"></param>
        /// <returns></returns>
        private static Point GetStartPositionOnBottomRight(Size requested, Size proportional)
        {
            int x = (int)(proportional.Width - requested.Width);
            int y = (int)(proportional.Height - requested.Height);
            return new Point(x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requested"></param>
        /// <param name="proportional"></param>
        /// <returns></returns>
        private static Point GetStartPositionOnBottomCenter(Size requested, Size proportional)
        {
            int x = (int)((proportional.Width - requested.Width) / 2);
            int y = (int)(proportional.Height - requested.Height);
            return new Point(x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requested"></param>
        /// <param name="proportional"></param>
        /// <returns></returns>
        private static Point GetStartPositionOnBottomLeft(Size requested, Size proportional)
        {
            int x = 0;
            int y = (int)(proportional.Height - requested.Height);
            return new Point(x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requested"></param>
        /// <param name="proportional"></param>
        /// <returns></returns>
        private static Point GetStartPositionOnMiddleRight(Size requested, Size proportional)
        {
            int x = (int)(proportional.Width - requested.Width);
            int y = (int)((proportional.Height - requested.Height) / 2);
            return new Point(x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requested"></param>
        /// <param name="proportional"></param>
        /// <returns></returns>
        private static Point GetStartPositionOnMiddleCenter(Size requested, Size proportional)
        {
            int x = (int)((proportional.Width - requested.Width) / 2);
            int y = (int)((proportional.Height - requested.Height) / 2);
            return new Point(x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requested"></param>
        /// <param name="proportional"></param>
        /// <returns></returns>
        private static Point GetStartPositionOnMiddleLeft(Size requested, Size proportional)
        {
            int x = 0;
            int y = (int)((proportional.Height - requested.Height) / 2);
            return new Point(x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requested"></param>
        /// <param name="proportional"></param>
        /// <returns></returns>
        private static Point GetStartPositionOnTopRight(Size requested, Size proportional)
        {
            int x = (int)(proportional.Width - requested.Width);
            int y = 0;
            return new Point(x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requested"></param>
        /// <param name="proportional"></param>
        /// <returns></returns>
        private static Point GetStartPositionOnTopCenter(Size requested, Size proportional)
        {
            int x = (int)((proportional.Width - requested.Width) / 2);
            int y = 0;
            return new Point(x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requested"></param>
        /// <param name="proportional"></param>
        /// <returns></returns>
        private static Point GetStartPositionOnTopLeft(Size requested, Size proportional)
        {
            int x = 0;
            int y = 0;
            return new Point(x, y);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="original"></param>
        /// <param name="requested"></param>
        /// <param name="fitInside"></param>
        /// <returns></returns>
        private static Size GetProportionalSize(Size original, Size requested, bool fitInside)
        {
            Size result = requested;
            double propW = (double)requested.Width / original.Width,
                   propH = (double)requested.Height / original.Height;

            double newProp;

            if (propH != propW)
            {
                if (fitInside)
                {
                    newProp = (propW < propH) ? propW : propH;
                }
                else
                {
                    newProp = (propW > propH) ? propW : propH;
                }

                result.Width = (int)(original.Width * newProp);
                result.Height = (int)(original.Height * newProp);
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static ImageFormat GetImageFormat(string fileName)
        {
            string fileExtension = Path.GetExtension(fileName).ToLower();
            ImageFormat result;
            switch (fileExtension)
            {
                case ".gif":
                    result = ImageFormat.Gif;
                    break;
                case ".png":
                    result = ImageFormat.Png;
                    break;
                default:
                    result = ImageFormat.Jpeg;
                    break;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="img"></param>
        /// <param name="size"></param>
        /// <returns></returns>
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
    }
}
