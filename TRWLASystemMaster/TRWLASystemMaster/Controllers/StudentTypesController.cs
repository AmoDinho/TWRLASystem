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
    public class StudentTypesController : Controller
    {
        private TWRLADB_Staging_V2Entities8 db = new TWRLADB_Staging_V2Entities8();

        // GET: StudentTypes
        public ActionResult Index(string searchStringST)
        {
            try
            {
                //return View(db.StudentTypes.ToList());

                var stutype = from st in db.StudentTypes
                              select st;
                if (!String.IsNullOrEmpty(searchStringST))
                {
                    stutype = stutype.Where(s => s.StudentTypeDescription.Contains(searchStringST));

                }



                return View(stutype.ToList());
            }
            catch (Exception)
            {
                return RedirectToAction("ErrorPage", "TRWLASchedules");
            }
        }

        // GET: StudentTypes/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                StudentType studentType = db.StudentTypes.Find(id);
                if (studentType == null)
                {
                    return HttpNotFound();
                }
                return View(studentType);
            }
            catch (Exception)
            {
                return RedirectToAction("ErrorPage", "TRWLASchedules");
            }
        }

        // GET: StudentTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StudentTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentTypeID,StudentTypeDescription")] StudentType studentType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int i = db.StudentTypes.Count();

                    if (i != 0)
                    {

                        int k = db.StudentTypes.Max(p => p.StudentTypeID);
                        int max = k + 1;

                        AuditLog myAudit = new AuditLog();
                        myAudit.DateDone = DateTime.Now;
                        myAudit.TypeTran = "Create";
                        myAudit.SYSUserProfileID = (int)Session["User"];
                        myAudit.TableAff = "StudentTypes";
                        db.AuditLogs.Add(myAudit);


                        studentType.StudentTypeID = max;

                        db.StudentTypes.Add(studentType);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }


                }

                else
                {

                    AuditLog myAudit = new AuditLog();
                    myAudit.DateDone = DateTime.Now;
                    myAudit.TypeTran = "Create";
                    myAudit.SYSUserProfileID = (int)Session["User"];
                    myAudit.TableAff = "StudentTypes";
                    db.AuditLogs.Add(myAudit);


                    db.StudentTypes.Add(studentType);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(studentType);
            }

            catch (Exception)
            {
                return RedirectToAction("ErrorPage", "TRWLASchedules");
            }
        }

        // GET: StudentTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                StudentType studentType = db.StudentTypes.Find(id);
                if (studentType == null)
                {
                    return HttpNotFound();
                }
                return View(studentType);
            }
            catch (Exception)
            {
                return RedirectToAction("ErrorPage", "TRWLASchedules");
            }
        }

        // POST: StudentTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentTypeID,StudentTypeDescription")] StudentType studentType)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    AuditLog myAudit = new AuditLog();
                    myAudit.DateDone = DateTime.Now;
                    myAudit.TypeTran = "Update";
                    myAudit.SYSUserProfileID = (int)Session["User"];
                    myAudit.TableAff = "StudentTypes";
                    db.AuditLogs.Add(myAudit);

                    db.Entry(studentType).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(studentType);
            }
            catch (Exception)
            {
                return RedirectToAction("ErrorPage", "TRWLASchedules");
            }
        }

        // GET: StudentTypes/Delete/5
        public ActionResult Delete(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentType studentType = db.StudentTypes.Find(id);
            if (studentType == null)
            {
                return HttpNotFound();
            }
            return View(studentType);
        }

        // POST: StudentTypes/Delete/5
        [HttpPost, ActionName("Details")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                StudentType studentType = db.StudentTypes.Find(id);
                db.StudentTypes.Remove(studentType);

                AuditLog myAudit = new AuditLog();
                myAudit.DateDone = DateTime.Now;
                myAudit.TypeTran = "Delete";
                myAudit.SYSUserProfileID = (int)Session["User"];
                myAudit.TableAff = "StudentTypes";
                db.AuditLogs.Add(myAudit);

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
