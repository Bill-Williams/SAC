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
using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace SAC.Web.Controllers
{
    [Authorize(Roles = "Tech Admin")]
    [RequireHttps]
    public class AwardsController : Controller
    {
        private SacContext db = new SacContext();

        // GET: Awards
        public ActionResult Admin()
        {
            return View(db.Awards.ToList());
        }

        // GET: Awards/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Awards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Icon")] Award award)
        {
            var image = Request.Files["image"];

            if (null == image || image.ContentLength == 0)
            {
                ModelState.AddModelError("Icon", "The Icon field is reaquired");
            }
            else if (ModelState.IsValid)
            {
                db.Awards.Add(award);
                //needed to get the id for the blob
                db.Database.BeginTransaction();
                try
                {
                    db.SaveChanges();

                    CloudBlockBlob blob = GetAwardBlob(award);

                    blob.Properties.ContentType = image.ContentType;

                    blob.UploadFromStream(image.InputStream);

                    award.Icon = blob.Uri.ToString();

                    db.SaveChanges();

                    db.Database.CurrentTransaction.Commit();
                }
                catch
                {
                    db.Database.CurrentTransaction.Rollback();

                    ModelState.AddModelError("Icon", "Error uploading file");

                    return View(award);
                }

                return RedirectToAction("Admin");
            }

            return View(award);
        }

        // GET: Awards/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Award award = db.Awards.Find(id);
            if (award == null)
            {
                return HttpNotFound();
            }
            return View(award);
        }

        // POST: Awards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Icon")] Award award)
        {
            if (ModelState.IsValid)
            {
                var image = Request.Files["image"];

                if (null != image || image.ContentLength != 0)
                {
                    CloudBlockBlob blob = GetAwardBlob(award);

                    blob.Properties.ContentType = image.ContentType;

                    blob.UploadFromStream(image.InputStream);

                    award.Icon = blob.Uri.ToString();
                }

                db.Entry(award).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Admin");
            }
            return View(award);
        }

        // GET: Awards/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Award award = db.Awards.Find(id);
            if (award == null)
            {
                return HttpNotFound();
            }
            return View(award);
        }

        // POST: Awards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Award award = db.Awards.Find(id);
            CloudBlockBlob blob = GetAwardBlob(award);
            blob.DeleteIfExistsAsync();
            db.Awards.Remove(award);
            db.SaveChanges();
            return RedirectToAction("Admin");
        }

        private CloudBlockBlob GetAwardBlob(Award award)
        {
            string storageConn = ConfigurationManager.ConnectionStrings["AzureStorageConn"].ConnectionString;

            var storageAccount = CloudStorageAccount.Parse(storageConn);

            var blobStorage = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = blobStorage.GetContainerReference("awardimages");

            if (container.CreateIfNotExists())
            {
                var permissions = container.GetPermissions();
                permissions.PublicAccess = BlobContainerPublicAccessType.Container;
                container.SetPermissions(permissions);
            }

            return container.GetBlockBlobReference(award.Id.ToString());
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
