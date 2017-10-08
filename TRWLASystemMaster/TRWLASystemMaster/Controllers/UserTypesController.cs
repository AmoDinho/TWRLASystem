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
    public class UserTypesController : Controller
    {
        private TWRLADB_Staging_V2Entities8 db = new TWRLADB_Staging_V2Entities8();

        // GET: UserTypes
        public ActionResult Index()
        {
            try {

                return View(db.UserTypes.ToList());
            }
            catch (Exception)
            {
                return RedirectToAction("ErrorPage", "TRWLASchedules");
            }
        }

        // GET: UserTypes/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                UserType userType = db.UserTypes.Find(id);
                if (userType == null)
                {
                    return HttpNotFound();
                }
                return View(userType);
            }
            catch (Exception)
            {
                return RedirectToAction("ErrorPage", "TRWLASchedules");
            }
        }

        // GET: UserTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserTypeID,Description,AccessRight")] UserType userType)
        {
            try
            {


                if (ModelState.IsValid)
                {
                    int i = db.UserTypes.Count();

                    if (i != 0)
                    {
                        int k = db.UserTypes.Max(p => p.UserTypeID);
                        int max = k + 1;

                        AuditLog myAudit = new AuditLog();
                        myAudit.DateDone = DateTime.Now;
                        myAudit.TypeTran = "Create";
                        myAudit.SYSUserProfileID = (int)Session["User"];
                        myAudit.TableAff = "UserTypes";
                        db.AuditLogs.Add(myAudit);


                        userType.UserTypeID = max;

                        db.UserTypes.Add(userType);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }

                    else
                    {
                        db.UserTypes.Add(userType);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }

                }

                return View(userType);
            }

            catch (Exception)
            {
                return RedirectToAction("ErrorPage", "TRWLASchedules");
            }
        }

        // GET: UserTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                UserType userType = db.UserTypes.Find(id);
                if (userType == null)
                {
                    return HttpNotFound();
                }
                return View(userType);
            }
            catch (Exception)
            {
                return RedirectToAction("ErrorPage", "TRWLASchedules");
            }
        }

        // POST: UserTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserTypeID,Description,AccessRight")] UserType userType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(userType).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(userType);
            }
            catch (Exception)
            {
                return RedirectToAction("ErrorPage", "TRWLASchedules");
            }
        }

        // GET: UserTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserType userType = db.UserTypes.Find(id);
            if (userType == null)
            {
                return HttpNotFound();
            }
            return View(userType);
        }

        // POST: UserTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                UserType userType = db.UserTypes.Find(id);
                db.UserTypes.Remove(userType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            catch (Exception)
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
