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
using SAC.Web.Models;

namespace SAC.Web.Controllers
{
    [Authorize(Roles = "Tech Admin")]
    [RequireHttps]
    public class ClassesController : Controller
    {
        private SacContext db = new SacContext();

        // GET: Classes
        [AllowAnonymous]
        public ActionResult Index()
        {
            var model = new ClassViewModel()
            {
                Classes = db.Classes.Include("Color").Include("Group").ToList(),
                Groups = db.Groups.OrderBy(g => g.SortOrder).ToList()
            };
            return View(model);
        }

        // GET: Classes/Admin
        public ActionResult Admin()
        {
            return View(db.Classes.Include("Color").Include("Group"));
        }
        
        // GET: Classes/Create
        public ActionResult Create()
        {
            this.SetupLists();
            return View();
        }

        // POST: Classes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "Code,Name,Description,Known,MaximumYardage,Restrictions,GroupId,ColorId")] Class @class)
        {
            if (ModelState.IsValid)
            {
                db.Classes.Add(@class);
                db.SaveChanges();
                return RedirectToAction("Admin");
            }
            this.SetupLists();
            return View(@class);
        }

        // GET: Classes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Class @class = db.Classes.Include(c => c.Color).FirstOrDefault(c => c.Id == id);
            if (@class == null)
            {
                return HttpNotFound();
            }
            this.SetupLists();
            return View(@class);
        }

        // POST: Classes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Code,Name,Description,Known,MaximumYardage,Restrictions,GroupId,ColorId")] Class @class)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@class).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Admin");
            }
            this.SetupLists();
            return View(@class);
        }

        // GET: Classes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Class @class = db.Classes.Include(c => c.Color).FirstOrDefault(c => c.Id == id);
            if (@class == null)
            {
                return HttpNotFound();
            }
            return View(@class);
        }

        // POST: Classes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Class @class = db.Classes.Find(id);
            db.Classes.Remove(@class);
            db.SaveChanges();
            return RedirectToAction("Admin");
        }

        private void SetupLists()
        {
            ViewBag.GroupList = new SelectList(db.Groups.OrderBy(g => g.Name), "Id", "Name");
            ViewBag.ColorList = new SelectList(db.Colors.OrderBy(c => c.Name), "Id", "Name");
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
