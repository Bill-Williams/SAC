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
    public class AdminController : Controller
    {
        private SacContext db = new SacContext();

        public MultiSelectList GetRoles()
        {
            return new MultiSelectList((IEnumerable<AspNetRole>)db.Roles , "Id", "Name");
        }

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

#region Users

        // GET: Admin/Users
        public ActionResult Users()
        {
            return View(db.Users.ToList());
        }

        // GET: Admin/UserDetails/5
        public ActionResult UserDetails(string id)
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

        // GET: Admin/UserEdit/5
        public ActionResult UserEdit(string id)
        {
            if (id == null)
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

        // POST: Admin/UserEdit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("UserEdit")]
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

                return RedirectToAction("Users");
            }
            return HttpNotFound();
        }

        // GET: Admin/UserDelete/5
        public ActionResult UserDelete(string id)
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

        // POST: Admin/UserDelete/5
        [HttpPost, ActionName("UserDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult UserDeleteConfirmed(string id)
        {
            AspNetUser aspNetUser = db.Users.Find(id);
            db.Users.Remove(aspNetUser);
            db.SaveChanges();
            return RedirectToAction("Users");
        }

        #endregion

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
