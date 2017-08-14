﻿using System;
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
    public class StudentsController : Controller
    {
        private TWRLADB_Staging_V2Entities db = new TWRLADB_Staging_V2Entities();

        // GET: Students
        public ActionResult Index(string searchStringStud)
        {
            //var students = db.Students.Include(s => s.Residence).Include(s => s.Attendance).Include(s => s.RSVP_Event).Include(s => s.StudentType).Include(s => s.UserType);
            //return View(students.ToList());

            var ven = from v in db.Students
                      select v;
            if (!String.IsNullOrEmpty(searchStringStud))
            {
                ven = ven.Where(s => s.Student_Name.Contains(searchStringStud));

            }



            return View(ven.ToList());
        }

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            ViewBag.ResID = new SelectList(db.Residences, "ResID", "Res_Name");
            ViewBag.StudentID = new SelectList(db.Attendances, "StudentID", "StudentID");
            ViewBag.StudentID = new SelectList(db.RSVP_Event, "StudentID", "StudentID");
            ViewBag.StudentTypeID = new SelectList(db.StudentTypes, "StudentTypeID", "StudentTypeDescription");
            ViewBag.UserTypeID = new SelectList(db.UserTypes, "UserTypeID", "Description");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentID,StudentNumber,Graduate,Degree,YearOfStudy,Student_Name,Student_Surname,Student_Phone,Student_Email,Student_DoB,Student_Password,ActiveStatus,ResID,UserTypeID,StudentTypeID")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index", "StudentProgress");
            }

            ViewBag.ResID = new SelectList(db.Residences, "ResID", "Res_Name", student.ResID);
            ViewBag.StudentID = new SelectList(db.Attendances, "StudentID", "StudentID", student.StudentID);
            ViewBag.StudentID = new SelectList(db.RSVP_Event, "StudentID", "StudentID", student.StudentID);
            ViewBag.StudentTypeID = new SelectList(db.StudentTypes, "StudentTypeID", "StudentTypeDescription", student.StudentTypeID);
            ViewBag.UserTypeID = new SelectList(db.UserTypes, "UserTypeID", "Description", student.UserTypeID);
            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.ResID = new SelectList(db.Residences, "ResID", "Res_Name", student.ResID);
            ViewBag.StudentID = new SelectList(db.Attendances, "StudentID", "StudentID", student.StudentID);
            ViewBag.StudentID = new SelectList(db.RSVP_Event, "StudentID", "StudentID", student.StudentID);
            ViewBag.StudentTypeID = new SelectList(db.StudentTypes, "StudentTypeID", "StudentTypeDescription", student.StudentTypeID);
            ViewBag.UserTypeID = new SelectList(db.UserTypes, "UserTypeID", "Description", student.UserTypeID);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentID,StudentNumber,Graduate,Degree,YearOfStudy,Student_Name,Student_Surname,Student_Phone,Student_Email,Student_DoB,Student_Password,ActiveStatus,ResID,UserTypeID,StudentTypeID")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ResID = new SelectList(db.Residences, "ResID", "Res_Name", student.ResID);
            ViewBag.StudentID = new SelectList(db.Attendances, "StudentID", "StudentID", student.StudentID);
            ViewBag.StudentID = new SelectList(db.RSVP_Event, "StudentID", "StudentID", student.StudentID);
            ViewBag.StudentTypeID = new SelectList(db.StudentTypes, "StudentTypeID", "StudentTypeDescription", student.StudentTypeID);
            ViewBag.UserTypeID = new SelectList(db.UserTypes, "UserTypeID", "Description", student.UserTypeID);
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
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
