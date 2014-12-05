using System.Web;
using System.Web.Mvc;

namespace IOC.FW.Web.MVC.Filters
{
    /// <summary>
    /// Atributo para incluir cabeçalho de no cache em actions
    /// </summary>
    public class NonCacheAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Método responsável por interceptar a execução de uma action e incluir o cabeçalho de no cache
        /// </summary>
        /// <param name="filterContext">Contexto da action para a inclusão do cabeçalho</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            base.OnActionExecuting(filterContext);
        }
    }
}