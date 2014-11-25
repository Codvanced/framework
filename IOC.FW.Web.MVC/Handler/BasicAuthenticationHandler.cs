using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Security.Principal;
using System.Web;
using System.Net;
using IOC.FW.Core.Base;

namespace IOC.FW.Web.MVC.Handler
{
    public class BasicAuthenticationHandler
        : DelegatingHandler
    {
        private bool OnAuthorize(BasicAuthenticationIdentity identity)
        {
            var authorize =
                        identity != null
                    && !string.IsNullOrEmpty(identity.Name)
                    && !string.IsNullOrEmpty(identity.Password);

            return authorize;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken
        )
        {
            var identity = this.ParseAuthorizationHeader(request);
            if (identity != null && OnAuthorize(identity))
            {
                var principal = new GenericPrincipal(identity, null);
                Thread.CurrentPrincipal = principal;

                if (HttpContext.Current != null)
                    HttpContext.Current.User = principal;
            }

            return
                base.SendAsync(request, cancellationToken)
                    .ContinueWith<HttpResponseMessage>(
                    task =>
                    {
                        var response = task.Result;

                        if (identity == null && response.StatusCode == HttpStatusCode.Unauthorized)
                            Challenge(request, response);

                        return response;
                    });
        }

        protected virtual BasicAuthenticationIdentity ParseAuthorizationHeader(HttpRequestMessage request)
        {
            string authHeader = null;
            var auth = request.Headers.Authorization;
            if (auth != null && auth.Scheme == "Basic")
                authHeader = auth.Parameter;

            if (string.IsNullOrEmpty(authHeader))
                return null;

            authHeader = Encoding.Default.GetString(Convert.FromBase64String(authHeader));

            var tokens = authHeader.Split(':');
            if (tokens.Length < 2)
                return null;

            return new BasicAuthenticationIdentity(tokens[0], tokens[1]);
        }

        private void Challenge(
            HttpRequestMessage request,
            HttpResponseMessage response
        )
        {
            var host = request.RequestUri.DnsSafeHost;
            var authenticateHeader = string.Format("Basic realm=\"{0}\"", host);
            response.Headers.Add("WWW-Authenticate", authenticateHeader);
        }
    }
}
