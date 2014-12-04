using System;
using System.Web;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Configuration;
using IOC.FW.Core;
using IOC.FW.Core.Images;
using IOC.FW.Configuration;
using IOC.FW.Core.Logging;

namespace IOC.FW.Web
{
    public class Thumb 
        : IHttpHandler
    {
        /// <summary>
        /// You will need to configure this handler in the web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpHandler Members
        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            string filename = context.Request.FilePath.Replace(".thumb.axd", string.Empty);

            bool crop = false;

            int? width = null; int w = 0;

            int? height = null; int h = 0;

            var refpoint = Enumerators.ReferencePoint.MiddleCenter;

            if (context.Request["refpoint"] != null)
                Enum.TryParse<Enumerators.ReferencePoint>(context.Request["refpoint"], out refpoint);

            if (context.Request["width"] != null)
                int.TryParse(context.Request["width"], out w);

            if (w > 0)
                width = w;

            if (context.Request["height"] != null)
                int.TryParse(context.Request["height"], out h);

            if (h > 0)
                height = h;

            if (context.Request["crop"] != null)
                bool.TryParse(context.Request["crop"], out crop);

            if (!width.HasValue)
                width = Configurations.Current.Thumb.DefaultWidth;

            if (crop && !height.HasValue)
                height = Configurations.Current.Thumb.DefaultHeight;
            else
                height = h;

            filename = HttpContext.Current.Server.MapPath(filename);

            Image img;

            if (File.Exists(filename))
                img = Image.FromFile(filename);
            else
                img = Image.FromFile(HttpContext.Current.Server.MapPath(Configurations.Current.Thumb.NotFoundPath));

            var size = new Size(width.Value, height.Value);

            try
            {
                using (img)
                using (
                        var image = crop ?
                            Thumbnail.Create(img, size, refpoint) :
                            Thumbnail.Resize(img, size)
                    )
                {
                    var stream = Thumbnail.Transform(image);
                    context.Response.ContentType = "image/png";
                    context.Response.BinaryWrite(stream.ToArray());
                    context.Response.Flush();
                }
            }
            catch (Exception e)
            {
                var log = LogFactory.CreateLog(typeof(Thumb));

                if (log.IsInfoEnabled)
                {
                    log.Info(String.Concat("Erro na URL ", context.Request.Url.AbsoluteUri), e);
                }
            }
        }
        #endregion
    }
}