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
using System.Threading.Tasks;
using SAC.Web.Models;

namespace SAC.Web.Controllers
{

    [RequireHttps]
    [Authorize(Roles = "Club Admin,Tech Admin,Club User")]
    public class CompetitorsController : Controller
    {
        private SacContext db = new SacContext();

        // GET: Competitors/Award/5
        public ActionResult Award(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Competitor competitor = db.Competitors.Include("Tournament").FirstOrDefault(c => c.Id == id);
            if (competitor == null)
            {
                return HttpNotFound();
            }
            SetupAwardsList();
            return View(competitor);
        }

        // POST: Competitors/Award/5
        [HttpPost, ActionName("Award")]
        [ValidateAntiForgeryToken]
        public ActionResult AwardConfirmed(Guid id, Guid? AwardId)
        {
            Competitor competitor = db.Competitors.Include("Tournament").FirstOrDefault(c => c.Id == id);
            competitor.AwardId = AwardId;
            db.SaveChanges();
            if(competitor.Tournament.Completed)
            {
                return RedirectToAction("Correction", "Tournaments", new { id = competitor.TournamentId });
            }
            else
            {
                return RedirectToAction("Complete", "Tournaments", new { id = competitor.TournamentId });
            }
        }

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
                SetupExistingCompetitorList(id.Value);

                return View(competitor);
            }

            return HttpNotFound();
        }

        // POST: Competitors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Competitor competitor)
        {
            if (ModelState.IsValid)
            {
                competitor.CreatedDate = DateTime.Now;
                db.Competitors.Add(competitor);
                db.SaveChanges();
                return RedirectToAction("Create", "Competitors", new { id = competitor.TournamentId });
            }

            SetupLists();
            return View(competitor);
        }

        // GET: Competitors/Edit/5
        public ActionResult Edit([Bind(Include = "Id,Description,CreatedDate,LastModifiedDate")] Guid? id)
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
        public ActionResult Edit([Bind(Include = "Id,Archer,TournamentId,ClassId,Score,Bonus,CreatedDate")] Competitor competitor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(competitor).State = EntityState.Modified;
                db.Entry(competitor).Property("CreatedDate").IsModified = false;
                db.Entry(competitor).Property("TournamentId").IsModified = false;
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
                .Distinct()
                .ToArray(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetClass(Guid id)
        {
            return PartialView("ClassSideBar", db.Classes.Include("Color").Include("Group").First(c => c.Id == id));
        }

        private void SetupExistingCompetitorList(Guid id)
        {
            var competitors = db.Competitors
                .Include(c => c.Tournament)
                .Where(c => c.TournamentId == id)
                .Select(c => new {c.Archer, c.CreatedDate})
                .OrderBy(c => c.CreatedDate)
                .ToList();

            var list = new List<CompetitorListItemViewModel>();
            for (int i = 0; i < competitors.Count; i++)
            {
                list.Add(new CompetitorListItemViewModel() { Archer = competitors[i].Archer, EntryOrder = i+1 });
            }

            ViewBag.CompetitorList = list;
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

            db.Classes.Include(c => c.Group).OrderBy(c => c.Group.SortOrder).ThenByDescending(c => c.MaximumYardage).ThenBy(c => c.Name).ToList()
                .ForEach(c => classes.Add(new SelectListItem()
                {
                    Group = groups[c.Group.Name],
                    Value = c.Id.ToString(),
                    Text = $"{c.Name} ({c.MaximumYardage})"
                }));

            ViewBag.ClassId = classes;
        }

        private void SetupAwardsList()
        {
            var awards = new HashSet<SelectListItem>();

            awards.Add(new SelectListItem()
            {
                Text = "None",
                Value = null
            });

            db.Awards.OrderBy(a => a.Name).ToList()
                .ForEach(a => awards.Add(new SelectListItem()
                {
                    Value = a.Id.ToString(),
                    Text = a.Name
                }));

            ViewBag.AwardList = awards;
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
