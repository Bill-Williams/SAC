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

namespace SAC.Web.Controllers
{
    [RequireHttps]
    public class ClubsController : Controller
    {
        private SacContext db = new SacContext();

        // GET: Club
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(db.Clubs.OrderBy(c => c.Name).Include(c => c.Contacts).ToList());
        }

        // GET: Club/Directions/5
        [AllowAnonymous]
        public ActionResult Directions(Guid? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Club club = db.Clubs.Find(id);
            if (club == null)
            {
                return HttpNotFound();
            }
            return View(club);
        }

        // GET: Admin
        [Authorize(Roles = "Club Admin,Tech Admin")]
        public ActionResult Admin()
        {
            return View(User.GetClubs(db));
        }
    
        // GET: Club/Create
        [Authorize(Roles = "Tech Admin")]
        public ActionResult Create()
        {
            return View(new Club());
        }

        // POST: Club/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Tech Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,ShortName,Address,CityStateZip,Contact,Phone,Email,Website,Directions,GeoLocation,IconFileName")] Club club)
        {
            if (ModelState.IsValid)
            {
                db.Clubs.Add(club);
                db.SaveChanges();
                return RedirectToAction("Admin");
            }
            return View(club);
        }

        // GET: Club/Edit/5
        [Authorize(Roles = "Club Admin,Tech Admin")]
        public ActionResult Edit(Guid? id)
        {
            if (id == null || !id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Club club = User.GetClubs(db).Include("Contacts").First(c => c.Id == id);
            if (club == null)
            {
                return HttpNotFound();
            }
            return View(club);
        }

        // POST: Club/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Club Admin,Tech Admin")]
        public ActionResult Edit([Bind(Include = "Id,Name,ShortName,Address,CityStateZip,Website,Directions,GeoLocation,IconFileName")] Club club)
        {
            if (ModelState.IsValid 
                && User.GetClubs(db).Any(c => c.Id == club.Id))
            {
                db.Entry(club).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Admin");
            }
            return View(club);
        }

        // GET: Club/Delete/5
        [Authorize(Roles = "Tech Admin")]
        public ActionResult Delete(Guid? id)
        {
            if (id == null || !id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Club club = db.Clubs.Find(id);
            if (club == null)
            {
                return HttpNotFound();
            }
            return View(club);
        }

        // POST: Club/Delete/5
        [Authorize(Roles = "Tech Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Club club = db.Clubs.Find(id);
            db.Clubs.Remove(club);
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
