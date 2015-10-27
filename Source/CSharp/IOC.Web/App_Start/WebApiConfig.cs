using System.Web.Http;
using IOC.FW.Web.MVC.Handler;

namespace IOC.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Formatters.XmlFormatter.UseXmlSerializer = true;
            config.MessageHandlers.Add(new BasicAuthenticationHandler());
            config.MessageHandlers.Add(new CompressHandler());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }  
            );
        }
    }
}
