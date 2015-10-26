using System.Web.Mvc;
using IOC.Web.Models;
using IOC.FW.Web.MVC.Filters;

namespace IOC.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new BeginRequestFilter());
            filters.Add(new LogExceptionFilter());
        }
    }
}