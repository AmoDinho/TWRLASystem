using System;
using System.Globalization;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using TRWLASystemMaster.Models;
using TRWLASystemMaster.Models.ViewModel;
using TRWLASystemMaster.Models.EntityManager;
using System.Web.Security;
using TRWLASystemMaster.Models.DB;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;
using System.Net.Mail;
using System.Activities.Expressions;

namespace TRWLASystemMaster.Controllers
{

    public class AccountController : Controller
    {
        //
        /// <summary>
        /// ACCOUNT CONTROLLER:
        /// 
        /// 
        /// 
        ///  Register 
        ///  
        /// Login
        /// 
        /// Sign out 
        /// 
        /// 
        /// </summary>
        /// 



        private TWRLADB_Staging_V2Entities8 db = new TWRLADB_Staging_V2Entities8();
        //Register Student
        public ActionResult Register()
        {

            ViewBag.UserTypeID = new SelectList(db.UserTypes, "UserTypeID", "Description", "AccessRight");
            ViewBag.ResID = new SelectList(db.Residences, "ResID", "Res_Name");
            return View();
        }


        //Register Action 
        [HttpPost]
        public ActionResult Register(UserSignUpView USV)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    UserManager UM = new UserManager();
                    if (!UM.IsLoginNameExist(USV.LoginName))
                    {

                        USV.UserTypeID = 1;

                        UM.AddUserAccount(USV);

                        FormsAuthentication.SetAuthCookie(USV.FirstName, false);

                        SYSUser myUser = db.SYSUsers.FirstOrDefault(p => p.LoginName == USV.LoginName && p.PasswordEncryptedText == USV.Password);
                        SYSUserProfile myUserP = db.SYSUserProfiles.FirstOrDefault(p => p.SYSUserID == myUser.SYSUserID);
                        Session["User"] = myUserP.SYSUserProfileID;

                        TempData["nUse"] = myUserP.SYSUserProfileID;
                        return RedirectToAction("SecurityQuestion", "Account");

                    }
                    else

                        ModelState.AddModelError("", "Login Name already taken.");
                }
                return View(USV);
            }


            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Account", "Register"));
            }
        }

        //public FileContentResult GetImage(int sysuserID)
        //{
        //    SYSUserProfile prod = db.SYSUserProfiles.FirstOrDefault(p => p.SYSUserProfileID == sysuserID);
        //    if (prod != null)
        //    {
        //        return File(prod.ImageData, prod.ImageMimeType);
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        //Register Volunteer
        public ActionResult RegisterVol()
        {
            ViewBag.UserTypeID = new SelectList(db.UserTypes, "UserTypeID", "Description", "AccessRight");

            ViewBag.ResID = new SelectList(db.Residences, "ResID", "Res_Name");

            return View();
        }

        //Register Volunteer

        [HttpPost]
        public ActionResult RegisterVol(UserSignUpViewVol USV)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    UserManager UM = new UserManager();
                    if (!UM.IsLoginNameExist(USV.LoginName))
                    {
                        USV.UserTypeID = 2;
                        UM.AddUserAccount(USV);

                        //Adding hashing here
                        SYSUser myUser = db.SYSUsers.FirstOrDefault(p => p.LoginName == USV.LoginName && p.PasswordEncryptedText == USV.Password);


                        SYSUserProfile myUserP = db.SYSUserProfiles.FirstOrDefault(p => p.SYSUserID == myUser.SYSUserID);
                        Session["User"] = myUserP.SYSUserProfileID;

                        FormsAuthentication.SetAuthCookie(USV.FirstName, false);
                        return RedirectToAction("Index", "TRWLASchedules");

                    }
                    else
                        ModelState.AddModelError("", "Login Name already taken.");
                }
                return View();
            }
            catch (Exception ex)
            {
                return View("ErrorSign", new HandleErrorInfo(ex, "Account", "Register"));
            }

        }

        //Login/////
        public ActionResult Login()
        {
            return View();
        }

        //Login Post 
        [HttpPost]
        public ActionResult Login(UserLoginView ULV, string returnUrl)
        {

            try
            {
                try
                {
            if (ModelState.IsValid)
            {
                UserManager UM = new UserManager();
                string password = UM.GetUserPassword(ULV.LoginName);

                var username = from n in db.SYSUsers
                               where n.LoginName == ULV.LoginName && n.PasswordEncryptedText == password
                               select n;

                SYSUser myUser = db.SYSUsers.FirstOrDefault(p => p.LoginName == ULV.LoginName && p.PasswordEncryptedText == ULV.Password);
                SYSUserProfile myUserP = db.SYSUserProfiles.FirstOrDefault(p => p.SYSUserID == myUser.SYSUserID);


                if (string.IsNullOrEmpty(password))
                    ModelState.AddModelError("", "The user login or password provided is incorrect.");
                else
                {
                    if (ULV.Password.Equals(password))
                    {
                        FormsAuthentication.SetAuthCookie(ULV.LoginName, false);

                        if (Convert.ToInt32(myUserP.UserTypeID) == 1)
                        {
                            Session["User"] = myUserP.SYSUserProfileID;
                            return RedirectToAction("StudentMainMenu", "TRWLASchedules");
                        }
                        else if (Convert.ToInt32(myUserP.UserTypeID) == 2)
                        {
                            Session["User"] = myUserP.SYSUserProfileID;
                            return RedirectToAction("Index", "TRWLASchedules");
                        }


                    }
                    else
                    {
                        ModelState.AddModelError("", "The password provided is incorrect.");
                    }
                }
            }
            return View(ULV);

             }

                catch (Exception )
                {
                    TempData["notice"] = " Your Username or Password is incorrect";
                    return View(/*"ErrorLogIn", new HandleErrorInfo(ex, "Account", "Login")*/);
                }



               // If we got this far, something failed, redisplay form  

            }
             catch (System.OutOfMemoryException e)
            {
                return View("ErrorLogIn", new HandleErrorInfo(e, "Account", "Login"));
            }




        }


        public ActionResult ForgotPassword()
        {
            return View();
        }


        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(Models.ForgotPasswordViewModel model)
        {


            var email = from c in db.SYSUserProfiles
                        where c.Email == model.Email
                        select c;

            //TempData["user"] = email;
            //  var user = db.SYSUserProfiles.Where(O => O.Email.Equals(email));








            if (email.ToList().Count == 1)
            {
                SYSUserProfile myUser = db.SYSUserProfiles.FirstOrDefault(p => p.Email == model.Email);


                int ID = myUser.SYSUserProfileID;
                TempData["User"] = ID;
                return RedirectToAction("SecuirtyAnswer", "Account");
            }



            return View(email);
            // If we got this far, something failed, redisplay form

        }

        //Secuirty Answer get
        [AllowAnonymous]
        public ActionResult SecuirtyAnswer()
        {
            int user = Convert.ToInt32(TempData["User"]);

            //    var ans = db.SecurityAnswers.Include(t => t.Security_Question).Where(o => o.SYSUserProfileID == user);

            SecurityAnswer myanswer = db.SecurityAnswers.FirstOrDefault(p => p.SYSUserProfileID == user);


            //ViewBag.SecuirtyQuestion = db.SecurityAnswers.Select(secans.Security_Question).W
            //var user = db.SYSUserProfiles.Where(O => O.SYSUserProfileID.Equals(id));

            //var ans = from a in db.SecurityAnswers
            //          where a.SYSUserProfileID = 
            //SecurityAnswer secans = db.SecurityAnswers.Find(id);

            //ViewBag.SecurityAnswerID = new SelectList(db.SecurityAnswers, "SecurityAnswerID ", "Security_Question", "Security_Answer");

            // secans.Security_Question = Convert.ToString(ques);


            TempData["Question"]= myanswer.Security_Question;


            return View();

            //return View(ans);
        }


        ////Secuirty Answer Post
        [HttpPost]
        public ActionResult SecuirtyAnswer(string answer)
        {
            int user = Convert.ToInt32(TempData["User"]);

            //var ans = from c in db.SecurityAnswers
            //         where c.Security_Answer.Contains(answer) && c.SYSUserProfileID == user
            //         select c;


            //SecurityAnswer ans2 = db.SecurityAnswers.FirstOrDefault(p => p.SYSUserProfileID == user).Security_Answer.Equals(answer);

            

            var s = from n in db.SecurityAnswers
                    where n.Security_Answer == answer && n.SYSUserProfileID == user
                    select n.Security_Answer;

            


            if (s!= null)
            {

                       return RedirectToAction("ResetPassword", "Account");


                }


            return View(s);
        }





        // [HttpPost]
        public ActionResult SecurityQuestion()
        {
            int ID = Convert.ToInt32(TempData["nUse"]);
            ViewBag.SecurityAnswerID = new SelectList(db.SecurityAnswers, "SecurityAnswerID ", "Security_Question", "Security_Answer");
            // SecurityAnswer sans = db.SecurityAnswers

            return View();
        }

        [HttpPost]
        public ActionResult SecurityQuestion([Bind(Include = "SecurityAnswerID,Security_Question,Security_Answer,SYSUserProfileID]")] SecurityAnswer seans)
        {


            if (ModelState.IsValid)
            {
                db.SecurityAnswers.Add(seans);
                db.SaveChanges();
                return RedirectToAction("StudentMainMenu", "TRWLASchedules");
            }


            return View(seans);
        }

        //// GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string rt)
        {

            return View();
        }

        //// POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword()
        {
            if (ModelState.IsValid)
            {

            }

            return View();
        }



        //
        // GET: /Account/ForgotPasswordConfirmation
        //[AllowAnonymous]
        //public ActionResult ForgotPasswordConfirmation()
        //{
        //    return View();
        //}








        //Sign Out

        [Authorize]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }




        internal class ChallengeResult : ActionResult
        {
            private string provider;
            private string v1;
            private string v2;

            public ChallengeResult(string provider, string v1, string v2)
            {
                this.provider = provider;
                this.v1 = v1;
                this.v2 = v2;
            }

            public override void ExecuteResult(ControllerContext context)
            {
                throw new NotImplementedException();
            }
        }

        private object confirmationToken;

        public ActionResult CameraBooth()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PhotoBooth()
        {
            return View();
        }



        [HttpGet]
        public ActionResult Changephoto()
        {
            if (Convert.ToString(Session["val"]) != string.Empty)
            {
                ViewBag.pic = "http://localhost:26515/WebImages/" + Session["val"].ToString();
            }
            else
            {
                ViewBag.pic = "../../WebImages/person.jpg";
            }
            return View();
        }
        public JsonResult Rebind()
        {
            string path = "http://localhost:26515/WebImages/" + Session["val"].ToString();
            return Json(path, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Capture()
        {
            var stream = Request.InputStream;
            string dump;
            using (var reader = new StreamReader(stream))
            {
                dump = reader.ReadToEnd();
                DateTime nm = DateTime.Now;
                string date = nm.ToString("yyyymmddMMss");
                var path = Server.MapPath("~/WebImages/" + date + "test.jpg");
                System.IO.File.WriteAllBytes(path, String_To_Bytes2(dump));
                ViewData["path"] = date + "test.jpg";
                Session["val"] = date + "test.jpg";
            }
            return View("Index");
        }
        private byte[] String_To_Bytes2(string strInput)
        {
            int numBytes = (strInput.Length) / 2;
            byte[] bytes = new byte[numBytes];
            for (int x = 0; x < numBytes; ++x)
            {
                bytes[x] = Convert.ToByte(strInput.Substring(x * 2, 2), 16);
            }
            return bytes;
        }

    }






}