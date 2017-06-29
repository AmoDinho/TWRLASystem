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
    public class AlumniController : Controller
    {
        private Db_TRWLA_StagingEntities db = new Db_TRWLA_StagingEntities();

        // GET: Alumni
        public ActionResult Index()
        {
            var alumni = db.Alumni.Include(a => a.Person);
            return View(alumni.ToList());
        }

        // GET: Alumni/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alumnus alumnus = db.Alumni.Find(id);
            if (alumnus == null)
            {
                return HttpNotFound();
            }
            return View(alumnus);
        }

        // GET: Alumni/Create
        public ActionResult Create()
        {
            ViewBag.PersonID = new SelectList(db.People, "PersonID", "FirstName");
            return View();
        }

        // POST: Alumni/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PersonID")] Alumnus alumnus)
        {
            if (ModelState.IsValid)
            {
                db.Alumni.Add(alumnus);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PersonID = new SelectList(db.People, "PersonID", "FirstName", alumnus.PersonID);
            return View(alumnus);
        }

        // GET: Alumni/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alumnus alumnus = db.Alumni.Find(id);
            if (alumnus == null)
            {
                return HttpNotFound();
            }
            ViewBag.PersonID = new SelectList(db.People, "PersonID", "FirstName", alumnus.PersonID);
            return View(alumnus);
        }

        // POST: Alumni/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PersonID")] Alumnus alumnus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(alumnus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PersonID = new SelectList(db.People, "PersonID", "FirstName", alumnus.PersonID);
            return View(alumnus);
        }

        // GET: Alumni/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alumnus alumnus = db.Alumni.Find(id);
            if (alumnus == null)
            {
                return HttpNotFound();
            }
            return View(alumnus);
        }

        // POST: Alumni/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Alumnus alumnus = db.Alumni.Find(id);
            db.Alumni.Remove(alumnus);
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
