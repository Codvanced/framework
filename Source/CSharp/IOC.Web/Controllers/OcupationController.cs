using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IOC.Abstraction.Business;
using IOC.FW.Core.Factory;
using IOC.Model;

namespace IOC.Web.Controllers
{
    public class OcupationController
        : Controller
    {
        private readonly OcupationBusinessAbstract _business;

        public OcupationController(OcupationBusinessAbstract business)
        {
            this._business = business;
        }

        public ActionResult Index()
        {
            var ocupation = this._business.SelectAll();
            return View();
        }
    }
}
