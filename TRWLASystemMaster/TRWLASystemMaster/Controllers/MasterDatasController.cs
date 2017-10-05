using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TRWLASystemMaster.Models.DB;

namespace TRWLASystemMaster.Controllers
{
    public class MasterDatasController : Controller
    {
        private TWRLADB_Staging_V2Entities8 db = new TWRLADB_Staging_V2Entities8();

        // GET: MasterDatas
        public ActionResult Index()
        {
            return View(db.MasterDatas.ToList());
        }

        // GET: MasterDatas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MasterData masterData = db.MasterDatas.Find(id);
            if (masterData == null)
            {
                return HttpNotFound();
            }
            return View(masterData);
        }

        // GET: MasterDatas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MasterDatas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MasterID,LecAttend,FuncAttend,ComAttend,GenAttend,RegDate,LogAttendTime,cancelevent")] MasterData masterData)
        {
            if (ModelState.IsValid)
            {
                db.MasterDatas.Add(masterData);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(masterData);
        }

        // GET: MasterDatas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MasterData masterData = db.MasterDatas.Find(id);
            if (masterData == null)
            {
                return HttpNotFound();
            }
            return View(masterData);
        }

        // POST: MasterDatas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MasterID,LecAttend,FuncAttend,ComAttend,GenAttend,RegDate,LogAttendTime,cancelevent")] MasterData masterData)
        {
            if (ModelState.IsValid)
            {
                AuditLog myAudit = new AuditLog();
                myAudit.DateDone = DateTime.Now;
                myAudit.TypeTran = "Update";
                myAudit.SYSUserProfileID = (int)Session["User"];
                myAudit.TableAff = "MasterData";
                db.AuditLogs.Add(myAudit);

                db.Entry(masterData).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = 1 });
            }
            return View(masterData);
        }

        // GET: MasterDatas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MasterData masterData = db.MasterDatas.Find(id);
            if (masterData == null)
            {
                return HttpNotFound();
            }
            return View(masterData);
        }

        // POST: MasterDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MasterData masterData = db.MasterDatas.Find(id);
            db.MasterDatas.Remove(masterData);
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
