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
    public class ComEngEventsController : Controller
    {
        private TWRLADB_Staging_V2Entities12 db = new TWRLADB_Staging_V2Entities12();

        // GET: ComEngEvents
        public ActionResult Index()
        {
            var comEngEvents = db.ComEngEvents.Include(c => c.Content).Include(c => c.Venue);
            return View(comEngEvents.ToList());
        }

        // GET: ComEngEvents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ComEngEvent comEngEvent = db.ComEngEvents.Find(id);
            if (comEngEvent == null)
            {
                return HttpNotFound();
            }
            return View(comEngEvent);
        }

        // GET: ComEngEvents/Create
        public ActionResult Create()
        {
            ViewBag.ContentID = new SelectList(db.Contents, "ContentID", "Content_Name");
            ViewBag.VenueID = new SelectList(db.Venues, "VenueID", "Venue_Name");
            return View();
        }

        // POST: ComEngEvents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ComEngID,ComEng_Name,ComEng_Summary,ComEng_Description,ComEng_Date,ComEnge_StartTime,ComEng_EndTime,ComEng_Theme,VenueID,ContentID")] ComEngEvent comEngEvent)
        {
            if (ModelState.IsValid)
            {
                int i = db.ComEngEvents.Count();

                if (i != 0)
                {
                    int max = db.ComEngEvents.Max(p => p.ComEngID);
                    int k = max + 1;
                    comEngEvent.ComEngID = k;

                    comEngEvent.ComEng_Name = comEngEvent.ComEng_Name + " (CE)";
                    db.ComEngEvents.Add(comEngEvent);

                    TRWLASchedule mySchedule = new TRWLASchedule();
                    mySchedule.ComEngID = comEngEvent.ComEngID;
                    db.TRWLASchedules.Add(mySchedule);
                    db.SaveChanges();
                    return RedirectToAction("Index", "TRWLASchedules");
                }
                else
                {
                    comEngEvent.ComEng_Name = comEngEvent.ComEng_Name + " (CE)";
                    db.ComEngEvents.Add(comEngEvent);
                    TRWLASchedule mySchedule = new TRWLASchedule();
                    mySchedule.ComEngID = comEngEvent.ComEngID;
                    db.TRWLASchedules.Add(mySchedule);
                    db.SaveChanges();
                    return RedirectToAction("Index", "TRWLASchedules");

                }
            }

            ViewBag.ContentID = new SelectList(db.Contents, "ContentID", "Content_Name", comEngEvent.ContentID);
            ViewBag.VenueID = new SelectList(db.Venues, "VenueID", "Venue_Name", comEngEvent.VenueID);
            return View(comEngEvent);
        }

        // GET: ComEngEvents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ComEngEvent comEngEvent = db.ComEngEvents.Find(id);
            if (comEngEvent == null)
            {
                return HttpNotFound();
            }
            ViewBag.ContentID = new SelectList(db.Contents, "ContentID", "Content_Name", comEngEvent.ContentID);
            ViewBag.VenueID = new SelectList(db.Venues, "VenueID", "Venue_Name", comEngEvent.VenueID);
            return View(comEngEvent);
        }

        // POST: ComEngEvents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ComEngID,ComEng_Name,ComEng_Summary,ComEng_Description,ComEng_Date,ComEnge_StartTime,ComEng_EndTime,ComEng_Theme,VenueID,ContentID")] ComEngEvent comEngEvent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comEngEvent).State = EntityState.Modified;
                db.SaveChanges();
                RedirectToAction("Index", "TRWLASchedules");
            }
            ViewBag.ContentID = new SelectList(db.Contents, "ContentID", "Content_Name", comEngEvent.ContentID);
            ViewBag.VenueID = new SelectList(db.Venues, "VenueID", "Venue_Name", comEngEvent.VenueID);
            return View(comEngEvent);
        }

        // GET: ComEngEvents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ComEngEvent comEngEvent = db.ComEngEvents.Find(id);
            if (comEngEvent == null)
            {
                return HttpNotFound();
            }
            return View(comEngEvent);
        }

        // POST: ComEngEvents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ComEngEvent comEngEvent = db.ComEngEvents.Find(id);
            db.ComEngEvents.Remove(comEngEvent);
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