using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TRWLASystemMaster.Models.DB;

namespace TRWLASystemMaster.Controllers
{
    public class AdminController : Controller
    {

        private TWRLADB_Staging_V2Entities7 DB = new TWRLADB_Staging_V2Entities7();

        // GET: Admin
        public ActionResult Index()
        {
           
            //var grad = from g in DB.Students
            //           select g;


           
            //grad = grad.Where(h => h.Graduate.Contains('1'));
                       

            return View();
        }


    }
}