using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TRWLASystemMaster.Models;
using System.Web.Helpers;
using System.Net.Mail;

namespace TRWLASystemMaster.Controllers
{
    public class FunctionEventsController : Controller
    {
        private TWRLADB_Staging_V2Entities db = new TWRLADB_Staging_V2Entities();

        // GET: FunctionEvents
        public ActionResult Index()
        {
            var functionEvents = db.FunctionEvents.Include(f => f.GuestSpeaker).Include(f => f.Venue);
            return View(functionEvents.ToList());
        }

        // GET: FunctionEvents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FunctionEvent functionEvent = db.FunctionEvents.Find(id);
            if (functionEvent == null)
            {
                return HttpNotFound();
            }
            return View(functionEvent);
        }

        // GET: FunctionEvents/Create
        public ActionResult Create()
        {
            ViewBag.GuestSpeakerID = new SelectList(db.GuestSpeakers, "GuestSpeakerID", "GuestSpeaker_Name");
            ViewBag.VenueID = new SelectList(db.Venues, "VenueID", "Venue_Name");
            return View();
        }

        // POST: FunctionEvents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FunctionID,Function_Name,Function_Summary,Function_Description,Function_Date,Function_StartTime,Function_EndTime,Function_Theme,GuestSpeakerID,VenueID")] FunctionEvent functionEvent)
        {

            if (ModelState.IsValid)
            {
                int i = db.FunctionEvents.Count();

                if (i != 0)
                {
                    int max = db.FunctionEvents.Max(p => p.FunctionID);
                    int k = max + 1;
                    functionEvent.FunctionID = k;
                    functionEvent.Function_Name = functionEvent.Function_Name + " (F)";


                    db.FunctionEvents.Add(functionEvent);
                    TRWLASchedule mySchedule = new TRWLASchedule();
                    mySchedule.FunctionID = functionEvent.FunctionID;
                    db.TRWLASchedules.Add(mySchedule);
                    db.SaveChanges();

                }

                else
                {
                    functionEvent.Function_Name = functionEvent.Function_Name + " (F)";

                    db.FunctionEvents.Add(functionEvent);
                    TRWLASchedule mySchedule = new TRWLASchedule();
                    mySchedule.FunctionID = functionEvent.FunctionID;
                    db.TRWLASchedules.Add(mySchedule);
                    db.SaveChanges();
                }

                try
                {
                    int k = Convert.ToInt32(functionEvent.GuestSpeakerID);

                    GuestSpeaker guest = db.GuestSpeakers.Find(k);



                    MailMessage msg = new MailMessage();
                    msg.From = new MailAddress("u15213626@tuks.co.za");
                    msg.To.Add(guest.GuestSpeaker_Email);
                    msg.Subject = functionEvent.Function_Name + " Invitation";
                    msg.Body = "Dear " + guest.GuestSpeaker_Name + "\n\n You have been invited to the following event (" + functionEvent.Function_Name + ") on the " + functionEvent.Function_Date.ToString("dd MM yyyy") + ".\n The event will take place from " + functionEvent.Function_StartTime + " until " + functionEvent.Function_EndTime + ". \n\n Should you wish to attend please reply to this email so that we can note your attendance. \n\n Kind Regards,\nTRLWA Management";

                    SmtpClient smtp = new SmtpClient();

                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential("u15213626@tuks.co.za", "Rootsms4");
                    smtp.EnableSsl = true;
                    smtp.Send(msg);

                    ModelState.Clear();


                    ViewBag.Status = "Email Sent Successfully.";
                }
                catch (Exception)
                {
                    ViewBag.Status = "Problem while sending email, Please check details.";

                }
                return RedirectToAction("Index", "TRWLASchedules");

            }

            ViewBag.GuestSpeakerID = new SelectList(db.GuestSpeakers, "GuestSpeakerID", "GuestSpeaker_Name", functionEvent.GuestSpeakerID);
            ViewBag.VenueID = new SelectList(db.Venues, "VenueID", "Venue_Name", functionEvent.VenueID);
            return View(functionEvent);
        }

        // GET: FunctionEvents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FunctionEvent functionEvent = db.FunctionEvents.Find(id);
            if (functionEvent == null)
            {
                return HttpNotFound();
            }
            ViewBag.GuestSpeakerID = new SelectList(db.GuestSpeakers, "GuestSpeakerID", "GuestSpeaker_Name", functionEvent.GuestSpeakerID);
            ViewBag.VenueID = new SelectList(db.Venues, "VenueID", "Venue_Name", functionEvent.VenueID);
            return View(functionEvent);
        }

        // POST: FunctionEvents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FunctionID,Function_Name,Function_Summary,Function_Description,Function_Date,Function_StartTime,Function_EndTime,Function_Theme,GuestSpeakerID,VenueID")] FunctionEvent functionEvent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(functionEvent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "TRWLASchedules");
            }
            ViewBag.GuestSpeakerID = new SelectList(db.GuestSpeakers, "GuestSpeakerID", "GuestSpeaker_Name", functionEvent.GuestSpeakerID);
            ViewBag.VenueID = new SelectList(db.Venues, "VenueID", "Venue_Name", functionEvent.VenueID);
            return View(functionEvent);
        }

        // GET: FunctionEvents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FunctionEvent functionEvent = db.FunctionEvents.Find(id);
            if (functionEvent == null)
            {
                return HttpNotFound();
            }
            return View(functionEvent);
        }

        // POST: FunctionEvents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FunctionEvent functionEvent = db.FunctionEvents.Find(id);
            db.FunctionEvents.Remove(functionEvent);
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