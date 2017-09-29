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
    public class SYSUserProfilesController : Controller
    {
        private TWRLADB_Staging_V2Entities db = new TWRLADB_Staging_V2Entities();
        //
        /// <summary>
        /// To do :
        ///  Use dropdown to search for vols/students and grads
        /// </summary>
        /// 
        /// searchString
        /// <returns></returns>



        // GET: SYSUserProfiles
        public ActionResult Index(string searchString, string sortOrder, string Graduated)
        {

            ViewBag.SurnameSortParm = String.IsNullOrEmpty(sortOrder) ? "sur_desc" : "Surname";


            var sYSUserProfiles = db.SYSUserProfiles.Include(s => s.Residence).Include(s => s.SecurityAnswer).Include(s => s.SYSUser).Include(s => s.UserType);

            if (!String.IsNullOrEmpty(searchString))
            {
                sYSUserProfiles = sYSUserProfiles.Where(s => s.UserType.Description.Contains(searchString)
                        || s.UserType.Description.Contains(searchString)
                        );
            }

            //id="UrlList" onchange="doNavigate()"

            if (!String.IsNullOrEmpty(Graduated))
            {

                sYSUserProfiles = sYSUserProfiles.Where(s => s.Graduate.Contains("(Graduated)"));
            }


            return View(sYSUserProfiles.ToList());
        }

        // GET: SYSUserProfiles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SYSUserProfile sYSUserProfile = db.SYSUserProfiles.Find(id);
            if (sYSUserProfile == null)
            {
                return HttpNotFound();
            }
            return View(sYSUserProfile);
        }

    }
}






//// GET: SYSUserProfiles/Create
//public ActionResult Create()
//{
//    ViewBag.ResID = new SelectList(db.Residences, "ResID", "Res_Name");
//    ViewBag.SecurityAnswerID = new SelectList(db.SecurityAnswers, "SecurityAnswerID", "Security_Question");
//    ViewBag.SYSUserID = new SelectList(db.SYSUsers, "SYSUserID", "LoginName");
//    ViewBag.UserTypeID = new SelectList(db.UserTypes, "UserTypeID", "Description");
//    return View();
//}

//// POST: SYSUserProfiles/Create
//// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//[HttpPost]
//[ValidateAntiForgeryToken]
//public ActionResult Create([Bind(Include = "SYSUserProfileID,SYSUserID,StudentNumber,FirstName,LastName,UserTypeID,Email,DoB,Phonenumber,SecurityAnswerID,Graduate,Degree,YearOfStudy,RowCreatedSYSUserID,RowCreatedDateTime,RowModifiedSYSUserID,RowModifiedDateTime,ResID")] SYSUserProfile sYSUserProfile)
//{
//    if (ModelState.IsValid)
//    {
//        db.SYSUserProfiles.Add(sYSUserProfile);
//        db.SaveChanges();
//        return RedirectToAction("Index");
//    }

//    ViewBag.ResID = new SelectList(db.Residences, "ResID", "Res_Name", sYSUserProfile.ResID);
//    ViewBag.SecurityAnswerID = new SelectList(db.SecurityAnswers, "SecurityAnswerID", "Security_Question", sYSUserProfile.SecurityAnswerID);
//    ViewBag.SYSUserID = new SelectList(db.SYSUsers, "SYSUserID", "LoginName", sYSUserProfile.SYSUserID);
//    ViewBag.UserTypeID = new SelectList(db.UserTypes, "UserTypeID", "Description", sYSUserProfile.UserTypeID);
//    return View(sYSUserProfile);
//}

//// GET: SYSUserProfiles/Edit/5
//public ActionResult Edit(int? id)
//{
//    if (id == null)
//    {
//        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//    }
//    SYSUserProfile sYSUserProfile = db.SYSUserProfiles.Find(id);
//    if (sYSUserProfile == null)
//    {
//        return HttpNotFound();
//    }
//    ViewBag.ResID = new SelectList(db.Residences, "ResID", "Res_Name", sYSUserProfile.ResID);
//    ViewBag.SecurityAnswerID = new SelectList(db.SecurityAnswers, "SecurityAnswerID", "Security_Question", sYSUserProfile.SecurityAnswerID);
//    ViewBag.SYSUserID = new SelectList(db.SYSUsers, "SYSUserID", "LoginName", sYSUserProfile.SYSUserID);
//    ViewBag.UserTypeID = new SelectList(db.UserTypes, "UserTypeID", "Description", sYSUserProfile.UserTypeID);
//    return View(sYSUserProfile);
//}

//// POST: SYSUserProfiles/Edit/5
//// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//[HttpPost]
//[ValidateAntiForgeryToken]
//public ActionResult Edit([Bind(Include = "SYSUserProfileID,SYSUserID,StudentNumber,FirstName,LastName,UserTypeID,Email,DoB,Phonenumber,SecurityAnswerID,Graduate,Degree,YearOfStudy,RowCreatedSYSUserID,RowCreatedDateTime,RowModifiedSYSUserID,RowModifiedDateTime,ResID")] SYSUserProfile sYSUserProfile)
//{
//    if (ModelState.IsValid)
//    {
//        db.Entry(sYSUserProfile).State = EntityState.Modified;
//        db.SaveChanges();
//        return RedirectToAction("Index");
//    }
//    ViewBag.ResID = new SelectList(db.Residences, "ResID", "Res_Name", sYSUserProfile.ResID);
//    ViewBag.SecurityAnswerID = new SelectList(db.SecurityAnswers, "SecurityAnswerID", "Security_Question", sYSUserProfile.SecurityAnswerID);
//    ViewBag.SYSUserID = new SelectList(db.SYSUsers, "SYSUserID", "LoginName", sYSUserProfile.SYSUserID);
//    ViewBag.UserTypeID = new SelectList(db.UserTypes, "UserTypeID", "Description", sYSUserProfile.UserTypeID);
//    return View(sYSUserProfile);
//}

//// GET: SYSUserProfiles/Delete/5
//public ActionResult Delete(int? id)
//{
//    if (id == null)
//    {
//        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//    }
//    SYSUserProfile sYSUserProfile = db.SYSUserProfiles.Find(id);
//    if (sYSUserProfile == null)
//    {
//        return HttpNotFound();
//    }
//    return View(sYSUserProfile);
//}

//// POST: SYSUserProfiles/Delete/5
//[HttpPost, ActionName("Delete")]
//[ValidateAntiForgeryToken]
//public ActionResult DeleteConfirmed(int id)
//{
//    SYSUserProfile sYSUserProfile = db.SYSUserProfiles.Find(id);
//    db.SYSUserProfiles.Remove(sYSUserProfile);
//    db.SaveChanges();
//    return RedirectToAction("Index");
//}

//protected override void Dispose(bool disposing)
//{
//    if (disposing)
//    {
//        db.Dispose();
//    }
//    base.Dispose(disposing);
//}