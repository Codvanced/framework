using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.IO.Compression;
using System.Net;

namespace IOC.FW.Web.MVC.Handler
{
    public class CompressHandler
        : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, 
            CancellationToken cancellationToken
        )
        {
            return 
                base.SendAsync(request, cancellationToken)
                .ContinueWith((responseToCompleteTask) =>
                {
                    HttpResponseMessage response = responseToCompleteTask.Result;

                    if (response.RequestMessage.Headers.AcceptEncoding != null 
                        && response.RequestMessage.Headers.AcceptEncoding.Count > 0
                        && response.Content != null)
                    {
                        string encodingType = response.RequestMessage.Headers.AcceptEncoding.First().Value;

                        response.Content = new CompressedContent(response.Content, encodingType);
                    }

                    return response;
                });
        }
    }

    public class CompressedContent 
        : HttpContent
    {
        private HttpContent originalContent;
        private string encodingType;

        public CompressedContent(HttpContent content, string encodingType)
        {
            if (content == null || string.IsNullOrWhiteSpace(encodingType))
                return;

            originalContent = content;
            this.encodingType = encodingType.ToLowerInvariant();

            foreach (KeyValuePair<string, IEnumerable<string>> header in originalContent.Headers)
            {
                this.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            this.Headers.ContentEncoding.Add(encodingType);
        }

        protected override bool TryComputeLength(out long length)
        {
            length = -1;
            return false;
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            Stream compressedStream = null;
            var culture = StringComparison.InvariantCultureIgnoreCase;

            if (encodingType.Equals("gzip", culture))
                compressedStream = new GZipStream(stream, CompressionMode.Compress, leaveOpen: true);
            else if (encodingType.Equals("deflate", culture))
                compressedStream = new DeflateStream(stream, CompressionMode.Compress, leaveOpen: true);

            return originalContent.CopyToAsync(compressedStream).ContinueWith(tsk =>
            {
                if (compressedStream != null)
                {
                    compressedStream.Dispose();
                }
            });
        }
    }
}