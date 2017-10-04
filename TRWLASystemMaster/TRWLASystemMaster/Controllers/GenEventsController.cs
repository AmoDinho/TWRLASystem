using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TRWLASystemMaster.Models.DB;

namespace TRWLASystemMaster.Controllers
{
    public class GenEventsController : Controller
    {
        private TWRLADB_Staging_V2Entities7 db = new TWRLADB_Staging_V2Entities7();

        // GET: GenEvents
        public ActionResult Index()
        {
            var genEvents = db.GenEvents.Include(g => g.Content).Include(g => g.GuestSpeaker).Include(g => g.Residence).Include(g => g.Venue);
            return View(genEvents.ToList());
        }

        // GET: GenEvents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GenEvent genEvent = db.GenEvents.Find(id);
            if (genEvent == null)
            {
                return HttpNotFound();
            }
            return View(genEvent);
        }

        // GET: GenEvents/Create
        public ActionResult Create()
        {
            ViewBag.ContentID = new SelectList(db.Contents, "ContentID", "Content_Name");
            ViewBag.GuestSpeakerID = new SelectList(db.GuestSpeakers, "GuestSpeakerID", "GuestSpeaker_Name");
            ViewBag.ResID = new SelectList(db.Residences, "ResID", "Res_Name");
            ViewBag.VenueID = new SelectList(db.Venues, "VenueID", "Venue_Name");
            return View();
        }


        [HttpPost]
        public ActionResult GU()
        {
            TempData["Gu"] = 1;
            int gu = (int)TempData["Gu"];
            return Json(gu);
        }

        // POST: GenEvents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GenID,Gen_Name,Gen_Summary,Gen_Description,Gen_Date,Gen_StartTime,Gen_EndTime,Gene_Theme,VenueID,ResID,ContentID,GuestSpeakerID,Type")] GenEvent genEvent)
        {
            if (ModelState.IsValid)
            {
                AuditLog myAudit = new AuditLog();

                int i = db.GenEvents.Count();

                if (i != 0)
                {
                    int max = db.GenEvents.Max(p => p.GenID);
                    int k = max + 1;

                    genEvent.GenID = k;

                    myAudit.DateDone = DateTime.Now;
                    myAudit.TypeTran = "Create";
                    myAudit.SYSUserProfileID = (int)Session["User"];
                    myAudit.TableAff = "GenEvent";
                    db.AuditLogs.Add(myAudit);

                    genEvent.Type = 4;

                    int v = (int)TempData["Venue"];
                    int c = (int)TempData["Con"];
                    int g = (int)TempData["Gu"];
                    int r = (int)TempData["Res"];

                    if (v == 1)
                    {
                        genEvent.VenueID = null;
                    }
                    if (c == 1)
                    {
                        genEvent.ContentID = null;
                    }
                    if (g == 1)
                    {
                        genEvent.GuestSpeakerID = null;
                    }
                    if (r == 1)
                    {
                        genEvent.ResID = null;
                    }


                    db.GenEvents.Add(genEvent);

                    TRWLASchedule mySchedule = new TRWLASchedule();
                    mySchedule.GenID = genEvent.GenID;
                    db.TRWLASchedules.Add(mySchedule);
                    db.SaveChanges();
                    return RedirectToAction("Index", "TRWLASchedules");
                }
                else
                {
                    myAudit.DateDone = DateTime.Now;
                    myAudit.TypeTran = "Create";
                    myAudit.SYSUserProfileID = (int)Session["User"];
                    myAudit.TableAff = "GenEvent";
                    db.AuditLogs.Add(myAudit);

                    genEvent.Type = 4;

                    int v = (int)TempData["Venue"];
                    int c = (int)TempData["Con"];
                    int g = (int)TempData["Gu"];
                    int r = (int)TempData["Res"];

                    if (v == 1)
                    {
                        genEvent.VenueID = null;
                    }
                    if (c == 1)
                    {
                        genEvent.ContentID = null;
                    }
                    if (g == 1)
                    {
                        genEvent.GuestSpeakerID = null;
                    }
                    if (r == 1)
                    {
                        genEvent.ResID = null;
                    }

                    db.GenEvents.Add(genEvent);

                    TRWLASchedule mySchedule = new TRWLASchedule();
                    mySchedule.GenID = genEvent.GenID;
                    db.TRWLASchedules.Add(mySchedule);
                    db.SaveChanges();
                    return RedirectToAction("Index", "TRWLASchedules");


                }

                db.GenEvents.Add(genEvent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ContentID = new SelectList(db.Contents, "ContentID", "Content_Name", genEvent.ContentID);
            ViewBag.GuestSpeakerID = new SelectList(db.GuestSpeakers, "GuestSpeakerID", "GuestSpeaker_Name", genEvent.GuestSpeakerID);
            ViewBag.ResID = new SelectList(db.Residences, "ResID", "Res_Name", genEvent.ResID);
            ViewBag.VenueID = new SelectList(db.Venues, "VenueID", "Venue_Name", genEvent.VenueID);
            return View(genEvent);
        }

        // GET: GenEvents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GenEvent genEvent = db.GenEvents.Find(id);
            if (genEvent == null)
            {
                return HttpNotFound();
            }
            ViewBag.ContentID = new SelectList(db.Contents, "ContentID", "Content_Name", genEvent.ContentID);
            ViewBag.GuestSpeakerID = new SelectList(db.GuestSpeakers, "GuestSpeakerID", "GuestSpeaker_Name", genEvent.GuestSpeakerID);
            ViewBag.ResID = new SelectList(db.Residences, "ResID", "Res_Name", genEvent.ResID);
            ViewBag.VenueID = new SelectList(db.Venues, "VenueID", "Venue_Name", genEvent.VenueID);
            return View(genEvent);
        }

        // POST: GenEvents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GenID,Gen_Name,Gen_Summary,Gen_Description,Gen_Date,Gen_StartTime,Gen_EndTime,Gene_Theme,VenueID,ResID,ContentID,GuestSpeakerID,Type")] GenEvent genEvent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(genEvent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ContentID = new SelectList(db.Contents, "ContentID", "Content_Name", genEvent.ContentID);
            ViewBag.GuestSpeakerID = new SelectList(db.GuestSpeakers, "GuestSpeakerID", "GuestSpeaker_Name", genEvent.GuestSpeakerID);
            ViewBag.ResID = new SelectList(db.Residences, "ResID", "Res_Name", genEvent.ResID);
            ViewBag.VenueID = new SelectList(db.Venues, "VenueID", "Venue_Name", genEvent.VenueID);
            return View(genEvent);
        }

        // GET: GenEvents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GenEvent genEvent = db.GenEvents.Find(id);
            if (genEvent == null)
            {
                return HttpNotFound();
            }
            return View(genEvent);
        }

        // POST: GenEvents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GenEvent genEvent = db.GenEvents.Find(id);
            db.GenEvents.Remove(genEvent);
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
