using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TRWLASystemMaster.Models.DB;
using System.Web.Mvc;

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
    }
}