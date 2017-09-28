using System.Web.Mvc;

namespace PolishWarehouse.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Polish");
        }
    }
}