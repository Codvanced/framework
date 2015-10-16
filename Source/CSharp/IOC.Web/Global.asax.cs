using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SimpleInjector.Integration.Web.Mvc;
using IOC.FW.Core.Logging;
using IOC.FW.Core.Implementation.DIContainer.SimpleInjector;
using IOC.Abstraction.Business;

namespace IOC.Web
{
    public class MvcApplication 
        : System.Web.HttpApplication
    {
        private log4net.ILog _log;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Create dapater and revolver dependency with framework
            var adapter = new SimpleInjectorAdapter();
            FW.Core.Implementation.DIContainer.DependencyResolver.Resolve(adapter);

            // Pass container of framework to dependency resolver of .NET MVC
            var resolve = new SimpleInjectorDependencyResolver(adapter._container);
            DependencyResolver.SetResolver(resolve);

            MvcHandler.DisableMvcResponseHeader = true;
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            var application = sender as HttpApplication;
            if (application != null && application.Context != null)
            {
                try
                {
                    application.Context.Response.Headers.Remove("Server");
                }
                catch
                {
                    // se ocorrer uma exception neste local, significa que você 
                    // está rodando no VS 2010 WebServer, em um ambiente de IIS/IIS Express não ocorrerá este problema
                }
            }
        }

        void Application_Error(object sender, EventArgs e)
        {
            if (_log == null)
            {
                _log = LogFactory.CreateLog();
            }

            var ex = Server.GetLastError();

            if (!(ex is HttpException) || ((HttpException)ex).GetHttpCode() != 404)
            {
                _log.Error(String.Format(
                        "Erro na seguinte url => {0} {1}",
                        Context.Request.HttpMethod,
                        Context.Request.Url.AbsoluteUri
                    ), ex);

            }
        }
    }
}