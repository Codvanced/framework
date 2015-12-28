using System;
using System.Linq;
using System.Web;
using System.IO;
using System.Drawing;
using IOC.FW.Configuration;
using IOC.FW.ImageTransformation;
using System.Web.Caching;

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
            get { return false; }
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
            var configuration = ConfigManager.GetConfig();
            var filePath = CleanFilePath(context.Request.FilePath);

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

            filePath = HttpContext.Current.Server.MapPath(filePath);
            Image img = null;

            if (File.Exists(filePath))
                img = Image.FromFile(filePath);
            else
                img = Image.FromFile(
                    HttpContext.Current.Server.MapPath(configuration.Thumb.NotFoundPath)
                );

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

                    var fileName = Path.GetFileName(filePath);
                    var stream = Transform.Convert(img);
                    context.Response.ContentType = "image/png";
                    context.Response.BinaryWrite(stream.ToArray());
                    context.Response.Flush();

                    if (configuration.Thumb.EnableCache
                                && context.Cache[fileName] == null
                            )
                    {
                        var cachePriority = CacheItemPriority.Normal;
                        if (configuration.Thumb.CachePriority > 0)
                        {
                            Enum.TryParse(configuration.Thumb.CachePriority.ToString(), out cachePriority);
                        }

                        context.Cache.Add(
                            fileName,
                            img,
                            null,
                            DateTime.Now.AddMilliseconds(configuration.Thumb.Expiration),
                            TimeSpan.FromMilliseconds(configuration.Thumb.SlidingExpiration),
                            cachePriority,
                            null
                        );
                    }
                }
        }

        private string CleanFilePath(string filePath)
        {
            var fileName = Path.GetFileName(filePath);

            if (!string.IsNullOrWhiteSpace(fileName))
            {
                var fileNameFragments = fileName.Split('.');
                if (fileNameFragments.Length > 1)
                {
                    filePath = filePath.Replace(
                        fileName,
                        string.Format(
                            "{0}.{1}",
                            fileNameFragments[0],
                            fileNameFragments[1]
                        )
                    );
                }
            }

            return filePath;
        }
    }
}