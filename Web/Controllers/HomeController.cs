using System.Web.Mvc;
using SAC.Web.App_Start;

namespace SAC.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Privacy()
        {
            return View();
        }

        public ActionResult TermsOfService()
        {
            return View();
        }
    }
}