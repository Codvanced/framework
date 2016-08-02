using IOC.FW.Abstraction.Logging;
using System.Web.Mvc;

namespace IOC.FW.Web.MVC.Filters
{
    /// <summary>
    /// Classe responsável por interceptar e logar exceções 
    /// </summary>
    public class LogExceptionFilter
        : IExceptionFilter
    {
        private readonly ILogger _logger;

        public LogExceptionFilter(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Método responsável por logar exceções
        /// </summary>
        /// <param name="filterContext">Contexto da action que gerou a exceção</param>
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext != null && filterContext.Exception != null)
            {
                _logger.Error(
                    string.Format(
                        "Erro na seguinte url => {0} {1}: {2}",
                        filterContext.HttpContext.Request.HttpMethod,
                        filterContext.HttpContext.Request.Url.AbsoluteUri,
                        filterContext.Exception.Message
                    ),
                    filterContext.Exception
                );
            }
            else
            {
                _logger.Error("LogExceptionFilter: filterContext ou filterContext.Exception nulos");
            }
        }
    }
}