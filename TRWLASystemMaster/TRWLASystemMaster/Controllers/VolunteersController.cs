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
    public class VolunteersController : Controller
    {
        private TWRLADB_Staging_V2Entities9 db = new TWRLADB_Staging_V2Entities9();

        // GET: Volunteers
        public ActionResult Index()
        {
            var volunteers = db.Volunteers.Include(v => v.AspNetUser).Include(v => v.UserType).Include(v => v.VolunteerType);
            return View(volunteers.ToList());
        }

        // GET: Volunteers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Volunteer volunteer = db.Volunteers.Find(id);
            if (volunteer == null)
            {
                return HttpNotFound();
            }
            return View(volunteer);
        }

        // GET: Volunteers/Create
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.UserTypeID = new SelectList(db.UserTypes, "UserTypeID", "Description");
            ViewBag.VolunteerTypeID = new SelectList(db.VolunteerTypes, "VolunteerTypeID", "VolunteerType_Description");
            return View();
        }

        // POST: Volunteers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VolunteerID,Volunteer_Name,Volunteer_Surname,Volunteer_Phone,Volunteer_DoB,ActiveStatus,Id,UserTypeID,VolunteerTypeID")] Volunteer volunteer)
        {
            if (ModelState.IsValid)
            {
                db.Volunteers.Add(volunteer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email", volunteer.Id);
            ViewBag.UserTypeID = new SelectList(db.UserTypes, "UserTypeID", "Description", volunteer.UserTypeID);
            ViewBag.VolunteerTypeID = new SelectList(db.VolunteerTypes, "VolunteerTypeID", "VolunteerType_Description", volunteer.VolunteerTypeID);
            return View(volunteer);
        }

        // GET: Volunteers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Volunteer volunteer = db.Volunteers.Find(id);
            if (volunteer == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email", volunteer.Id);
            ViewBag.UserTypeID = new SelectList(db.UserTypes, "UserTypeID", "Description", volunteer.UserTypeID);
            ViewBag.VolunteerTypeID = new SelectList(db.VolunteerTypes, "VolunteerTypeID", "VolunteerType_Description", volunteer.VolunteerTypeID);
            return View(volunteer);
        }

        // POST: Volunteers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VolunteerID,Volunteer_Name,Volunteer_Surname,Volunteer_Phone,Volunteer_DoB,ActiveStatus,Id,UserTypeID,VolunteerTypeID")] Volunteer volunteer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(volunteer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email", volunteer.Id);
            ViewBag.UserTypeID = new SelectList(db.UserTypes, "UserTypeID", "Description", volunteer.UserTypeID);
            ViewBag.VolunteerTypeID = new SelectList(db.VolunteerTypes, "VolunteerTypeID", "VolunteerType_Description", volunteer.VolunteerTypeID);
            return View(volunteer);
        }

        // GET: Volunteers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Volunteer volunteer = db.Volunteers.Find(id);
            if (volunteer == null)
            {
                return HttpNotFound();
            }
            return View(volunteer);
        }

        // POST: Volunteers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Volunteer volunteer = db.Volunteers.Find(id);
            db.Volunteers.Remove(volunteer);
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
