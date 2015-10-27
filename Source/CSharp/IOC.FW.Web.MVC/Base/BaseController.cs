using IOC.FW.Core.Model;
using System;
using System.Web.Mvc;

namespace IOC.FW.Web.MVC.Base
{
    /// <summary>
    /// Classe responsável por oferecer recursos genéricos para controllers
    /// </summary>
    public class BaseController
        : Controller
    {
        /// <summary>
        /// Método sobreescrito responsável por tratar exceções em actions
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                string actionName = filterContext.RouteData.Values["action"].ToString();
                Type controllerType = filterContext.Controller.GetType();
                var method = controllerType.GetMethod(actionName);
                var returnType = method.ReturnType;

                if (returnType.Equals(typeof(JsonResult)))
                {
                    filterContext.Result = new JsonResult()
                    {
                        Data = new BaseServiceModel
                        {   
                            Data = null,
                            IdOp = -1,
                            Message = "Sorry, an error occurred while processing your request.",
                            Success = false
                        }, 
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
                else if (returnType.Equals(typeof(ActionResult))
                    || (returnType).IsSubclassOf(typeof(ActionResult)))
                {
                    filterContext.Result = new ViewResult()
                    {
                        ViewName = "Error"
                    };
                }

                filterContext.ExceptionHandled = true;
            }

            base.OnException(filterContext);
        }
    }
}