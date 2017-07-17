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
using SAC.Web.Services;

namespace SAC.Web.Controllers
{
    [RequireHttps]
    public class TournamentsController : Controller
    {
        private SacContext db = new SacContext();

        // GET: Tournaments
        [AllowAnonymous]
        public ActionResult Index()
        {
            var tournaments = db.Tournaments.Include("Schedules.Club").Where(t => t.Completed).OrderByDescending(t => t.Schedules.FirstOrDefault().FromDate);
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
            var tournament = db.Tournaments
                                .Include("Schedules.Club")
                                .Include("Competitors.Class.Group")
                                .Include("Competitors.Class.Color")
                                .Include("Competitors.Award")
                                .FirstOrDefault(t => t.Id == id);
            if (tournament == null)
            {
                return HttpNotFound();
            }

            TournamentResultViewModel result = GetResult(tournament);

            return View(result);
        }

        

        // GET: Tournaments/Admin
        [Authorize(Roles = "Club Admin,Tech Admin,Club User")]
        public ActionResult Admin()
        {
            return View(User.GetTournaments(db));
        }

        // GET: Tournaments/Create
        [Authorize(Roles = "Club Admin,Tech Admin")]
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
        [Authorize(Roles = "Club Admin,Tech Admin")]
        public ActionResult Create([Bind(Include = "Schedules")] Guid[] schedules)
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
        [Authorize(Roles = "Club Admin,Tech Admin,Club User")]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tournament = User.GetTournaments(db).Include("Competitors.Class.Group").FirstOrDefault(t => t.Id == id);
            if (tournament == null)
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
        [Authorize(Roles = "Club Admin,Tech Admin,Club User")]
        public ActionResult Edit([Bind(Include = "Id,Completed")] Tournament tournament)
        {
            if (ModelState.IsValid && User.GetTournaments(db).Any(t => t.Id == tournament.Id))
            {
                db.Entry(tournament).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Admin");
            }
            SetupLists();
            return View(tournament);
        }

        // GET: Tournaments/Delete/5
        [Authorize(Roles = "Club Admin,Tech Admin")]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tournament = User.GetTournaments(db)
                                .Include("Schedules.Club")
                                .Include("Competitors")
                                //.Include("Competitors.Class.Group")
                                //.Include("Competitors.Class.Color")
                                //.Include("Competitors.Award")
                                .FirstOrDefault(t => t.Id == id);
            if (tournament == null)
            {
                return HttpNotFound();
            }
            return View(tournament);
        }

        // POST: Tournaments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Club Admin,Tech Admin")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            var tournament = User.GetTournaments(db).First(t => t.Id == id);
            tournament.Schedules.Clear();
            db.Tournaments.Remove(tournament);
            db.SaveChanges();
            return RedirectToAction("Admin");
        }

        // GET: Tournaments/Complete/5
        [Authorize(Roles = "Club Admin,Tech Admin")]
        public ActionResult Complete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tournament = User.GetTournaments(db).Include("Competitors.Class.Group").Include("Competitors.Class.Color").Include("Competitors.Award").FirstOrDefault(t => t.Id == id);
            if (tournament == null)
            {
                return HttpNotFound();
            }

            TournamentResultViewModel result = GetResult(tournament);

            return View(result);
        }

        // POST: Tournaments/Complete/5
        [HttpPost, ActionName("Complete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Club Admin,Tech Admin")]
        public async Task<ActionResult> CompleteConfirmed(Guid id)
        {
            var tournament = User.GetTournaments(db).FirstOrDefault(t => t.Id == id);
            if (tournament == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tournament.Completed = true;
            db.SaveChanges();

            // Send out e-mail that tournament is complete
            await SendCompleteBlast(tournament, "SAC Tournament Results");

            // Send to results view when done
            return RedirectToAction("Results", new { id = tournament.Id });
        }
        
        // GET: Tournaments/Correction/5
        [Authorize(Roles = "Club Admin,Tech Admin")]
        public ActionResult Correction(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tournament = User.GetTournaments(db).Include("Competitors.Class.Group").Include("Competitors.Class.Color").Include("Competitors.Award").FirstOrDefault(t => t.Id == id);
            if (tournament == null)
            {
                return HttpNotFound();
            }

            TournamentResultViewModel result = GetResult(tournament);

            return View(result);
        }

        // POST: Tournaments/Correction/5
        [HttpPost, ActionName("Correction")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Club Admin,Tech Admin")]
        public async Task<ActionResult> CorrectioneConfirmed(Guid id)
        {
            var tournament = User.GetTournaments(db).FirstOrDefault(t => t.Id == id);
            if (tournament == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Send out e-mail that tournament is complete
            await SendCompleteBlast(tournament, "CORRECTION: SAC Tournament Results");

            // Send to results view when done
            return RedirectToAction("Results", new { id = tournament.Id });
        }

        private void SetupLists()
        {
            ViewBag.ScheduleList = new SelectList(User.GetSchedules(db).Where(s => null == s.Tournament).OrderBy(s => s.FromDate).ToArray(), "Id", "DisplayClubWithShortDate");
        }

        private async Task SendCompleteBlast(Tournament tournament, string subject)
        {
            var body = this.RenderPartialViewToEmailString("~/Views/Mailer/TournamentResults.cshtml", tournament);
            var email = new EmailService();
            await email.SendBlastAsync(subject, body);
        }

        private TournamentResultViewModel GetResult(Tournament tournament)
        {
            return new TournamentResultViewModel()
            {
                Tournament = tournament,
                Groups = tournament.Competitors.Select(c => c.Class.Group).Distinct().OrderBy(o => o.SortOrder).ToList(),
                Classes = tournament.Competitors.Select(c => c.Class).Distinct().OrderByDescending(c => c.MaximumYardage).ThenBy(c => c.Known.ToString()).ThenBy(c => c.Name).ToList(),
                Competitors = tournament.Competitors.OrderByDescending(c => c.Score).ThenByDescending(c => c.Bonus).GroupBy(c => c.ClassId).SelectMany(g => g.Select((c, i) => new RankedCompetitor() { Competitor = c, Rank = i + 1 })).ToList()
            };
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
