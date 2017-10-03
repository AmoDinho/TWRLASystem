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


       
        private TWRLADB_Staging_V2Entities db = new TWRLADB_Staging_V2Entities();
        //Register Student
        public ActionResult Register()
        {

            ViewBag.UserTypeID = new SelectList(db.UserTypes, "UserTypeID", "Description", "AccessRight");
         
            ViewBag.ResID = new SelectList(db.Residences, "ResID", "Res_Name");
            return View();
        }


        //Register Action 
        [HttpPost]
        public ActionResult Register(UserSignUpView USV,HttpPostedFileBase image)
        {

            try
            {
               if (ModelState.IsValid)
                {
                    UserManager UM = new UserManager();
                    if (!UM.IsLoginNameExist(USV.LoginName))
                    {
                        if(image !=null)
                        {
                            USV.ImageMimeType = image.ContentType;
                            USV.ImageData = new byte[image.ContentLength];
                            image.InputStream.Read(USV.ImageData, 0, image.ContentLength);

                            //var filename = image.FileName;
                            //var filePathOriginal = Server.MapPath("/Content");
                            //string savedFileName = Path.Combine(filePathOriginal);


                            var uploadDir = "~/Content";
                                var imagePath = Path.Combine(Server.MapPath(uploadDir));

                            image.SaveAs(imagePath);
                        }
                        USV.UserTypeID = 1;
                        
                        UM.AddUserAccount(USV);
                    
                        FormsAuthentication.SetAuthCookie(USV.FirstName, false);

                        SYSUser myUser = db.SYSUsers.FirstOrDefault(p => p.LoginName == USV.LoginName && p.PasswordEncryptedText == USV.Password);
                        SYSUserProfile myUserP = db.SYSUserProfiles.FirstOrDefault(p => p.SYSUserID == myUser.SYSUserID);
                        Session["User"] = myUserP.SYSUserProfileID;

                        return RedirectToAction("StudentMainMenu", "TRWLASchedules");

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

        public FileContentResult GetImage(int sysuserID)
        {
            SYSUserProfile prod = db.SYSUserProfiles.FirstOrDefault(p => p.SYSUserProfileID == sysuserID);
            if (prod != null)
            {
                return File(prod.ImageData, prod.ImageMimeType);
            }
            else
            {
                return null;
            }
        }

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

            //try
            //{
            //    try
            //    {
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

               // }
            //    catch (Exception ex)
            //    {
            //        return View("ErrorLogIn", new HandleErrorInfo(ex, "Account", "Register"));
            //    }



            //    // If we got this far, something failed, redisplay form  
               
            //}
            // catch (System.OutOfMemoryException e)
            //{
            //    return View("ErrorLogIn", new HandleErrorInfo(e, "Account", "Register"));
            //}


           

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


            if (email.ToList().Count == 1)
            {

                return RedirectToAction("SecuirtyAnswer", "Account");
            }



            return View(email);
            // If we got this far, something failed, redisplay form
           
        }

        //Secuirty Answer get
        [AllowAnonymous]
        public ActionResult SecuirtyAnswer(SecurityAnswer secans)
        {


            return View();
        }


        ////Secuirty Answer Post
        //[HttpPost]
        //public ActionResult SecuirtyAnswer()
        //{

        //    return View();
        //}

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