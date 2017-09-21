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
    public class GuestSpeakerController : Controller
    {
        private TWRLADB_Staging_V2Entities1 db = new TWRLADB_Staging_V2Entities1();

        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                GuestSpeaker guestSpeaker = db.GuestSpeakers.Find(id);
                if (guestSpeaker == null)
                {
                    return HttpNotFound();
                }
                return View(guestSpeaker);
            }
            catch
            {
                return RedirectToAction("ErrorPage", "TRWLASchedules");
            }
        }

        // GET: GuestSpeakers1
        public ActionResult Index(string sortOrder, string searchString)
        {
            try
            {
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.SurnameSortParm = String.IsNullOrEmpty(sortOrder) ? "sur_desc" : "Surname";
                ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";



                var guestSpeaker = from s in db.GuestSpeakers
                                   select s;

                if (!String.IsNullOrEmpty(searchString))
                {
                    guestSpeaker = guestSpeaker.Where(s => s.GuestSpeaker_Name.Contains(searchString)
                                           || s.GuestSpeaker_Surname.Contains(searchString));
                }


                switch (sortOrder)
                {
                    case "name_desc":
                        guestSpeaker = guestSpeaker.OrderByDescending(s => s.GuestSpeaker_Name);
                        break;
                    case "sur_desc":
                        guestSpeaker = guestSpeaker.OrderByDescending(s => s.GuestSpeaker_Surname);
                        break;
                    case "Surname":
                        guestSpeaker = guestSpeaker.OrderByDescending(s => s.GuestSpeaker_Surname);
                        break;
                    default:
                        guestSpeaker = guestSpeaker.OrderBy(s => s.GuestSpeaker_Name);
                        break;
                }

                return View(guestSpeaker.ToList());
            }
            catch
            {
                return RedirectToAction("ErrorPage", "TRWLASchedules");
            }
        }



        // GET: GuestSpeakers1/Details/5


        // GET: GuestSpeakers1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GuestSpeakers1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GuestSpeakerID,GuestSpeaker_Name,GuestSpeaker_Surname,GuestSpeaker_Phone,GuestSpeaker_Email,GuestSpeaker_PictureLink")] GuestSpeaker guestSpeaker)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int i = db.GuestSpeakers.Count();

                    if (i != 0)
                    {
                        int k = db.GuestSpeakers.Max(p => p.GuestSpeakerID);
                        int max = k + 1;

                        guestSpeaker.GuestSpeakerID = max;

                        db.GuestSpeakers.Add(guestSpeaker);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }

                    else
                    {
                        db.GuestSpeakers.Add(guestSpeaker);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }


                }

                return View(guestSpeaker);
            }
            catch
            {
                return RedirectToAction("ErrorPage", "TRWLASchedules");
            }
        }

        // GET: GuestSpeakers1/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                GuestSpeaker guestSpeaker = db.GuestSpeakers.Find(id);
                if (guestSpeaker == null)
                {
                    return HttpNotFound();
                }
                return View(guestSpeaker);
            }
            catch
            {
                return RedirectToAction("ErrorPage", "TRWLASchedules");
            }
        }

        // POST: GuestSpeakers1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GuestSpeakerID,GuestSpeaker_Name,GuestSpeaker_Surname,GuestSpeaker_Phone,GuestSpeaker_Email,GuestSpeaker_PictureLink")] GuestSpeaker guestSpeaker)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(guestSpeaker).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(guestSpeaker);
            }
            catch
            {
                return RedirectToAction("ErrorPage", "TRWLASchedules");
            }
        }

        // GET: GuestSpeakers1/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                GuestSpeaker guestSpeaker = db.GuestSpeakers.Find(id);
                if (guestSpeaker == null)
                {
                    return HttpNotFound();
                }
                return View(guestSpeaker);
            }
            catch
            {
                return RedirectToAction("ErrorPage", "TRWLASchedules");
            }
        }

        // POST: GuestSpeakers1/Delete/5
        [HttpPost, ActionName("Details")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                GuestSpeaker guestSpeaker = db.GuestSpeakers.Find(id);

                try
                {
                    db.GuestSpeakers.Remove(guestSpeaker);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    TempData["notice"] = " Please note: This guest speaker is assigned to an event and cannot be deleted.";
                    return View(guestSpeaker);
                }
            }
            catch
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