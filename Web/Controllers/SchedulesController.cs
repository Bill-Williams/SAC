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
using System.Threading.Tasks;
using SAC.Web.Extensions;
using SAC.Web.Services;

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
            var model = from s in db.Schedules.Include(s => s.Club)
                        where s.FromDate >= DateTime.Today
                        orderby s.FromDate
                        select s;
                        
            return View(model.Take(8));
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
        public ActionResult Create([Bind(Include = "Id,ClubId,FromDate, ToDate")] Schedule schedule)
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

        [AllowAnonymous]
        public async Task<ActionResult> WeeklyMailer(string id)
        {
            if(Environment.GetEnvironmentVariable("siteApiKey") == id)
            {
                var toDate = DateTime.Today.AddDays(7);
                var schedules = from s in db.Schedules.Include("Club")
                                where s.FromDate >= DateTime.Today
                                    && s.FromDate < toDate
                                orderby s.FromDate, s.Club.Name
                                select s;

                if(schedules.Any())
                {
                    var body = this.RenderPartialViewToEmailString("~/Views/Mailer/WeeklyMailer.cshtml", schedules);
                    var email = new EmailService();
                    await email.SendBlastAsync("Events - Southern Archery Circuit", body);
                }
            }

            // Always return ok even if code doesn't match
            return new HttpStatusCodeResult(HttpStatusCode.OK);
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
