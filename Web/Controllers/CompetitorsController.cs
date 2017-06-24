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

namespace SAC.Web.Controllers
{

    [RequireHttps]
    [Authorize(Roles = "Club Admin,Tech Admin")]
    public class CompetitorsController : Controller
    {
        private SacContext db = new SacContext();

        // GET: Competitors/Create
        public ActionResult Create(Guid? id)
        {
            if(ModelState.IsValid && id.HasValue)
            {
                var competitor = new Competitor()
                {
                    TournamentId = id.Value
                };
                SetupLists();
                return View(competitor);
            }

            return HttpNotFound();
        }

        // POST: Competitors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TournamentId,Archer,ClassId,Score,Bonus")] Competitor competitor)
        {
            if (ModelState.IsValid)
            {
                db.Competitors.Add(competitor);
                db.SaveChanges();
                return RedirectToAction("Edit", "Tournaments", new { id = competitor.TournamentId });
            }

            SetupLists();
            return View(competitor);
        }

        // GET: Competitors/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var competitor = db.Competitors.Include(c => c.Class).FirstOrDefault(c => c.Id == id);
            if (competitor == null)
            {
                return HttpNotFound();
            }
            SetupLists();
            return View(competitor);
        }

        // POST: Competitors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Archer,TournamentId,ClassId,Score,Bonus")] Competitor competitor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(competitor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", "Tournaments", new { id = competitor.TournamentId });
            }
            SetupLists();
            return View(competitor);
        }

        // GET: Competitors/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Competitor competitor = db.Competitors.Include(c => c.Class).FirstOrDefault(c => c.Id == id);
            if (competitor == null)
            {
                return HttpNotFound();
            }
            return View(competitor);
        }

        // POST: Competitors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Competitor competitor = db.Competitors.Find(id);
            db.Competitors.Remove(competitor);
            db.SaveChanges();
            return RedirectToAction("Edit", "Tournaments", new { id = competitor.TournamentId });
        }

        private void SetupLists()
        {
            ViewBag.ClassId = new SelectList(db.Classes.Include(c => c.Group).OrderBy(c => c.Group.SortOrder).ThenBy(c => c.Name), "Id", "Name", "GroupName", new object());
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
