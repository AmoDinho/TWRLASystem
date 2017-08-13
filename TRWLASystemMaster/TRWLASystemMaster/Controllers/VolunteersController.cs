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
        private TWRLADB_Staging_V2Entities db = new TWRLADB_Staging_V2Entities();

        // GET: Volunteers
        public ActionResult Index()
        {
            var volunteers = db.Volunteers.Include(v => v.UserType).Include(v => v.VolunteerType);
            return View(volunteers.ToList());
        }

        // GET: Volunteers/Detailsc:\users\residences\documents\visual studio 2015\Projects\WebApplication1\WebApplication1\Controllers\ManageController.cs/5
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
            ViewBag.UserTypeID = new SelectList(db.UserTypes, "UserTypeID", "Description");
            ViewBag.VolunteerTypeID = new SelectList(db.VolunteerTypes, "VolunteerTypeID", "VolunteerType_Description");
            return View();
        }

        // POST: Volunteers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VolunteerID,Volunteer_Name,Volunteer_Surname,Volunteer_Phone,Volunteer_Email,Volunteer_DoB,Volunteer_Password,ActiveStatus,UserTypeID,VolunteerTypeID")] Volunteer volunteer)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = volunteer.Volunteer_Email, Email = volunteer.Volunteer_Email };
                //var result = await UserManager.CreateAsync(user, model.Password);
                db.Volunteers.Add(volunteer);
                db.SaveChanges();
                return RedirectToAction("Index", "StudentProgress");
            }

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
            ViewBag.UserTypeID = new SelectList(db.UserTypes, "UserTypeID", "Description", volunteer.UserTypeID);
            ViewBag.VolunteerTypeID = new SelectList(db.VolunteerTypes, "VolunteerTypeID", "VolunteerType_Description", volunteer.VolunteerTypeID);
            return View(volunteer);
        }

        // POST: Volunteers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VolunteerID,Volunteer_Name,Volunteer_Surname,Volunteer_Phone,Volunteer_Email,Volunteer_DoB,Volunteer_Password,ActiveStatus,UserTypeID,VolunteerTypeID")] Volunteer volunteer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(volunteer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
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
