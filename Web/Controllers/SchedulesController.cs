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
    [Authorize(Roles = "Tech Admin,Scheduler")]
    [RequireHttps]
    public class SchedulesController : Controller
    {
        private SacContext db = new SacContext();

        // GET: Schedules
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(db.Schedules.Include(s => s.Club));
        }

        // GET: Schedules/Admin
        public ActionResult Admin()
        {
            return View(db.Schedules.Include(s => s.Club));
        }

        // GET: Schedules/Create
        public ActionResult Create()
        {
            this.SetupLists();
            return View();
        }

        // POST: Schedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ClubId,Date")] Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                db.Schedules.Add(schedule);
                db.SaveChanges();
                return RedirectToAction("Admin");
            }
            this.SetupLists();
            return View(schedule);
        }

        // GET: Schedules/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            this.SetupLists();
            return View(schedule);
        }

        // POST: Schedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ClubId,Date")] Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(schedule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Admin");
            }
            this.SetupLists();
            return View(schedule);
        }

        // GET: Schedules/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var schedule = db.Schedules.Include(s => s.Club).FirstOrDefault(s => s.Id == id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        // POST: Schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            var schedule = db.Schedules.Include(s => s.Club).FirstOrDefault(s => s.Id == id);
            db.Schedules.Remove(schedule);
            db.SaveChanges();
            return RedirectToAction("Admin");
        }

        private void SetupLists()
        {
            ViewBag.ClubList = new SelectList(db.Clubs.OrderBy(c => c.Name), "Id", "Name");
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
