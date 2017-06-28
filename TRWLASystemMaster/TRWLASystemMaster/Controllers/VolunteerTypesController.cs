﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TRWLASystemMaster.Models.Usertype;

namespace TRWLASystemMaster.Controllers
{
    public class VolunteerTypesController : Controller
    {
        private Db_TRWLA_StagingEntities db = new Db_TRWLA_StagingEntities();

        // GET: VolunteerTypes
        public ActionResult Index()
        {
            return View(db.VolunteerTypes.ToList());
        }

        // GET: VolunteerTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VolunteerType volunteerType = db.VolunteerTypes.Find(id);
            if (volunteerType == null)
            {
                return HttpNotFound();
            }
            return View(volunteerType);
        }

        // GET: VolunteerTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VolunteerTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VolunteerTypeID,Description")] VolunteerType volunteerType)
        {
            if (ModelState.IsValid)
            {
                db.VolunteerTypes.Add(volunteerType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(volunteerType);
        }

        // GET: VolunteerTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VolunteerType volunteerType = db.VolunteerTypes.Find(id);
            if (volunteerType == null)
            {
                return HttpNotFound();
            }
            return View(volunteerType);
        }

        // POST: VolunteerTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VolunteerTypeID,Description")] VolunteerType volunteerType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(volunteerType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(volunteerType);
        }

        // GET: VolunteerTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VolunteerType volunteerType = db.VolunteerTypes.Find(id);
            if (volunteerType == null)
            {
                return HttpNotFound();
            }
            return View(volunteerType);
        }

        // POST: VolunteerTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VolunteerType volunteerType = db.VolunteerTypes.Find(id);
            db.VolunteerTypes.Remove(volunteerType);
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
