using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IOC.FW.Core.Abstraction.Business;

namespace %NAME_REPLACE%.Web.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public HomeController()
        {
            
        }

        public ActionResult Index()
        {
            return View();
        }

    }

}
