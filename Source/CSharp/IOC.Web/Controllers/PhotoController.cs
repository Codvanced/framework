using System.Web.Mvc;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using IOC.FW.Configuration;
using IOC.FW.Extensions;

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
                var configuration = ConfigManager.GetConfig();
                filename = Server.MapPath(configuration.Thumb.NotFoundPath);
            }

            var image = Bitmap.FromFile(filename);
            image = FW.ImageTransformation.Transform.Crop(image, new Rectangle(100, 100, 150, 150));

            image.Save(memoryStream, ImageFormat.Png);
            memoryStream.Position = 0;

            return new
                FileStreamResult(memoryStream, image.RawFormat.GetMimeType());
        }
    }
}