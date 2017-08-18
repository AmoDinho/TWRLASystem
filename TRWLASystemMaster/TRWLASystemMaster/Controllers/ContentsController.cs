using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TRWLASystemMaster.Models;

namespace TRWLASystemMaster.Controllers
{
    public class ContentsController : Controller
    {
        private TWRLADB_Staging_V2Entities5 db = new TWRLADB_Staging_V2Entities5();

        // GET: Contents1
        public ActionResult Index(string sortOrder, string searchString, string locked, string unlocked, string all)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.SurnameSortParm = String.IsNullOrEmpty(sortOrder) ? "sur_desc" : "Surname";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";


            var content = from s in db.Contents
                          select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                content = content.Where(s => s.Content_Name.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(locked))
            {

                content = content.Where(s => s.Content_Status.Equals(1));
            }

            if (!String.IsNullOrEmpty(unlocked))
            {

                content = content.Where(s => s.Content_Status.Equals(0));
            }

            if (!String.IsNullOrEmpty(all))
            {

                content = content.Where(s => s.Content_Status.Equals(0) || s.Content_Status.Equals(1));
            }


            switch (sortOrder)
            {
                case "name_desc":
                    content = content.OrderByDescending(s => s.Content_Name);
                    break;
                case "sur_desc":
                    content = content.OrderByDescending(s => s.Content_Name);
                    break;
                case "Surname":
                    content = content.OrderByDescending(s => s.Content_Name);
                    break;
                default:
                    content = content.OrderBy(s => s.Content_Name);
                    break;
            }

            return View(content.ToList());
        }

        // GET: Contents1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Content content = db.Contents.Find(id);
            if (content == null)
            {
                return HttpNotFound();
            }
            return View(content);
        }

        // GET: Contents1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Contents1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ContentID,Content_Name,Content_Link,Content_Status,Content_Description")] Content content)
        {
            if (ModelState.IsValid)
            {
                int i = db.Contents.Count();

                if (i != 0)
                {
                    int k = db.Contents.Max(p => p.ContentID);
                    int max = k + 1;

                    content.Content_Status = 1;

                    content.ContentID = max;
                    db.Contents.Add(content);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    db.Contents.Add(content);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(content);
        }

        // GET: Contents1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Content content = db.Contents.Find(id);
            if (content == null)
            {
                return HttpNotFound();
            }
            return View(content);
        }

        // POST: Contents1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ContentID,Content_Name,Content_Link,Content_Status,Content_Description")] Content content)
        {
            if (ModelState.IsValid)
            {
                db.Entry(content).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(content);
        }

        // GET: Contents1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Content content = db.Contents.Find(id);
            if (content == null)
            {
                return HttpNotFound();
            }
            return View(content);
        }

        // POST: Contents1/Delete/5
        [HttpPost, ActionName("Details")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Content content = db.Contents.Find(id);

            try
            {
                db.Contents.Remove(content);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["notice"] = " Please note: This content is assigned to an event and cannot be deleted.";
                return View(content);
            }

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