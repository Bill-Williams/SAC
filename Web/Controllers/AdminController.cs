﻿using System;
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
        private SacContext db = SacContext.GetCurrentContext();

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
            AspNetUser aspNetUser = db.Users.Find(id);
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
        public ActionResult UserEditPost(string id)
        {
            if (ModelState.IsValid)
            {
                var sourceUser = db.Users.Find(id);

                if (TryUpdateModel<AspNetUser>(sourceUser))
                {
                    //foreach(var role in sourceUser.AspNetRoles)
                    //{
                    //    db.Set<AspNetRole>().Attach(role);
                    //    db.Entry<AspNetRole>(role).State = EntityState.Modified;
                    //}
                    
                    db.SaveChanges();
                }
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
                //db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
