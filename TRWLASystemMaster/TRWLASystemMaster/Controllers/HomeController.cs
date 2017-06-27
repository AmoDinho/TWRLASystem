using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TRWLASystemMaster.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoggedIn()
        {
            return View();
        }

        public ActionResult Gallery()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Who are we?";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Get in touch with TRWLA.";

            return View();
        }
       
    }
}