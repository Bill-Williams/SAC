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

namespace SAC.Web.Controllers
{

    [RequireHttps]
    [Authorize(Roles = "Club Admin,Tech Admin,Club User")]
    public class CompetitorsController : Controller
    {
        private SacContext db = new SacContext();

        // GET: Competitors/Create
        public ActionResult Create(Guid? id)
        {
            if(ModelState.IsValid && id.HasValue)
            {
                var competitor = new Competitor()
                {
                    TournamentId = id.Value
                };
                SetupLists();
                return View(competitor);
            }

            return HttpNotFound();
        }

        // POST: Competitors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TournamentId,Archer,ClassId,Score,Bonus")] Competitor competitor)
        {
            if (ModelState.IsValid)
            {
                db.Competitors.Add(competitor);
                db.SaveChanges();
                return RedirectToAction("Edit", "Tournaments", new { id = competitor.TournamentId });
            }

            SetupLists();
            return View(competitor);
        }

        // GET: Competitors/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var competitor = db.Competitors.Include(c => c.Class).FirstOrDefault(c => c.Id == id);
            if (competitor == null)
            {
                return HttpNotFound();
            }
            SetupLists();
            return View(competitor);
        }

        // POST: Competitors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Archer,TournamentId,ClassId,Score,Bonus")] Competitor competitor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(competitor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", "Tournaments", new { id = competitor.TournamentId });
            }
            SetupLists();
            return View(competitor);
        }

        // GET: Competitors/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Competitor competitor = db.Competitors.Include(c => c.Class).FirstOrDefault(c => c.Id == id);
            if (competitor == null)
            {
                return HttpNotFound();
            }
            return View(competitor);
        }

        // POST: Competitors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Competitor competitor = db.Competitors.Find(id);
            db.Competitors.Remove(competitor);
            db.SaveChanges();
            return RedirectToAction("Edit", "Tournaments", new { id = competitor.TournamentId });
        }

        // GET: Competitors/Search/5
        public ActionResult Search(string id)
        {
            IQueryable<Competitor> results = db.Competitors;

            if (id != null)
            {
                results = results.Where(c => c.Archer.Contains(id));
            }

            return Json(
                results
                .OrderBy(c => c.Archer)
                .Select(c => new { Archer = c.Archer })
                .ToArray(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetClass(Guid id)
        {
            return PartialView("ClassSideBar", db.Classes.Include("Color").Include("Group").First(c => c.Id == id));
        }

        private void SetupLists()
        {
            var classes = new HashSet<SelectListItem>();
            var groups = db.Groups.ToDictionary(k => k.Name, v => new SelectListGroup() { Name = v.Name });

            classes.Add(new SelectListItem()
            {
                Disabled = true,
                Group = null,
                Selected = true,
                Text = null,
                Value = "0"
            });

            db.Classes.Include(c => c.Group).OrderBy(c => c.Group.SortOrder).ThenBy(c => c.Name).ToList()
                .ForEach(c => classes.Add(new SelectListItem()
                {
                    Group = groups[c.Group.Name],
                    Value = c.Id.ToString(),
                    Text = $"{c.Name} ({c.MaximumYardage})"
                }));

            ViewBag.ClassId = classes;
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
