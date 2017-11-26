using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TRWLASystemMaster.Models;
using TRWLASystemMaster.Models.DB;
using System.Web.Mvc;
using TRWLASystemMaster.Models.ViewModel;
using TRWLASystemMaster.Models.EntityManager;

namespace TRWLASystemMaster.Controllers
{

    //This Controller allows the user to select whether they are student or volunteer
    public class SelectController : Controller
    {

        private TWRLADB_Staging_V2Entities8 db = new TWRLADB_Staging_V2Entities8();
        // GET: TWRLADB_Staging_V2Entities7
        public ActionResult SelectVolStud()
        {

           
            return View();
        }

        public ActionResult VerifyCode()
        {
            return View();
        }
        [HttpPost]
        public ActionResult VerifyCode(string uniqe)
        {
            if (ModelState.IsValid)
            {
                
                if (uniqe != "")
                {
                    ViewBag.NameSortParm = String.IsNullOrEmpty(uniqe) ? "name_desc" : "";

                    int variable;

                    try
                    {
                        variable = Convert.ToInt32(uniqe);
                    }
                    catch
                    {
                        ViewBag.Error = "Please input a number";
                        return View();
                    }

                    int code = (from c in db.UniqueCodes
                                where c.Code == variable
                                select c).Count();


                    if (code != 0)
                    {
                        UniqueCode myCode = db.UniqueCodes.FirstOrDefault(p => p.Code == variable);
                        db.UniqueCodes.Remove(myCode);
                        db.SaveChanges();

                        return RedirectToAction("RegisterVol", "Account");
                    }
                    else
                    {
                        ViewBag.Error = "Please input a valid code";
                        return View();
                    }
                }
                else
                {
                    ViewBag.Error = "Please enter a code";
                    return View();
                }
            }
            return View();
        }
    }
}