using System;
using System.Linq;
using System.Web;
using System.IO;
using System.Drawing;
using IOC.FW.Configuration;
using IOC.FW.ImageTransformation;
using IOC.FW.Logging;

namespace IOC.FW.Web.Handler
{
    /// <summary>
    /// Classe responsável por tratar as requisições atráves do "{imagem}.transform.axd"
    /// </summary>
    public class TransformHandler
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
        /// Método responsável por tratar a requisição para "{imagem}.transform.axd"
        /// </summary>
        /// <example>
        /// resize=200x200&crop=location:10x10|size:20x20
        /// </example>
        /// <param name="context">Contexto da requisição</param>
        public void ProcessRequest(HttpContext context)
        {
            string filename = context.Request.FilePath.Replace(".transform.axd", string.Empty);

            bool needResize = false,
                needCrop = false;

            var resize = Size.Empty;
            var crop = Rectangle.Empty;

            if (needResize = context.Request.QueryString["resize"] != null)
            {
                string resizeParams = context.Request.QueryString["resize"];

                if (!string.IsNullOrEmpty(resizeParams))
                {
                    resizeParams = resizeParams.ToLower();
                    string[] splittedParams = resizeParams.Split('x');
                    if (splittedParams != null && splittedParams.Length == 2)
                    {
                        int tempWidth = 0;
                        int tempHeight = 0;

                        int.TryParse(splittedParams[0], out tempWidth);
                        int.TryParse(splittedParams[1], out tempHeight);

                        resize = new Size(tempWidth, tempHeight);
                    }
                }
                else
                {
                    needResize = false;
                }
            }

            if (needCrop = context.Request.QueryString["crop"] != null)
            {
                string cropParams = context.Request.QueryString["crop"];

                if (!string.IsNullOrEmpty(cropParams))
                {
                    cropParams = cropParams.ToLower();
                    string[] splittedParams = cropParams.Split('|');
                    if (splittedParams != null && splittedParams.Length == 2)
                    {
                        string locationParam = splittedParams.First(
                            w => w.Contains("location:")
                        );

                        string sizeParam = splittedParams.First(
                            w => w.Contains("size:")
                        );

                        crop = new Rectangle();
                        if (!string.IsNullOrEmpty(locationParam))
                        {
                            locationParam = locationParam.Replace("location:", "");
                            var splittedLocationParams = locationParam.Split('x');

                            if (splittedLocationParams != null && splittedLocationParams.Length > 0)
                            {
                                int tempX = 0;
                                int tempY = 0;

                                int.TryParse(splittedLocationParams[0], out tempX);
                                int.TryParse(splittedLocationParams[1], out tempY);

                                crop.X = tempX;
                                crop.Y = tempY;
                            }
                        }

                        if (!string.IsNullOrEmpty(sizeParam))
                        {
                            sizeParam = sizeParam.Replace("size:", "");
                            var splittedSizeParams = sizeParam.Split('x');

                            if (splittedSizeParams != null && splittedSizeParams.Length > 0)
                            {
                                int tempWidth = 0;
                                int tempHeight = 0;

                                int.TryParse(splittedSizeParams[0], out tempWidth);
                                int.TryParse(splittedSizeParams[1], out tempHeight);

                                crop.Width = tempWidth;
                                crop.Height = tempHeight;
                            }
                        }
                    }
                }
                else
                {
                    needCrop = false;
                }
            }

            filename = HttpContext.Current.Server.MapPath(filename);
            Image img = null;

            if (File.Exists(filename))
                img = Image.FromFile(filename);
            else
                img = Image.FromFile(HttpContext.Current.Server.MapPath(Configurations.Current.Thumb.NotFoundPath));

            try
            {
                if (needResize || needCrop)
                {
                    if (needResize)
                    {
                        img = Transform.Resize(img, resize);
                    }

                    if (needCrop)
                    {
                        img = Transform.Crop(img, crop);
                    }

                    var stream = Transform.Convert(img);
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
