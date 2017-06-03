using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SAC.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Southern Archery Circuit";
            ViewBag.Message = "Tournament Scores";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Title = "Southern Archery Circuit";
            ViewBag.Message = "The Southern Archery Circuit is comprised of archery clubs that host 3D archery tournaments in NC and SC.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Url = "https://www.facebook.com/southernarcherycircuit/";

            return View();
        }
    }
}