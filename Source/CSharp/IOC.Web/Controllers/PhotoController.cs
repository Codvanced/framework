using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Drawing.Imaging;
using IOC.FW.Core;
using IOC.FW.Configuration;

namespace IOC.Web.Controllers
{
    public class PhotoController
        : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public FileStreamResult Thumb(string filename, int width)
        {
            MemoryStream memoryStream = new MemoryStream();

            filename = Server.MapPath(filename);
            if (!System.IO.File.Exists(filename) || !filename.IsImage())
            {
                filename = Server.MapPath(Configurations.Current.Thumb.NotFoundPath);
            }

            var image = Bitmap.FromFile(filename);
            image = IOC.FW.Core.Images.Transform.Crop(image, new Rectangle(100, 100, 150, 150));

            image.Save(memoryStream, ImageFormat.Png);
            memoryStream.Position = 0;
            //int oldWidth = image.Width;
            //int oldHeight = image.Height;

            //int newHeight = (oldHeight * width / oldWidth);

            //using (var newImage = new Bitmap(width, newHeight))
            //using (var graphic = Graphics.FromImage(newImage))
            //{
            //    graphic.SmoothingMode = SmoothingMode.AntiAlias;
            //    graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //    graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
            //    graphic.DrawImage(image, new Rectangle(0, 0, width, newHeight));

            //    newImage.Save(memoryStream, ImageFormat.Png);
            //    memoryStream.Position = 0;
            //}

            return new
                FileStreamResult(memoryStream, image.RawFormat.GetMimeType());
        }
    }
}