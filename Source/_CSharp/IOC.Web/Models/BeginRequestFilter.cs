using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IOC.Web.Models
{
    public class BeginRequestFilter
        : FilterAttribute, IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
        }

        void IResultFilter.OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.Title = "Title Teste - Begin Request Filter";
        }
    }
}