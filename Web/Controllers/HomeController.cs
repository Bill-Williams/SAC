using SAC.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SAC.Domain;
using SAC.Domain.Models;
using System.Data.Entity;

namespace SAC.Web.Controllers
{
    public class HomeController : Controller
    {
        //private SacContext db = new SacContext();

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