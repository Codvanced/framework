using System;
using System.Web;
using System.IO;
using System.Drawing;
using IOC.FW.Configuration;
using IOC.FW.Shared.Enumerators;
using IOC.FW.ImageTransformation;
using System.Web.Caching;

namespace IOC.FW.Web.Handler
{
    /// <summary>
    /// Classe responsável por tratar as requisições atráves do "{imagem}.thumb.axd"
    /// </summary>
    public class ThumbHandler
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
        /// Método responsável por tratar a requisição para "{imagem}.thumb.axd"
        /// </summary>
        /// <param name="context">Contexto da requisição</param>
        public void ProcessRequest(HttpContext context)
        {
            var configuration = ConfigManager.GetConfig();
            var refpoint = ImageEnumerator.ReferencePoint.MiddleCenter;
            var crop = false;
            var width = configuration.Thumb.DefaultWidth;
            var height = configuration.Thumb.DefaultHeight;

            if (context.Request["refpoint"] != null)
                Enum.TryParse(context.Request["refpoint"], out refpoint);

            if (context.Request["crop"] != null)
                bool.TryParse(context.Request["crop"], out crop);

            if (context.Request["width"] != null)
                int.TryParse(context.Request["width"], out width);

            if (context.Request["height"] != null)
                int.TryParse(context.Request["height"], out height);

            var filePath = CleanFilePath(context.Request.FilePath);
            if (string.IsNullOrWhiteSpace(filePath))
            {
                filePath = configuration.Thumb.NotFoundPath;
            }

            if (!string.IsNullOrWhiteSpace(filePath))
            {
                filePath = context.Server.MapPath(filePath);
                if (File.Exists(filePath))
                {
                    var size = new Size(width, height);
                    using (var img = Image.FromFile(filePath))
                    {
                        var image = crop ?
                            Thumbnail.Create(img, size, refpoint) :
                            Thumbnail.Resize(img, size);

                        using (image)
                        {
                            var fileName = Path.GetFileName(filePath);
                            var stream = Transform.Convert(image);
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
                                    image,
                                    null,
                                    DateTime.Now.AddMilliseconds(configuration.Thumb.Expiration),
                                    TimeSpan.FromMilliseconds(configuration.Thumb.SlidingExpiration),
                                    cachePriority,
                                    null
                                );
                            }
                        }
                    }
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