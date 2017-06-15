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
    [Authorize(Roles = "Tech Admin,Club Admin,Club User")]
    [RequireHttps]
    public class ArchersController : Controller
    {
        private SacContext db = new SacContext();

        // GET: Archers
        public ActionResult Admin()
        {
            return View(db.Archers.ToList());
        }

        // GET: Archers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Archers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Archer archer)
        {
            if (ModelState.IsValid)
            {
                db.Archers.Add(archer);
                db.SaveChanges();
                return RedirectToAction("Admin");
            }

            return View(archer);
        }

        // GET: Archers/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Archer archer = db.Archers.Find(id);
            if (archer == null)
            {
                return HttpNotFound();
            }
            return View(archer);
        }

        // POST: Archers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Archer archer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(archer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Admin");
            }
            return View(archer);
        }

        // GET: Archers/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Archer archer = db.Archers.Find(id);
            if (archer == null)
            {
                return HttpNotFound();
            }
            return View(archer);
        }

        // POST: Archers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Archer archer = db.Archers.Find(id);
            db.Archers.Remove(archer);
            db.SaveChanges();
            return RedirectToAction("Admin");
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
