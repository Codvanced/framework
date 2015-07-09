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
        private readonly IOcupationBusiness _ocupationBusiness;

        public OcupationController(IOcupationBusiness ocupationBusiness)
        {
            this._ocupationBusiness = ocupationBusiness;
        }

        public ActionResult Index()
        {
            var ocupation = this._ocupationBusiness.SelectAll();
            return View();
        }
    }
}
