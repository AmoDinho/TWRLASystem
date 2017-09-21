using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

using System.Net;
using TRWLASystemMaster.Models.DB;

namespace TRWLASystemMaster.Controllers
{
    public class UniqueCodeController : Controller
    {
        private TWRLADB_Staging_V2Entities db = new TWRLADB_Staging_V2Entities();
        // GET: UniqueCode
        public ActionResult UniqueCode()
        {



            return View();


        }

        public ActionResult NewUniCode()
        {

            Random rnd = new Random();

            UniqueCode unic = new UniqueCode();

            unic.Code = rnd.Next(10000, 99999);
            unic.stamptime = DateTime.Now;

            db.UniqueCodes.Add(unic);
            db.SaveChanges();

            return View(db.UniqueCodes.ToList());
        }


        public ActionResult AddCode()
        {
            return View();
        }
    }
}