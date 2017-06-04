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

        public MultiSelectList GetRoles()
        {
            return new MultiSelectList((IEnumerable<AspNetRole>)db.Roles , "Id", "Name");
        }

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.Include("AspNetRoles").ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(Guid id)
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

        // GET: Users/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null || !ModelState.IsValid)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AspNetUser aspNetUser = db.Users.Include("AspNetRoles").First(u => u.Id == id);

            if (aspNetUser == null)
            {
                return HttpNotFound();
            }

            return View(aspNetUser);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult UserEditPost(AspNetUser user)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(user).State = EntityState.Modified;
                var _user = db.Users.Include("AspNetRoles").First(u => u.Id == user.Id);

                var deletedRoles = _user.AspNetRoles.Except(user.AspNetRoles);

                var addedRoles = user.AspNetRoles.Except(_user.AspNetRoles);

                deletedRoles.ToList().ForEach(r => _user.AspNetRoles.Remove(r));

                foreach(var role in addedRoles)
                {
                    if (db.Entry(role).State == EntityState.Detached)
                    {
                        var addedRole = db.Roles.Find(role.Id);
                        _user.AspNetRoles.Add(addedRole);
                    }
                }

                db.SaveChanges();

                return RedirectToAction("Index");
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
