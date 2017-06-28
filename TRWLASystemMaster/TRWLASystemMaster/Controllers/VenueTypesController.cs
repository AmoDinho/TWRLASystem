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
    public class VenueTypesController : Controller
    {
        private Db_TRWLA_StagingEntities db = new Db_TRWLA_StagingEntities();

        // GET: VenueTypes
        public ActionResult Index(string searchStringVT)
        {
            /*
       * Linq Query
       * 
       * Search String is for DESCRRIPTION!!
       * 
       * */
            var ventype = from vt in db.VenueTypes
                      select vt;
            if (!String.IsNullOrEmpty(searchStringVT))
            {
                ventype = ventype.Where(s => s.Description.Contains(searchStringVT));

            }



            return View(ventype.ToList());
        }

        // GET: VenueTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VenueType venueType = db.VenueTypes.Find(id);
            if (venueType == null)
            {
                return HttpNotFound();
            }
            return View(venueType);
        }

        // GET: VenueTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VenueTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VenueTypeID,Description")] VenueType venueType)
        {
            if (ModelState.IsValid)
            {
                db.VenueTypes.Add(venueType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(venueType);
        }

        // GET: VenueTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VenueType venueType = db.VenueTypes.Find(id);
            if (venueType == null)
            {
                return HttpNotFound();
            }
            return View(venueType);
        }

        // POST: VenueTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VenueTypeID,Description")] VenueType venueType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(venueType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(venueType);
        }

        // GET: VenueTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VenueType venueType = db.VenueTypes.Find(id);
            if (venueType == null)
            {
                return HttpNotFound();
            }
            return View(venueType);
        }

        // POST: VenueTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VenueType venueType = db.VenueTypes.Find(id);
            db.VenueTypes.Remove(venueType);
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
