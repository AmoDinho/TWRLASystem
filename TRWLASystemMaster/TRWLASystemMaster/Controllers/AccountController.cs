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
using System.Net;

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
            try
            {
                ViewBag.UserTypeID = new SelectList(db.UserTypes, "UserTypeID", "Description", "AccessRight");
                ViewBag.ResID = new SelectList(db.Residences, "ResID", "Res_Name");
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("ErrorPage", "TRWLASchedules");
            }
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
                    if (UM.IsEmailExist(USV.Email) == "no")
                    {
                        if (UM.IsLoginNameExist(USV.LoginName) == "no")
                        {
                            //if (!UM.IsLoginNameExist(USV.LoginName))
                            //{

                            USV.UserTypeID = 1;

                            UM.AddUserAccount(USV);

                            FormsAuthentication.SetAuthCookie(USV.FirstName, false);

                            SYSUser myUser = db.SYSUsers.FirstOrDefault(p => p.LoginName == USV.LoginName && p.PasswordEncryptedText == USV.Password);
                            SYSUserProfile myUserP = db.SYSUserProfiles.FirstOrDefault(p => p.SYSUserID == myUser.SYSUserID);
                            Session["User"] = myUserP.SYSUserProfileID;

                            TempData["nUse"] = myUserP.SYSUserProfileID;
                            return RedirectToAction("SecurityQuestion", "Account");

                            //}
                            //else
                            //{
                            //    ViewBag.error = "Something went wrong with your registration";
                            //    return RedirectToAction("SelectVolStud", "Select");
                            //}
                        }
                        else
                        {
                            ViewBag.error = "Your username has been taken.";
                            return RedirectToAction("SelectVolStud", "Select");
                        }
                    }
                    else
                    {
                        ViewBag.error = "Your email already exists.";
                        return RedirectToAction("Login", "Account");
                    }
                   
                }
                return View(USV);
            }


            catch (Exception)
            {
                return RedirectToAction("ErrorPage", "TRWLASchedules");
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
            try
            {
                ViewBag.UserTypeID = new SelectList(db.UserTypes, "UserTypeID", "Description", "AccessRight");
                ViewBag.ResID = new SelectList(db.Residences, "ResID", "Res_Name");

                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("ErrorPage", "TRWLASchedules");
            }
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
                    if (UM.IsEmailExist(USV.Email) == "no")
                    {
                        if(UM.IsLoginNameExist(USV.LoginName) == "no")
                        {
                        //if (!UM.IsLoginNameExist(USV.LoginName))
                        //{
                            USV.UserTypeID = 2;
                            UM.AddUserAccount(USV);

                            FormsAuthentication.SetAuthCookie(USV.FirstName, false);
                            //Adding hashing here
                            SYSUser myUser = db.SYSUsers.FirstOrDefault(p => p.LoginName == USV.LoginName && p.PasswordEncryptedText == USV.Password);
                            SYSUserProfile myUserP = db.SYSUserProfiles.FirstOrDefault(p => p.SYSUserID == myUser.SYSUserID);


                            Session["User"] = myUserP.SYSUserProfileID;
                            TempData["nUse"] = myUserP.SYSUserProfileID;

                            return RedirectToAction("SecurityQuestion", "Account");

                            //}
                            //else
                            //{
                            //    ViewBag.error = "Something went wrong with your registration";
                            //    return RedirectToAction("SelectVolStud", "Select");
                            //}
                        }
                        else
                        {
                            ViewBag.error = "Your username has been taken.";
                            return RedirectToAction("SelectVolStud", "Select");
                        }
                    }
                    else
                    {
                        ViewBag.error = "Your email already exists.";
                        return RedirectToAction("Login", "Account");
                    }
                }
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("ErrorPage", "TRWLASchedules");
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
                catch (Exception)
                {
                    TempData["notice"] = "Your username or password is incorrect";
                    return View();
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
            try
            {
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("ErrorPage", "TRWLASchedules");
            }
        }


        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(Models.ForgotPasswordViewModel model)
        {

            try
            {
                var email = from c in db.SYSUserProfiles
                            where c.Email == model.Email
                            select c;

                //TempData["user"] = email;
                //  var user = db.SYSUserProfiles.Where(O => O.Email.Equals(email));

                int count = email.Count();

                if (count != 0)
                {
                    if (email.ToList().Count == 1)
                    {
                        SYSUserProfile myUser = db.SYSUserProfiles.FirstOrDefault(p => p.Email == model.Email);


                        int ID = myUser.SYSUserProfileID;
                        TempData["User"] = ID;
                        

                        SYSUser ures = db.SYSUsers.FirstOrDefault(c => c.SYSUserID == myUser.SYSUserID);

                        int ID2 = ures.SYSUserID;
                        TempData["User2"] = ID2;

                        return RedirectToAction("SecuirtyAnswer", "Account");
                    }



                    return View(email);
                }
                else
                {
                    TempData["Email"] = "This email does not exist.";
                    return View();
                }
                // If we got this far, something failed, redisplay form
            }
            catch (Exception)
            {
                return RedirectToAction("ErrorPage", "TRWLASchedules");
            }

        }

        //Secuirty Answer get
        [AllowAnonymous]
        public ActionResult SecuirtyAnswer()
        {
            try
            {
                int user = Convert.ToInt32(TempData["User"]);
                TempData["User"] = user;

                //    var ans = db.SecurityAnswers.Include(t => t.Security_Question).Where(o => o.SYSUserProfileID == user);

                SecurityAnswer myanswer = db.SecurityAnswers.FirstOrDefault(p => p.SYSUserProfileID == user);


                

                TempData["Question"] = myanswer.SecurityQuestion.Question;
                TempData["Carry"] = myanswer.SecurityQuestion.Question;

                return View();

                //return View(ans);
            }
            catch (Exception)
            {
                return RedirectToAction("ErrorPage", "TRWLASchedules");
            }
        }


        ////Secuirty Answer Post
        [HttpPost]
        public ActionResult SecuirtyAnswer(string answer)
        {
            try
            {
                int user = Convert.ToInt32(TempData["User"]);
                TempData["User"] = user;

                if (TempData["Carry"] != null)
                {
                    string question = TempData["Carry"].ToString();
                    TempData["Question"] = question;
                }
                
                //var ans = from c in db.SecurityAnswers
                //         where c.Security_Answer.Contains(answer) && c.SYSUserProfileID == user
                //         select c;


                //SecurityAnswer ans2 = db.SecurityAnswers.FirstOrDefault(p => p.SYSUserProfileID == user).Security_Answer.Equals(answer);

                int s = 0;
                try
                {
                    s = (from n in db.SecurityAnswers
                            where n.SYSUserProfileID == user
                            select n.Security_Answer).Count();
                }
                catch
                {
                    ViewBag.Err = "Please enter an answer to your security question";
                    TempData["User"] = user;
                }

                
                if (s != 0)
                {

                    SecurityAnswer ans = db.SecurityAnswers.FirstOrDefault(p => p.SYSUserProfileID == user);

                    if (ans.Security_Answer == answer)
                    {
                        return RedirectToAction("Edit", "Account");
                    }
                    else
                    {
                        TempData["User"] = user;
                        return View();
                    }
                }
                else
                {
                    //    var ans = db.SecurityAnswers.Include(t => t.Security_Question).Where(o => o.SYSUserProfileID == user);

                    SecurityAnswer myanswer = db.SecurityAnswers.FirstOrDefault(p => p.SYSUserProfileID == user);
                    TempData["User"] = user;
                    TempData["Question"] = myanswer.SecurityQuestion.Question;
                    ViewBag.Error = "The answer to your question is wrong";
                    return View();
                }
            }
            catch (Exception)
            {
                return RedirectToAction("ErrorPage", "TRWLASchedules");
            }
        }



        public ActionResult Edit(int? id)
        {
            try
            {
                id = Convert.ToInt32(TempData["User2"]);
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                SYSUser sYSUser = db.SYSUsers.Find(id);
                if (sYSUser == null)
                {
                    return HttpNotFound();
                }
                return View(sYSUser);
            }
            catch (Exception)
            {
                return RedirectToAction("ErrorPage", "TRWLASchedules");
            }
        }

        // POST: SYSUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SYSUserID,LoginName,PasswordEncryptedText,RowCreatedSYSUserID,RowCreatedDateTime,RowModifiedSYSUserID,RowModifiedDateTime")] SYSUser sYSUser)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(sYSUser).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Login", "Account");
                }
                return View(sYSUser);
            }
            catch (Exception)
            {
                return RedirectToAction("ErrorPage", "TRWLASchedules");
            }
        }

        // [HttpPost]
        public ActionResult SecurityQuestion()
        {
            try
            {
                ViewBag.QuestionID = new SelectList(db.SecurityQuestions, "QuestionID", "Question");


                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("ErrorPage", "TRWLASchedules");
            }
        }

        [HttpPost]
        public ActionResult SecurityQuestion([Bind(Include = "SecurityAnswerID,QuestionID,Security_Answer,SYSUserProfileID]")] SecurityAnswer seans)
        {
            try
            {
                int ID = Convert.ToInt32(TempData["nUse"]);
                if (ModelState.IsValid)
                {
                    seans.SYSUserProfileID = ID;
                    db.SecurityAnswers.Add(seans);
                    db.SaveChanges();

                    SYSUserProfile myuser = db.SYSUserProfiles.Find(ID);

                    if (myuser.UserTypeID == 1)
                    {

                        return RedirectToAction("StudentMainMenu", "TRWLASchedules");
                    }
                    else if (myuser.UserTypeID == 2)
                    {
                        return RedirectToAction("index", "TRWLASchedules");
                    }


                }


                return View(seans);
            }
            catch (Exception)
            {
                return RedirectToAction("ErrorPage", "TRWLASchedules");
            }
        }

        //// GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string rt)
        {
            try {
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("ErrorPage", "TRWLASchedules");
            }
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
            try
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                return RedirectToAction("ErrorPage", "TRWLASchedules");
            }
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