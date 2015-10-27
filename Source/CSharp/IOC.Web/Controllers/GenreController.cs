using System.Web.Mvc;
using IOC.Model;
using IOC.FW.Core.Abstraction.Business;

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
