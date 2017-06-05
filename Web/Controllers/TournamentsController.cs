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
    public class TournamentsController : Controller
    {
        private SacContext db = new SacContext();

        // GET: Tournaments
        public ActionResult Index()
        {
            var tournaments = db.Tournaments.Include(t => t.Schedule);
            return View(tournaments.ToList());
        }

        // GET: Tournaments/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournament tournament = db.Tournaments.Find(id);
            if (tournament == null)
            {
                return HttpNotFound();
            }
            return View(tournament);
        }

        // GET: Tournaments/Create
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(db.Schedules, "Id", "Id");
            return View();
        }

        // POST: Tournaments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Completed")] Tournament tournament)
        {
            if (ModelState.IsValid)
            {
                tournament.Id = Guid.NewGuid();
                db.Tournaments.Add(tournament);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id = new SelectList(db.Schedules, "Id", "Id", tournament.Id);
            return View(tournament);
        }

        // GET: Tournaments/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournament tournament = db.Tournaments.Find(id);
            if (tournament == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.Schedules, "Id", "Id", tournament.Id);
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
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.Schedules, "Id", "Id", tournament.Id);
            return View(tournament);
        }

        // GET: Tournaments/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournament tournament = db.Tournaments.Find(id);
            if (tournament == null)
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
            Tournament tournament = db.Tournaments.Find(id);
            db.Tournaments.Remove(tournament);
            db.SaveChanges();
            return RedirectToAction("Index");
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
