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
    public class VenuesController : Controller
    {
        private TWRLADB_Staging_V2Entities8 db = new TWRLADB_Staging_V2Entities8();

        // GET: Venues
        public ActionResult Index(string searchStringV)
        {
            //var venues = db.Venues.Include(v => v.Address).Include(v => v.VenueType);
            //return View(venues.ToList());

            var ven = from v in db.Venues
                          select v;

           if (!String.IsNullOrEmpty(searchStringV))
          {
             ven = ven.Where(s => s.Venue_Name.Contains(searchStringV));

          }



            return View(ven.ToList());





        }

        // GET: Venues/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Venue venue = db.Venues.Find(id);
            if (venue == null)
            {
                return HttpNotFound();
            }
            return View(venue);
        }

        // GET: Venues/Create
        public ActionResult Create()
        {
            ViewBag.VenueTypeID = new SelectList(db.VenueTypes, "VenueTypeID", "VenueType_Description");
            return View();
        }

        // POST: Venues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VenueID,Venue_Name,VenueTypeID,StreeNumber,StreetName,Suburb,City,Province,PostCode")] Venue venue)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int i = db.Venues.Count();

                    if (i != 0)
                    {

                        int k = db.Venues.Max(p => p.VenueID);
                        int max = k + 1;

                        AuditLog myAudit = new AuditLog();
                        myAudit.DateDone = DateTime.Now;
                        myAudit.TypeTran = "Create";
                        myAudit.SYSUserProfileID = (int)Session["User"];
                        myAudit.TableAff = "Venues";
                        db.AuditLogs.Add(myAudit);


                        db.Venues.Add(venue);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        AuditLog myAudit = new AuditLog();
                        myAudit.DateDone = DateTime.Now;
                        myAudit.TypeTran = "Create";
                        myAudit.SYSUserProfileID = (int)Session["User"];
                        myAudit.TableAff = "Venues";
                        db.AuditLogs.Add(myAudit);


                        db.Venues.Add(venue);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                
                ViewBag.VenueTypeID = new SelectList(db.VenueTypes, "VenueTypeID", "VenueType_Description", venue.VenueTypeID);
                return View(venue);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Residences", "Create"));
            }
        }

        // GET: Venues/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Venue venue = db.Venues.Find(id);
            if (venue == null)
            {
                return HttpNotFound();
            }
            ViewBag.VenueTypeID = new SelectList(db.VenueTypes, "VenueTypeID", "VenueType_Description", venue.VenueTypeID);
            return View(venue);
        }

        // POST: Venues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VenueID,Venue_Name,AddressID,VenueTypeID,StreeNumber, StreetName, Suburb, City, Province, PostCode")] Venue venue)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(venue).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.VenueTypeID = new SelectList(db.VenueTypes, "VenueTypeID", "VenueType_Description", venue.VenueTypeID);
                return View(venue);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Residences", "Create"));
            }

        }

        // GET: Venues/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Venue venue = db.Venues.Find(id);
            if (venue == null)
            {
                return HttpNotFound();
            }
            return View(venue);
        }

        // POST: Venues/Delete/5
        [HttpPost, ActionName("Details")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {

                Venue venue = db.Venues.Find(id);
                db.Venues.Remove(venue);
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
