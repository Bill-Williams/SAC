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
        private SacContext db = new SacContext();

        public ActionResult Index()
        {
            ViewBag.Title = "Home";
            //ViewBag.Message = "Tournament Scores";

            //var tournaments = db.Tournaments
            //    .Where(t => t.Schedule.Date.Year == DateTime.Now.Year)
            //    .Include(t => t.Schedule.Club);

            //return View(tournaments.OrderByDescending(t => t.Schedule.Date));
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