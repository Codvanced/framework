using System;
using System.Web;
using System.Threading;
using System.Text;
using System.Security.Principal;
using System.Net;
using IOC.FW.Authentication;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Net.Http;

namespace IOC.FW.Web.MVC.Filters
{
    /// <summary>
    /// Classe responsável por gerenciar autenticações 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public abstract class BasicAuthenticationFilter
        : AuthorizationFilterAttribute
    {
        /// <summary>
        /// Método responsável por validar a autenticidade do usuário
        /// </summary>
        /// <param name="username">Usuário ou login</param>
        /// <param name="password">Senha</param>
        /// <param name="actionContext">Contexto da action que requer autenticação</param>
        /// <returns>Booleano informando se o login é valido</returns>
        private bool OnAuthorizeUser(string username, string password, HttpActionContext actionContext)
        {
            bool validUser =
                   actionContext != null
                && !string.IsNullOrWhiteSpace(username)
                && !string.IsNullOrWhiteSpace(password)
                && Login(username, password);

            return validUser;
        }

        /// <summary>
        /// Método responsável por adicionar os cabeçalhos de desafio do Basic Authentication
        /// </summary>
        /// <param name="actionContext">Contexto da action que requer autenticação</param>
        private void Challenge(HttpActionContext actionContext)
        {
            var host = actionContext.Request.RequestUri.DnsSafeHost;
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            actionContext.Response.Headers.Add("WWW-Authenticate", string.Format("xBasic realm=\"{0}\"", host));
        }

        /// <summary>
        /// Método responsável por converter a autenticação enviada em base64 no cabeçalho de "Authorization"  (permite sobreescrita)
        /// </summary>
        /// <param name="actionContext">Contexto da action que requer autenticação</param>
        /// <returns>Objeto com dados de usuário e senha</returns>
        protected virtual BasicAuthenticationIdentity ParseAuthorizationHeader(HttpActionContext actionContext)
        {
            string authHeader = null;
            var auth = actionContext.Request.Headers.Authorization;
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

        /// <summary>
        /// Método sobreescrito de AuthorizationFilterAttribute para validar ou enviar o desafio de BasicAuthentication
        /// </summary>
        /// <param name="actionContext">Contexto da action que requer autenticação</param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var identity = ParseAuthorizationHeader(actionContext);
            if (identity == null || !OnAuthorizeUser(identity.Name, identity.Password, actionContext))
            {
                Challenge(actionContext);
                return;
            }

            var principal = new GenericPrincipal(identity, null);
            Thread.CurrentPrincipal = principal;

            if (HttpContext.Current != null)
                HttpContext.Current.User = principal;

            base.OnAuthorization(actionContext);
        }

        /// <summary>
        /// Método abstrato para que quem o estenda possa definir a regra de validação de usuário e senha
        /// </summary>
        /// <param name="user">Usuário ou login</param>
        /// <param name="password">Senha</param>
        /// <returns>Booleano informando se o login é valido</returns>
        public abstract bool Login(string user, string password);
    }
}