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
    /// <summary>
    /// Classe responsável por tratar as requisições atráves do "{imagem}.thumb.axd"
    /// </summary>
    public class Thumb
        : IHttpHandler
    {
        /// <summary>
        /// Propriedade referente a reutilização dos recursos para outras requests
        /// </summary>
        public bool IsReusable
        {
            get { return true; }
        }

        /// <summary>
        /// Método responsável por tratar a requisição para "{imagem}.thumb.axd"
        /// </summary>
        /// <param name="context">Contexto da requisição</param>
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
                    var stream = Transform.Convert(image);
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
    }
}