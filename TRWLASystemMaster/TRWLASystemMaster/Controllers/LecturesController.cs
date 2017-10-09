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
    public class LecturesController : Controller
    {
        private TWRLADB_Staging_V2Entities8 db = new TWRLADB_Staging_V2Entities8();

        // GET: Lectures
        public ActionResult Index()
        {
            try
            {
                var lectures = db.Lectures.Include(l => l.Content).Include(l => l.Residence).Include(l => l.Venue);
                return View(lectures.ToList());
            }
            catch
            {
                return RedirectToAction("ErrorPage", "TRWLASchedules");
            }
        }

        // GET: Lectures/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Lecture lecture = db.Lectures.Find(id);
                if (lecture == null)
                {
                    return HttpNotFound();
                }
                return View(lecture);
            }
            catch
            {
                return RedirectToAction("ErrorPage", "TRWLASchedules");
            }
        }

        // GET: Lectures/Create
        public ActionResult Create()
        {
            try
            {
                ViewBag.ContentID = new SelectList(db.Contents, "ContentID", "Content_Name");
                ViewBag.ResidenceID = new SelectList(db.Residences, "ResID", "Res_Name");
                ViewBag.VenueID = new SelectList(db.Venues, "VenueID", "Venue_Name");
                return View();
            }
            catch
            {
                return RedirectToAction("ErrorPage", "TRWLASchedules");
            }
        }

        // POST: Lectures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LectureID,Lecture_Name,Lecture_Summary,Lecture_Description,Lecture_Date,Lecture_StartTime,Lecture_EndTime,Lecture_Theme,VenueID,ResidenceID,ContentID")] Lecture lecture)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int i = db.Lectures.Count();

                    if (i != 0)
                    {

                        AuditLog myAudit = new AuditLog();
                        myAudit.DateDone = DateTime.Now;
                        myAudit.TypeTran = "Create";
                        myAudit.SYSUserProfileID = (int)Session["User"];
                        myAudit.TableAff = "Lecture";

                        db.AuditLogs.Add(myAudit);

                        int max = db.Lectures.Max(p => p.LectureID);
                        int k = max + 1;
                        lecture.LectureID = k;

                        lecture.Type = 2;
                        db.Lectures.Add(lecture);
                        TRWLASchedule mySchedule = new TRWLASchedule();
                        mySchedule.LectureID = lecture.LectureID;
                        db.TRWLASchedules.Add(mySchedule);
                        db.SaveChanges();
                        return RedirectToAction("Index", "TRWLASchedules");
                    }

                    else
                    {
                        lecture.Type = 2;
                        db.Lectures.Add(lecture);
                        TRWLASchedule mySchedule = new TRWLASchedule();
                        mySchedule.LectureID = lecture.LectureID;
                        db.TRWLASchedules.Add(mySchedule);
                        db.SaveChanges();
                        return RedirectToAction("Index", "TRWLASchedules");
                    }



                }

                ViewBag.ContentID = new SelectList(db.Contents, "ContentID", "Content_Name", lecture.ContentID);
                ViewBag.ResidenceID = new SelectList(db.Residences, "ResID", "Res_Name", lecture.ResidenceID);
                ViewBag.VenueID = new SelectList(db.Venues, "VenueID", "Venue_Name", lecture.VenueID);
                return View(lecture);
            }
            catch
            {
                return RedirectToAction("ErrorPage", "TRWLASchedules");
            }
        }

        // GET: Lectures/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Lecture lecture = db.Lectures.Find(id);
                TempData["Date"] = lecture.Lecture_Date.ToString("dd MMMM yyyy");

                if (lecture == null)
                {
                    return HttpNotFound();
                }
                ViewBag.ContentID = new SelectList(db.Contents, "ContentID", "Content_Name", lecture.ContentID);
                ViewBag.ResidenceID = new SelectList(db.Residences, "ResID", "Res_Name", lecture.ResidenceID);
                ViewBag.VenueID = new SelectList(db.Venues, "VenueID", "Venue_Name", lecture.VenueID);
                return View(lecture);
            }
            catch
            {
                return RedirectToAction("ErrorPage", "TRWLASchedules");
            }
        }

        // POST: Lectures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LectureID,Lecture_Name,Lecture_Summary,Lecture_Description,Lecture_Date,Lecture_StartTime,Lecture_EndTime,Lecture_Theme,VenueID,ResidenceID,ContentID")] Lecture lecture)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string date = TempData["Date"].ToString();
                    TempData["Date"] = date;

                    AuditLog myAudit = new AuditLog();
                    myAudit.DateDone = DateTime.Now;
                    myAudit.TypeTran = "Update";
                    myAudit.SYSUserProfileID = (int)Session["User"];
                    myAudit.TableAff = "Lecture";


                    db.Entry(lecture).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "TRWLASchedules");
                }
                ViewBag.ContentID = new SelectList(db.Contents, "ContentID", "Content_Name", lecture.ContentID);
                ViewBag.ResidenceID = new SelectList(db.Residences, "ResID", "Res_Name", lecture.ResidenceID);
                ViewBag.VenueID = new SelectList(db.Venues, "VenueID", "Venue_Name", lecture.VenueID);
                return View(lecture);
            }
            catch
            {
                return RedirectToAction("ErrorPage", "TRWLASchedules");
            }
        }

        // GET: Lectures/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lecture lecture = db.Lectures.Find(id);
            if (lecture == null)
            {
                return HttpNotFound();
            }
            return View(lecture);
        }

        // POST: Lectures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Lecture lecture = db.Lectures.Find(id);
                db.Lectures.Remove(lecture);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("ErrorPage", "TRWLASchedules");
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