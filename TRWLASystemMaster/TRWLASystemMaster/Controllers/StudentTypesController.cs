using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TRWLASystemMaster.Models.Usertype;

namespace TRWLASystemMaster.Controllers
{
    public class StudentTypesController : Controller
    {
        private Db_TRWLA_StagingEntities db = new Db_TRWLA_StagingEntities();

        // GET: StudentTypes
        public ActionResult Index()
        {
            return View(db.StudentTypes.ToList());
        }

        // GET: StudentTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentType studentType = db.StudentTypes.Find(id);
            if (studentType == null)
            {
                return HttpNotFound();
            }
            return View(studentType);
        }

        // GET: StudentTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StudentTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentTypeID,Description")] StudentType studentType)
        {
            if (ModelState.IsValid)
            {
                db.StudentTypes.Add(studentType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(studentType);
        }

        // GET: StudentTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentType studentType = db.StudentTypes.Find(id);
            if (studentType == null)
            {
                return HttpNotFound();
            }
            return View(studentType);
        }

        // POST: StudentTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentTypeID,Description")] StudentType studentType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(studentType);
        }

        // GET: StudentTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentType studentType = db.StudentTypes.Find(id);
            if (studentType == null)
            {
                return HttpNotFound();
            }
            return View(studentType);
        }

        // POST: StudentTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentType studentType = db.StudentTypes.Find(id);
            db.StudentTypes.Remove(studentType);
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
