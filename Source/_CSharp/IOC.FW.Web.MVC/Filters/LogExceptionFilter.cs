using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using log4net;
using IOC.FW.Core.Logging;

namespace IOC.FW.Web.MVC.Filters
{
    /// <summary>
    /// Classe responsável por interceptar e logar exceções 
    /// </summary>
    public class LogExceptionFilter : IExceptionFilter
    {
        /// <summary>
        /// Método responsável por logar exceções
        /// </summary>
        /// <param name="filterContext">Contexto da action que gerou a exceção</param>
        public void OnException(ExceptionContext filterContext)
        {
            var log = LogFactory.CreateLog(filterContext.Controller.GetType());

            if (null != filterContext && null != filterContext.Exception)
            {
                log.Error(
                        String.Format(
                            "Erro na seguinte url => {0} {1}: {2}",
                            filterContext.HttpContext.Request.HttpMethod,
                            filterContext.HttpContext.Request.Url.AbsoluteUri,
                            filterContext.Exception.Message
                        ),
                    filterContext.Exception);
            }
            else
            {
                log.Error("LogExceptionFilter: filterContext ou filterContext.Exception nulos");
            }
        }
    }
}
