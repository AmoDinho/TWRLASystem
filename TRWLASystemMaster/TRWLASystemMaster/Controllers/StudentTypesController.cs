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
    public class StudentTypesController : Controller
    {
        private TWRLADB_Staging_V2Entities12 db = new TWRLADB_Staging_V2Entities12();

        // GET: StudentTypes
        public ActionResult Index(string searchStringST)
        {
            //return View(db.StudentTypes.ToList());

            var stutype = from st in db.StudentTypes
                          select st;
            if (!String.IsNullOrEmpty(searchStringST))
            {
                stutype = stutype.Where(s => s.StudentTypeDescription.Contains(searchStringST));

            }



            return View(stutype.ToList());
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
        public ActionResult Create([Bind(Include = "StudentTypeID,StudentTypeDescription")] StudentType studentType)
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
        public ActionResult Edit([Bind(Include = "StudentTypeID,StudentTypeDescription")] StudentType studentType)
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
