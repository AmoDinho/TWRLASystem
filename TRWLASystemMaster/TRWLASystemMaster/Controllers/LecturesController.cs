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
        private TWRLADB_Staging_V2Entities15 db = new TWRLADB_Staging_V2Entities15();

        // GET: Lectures
        public ActionResult Index()
        {
            var lectures = db.Lectures.Include(l => l.Content).Include(l => l.Residence).Include(l => l.Venue);
            return View(lectures.ToList());
        }

        // GET: Lectures/Details/5
        public ActionResult Details(int? id)
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

        // GET: Lectures/Create
        public ActionResult Create()
        {
            ViewBag.ContentID = new SelectList(db.Contents, "ContentID", "Content_Name");
            ViewBag.ResidenceID = new SelectList(db.Residences, "ResID", "Res_Name");
            ViewBag.VenueID = new SelectList(db.Venues, "VenueID", "Venue_Name");
            return View();
        }

        // POST: Lectures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LectureID,Lecture_Name,Lecture_Summary,Lecture_Description,Lecture_Date,Lecture_StartTime,Lecture_EndTime,Lecture_Theme,VenueID,ResidenceID,ContentID")] Lecture lecture)
        {

            if (ModelState.IsValid)
            {
                int i = db.Lectures.Count();

                if (i != 0)
                {
                    int max = db.Lectures.Max(p => p.LectureID);
                    int k = max + 1;
                    lecture.LectureID = k;

                    lecture.Lecture_Name = lecture.Lecture_Name + " (L)";
                    db.Lectures.Add(lecture);
                    TRWLASchedule mySchedule = new TRWLASchedule();
                    mySchedule.LectureID = lecture.LectureID;
                    db.TRWLASchedules.Add(mySchedule);
                    db.SaveChanges();
                    return RedirectToAction("Index", "TRWLASchedules");
                }

                else
                {
                    lecture.Lecture_Name = lecture.Lecture_Name + " (L)";
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

        // GET: Lectures/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.ContentID = new SelectList(db.Contents, "ContentID", "Content_Name", lecture.ContentID);
            ViewBag.ResidenceID = new SelectList(db.Residences, "ResID", "Res_Name", lecture.ResidenceID);
            ViewBag.VenueID = new SelectList(db.Venues, "VenueID", "Venue_Name", lecture.VenueID);
            return View(lecture);
        }

        // POST: Lectures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LectureID,Lecture_Name,Lecture_Summary,Lecture_Description,Lecture_Date,Lecture_StartTime,Lecture_EndTime,Lecture_Theme,VenueID,ResidenceID,ContentID")] Lecture lecture)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lecture).State = EntityState.Modified;
                db.SaveChanges();
                RedirectToAction("Index", "TRWLASchedules");
            }
            ViewBag.ContentID = new SelectList(db.Contents, "ContentID", "Content_Name", lecture.ContentID);
            ViewBag.ResidenceID = new SelectList(db.Residences, "ResID", "Res_Name", lecture.ResidenceID);
            ViewBag.VenueID = new SelectList(db.Venues, "VenueID", "Venue_Name", lecture.VenueID);
            return View(lecture);
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
            Lecture lecture = db.Lectures.Find(id);
            db.Lectures.Remove(lecture);
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