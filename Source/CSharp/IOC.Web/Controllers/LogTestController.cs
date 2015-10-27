using IOC.FW.Logging;
using System.Web;
using System.Web.Mvc;

namespace IOC.Web.Controllers
{
    public class LogTestController : Controller
    {
        //
        // GET: /LogTest/

        public ActionResult Index()
        {
            var log = LogFactory.CreateLog();

            if (log.IsInfoEnabled)
            {
                log.Info("Running...");
            }

            throw new HttpException(500, "Erro");
        }

    }
}
