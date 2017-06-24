using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SAC.Domain;
using SAC.Domain.Models;
using SAC.Web.Extensions;
using SAC.Web.Models;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;

namespace SAC.Web.Controllers
{
    [RequireHttps]
    [Authorize(Roles = "Club Admin,Tech Admin")]
    public class TournamentsController : Controller
    {
        private SacContext db = new SacContext();

        // GET: Tournaments
        [AllowAnonymous]
        public ActionResult Index()
        {
            var tournaments = db.Tournaments.Include("Schedules.Club");
            return View(tournaments.ToList());
        }

        // GET: Tournaments/Results/5
        [AllowAnonymous]
        public ActionResult Results(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tournament = db.Tournaments.Include("Schedules.Club").FirstOrDefault(t => t.Id == id);
            if (tournament == null)
            {
                return HttpNotFound();
            }

            TournamentResultViewModel result = new TournamentResultViewModel()
            {
                Tournament = tournament,
                Groups = db.Groups.ToList(),
                Classes = db.Classes.Include(c => c.Color).ToList()
            };
            return View(result);
        }

        // GET: Tournaments/Admin
        public ActionResult Admin()
        {
            IEnumerable<Tournament> tournaments;
            if (User.IsInRole("Tech Admin"))
            {
                tournaments = db.Tournaments.Include("Schedules.Club");
            }
            else
            {
                var clubs = User.Identity.GetClubs(db).Select(c => c.Id);
                tournaments = db.Tournaments.Include("Schedules.Club").Where(t => t.Schedules.Select(s => s.Club.Id).Intersect(clubs).Count() > 0);
            }
            return View(tournaments);
        }

        // GET: Tournaments/Create
        public ActionResult Create()
        {
            SetupLists();
            return View();
        }

        // POST: Tournaments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "Schedules")] Guid[] schedules)
        {
            Tournament tournament = new Tournament();
            foreach (Guid id in schedules)
            {
                tournament.Schedules.Add(db.Schedules.Find(id));
            }
            if (ModelState.IsValid)
            {
                
                db.Tournaments.Add(tournament);
                db.SaveChanges();
                return RedirectToAction("Admin");
            }
            SetupLists();
            return View(tournament);
        }

        // GET: Tournaments/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tournament = db.Tournaments.Include("Schedules.Club").Include("Competitors.Class.Group").FirstOrDefault(t => t.Id == id);
            if (tournament == null)
            {
                return HttpNotFound();
            }
            if (!User.IsInRole("Tech Admin")
                // User has rights to a Club associated to this tournament
                && tournament.Schedules.Select(s => s.Club).Intersect(User.Identity.GetClubs(db)).Count() == 0)
            {
                return HttpNotFound();
            }
            SetupLists();
            return View(tournament);
        }

        // POST: Tournaments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Completed")] Tournament tournament)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tournament).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Admin");
            }
            SetupLists();
            return View(tournament);
        }

        // GET: Tournaments/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournament tournament = db.Tournaments.Include("Schedules.Club").Include("Competitors").FirstOrDefault(t => t.Id == id); ;
            if (tournament == null)
            {
                return HttpNotFound();
            }
            if (!User.IsInRole("Tech Admin")
                // User has rights to a Club associated to this tournament
                && tournament.Schedules.Select(s => s.Club).Intersect(User.Identity.GetClubs(db)).Count() == 0)
            {
                return HttpNotFound();
            }
            return View(tournament);
        }

        // POST: Tournaments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Tournament tournament = db.Tournaments.Include("Schedules.Club").FirstOrDefault(t => t.Id == id);
            if(User.IsInRole("Tech Admin")
                // User has rights to a Club associated to this tournament
                || tournament.Schedules.Select(s => s.Club).Intersect(User.Identity.GetClubs(db)).Count() > 0)
            {
                tournament.Schedules.Clear();
                db.Tournaments.Remove(tournament);
                db.SaveChanges();
                return RedirectToAction("Admin");
            }
            return View(tournament);
        }

        // GET: Tournaments/Complete/5
        public ActionResult Complete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournament tournament = db.Tournaments.Include("Schedules.Club").Include("Competitors").FirstOrDefault(t => t.Id == id); ;
            if (tournament == null)
            {
                return HttpNotFound();
            }
            if (!User.IsInRole("Tech Admin")
                // User has rights to a Club associated to this tournament
                && tournament.Schedules.Select(s => s.Club).Intersect(User.Identity.GetClubs(db)).Count() == 0)
            {
                return HttpNotFound();
            }
            return View(tournament);
        }

        // POST: Tournaments/Complete/5
        [HttpPost, ActionName("Complete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CompleteConfirmed(Guid id)
        {
            Tournament tournament = db.Tournaments.Include("Schedules.Club").FirstOrDefault(t => t.Id == id);
            if (User.IsInRole("Tech Admin")
                // User has rights to a Club associated to this tournament
                || tournament.Schedules.Select(s => s.Club).Intersect(User.Identity.GetClubs(db)).Count() > 0)
            {
                // Update completed flag and save
                tournament.Completed = true;
                db.SaveChanges();

                // Send out e-mail that tournament is complete
                await SendCompleteBlast(tournament.Id);

                // Send to results view when done
                return RedirectToAction("Results", new { id = tournament.Id });
            }
            return new HttpStatusCodeResult(403);
        }
        
        // GET: Tournaments/Correction/5
        public ActionResult Correction(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournament tournament = db.Tournaments.Include("Schedules.Club").Include("Competitors").FirstOrDefault(t => t.Id == id); ;
            if (tournament == null)
            {
                return HttpNotFound();
            }
            if (!User.IsInRole("Tech Admin")
                // User has rights to a Club associated to this tournament
                && tournament.Schedules.Select(s => s.Club).Intersect(User.Identity.GetClubs(db)).Count() == 0)
            {
                return new HttpStatusCodeResult(403);
            }
            return View(tournament);
        }

        // POST: Tournaments/Correction/5
        [HttpPost, ActionName("Correction")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CorrectioneConfirmed(Guid id)
        {
            Tournament tournament = db.Tournaments.Include("Schedules.Club").FirstOrDefault(t => t.Id == id);
            if (User.IsInRole("Tech Admin")
                // User has rights to a Club associated to this tournament
                || tournament.Schedules.Select(s => s.Club).Intersect(User.Identity.GetClubs(db)).Count() > 0)
            {
                // Send out e-mail that tournament is complete
                await SendCompleteBlast(id);

                return RedirectToAction("Admin");
            }
            return new HttpStatusCodeResult(403);
        }

        private void SetupLists()
        {
            if(User.IsInRole("Tech Admin"))
            {
                ViewBag.ScheduleList = new MultiSelectList(db.Schedules.Where(s => null == s.Tournament).OrderBy(s => s.Date), "Id", "ShortDate");
            }
            else // Get Schedules for Clubs you have an association to your user
            {
                var clubs = User.Identity.GetClubs(db).Select(c => c.Id);
                ViewBag.ScheduleList = new MultiSelectList(db.Schedules.Where(s => null == s.Tournament && clubs.Contains(s.Club.Id)).OrderBy(s => s.Date), "Id", "ShortDate");
            }
        }

        private async Task SendCompleteBlast(Guid tournamentId)
        {
            var emailService = new EmailService();
            var callbackUrl = Url.Action("Results", "Tournaments", new { id = tournamentId }, protocol: Request.Url.Scheme);
            await emailService.SendCompleteBlastAsync(callbackUrl);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
