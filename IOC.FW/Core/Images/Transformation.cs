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
    #region enum ReferencePoint
    public enum ReferencePoint : byte
    {
        TopLeft = 1,
        TopCenter = 2,
        TopRight = 3,
        MiddleLeft = 4,
        MiddleCenter = 5,
        MiddleRight = 6,
        BottomLeft = 7,
        BottomCenter = 8,
        BottomRight = 9
    }
    #endregion

    public static class ImageTransformation
    {
        public static Image Crop(Image img, Size cropArea, ReferencePoint refPoint = ReferencePoint.MiddleCenter, bool fitInside = false)
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

        #region GetStartPosition
        public static Point GetStartPosition(Size requested, Size proportional, ReferencePoint refPoint)
        {
            Point result;

            switch (refPoint)
            {
                case ReferencePoint.TopLeft:
                    result = GetStartPositionOnTopLeft(requested, proportional);
                    break;
                case ReferencePoint.TopCenter:
                    result = GetStartPositionOnTopCenter(requested, proportional);
                    break;
                case ReferencePoint.TopRight:
                    result = GetStartPositionOnTopRight(requested, proportional);
                    break;
                case ReferencePoint.MiddleLeft:
                    result = GetStartPositionOnMiddleLeft(requested, proportional);
                    break;
                case ReferencePoint.MiddleCenter:
                    result = GetStartPositionOnMiddleCenter(requested, proportional);
                    break;
                case ReferencePoint.MiddleRight:
                    result = GetStartPositionOnMiddleRight(requested, proportional);
                    break;
                case ReferencePoint.BottomLeft:
                    result = GetStartPositionOnBottomLeft(requested, proportional);
                    break;
                case ReferencePoint.BottomCenter:
                    result = GetStartPositionOnBottomCenter(requested, proportional);
                    break;
                case ReferencePoint.BottomRight:
                    result = GetStartPositionOnBottomRight(requested, proportional);
                    break;
                default:
                    result = new Point(0, 0);
                    break;
            }

            //result.X = (requested.Width == proportional.Width) ? 0 : (int)((requested.Width - proportional.Width) / 2);
            //result.Y = (requested.Height == proportional.Height) ? 0 : (int)((requested.Height - proportional.Height) / 2);

            return result;
        }

        private static Point GetStartPositionOnBottomRight(Size requested, Size proportional)
        {
            int x = (int)(proportional.Width - requested.Width);
            int y = (int)(proportional.Height - requested.Height);
            return new Point(x, y);
        }

        private static Point GetStartPositionOnBottomCenter(Size requested, Size proportional)
        {
            int x = (int)((proportional.Width - requested.Width) / 2);
            int y = (int)(proportional.Height - requested.Height);
            return new Point(x, y);
        }

        private static Point GetStartPositionOnBottomLeft(Size requested, Size proportional)
        {
            int x = 0;
            int y = (int)(proportional.Height - requested.Height);
            return new Point(x, y);
        }

        private static Point GetStartPositionOnMiddleRight(Size requested, Size proportional)
        {
            int x = (int)(proportional.Width - requested.Width);
            int y = (int)((proportional.Height - requested.Height) / 2);
            return new Point(x, y);
        }

        private static Point GetStartPositionOnMiddleCenter(Size requested, Size proportional)
        {
            int x = (int)((proportional.Width - requested.Width) / 2);
            int y = (int)((proportional.Height - requested.Height) / 2);
            return new Point(x, y);
        }

        private static Point GetStartPositionOnMiddleLeft(Size requested, Size proportional)
        {
            int x = 0;
            int y = (int)((proportional.Height - requested.Height) / 2);
            return new Point(x, y);
        }

        private static Point GetStartPositionOnTopRight(Size requested, Size proportional)
        {
            int x = (int)(proportional.Width - requested.Width);
            int y = 0;
            return new Point(x, y);
        }

        private static Point GetStartPositionOnTopCenter(Size requested, Size proportional)
        {
            int x = (int)((proportional.Width - requested.Width) / 2);
            int y = 0;
            return new Point(x, y);
        }

        private static Point GetStartPositionOnTopLeft(Size requested, Size proportional)
        {
            int x = 0;
            int y = 0;
            return new Point(x, y);
        }
        #endregion

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

        public static MemoryStream Transform(Image image)
        {
            MemoryStream memoryStream = new MemoryStream();
            image.Save(memoryStream, ImageFormat.Png);
            memoryStream.Position = 0;
            return memoryStream;
        }

        private static string ApplicationPath(string file)
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            
            string path = Path.GetDirectoryName(
                Uri.UnescapeDataString(uri.Path)
            );

            path = Path.Combine(path, file);
            return path;
        }
    }
}
