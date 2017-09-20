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
    public class SelectController : Controller
    {

        private TWRLADB_Staging_V2Entities db = new TWRLADB_Staging_V2Entities();
        // GET: Select
        public ActionResult SelectVolStud()
        {

           
            return View();
        }

        public ActionResult VerifyCode()
        {
            return View();
        }
        [HttpPost]
        public ActionResult VerifyCode(UniqueCode vc)
        {

            //if(!ModelState.IsValid)
            //{
            //    RedirectToAction("RegisterVol", "Account");
            //    return View(vc);
            //}

            //return View();
            var code = from c in db.UniqueCodes
                       where c.Code == vc.Code
                       select c;


            if (code != null)
            {
                RedirectToAction("RegisterVol", "Account");
            }

            return View(code);

        }
    }
}