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
    [Authorize(Roles = "Tech Admin")]
    [RequireHttps]
    public class UsersController : Controller
    {
        private SacContext db = new SacContext();

        // GET: Users/Admin
        public ActionResult Admin()
        {
            return View(db.Users.Include("AspNetRoles").Include("Clubs").ToList());
        }

        // GET: Users/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null || !ModelState.IsValid)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AspNetUser aspNetUser = db.Users
                                        .Include("AspNetRoles")
                                        .Include("Clubs")
                                        .First(u => u.Id == id);

            if (aspNetUser == null)
            {
                return HttpNotFound();
            }

            ViewBag.ClubList = new MultiSelectList(db.Clubs, "Id", "ShortName");
            ViewBag.RoleList = new MultiSelectList(db.Roles, "Id", "Name");

            return View(aspNetUser);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "Id,UserName,Email,EmailConfirmed,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnabled,LockoutEndDateUtc,AccessFailedCount")] AspNetUser user,
            [Bind(Include = "AspNetRoles")] Guid[] aspNetRoles,
            [Bind(Include = "Clubs")] Guid[] clubs)
        {
            if (ModelState.IsValid)
            {
                var _user = db.Users
                                .Include("AspNetRoles")
                                .Include("Clubs")
                                .First(u => u.Id == user.Id);

                if (TryUpdateModel(_user, new string[] { "UserName", "Email", "EmailConfirmed", "PhoneNumber", "PhoneNumberConfirmed", "TwoFactorEnabled", "LockoutEnabled", "LockoutEndDateUtc", "AccessFailedCount" }))
                {
                    _user.AspNetRoles.Clear();
                    if (null != aspNetRoles)
                    {
                        foreach (Guid roleId in aspNetRoles)
                        {
                            AspNetRole role = db.Roles.Find(roleId);
                            _user.AspNetRoles.Add(role);
                        }
                    }

                    _user.Clubs.Clear();
                    if (null != clubs)
                    {
                        foreach (Guid clubId in clubs)
                        {
                            Club club = db.Clubs.Find(clubId);
                            _user.Clubs.Add(club);
                        }
                    }
                }
                
                db.SaveChanges();
                return RedirectToAction("Admin");
            }
            return HttpNotFound();
        }

        // GET: Users/Delete/5
        public ActionResult Delete(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.Users.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            AspNetUser aspNetUser = db.Users.Find(id);
            db.Users.Remove(aspNetUser);
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
