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
    public class TRWLASchedulesController : Controller
    {
        private TWRLADB_Staging_V2Entities db = new TWRLADB_Staging_V2Entities();

        // GET: TRWLASchedules
        public ActionResult Index(string sortOrder, string searchString, string F, string CO, string L, string all)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.SurnameSortParm = String.IsNullOrEmpty(sortOrder) ? "sur_desc" : "Surname";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            var tRWLASchedules = db.TRWLASchedules.Include(t => t.ComEngEvent).Include(t => t.FunctionEvent).Include(t => t.Lecture);


            if (!String.IsNullOrEmpty(searchString))
            {
                tRWLASchedules = tRWLASchedules.Where(s => s.FunctionEvent.Function_Name.Contains(searchString)
                        || s.Lecture.Lecture_Name.Contains(searchString)
                        || s.ComEngEvent.ComEng_Name.Contains(searchString));
            }


            if (!String.IsNullOrEmpty(F))
            {
                tRWLASchedules = tRWLASchedules.Where(s => s.FunctionEvent.Function_Name.Contains("(F)"));
            }

            if (!String.IsNullOrEmpty(CO))
            {
                tRWLASchedules = tRWLASchedules.Where(s => s.ComEngEvent.ComEng_Name.Contains("(CO)"));
            }

            if (!String.IsNullOrEmpty(L))
            {
                tRWLASchedules = tRWLASchedules.Where(s => s.Lecture.Lecture_Name.Contains("(L)"));
            }

            if (!String.IsNullOrEmpty(all))
            {
                tRWLASchedules = tRWLASchedules.Where(s => s.Lecture.Lecture_Name.Contains("(L)")
                        || s.ComEngEvent.ComEng_Name.Contains("(CO)")
                        || s.FunctionEvent.Function_Name.Contains("(F)"));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    tRWLASchedules = tRWLASchedules.OrderByDescending(s => s.FunctionEvent.Function_Name.Contains(searchString)
                        || s.Lecture.Lecture_Name.Contains(searchString)
                        || s.ComEngEvent.ComEng_Name.Contains(searchString));
                    break;
                case "sur_desc":
                    tRWLASchedules = tRWLASchedules.OrderByDescending(s => s.FunctionEvent.Function_Name.Contains(searchString)
                        || s.Lecture.Lecture_Name.Contains(searchString)
                        || s.ComEngEvent.ComEng_Name.Contains(searchString));
                    break;
                case "Surname":
                    tRWLASchedules = tRWLASchedules.OrderByDescending(s => s.FunctionEvent.Function_Name.Contains(searchString)
                        || s.Lecture.Lecture_Name.Contains(searchString)
                        || s.ComEngEvent.ComEng_Name.Contains(searchString));
                    break;
                default:
                    tRWLASchedules = tRWLASchedules.OrderBy(s => s.FunctionEvent.Function_Name.Contains(searchString)
                        || s.Lecture.Lecture_Name.Contains(searchString)
                        || s.ComEngEvent.ComEng_Name.Contains(searchString));
                    break;
            }


            return View(tRWLASchedules.ToList());
        }



        // GET: TRWLASchedules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TRWLASchedule tRWLASchedule = db.TRWLASchedules.Find(id);
            if (tRWLASchedule == null)
            {
                return HttpNotFound();
            }
            return View(tRWLASchedule);
        }

        public ActionResult RSVP(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TRWLASchedule tRWLASchedule = db.TRWLASchedules.Find(id);
            if (tRWLASchedule == null)
            {
                return HttpNotFound();
            }
            return View(tRWLASchedule);
        }

        public ActionResult EventType(int? id)
        {
            return View();
        }

        [HttpPost, ActionName("EventType")]
        [ValidateAntiForgeryToken]
        public ActionResult EventTypeLoad(int id)
        {
            if (id == 1)
            {
                return View("../FunctionEvents/Create");
            }
            else if (id == 2)
            {
                return View("../ComEngEvents/Create");
            }
            else
            {
                return View("../Lectures/Create");
            }
        }




        // GET: TRWLASchedules/Create
        public ActionResult Create()
        {
            ViewBag.ComEngID = new SelectList(db.ComEngEvents, "ComEngID", "ComEng_Name");
            ViewBag.FunctionID = new SelectList(db.FunctionEvents, "FunctionID", "Function_Name");
            ViewBag.LectureID = new SelectList(db.Lectures, "LectureID", "Lecture_Name");
            return View();
        }

        // POST: TRWLASchedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ScheduleID,FunctionID,LectureID,ComEngID")] TRWLASchedule tRWLASchedule)
        {
            if (ModelState.IsValid)
            {
                db.TRWLASchedules.Add(tRWLASchedule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ComEngID = new SelectList(db.ComEngEvents, "ComEngID", "ComEng_Name", tRWLASchedule.ComEngID);
            ViewBag.FunctionID = new SelectList(db.FunctionEvents, "FunctionID", "Function_Name", tRWLASchedule.FunctionID);
            ViewBag.LectureID = new SelectList(db.Lectures, "LectureID", "Lecture_Name", tRWLASchedule.LectureID);
            return View(tRWLASchedule);
        }



        // GET: TRWLASchedules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TRWLASchedule tRWLASchedule = db.TRWLASchedules.Find(id);
            if (tRWLASchedule == null)
            {
                return HttpNotFound();
            }
            ViewBag.ComEngID = new SelectList(db.ComEngEvents, "ComEngID", "ComEng_Name", tRWLASchedule.ComEngID);
            ViewBag.FunctionID = new SelectList(db.FunctionEvents, "FunctionID", "Function_Name", tRWLASchedule.FunctionID);
            ViewBag.LectureID = new SelectList(db.Lectures, "LectureID", "Lecture_Name", tRWLASchedule.LectureID);

            if (tRWLASchedule.LectureID != null)
            {
                ViewBag.ContentID = new SelectList(db.Contents, "ContentID", "Content_Name", tRWLASchedule.Lecture.ContentID);
                ViewBag.ResidenceID = new SelectList(db.Residences, "ResID", "Res_Name", tRWLASchedule.Lecture.ResidenceID);
                ViewBag.VenueID = new SelectList(db.Venues, "VenueID", "Venue_Name", tRWLASchedule.Lecture.VenueID);
            }
            else if (tRWLASchedule.FunctionID != null)
            {
                ViewBag.GuestSpeakerID = new SelectList(db.GuestSpeakers, "GuestSpeakerID", "GuestSpeaker_Name", tRWLASchedule.FunctionEvent.GuestSpeakerID);
                ViewBag.VenueID = new SelectList(db.Venues, "VenueID", "Venue_Name", tRWLASchedule.FunctionEvent.VenueID);
            }
            else if (tRWLASchedule.ComEngID != null)
            {
                ViewBag.ContentID = new SelectList(db.Contents, "ContentID", "Content_Name", tRWLASchedule.ComEngEvent.ContentID);
                ViewBag.VenueID = new SelectList(db.Venues, "VenueID", "Venue_Name", tRWLASchedule.ComEngEvent.VenueID);
            }
            return View(tRWLASchedule);
        }

        // POST: TRWLASchedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ScheduleID,FunctionID,LectureID,ComEngID")] TRWLASchedule tRWLASchedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tRWLASchedule).State = EntityState.Modified;

                if (tRWLASchedule.FunctionID != null)
                {
                    FunctionEvent func = db.FunctionEvents.Find(tRWLASchedule.FunctionID);


                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ComEngID = new SelectList(db.ComEngEvents, "ComEngID", "ComEng_Name", tRWLASchedule.ComEngID);
            ViewBag.FunctionID = new SelectList(db.FunctionEvents, "FunctionID", "Function_Name", tRWLASchedule.FunctionID);
            ViewBag.LectureID = new SelectList(db.Lectures, "LectureID", "Lecture_Name", tRWLASchedule.LectureID);

            if (tRWLASchedule.LectureID != null)
            {
                ViewBag.ContentID = new SelectList(db.Contents, "ContentID", "Content_Name", tRWLASchedule.Lecture.ContentID);
                ViewBag.ResidenceID = new SelectList(db.Residences, "ResID", "Res_Name", tRWLASchedule.Lecture.ResidenceID);
                ViewBag.VenueID = new SelectList(db.Venues, "VenueID", "Venue_Name", tRWLASchedule.Lecture.VenueID);
            }
            else if (tRWLASchedule.FunctionID != null)
            {
                ViewBag.GuestSpeakerID = new SelectList(db.GuestSpeakers, "GuestSpeakerID", "GuestSpeaker_Name", tRWLASchedule.FunctionEvent.GuestSpeakerID);
                ViewBag.VenueID = new SelectList(db.Venues, "VenueID", "Venue_Name", tRWLASchedule.FunctionEvent.VenueID);
            }
            else if (tRWLASchedule.ComEngID != null)
            {
                ViewBag.ContentID = new SelectList(db.Contents, "ContentID", "Content_Name", tRWLASchedule.ComEngEvent.ContentID);
                ViewBag.VenueID = new SelectList(db.Venues, "VenueID", "Venue_Name", tRWLASchedule.ComEngEvent.VenueID);
            }

            return View(tRWLASchedule);
        }

        // GET: TRWLASchedules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TRWLASchedule tRWLASchedule = db.TRWLASchedules.Find(id);
            if (tRWLASchedule == null)
            {
                return HttpNotFound();
            }
            return View(tRWLASchedule);
        }

        [HttpPost, ActionName("RSVP")]
        [ValidateAntiForgeryToken]
        public ActionResult RSVPConfirmed(RSVP_Event @event, int id, TRWLASchedule trwla)
        {
            //Counts if the table has data in it
            int i = db.RSVP_Event.Count();

            if (i == 0)
            {
                //this is what adds data to the table
                @event.StudentID = 1;

                if (trwla.FunctionID != null)
                {
                    @event.FunctionID = id;
                }
                else if (trwla.ComEngID != null)
                {
                    @event.ComEngID = id;
                }
                else if (trwla.LectureID != null)
                {
                    @event.LectureID = id;
                }
                //@event.rsvpID = 0;
                db.RSVP_Event.Add(@event);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                //int max = db.RSVP_Event.Max(p => p.rsvpID);
                //int l = max + 1;
                @event.StudentID = 1;
                if (trwla.FunctionID != null)
                {
                    @event.FunctionID = id;
                }
                else if (trwla.ComEngID != null)
                {
                    @event.ComEngID = id;
                }
                else if (trwla.LectureID != null)
                {
                    @event.LectureID = id;
                }
                //@event.rsvpID = l;
                db.RSVP_Event.Add(@event);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

        }

        // POST: TRWLASchedules/Delete/5
        [HttpPost, ActionName("Details")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            TRWLASchedule tRWLASchedule = db.TRWLASchedules.Find(id);

            if (tRWLASchedule.FunctionID != null)
            {
                FunctionEvent functions = db.FunctionEvents.Find(tRWLASchedule.FunctionID);
                db.FunctionEvents.Remove(functions);
            }
            else if (tRWLASchedule.LectureID != null)
            {
                Lecture lectures = db.Lectures.Find(tRWLASchedule.LectureID);
                db.Lectures.Remove(lectures);
            }
            else if (tRWLASchedule.ComEngID != null)
            {
                ComEngEvent comeng = db.ComEngEvents.Find(tRWLASchedule.ComEngID);
                db.ComEngEvents.Remove(comeng);
            }

            db.TRWLASchedules.Remove(tRWLASchedule);
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