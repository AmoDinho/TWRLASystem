using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TRWLASystemMaster.Models;
using TRWLASystemMaster.Models.DB;

namespace TRWLASystemMaster.Controllers
{
    public class VenueTypesController : Controller
    {
        private TWRLADB_Staging_V2Entities19 db = new TWRLADB_Staging_V2Entities19();

        // GET: VenueTypes
        public ActionResult Index(string searchStringVT)
        {
            //return View(db.VenueTypes.ToList());

            var ventype = from vt in db.VenueTypes
                          select vt;
            if (!String.IsNullOrEmpty(searchStringVT))
            {
                ventype = ventype.Where(s => s.VenueType_Description.Contains(searchStringVT));

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
        public ActionResult Create([Bind(Include = "VenueTypeID,VenueType_Description")] VenueType venueType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int i = db.VenueTypes.Count();

                    if (i != 0)
                    {

                        int k = db.VenueTypes.Max(p => p.VenueTypeID);
                        int max = k + 1;


                        venueType.VenueTypeID = max;

                        db.VenueTypes.Add(venueType);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        db.VenueTypes.Add(venueType);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }

                return View(venueType);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Residences", "Create"));
            }
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
        public ActionResult Edit([Bind(Include = "VenueTypeID,VenueType_Description")] VenueType venueType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(venueType).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(venueType);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Residences", "Create"));
            }
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
            try
            {
                VenueType venueType = db.VenueTypes.Find(id);
                db.VenueTypes.Remove(venueType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Residences", "Create"));
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
