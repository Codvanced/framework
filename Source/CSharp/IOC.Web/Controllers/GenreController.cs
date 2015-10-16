using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimpleInjector;
using IOC.Model;
using IOC.Validation;
using IOC.Abstraction.Business;
using IOC.FW.Core.Abstraction.Business;
using IOC.FW.Web.MVC.Base;

namespace IOC.Web.Controllers
{
    public class GenreController
        : Controller
    {
        private readonly IBaseBusiness<Genre> _business;

        public GenreController(IBaseBusiness<Genre> business)
        {
            this._business = business;
        }

        public ActionResult Index()
        {
            var items = _business.SelectAll();

            return View(items);
        }

        public JsonResult GetJson()
        {
            var items = _business.SelectAll();
            return Json(items, JsonRequestBehavior.AllowGet);
        }
    }
}
