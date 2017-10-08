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
    public class ResidencesController : Controller
    {
        private TWRLADB_Staging_V2Entities8 db = new TWRLADB_Staging_V2Entities8();

        // GET: Residences
        public ActionResult Index(string searchString)
        {
            try
            {
                var res = from s in db.Residences
                          select s;

                if (!String.IsNullOrEmpty(searchString))
                {
                    res = res.Where(s => s.Res_Name.Contains(searchString));

                }

                return View(res.ToList());
            }
            catch (Exception)
            {
                return RedirectToAction("ErrorPage", "TRWLASchedules");
            }
            //return View(db.Residences.ToList());
        }

        // GET: Residences/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Residence residence = db.Residences.Find(id);
                if (residence == null)
                {
                    return HttpNotFound();

                }


                int res = (from s in db.SYSUserProfiles
                           where s.ResID == id
                           select s).Count();

                ViewBag.Res = Convert.ToString(res);
                return View(residence);
            }
            catch (Exception)
            {
                return RedirectToAction("ErrorPage", "TRWLASchedules");
            }
        }

        // GET: Residences/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Residences/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ResID,Res_Name")] Residence residence)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int i = db.Residences.Count();

                    if (i != 0)
                    {

                        AuditLog myAudit = new AuditLog();
                        myAudit.DateDone = DateTime.Now;
                        myAudit.TypeTran = "Create";
                        myAudit.SYSUserProfileID = (int)Session["User"];
                        myAudit.TableAff = "Residence";

                        int k = db.Residences.Max(p => p.ResID);
                        int max = k + 1;


                        residence.ResID = max;

                        db.Residences.Add(residence);
                        db.SaveChanges();
                        return RedirectToAction("Index");



                    }
                    else
                    {
                        db.Residences.Add(residence);
                        db.SaveChanges();
                        return RedirectToAction("Index");

                    }

                }

                return View(residence);
            }

            catch (Exception)
            {
                return RedirectToAction("ErrorPage", "TRWLASchedules");
            }
        }

        // GET: Residences/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Residence residence = db.Residences.Find(id);
                if (residence == null)
                {
                    return HttpNotFound();
                }
                return View(residence);
            }
            catch (Exception)
            {
                return RedirectToAction("ErrorPage", "TRWLASchedules");
            }
        }

        // POST: Residences/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ResID,Res_Name")] Residence residence)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    AuditLog myAudit = new AuditLog();
                    myAudit.DateDone = DateTime.Now;
                    myAudit.TypeTran = "Update";
                    myAudit.SYSUserProfileID = (int)Session["User"];
                    myAudit.TableAff = "Residence";

                    db.Entry(residence).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(residence);

            }
            catch (Exception)
            {
                return RedirectToAction("ErrorPage", "TRWLASchedules");
            }
        }

        // GET: Residences/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Residence residence = db.Residences.Find(id);
                if (residence == null)
                {
                    return HttpNotFound();
                }
                return View(residence);
            }
            catch (Exception)
            {
                return RedirectToAction("ErrorPage", "TRWLASchedules");
            }
        }

        // POST: Residences/Delete/5
        [HttpPost, ActionName("Details")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Residence residence = db.Residences.Find(id);
                try
                {
                    AuditLog myAudit = new AuditLog();
                    myAudit.DateDone = DateTime.Now;
                    myAudit.TypeTran = "Delete";
                    myAudit.SYSUserProfileID = (int)Session["User"];
                    myAudit.TableAff = "Residence";

                    db.Residences.Remove(residence);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                catch (Exception)
                {
                    int res = (from s in db.SYSUserProfiles
                               where s.ResID == id
                               select s).Count();

                    ViewBag.Res = Convert.ToString(res);
                    TempData["notice"] = "Please note: The residence has users assigned to it thus you are unable to delete it";
                    return View(residence);
                }
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
