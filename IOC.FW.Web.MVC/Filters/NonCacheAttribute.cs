using System.Web;
using System.Web.Mvc;

namespace IOC.FW.Web.MVC.Filters
{
    /// <summary>
    /// Atributo que muda o header de resposta dizendo para não cachear o conteúdo
    /// </summary>
    public class NonCacheAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            base.OnActionExecuting(filterContext);
        }
    }
}