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
            ViewBag.Title = "Southern Archery Circuit";
            ViewBag.Message = "Tournament Scores";

            var tournaments = db.Tournaments
                .Where(t => t.Schedule.Date.Year == DateTime.Now.Year)
                .Include(t => t.Schedule.Club);

            return View(tournaments.OrderByDescending(t => t.Schedule.Date));
        }

        public ActionResult About()
        {
            ViewBag.Title = "Southern Archery Circuit";
            ViewBag.Message = "The Southern Archery Circuit is comprised of archery clubs that host 3D archery tournaments in NC and SC.";

            return View();
        }

        public ActionResult Contact()
        {
            return View(new ContactViewModel());
        }
    }
}