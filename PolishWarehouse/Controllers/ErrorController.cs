using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PolishWarehouse.Models;

namespace PolishWarehouse.Controllers
{
    public class ErrorController : Controller
    {
        [HandleError]
        public ActionResult Index()
        {
            var raisedException = Server.GetLastError();
            Logging.LogEvent(LogTypes.Error, "500 Error triggered", "Internal Error Occured", raisedException);
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult Unavailable()
        {
            return View("Unavailable", "_ErrorLayout");
        }

        //protected override void OnException(ExceptionContext filterContext)
        //{
        //    var raisedException = Server.GetLastError();
        //    Logging.LogEvent(LogTypes.Error, "Error triggered", "Internal Error Occured", raisedException);

        //}
    }
}