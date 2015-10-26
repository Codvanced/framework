using System.Web.Mvc;
using IOC.Abstraction.Business;

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
