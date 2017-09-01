using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TRWLASystemMaster.Models;
using System.Web.Helpers;
using System.Net.Mail;
using System.Text;
using System.IO;
using System.Drawing;
using ClosedXML.Excel;
using System.Web.UI;
using System.Web.UI.WebControls;
using TRWLASystemMaster.Models.DB;
using System.Collections;

namespace TRWLASystemMaster.Controllers
{
    public class TRWLASchedulesController : Controller
    {
        private TWRLADB_Staging_V2Entities19 db = new TWRLADB_Staging_V2Entities19();

        public ActionResult StudentMainMenu(string sortOrder, string searchString, string F, string CO, string L, string all)
        {


            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.SurnameSortParm = String.IsNullOrEmpty(sortOrder) ? "sur_desc" : "Surname";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            var tRWLASchedules = db.TRWLASchedules.Include(t => t.ComEngEvent).Include(t => t.FunctionEvent).Include(t => t.Lecture);


            if (!String.IsNullOrEmpty(searchString))
            {
                tRWLASchedules = tRWLASchedules.Where(s => s.FunctionEvent.Function_Name.Contains(searchString)
                        || s.Lecture.Lecture_Name.Contains(searchString)
                        || s.ComEngEvent.ComEng_Name.Contains(searchString));
            }



            if (!String.IsNullOrEmpty(F))
            {
                tRWLASchedules = tRWLASchedules.Where(s => s.FunctionEvent.Function_Name.Contains("(F)"));
            }

            if (!String.IsNullOrEmpty(CO))
            {
                tRWLASchedules = tRWLASchedules.Where(s => s.ComEngEvent.ComEng_Name.Contains("(CE)"));
            }

            if (!String.IsNullOrEmpty(L))
            {
                tRWLASchedules = tRWLASchedules.Where(s => s.Lecture.Lecture_Name.Contains("(L)"));
            }

            if (!String.IsNullOrEmpty(all))
            {
                tRWLASchedules = tRWLASchedules.Where(s => s.Lecture.Lecture_Name.Contains("(L)")
                        || s.ComEngEvent.ComEng_Name.Contains("(CE)")
                        || s.FunctionEvent.Function_Name.Contains("(F)"));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    tRWLASchedules = tRWLASchedules.OrderByDescending(s => s.FunctionEvent.Function_Name.Contains(searchString)
                        || s.Lecture.Lecture_Name.Contains(searchString)
                        || s.ComEngEvent.ComEng_Name.Contains(searchString));
                    break;
                case "sur_desc":
                    tRWLASchedules = tRWLASchedules.OrderByDescending(s => s.FunctionEvent.Function_Name.Contains(searchString)
                        || s.Lecture.Lecture_Name.Contains(searchString)
                        || s.ComEngEvent.ComEng_Name.Contains(searchString));
                    break;
                case "Surname":
                    tRWLASchedules = tRWLASchedules.OrderByDescending(s => s.FunctionEvent.Function_Name.Contains(searchString)
                        || s.Lecture.Lecture_Name.Contains(searchString)
                        || s.ComEngEvent.ComEng_Name.Contains(searchString));
                    break;
                default:
                    tRWLASchedules = tRWLASchedules.OrderBy(s => s.FunctionEvent.Function_Name.Contains(searchString)
                        || s.Lecture.Lecture_Name.Contains(searchString)
                        || s.ComEngEvent.ComEng_Name.Contains(searchString));
                    break;
            }

            tRWLASchedules = tRWLASchedules.Where(p => p.Lecture.Lecture_Date >= DateTime.Now || p.FunctionEvent.Function_Date >= DateTime.Now || p.ComEngEvent.ComEng_Date >= DateTime.Now);

            return View(tRWLASchedules.ToList());
        }
        // GET: TRWLASchedules
        [AllowAnonymous]
        public ActionResult Index(string sortOrder, string searchString, string F, string CO, string L, string all)
        {
            

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.SurnameSortParm = String.IsNullOrEmpty(sortOrder) ? "sur_desc" : "Surname";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            var tRWLASchedules = db.TRWLASchedules.Include(t => t.ComEngEvent).Include(t => t.FunctionEvent).Include(t => t.Lecture);


            if (!String.IsNullOrEmpty(searchString))
            {
                tRWLASchedules = tRWLASchedules.Where(s => s.FunctionEvent.Function_Name.Contains(searchString)
                        || s.Lecture.Lecture_Name.Contains(searchString)
                        || s.ComEngEvent.ComEng_Name.Contains(searchString));
            }



            if (!String.IsNullOrEmpty(F))
            {
                tRWLASchedules = tRWLASchedules.Where(s => s.FunctionEvent.Function_Name.Contains("(F)"));
            }

            if (!String.IsNullOrEmpty(CO))
            {
                tRWLASchedules = tRWLASchedules.Where(s => s.ComEngEvent.ComEng_Name.Contains("(CE)"));
            }

            if (!String.IsNullOrEmpty(L))
            {
                tRWLASchedules = tRWLASchedules.Where(s => s.Lecture.Lecture_Name.Contains("(L)"));
            }

            if (!String.IsNullOrEmpty(all))
            {
                tRWLASchedules = tRWLASchedules.Where(s => s.Lecture.Lecture_Name.Contains("(L)")
                        || s.ComEngEvent.ComEng_Name.Contains("(CE)")
                        || s.FunctionEvent.Function_Name.Contains("(F)"));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    tRWLASchedules = tRWLASchedules.OrderByDescending(s => s.FunctionEvent.Function_Name.Contains(searchString)
                        || s.Lecture.Lecture_Name.Contains(searchString)
                        || s.ComEngEvent.ComEng_Name.Contains(searchString));
                    break;
                case "sur_desc":
                    tRWLASchedules = tRWLASchedules.OrderByDescending(s => s.FunctionEvent.Function_Name.Contains(searchString)
                        || s.Lecture.Lecture_Name.Contains(searchString)
                        || s.ComEngEvent.ComEng_Name.Contains(searchString));
                    break;
                case "Surname":
                    tRWLASchedules = tRWLASchedules.OrderByDescending(s => s.FunctionEvent.Function_Name.Contains(searchString)
                        || s.Lecture.Lecture_Name.Contains(searchString)
                        || s.ComEngEvent.ComEng_Name.Contains(searchString));
                    break;
                default:
                    tRWLASchedules = tRWLASchedules.OrderBy(s => s.FunctionEvent.Function_Name.Contains(searchString)
                        || s.Lecture.Lecture_Name.Contains(searchString)
                        || s.ComEngEvent.ComEng_Name.Contains(searchString));
                    break;
            }

            tRWLASchedules = tRWLASchedules.Where(p => p.Lecture.Lecture_Date >= DateTime.Now || p.FunctionEvent.Function_Date >= DateTime.Now || p.ComEngEvent.ComEng_Date >= DateTime.Now);

            return View(tRWLASchedules.ToList());
        }

        //Begginning of Report Generation 

        public IList<ClassAttendance> GetClassAttendance()
        {
            var at = (from l in db.RSVP_Event
                      join s in db.SYSUserProfiles on l.SYSUserProfileID equals s.SYSUserProfileID
                      select new ClassAttendance
                      {
                          Name = s.FirstName,
                          Surname = s.LastName,
                          EventCount = db.RSVP_Event.Count(),
                          StudentID = s.SYSUserProfileID,
                          PersonalCount = (db.RSVP_Event.Distinct().Where(p => p.SYSUserProfileID == s.SYSUserProfileID).Count()) / (db.RSVP_Event.Count()) * 100
                      }).ToList();

            return at;
        }

        public IList<Demographic> GetDemo()
        {
            var at = (from l in db.SYSUserProfiles
                      join r in db.Residences on l.ResID equals r.ResID
                      select new Demographic
                      {
                          StudNo = l.StudentNumber,
                          Name = l.FirstName,
                          Surname = l.LastName,
                          DoB = l.DoB,
                          Degree = l.Degree,
                          Res = r.Res_Name
                      }).ToList();

            return at;
        }

        public IList<AttendanceViewModel> GetLectureAttendance()
        {
            var at = (from l in db.Attendances
                      where l.LectureID != null
                      select new AttendanceViewModel
                      {
                          EventName = l.Lecture.Lecture_Name,
                          EventDate = l.Lecture.Lecture_Date,
                          Student_Name = l.SYSUserProfile.FirstName
                      }).ToList();
            return at;
        }
        public IList<AttendanceViewModel> GetFunctionAttendance()
        {
            var attendance = (from s in db.Attendances
                              where s.FunctionID != null
                              select new AttendanceViewModel
                              {
                                  EventName = s.FunctionEvent.Function_Name,
                                  EventDate = s.FunctionEvent.Function_Date,
                                  Student_Name = s.SYSUserProfile.FirstName
                              }).ToList();
            return attendance;
        }



        public IList<AttendanceViewModel> GetComAttendance()
        {
            var attend = (from m in db.Attendances
                          where m.ComEngID != null
                          select new AttendanceViewModel
                          {
                              EventName = m.ComEngEvent.ComEng_Name,
                              EventDate = m.ComEngEvent.ComEng_Date,
                              Student_Name = m.SYSUserProfile.FirstName
                          }).ToList();
            return attend;
        }

        public IList<NoAttend> GetNoAttendance()
        {
            var attend = (from m in db.RSVP_Event
                          join s in db.SYSUserProfiles on m.SYSUserProfileID equals s.SYSUserProfileID
                          where m.Attended == null
                          select new NoAttend
                          {
                              StudNo = s.StudentNumber,
                              Name = s.FirstName,
                              Surname = s.LastName,
                              DoB = s.DoB,
                              Degree = s.Degree,
                              Res = s.Residence.Res_Name,
                              FunctionDate = m.FunctionEvent.Function_Date,
                              LectureDate = m.Lecture.Lecture_Date,
                              ComEngDate = m.ComEngEvent.ComEng_Date
                          }).ToList();

            return attend;
        }

        //Generate the Excel Spreadsheets for the data
        public ActionResult ExportToExcel1()
        {
            var gv = new GridView();
            gv.DataSource = this.GetFunctionAttendance();
            gv.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            string name = "Function Attendance Report " + Convert.ToString(DateTime.Now);
            Response.AddHeader("content-disposition", "attachment; filename=" + name + ".xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);

            gv.AllowPaging = false;

            //   Create a form to contain the grid
            Table table = new Table();
            TableRow title = new TableRow();
            title.BackColor = Color.Cyan;
            TableCell titlecell = new TableCell();
            titlecell.ColumnSpan = 3;//Should span across all columns
            Label lbl = new Label();
            lbl.Text = "Function Attendance Report " + Convert.ToString(DateTime.Now);
            titlecell.Controls.Add(lbl);
            title.Cells.Add(titlecell);
            table.Rows.Add(title);

            table.GridLines = gv.GridLines;

            TableCell cell = new TableCell();
            cell.Text = gv.Caption;
            cell.ColumnSpan = 10;
            TableRow tr = new TableRow();
            tr.Controls.Add(cell);
            table.Rows.Add(tr);

            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
            return View("Index");
        }

        public ActionResult ExportToExcel2()
        {
            var gv = new GridView();
            gv.DataSource = this.GetLectureAttendance();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            string name = "Lecture Attendance Report " + Convert.ToString(DateTime.Now);
            Response.AddHeader("content-disposition", "attachment; filename=" + name + ".xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
            return View("Index");
        }

        public ActionResult ExportToExcel3()
        {
            var gv = new GridView();
            gv.DataSource = this.GetComAttendance();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            string name = "Community Engagement Attendance Report " + Convert.ToString(DateTime.Now);
            Response.AddHeader("content-disposition", "attachment; filename=" + name + ".xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
            return View("Index");
        }

        public ActionResult ExportToExcel4()
        {
            var gv = new GridView();
            gv.DataSource = this.GetDemo();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            string name = "Demographic Report " + Convert.ToString(DateTime.Now);
            Response.AddHeader("content-disposition", "attachment; filename=" + name + ".xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
            return View("Index");
        }

        public ActionResult ExportToExcel5()
        {
            var gv = new GridView();
            gv.DataSource = this.GetNoAttendance();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            string name = "No Attendance Report " + Convert.ToString(DateTime.Now);
            Response.AddHeader("content-disposition", "attachment; filename=" + name + ".xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
            return View("Index");
        }

        public ActionResult StudentDemographic()
        {
            return View(this.GetDemo());
        }

        public ActionResult NoAttend()
        {
            return View(this.GetNoAttendance());
        }

        public ActionResult ClassAttendance()
        {
            var _contenxt = new TWRLADB_Staging_V2Entities19();
            ArrayList xValue = new ArrayList();
            ArrayList yValue = new ArrayList();

            var results = (from c in _contenxt.Progresses select c);
            results.ToList().ForEach(rs => xValue.Add(rs.SYSUserProfile.StudentNumber));
            results.ToList().ForEach(rs => yValue.Add(rs.ProgressCount));


            new Chart(width: 600, height: 400, theme: ChartTheme.Blue)
                .AddTitle("Class Attendance for Students")
                .AddSeries("Default", chartType: "Column", xValue: xValue, yValues: yValue)
                .Write("jpeg");

            return View();
        }

        public ActionResult FunctionAttendance()
        {
            return View(this.GetFunctionAttendance());
        }

        public ActionResult ComAttendance()
        {
            return View(this.GetComAttendance());
        }

        public ActionResult LecAttendance()
        {
            return View(this.GetLectureAttendance());
        }

        public ActionResult SelectRecip()
        {
            var rsvp = from s in db.RSVP_Event
                       select s; 

            return View(rsvp.ToList());
        }

        public ActionResult SendNotification(int? id)
        {
            RSVP_Event rsvp = db.RSVP_Event.Find(id);

            if (rsvp.FunctionID != null)
            {
                ViewBag.Name = rsvp.FunctionEvent.Function_Name;
            }
            else if (rsvp.LectureID != null)
            {
                ViewBag.Name = rsvp.Lecture.Lecture_Name;
            }
            else if (rsvp.ComEngID != null)
            {
                ViewBag.Name = rsvp.ComEngEvent.ComEng_Name;
            }

            return View();
        }

        [HttpPost, ActionName("SendNotification")]
        [ValidateAntiForgeryToken]
        public ActionResult SendNotificationConfirmed(EventMessage mess,int id)
        {
            int i = db.EventMessages.Count();
            RSVP_Event rsvp = db.RSVP_Event.Find(id);
            var select = from s in db.RSVP_Event
                         select s;


            if (i == 0)
            {
                if (rsvp.FunctionID != null)
                {
                    foreach (var s in select.Where(p => p.FunctionID == rsvp.FunctionID))
                    {
                        mess.SYSUserProfileID = Convert.ToInt32(s.SYSUserProfileID);
                        mess.TimeMes = DateTime.Now.TimeOfDay;



                        try
                        {
                            int k = Convert.ToInt32(rsvp.SYSUserProfileID);

                            SYSUserProfile myStu = db.SYSUserProfiles.Find(k);
                            MailMessage msg = new MailMessage();
                            msg.From = new MailAddress("u15213626@tuks.co.za");
                            msg.To.Add(myStu.Email);
                            msg.Subject = rsvp.FunctionEvent.Function_Name + " Notification";
                            msg.Body = "Dear " + myStu.FirstName + "\n\n " + mess.Msg;

                            SmtpClient smtp = new SmtpClient();

                            smtp.Host = "smtp.gmail.com";
                            smtp.Port = 587;
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = new System.Net.NetworkCredential("u15213626@tuks.co.za", "Rootsms4");
                            smtp.EnableSsl = true;
                            smtp.Send(msg);

                            ModelState.Clear();


                            ViewBag.Status = "Email Sent Successfully.";
                        }
                        catch (Exception)
                        {
                            ViewBag.Status = "Problem while sending email, Please check details.";
                        }

                        

                        db.EventMessages.Add(mess);
                    }
                }
                else if (rsvp.LectureID != null)
                {
                    foreach (var s in select.Where(p => p.LectureID == rsvp.LectureID))
                    {
                        mess.SYSUserProfileID = Convert.ToInt32(s.SYSUserProfileID);
                        try
                        {
                            int k = Convert.ToInt32(rsvp.SYSUserProfileID);
                            mess.TimeMes = DateTime.Now.TimeOfDay;

                            SYSUserProfile myStu = db.SYSUserProfiles.Find(k);
                            MailMessage msg = new MailMessage();
                            msg.From = new MailAddress("u15213626@tuks.co.za");
                            msg.To.Add(myStu.Email);
                            msg.Subject = rsvp.Lecture.Lecture_Name + " Notification";
                            msg.Body = "Dear " + myStu.FirstName + "\n\n " + mess.Msg;

                            SmtpClient smtp = new SmtpClient();

                            smtp.Host = "smtp.gmail.com";
                            smtp.Port = 587;
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = new System.Net.NetworkCredential("u15213626@tuks.co.za", "Rootsms4");
                            smtp.EnableSsl = true;
                            smtp.Send(msg);

                            ModelState.Clear();


                            ViewBag.Status = "Email Sent Successfully.";
                        }
                        catch (Exception)
                        {
                            ViewBag.Status = "Problem while sending email, Please check details.";
                        }
                        db.EventMessages.Add(mess);
                    }
                }
                else if (rsvp.ComEngID != null)
                {
                    foreach (var s in select.Where(p => p.ComEngID == rsvp.ComEngID))
                    {
                        mess.SYSUserProfileID = Convert.ToInt32(s.SYSUserProfileID);
                        try
                        {
                            int k = Convert.ToInt32(rsvp.SYSUserProfileID);
                            mess.TimeMes = DateTime.Now.TimeOfDay;

                            SYSUserProfile myStu = db.SYSUserProfiles.Find(k);
                            MailMessage msg = new MailMessage();
                            msg.From = new MailAddress("u15213626@tuks.co.za");
                            msg.To.Add(myStu.Email);
                            msg.Subject = rsvp.ComEngEvent.ComEng_Name + " Notification";
                            msg.Body = "Dear " + myStu.FirstName + "\n\n " + mess.Msg;

                            SmtpClient smtp = new SmtpClient();

                            smtp.Host = "smtp.gmail.com";
                            smtp.Port = 587;
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = new System.Net.NetworkCredential("u15213626@tuks.co.za", "Rootsms4");
                            smtp.EnableSsl = true;
                            smtp.Send(msg);

                            ModelState.Clear();


                            ViewBag.Status = "Email Sent Successfully.";
                        }
                        catch (Exception)
                        {
                            ViewBag.Status = "Problem while sending email, Please check details.";
                        }
                        db.EventMessages.Add(mess);
                    }
                }
            }
            else
            {
                int max = db.EventMessages.Max(p => p.MessID);
                int l = max + 1;

                mess.MessID = l;

                if (rsvp.FunctionID != null)
                {
                    foreach (var s in select.Where(p => p.FunctionID == rsvp.FunctionID))
                    {
                        mess.SYSUserProfileID = Convert.ToInt32(s.SYSUserProfileID);
                        try
                        {
                            int k = Convert.ToInt32(rsvp.SYSUserProfileID);
                            mess.TimeMes = DateTime.Now.TimeOfDay;

                            SYSUserProfile myStu = db.SYSUserProfiles.Find(k);
                            MailMessage msg = new MailMessage();
                            msg.From = new MailAddress("u15213626@tuks.co.za");
                            msg.To.Add(myStu.Email);
                            msg.Subject = rsvp.FunctionEvent.Function_Name + " Notification";
                            msg.Body = "Dear " + myStu.FirstName + "\n\n " + mess.Msg;

                            SmtpClient smtp = new SmtpClient();

                            smtp.Host = "smtp.gmail.com";
                            smtp.Port = 587;
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = new System.Net.NetworkCredential("u15213626@tuks.co.za", "Rootsms4");
                            smtp.EnableSsl = true;
                            smtp.Send(msg);

                            ModelState.Clear();


                            ViewBag.Status = "Email Sent Successfully.";
                        }
                        catch (Exception)
                        {
                            ViewBag.Status = "Problem while sending email, Please check details.";
                        }
                        db.EventMessages.Add(mess);
                    }
                }
                else if (rsvp.LectureID != null)
                {
                    foreach (var s in select.Where(p => p.LectureID == rsvp.LectureID))
                    {
                        mess.SYSUserProfileID = Convert.ToInt32(s.SYSUserProfileID);
                        try
                        {
                            int k = Convert.ToInt32(rsvp.SYSUserProfileID);
                            mess.TimeMes = DateTime.Now.TimeOfDay;

                            SYSUserProfile myStu = db.SYSUserProfiles.Find(k);
                            MailMessage msg = new MailMessage();
                            msg.From = new MailAddress("u15213626@tuks.co.za");
                            msg.To.Add(myStu.Email);
                            msg.Subject = rsvp.Lecture.Lecture_Name + " Notification";
                            msg.Body = "Dear " + myStu.FirstName + "\n\n " + mess.Msg;

                            SmtpClient smtp = new SmtpClient();

                            smtp.Host = "smtp.gmail.com";
                            smtp.Port = 587;
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = new System.Net.NetworkCredential("u15213626@tuks.co.za", "Rootsms4");
                            smtp.EnableSsl = true;
                            smtp.Send(msg);

                            ModelState.Clear();


                            ViewBag.Status = "Email Sent Successfully.";
                        }
                        catch (Exception)
                        {
                            ViewBag.Status = "Problem while sending email, Please check details.";
                        }
                        db.EventMessages.Add(mess);
                    }
                }
                else if (rsvp.ComEngID != null)
                {
                    foreach (var s in select.Where(p => p.ComEngID == rsvp.ComEngID))
                    {
                        mess.SYSUserProfileID = Convert.ToInt32(s.SYSUserProfileID);
                        try
                        {
                            int k = Convert.ToInt32(rsvp.SYSUserProfileID);
                            mess.TimeMes = DateTime.Now.TimeOfDay;

                            SYSUserProfile myStu = db.SYSUserProfiles.Find(k);
                            MailMessage msg = new MailMessage();
                            msg.From = new MailAddress("u15213626@tuks.co.za");
                            msg.To.Add(myStu.Email);
                            msg.Subject = rsvp.ComEngEvent.ComEng_Name + " Notification";
                            msg.Body = "Dear " + myStu.FirstName + "\n\n " + mess.Msg;

                            SmtpClient smtp = new SmtpClient();

                            smtp.Host = "smtp.gmail.com";
                            smtp.Port = 587;
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = new System.Net.NetworkCredential("u15213626@tuks.co.za", "Rootsms4");
                            smtp.EnableSsl = true;
                            smtp.Send(msg);

                            ModelState.Clear();


                            ViewBag.Status = "Email Sent Successfully.";
                        }
                        catch (Exception)
                        {
                            ViewBag.Status = "Problem while sending email, Please check details.";
                        }
                        db.EventMessages.Add(mess);
                    }
                }



            }

            db.SaveChanges();
            return RedirectToAction("Index", "TRWLASchedules");

        }

        public ActionResult ViewNotification()
        {
            var user = (int)Session["User"];
            var mes = from n in db.EventMessages
                      where n.SYSUserProfileID == user
                      select n;

            //Timer myTime = new Timer();
            //myTime.Enabled = true;

            //if (myTime <= DateTime.Now.AddSeconds(25))
            //{

            //}

            return View(mes.ToList());


        }

        public ActionResult LogAttendance()
        {
            var trwla = from s in db.TRWLASchedules
                        select s;

            trwla = trwla.Where(p => p.ComEngEvent.ComEng_Date >= DateTime.Now || p.FunctionEvent.Function_Date >= DateTime.Now || p.Lecture.Lecture_Date >= DateTime.Now);

            return View(trwla.ToList());
        }

        public ActionResult LogAttendancePast()
        {
            var trwla = from s in db.TRWLASchedules
                        select s;

            trwla = trwla.Where(p => p.ComEngEvent.ComEng_Date < DateTime.Now || p.FunctionEvent.Function_Date < DateTime.Now || p.Lecture.Lecture_Date < DateTime.Now);

            return View(trwla.ToList());
        }

        public ActionResult AddNewStudent(string sortOrder, string searchString)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.SurnameSortParm = String.IsNullOrEmpty(sortOrder) ? "sur_desc" : "Surname";

            var stud = from s in db.SYSUserProfiles
                       join r in db.RSVP_Event on s.SYSUserProfileID equals r.SYSUserProfileID
                       where s.SYSUserProfileID != r.SYSUserProfileID
                       select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                stud = stud.Where(s => s.FirstName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    stud = stud.OrderByDescending(s => s.FirstName.Contains(searchString));
                    break;
                case "sur_desc":
                    stud = stud.OrderByDescending(s => s.FirstName.Contains(searchString));
                    break;
                case "Surname":
                    stud = stud.OrderByDescending(s => s.FirstName.Contains(searchString));
                    break;
                default:
                    stud = stud.OrderByDescending(s => s.FirstName.Contains(searchString));
                    break;
            }

            return View(stud.ToList());
        }

        public ActionResult StudentDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SYSUserProfile student = db.SYSUserProfiles.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            
            return View(student);
        }

        [HttpPost, ActionName("StudentDetails")]
        [ValidateAntiForgeryToken]
        public ActionResult AttendanceConfirmed(Attendance att, int id)
        {

            int stude = Convert.ToInt32(TempData["NewStudent"]);

            SYSUserProfile stud = db.SYSUserProfiles.Find(id);
            RSVP_Event ev = db.RSVP_Event.Find(stude);


            int i = db.Attendances.Count();

            try
            {
                if (i == 0)
                {
                    if (ev.FunctionID != null)
                    {
                        att.FunctionID = ev.FunctionID;
                        att.SYSUserProfileID = stud.SYSUserProfileID;
                        db.Attendances.Add(att);
                    }
                    else if (ev.LectureID != null)
                    {
                        att.LectureID = ev.LectureID;
                        att.SYSUserProfileID = stud.SYSUserProfileID;
                        db.Attendances.Add(att);
                    }
                    else
                    {
                        att.ComEngID = ev.ComEngID;
                        att.SYSUserProfileID = stud.SYSUserProfileID;
                        db.Attendances.Add(att);
                    }

                }
                else
                {
                    int max = db.Attendances.Max(p => p.attendanceID);
                    int l = max + 1;

                    att.attendanceID = l;

                    if (ev.FunctionID != null)
                    {
                        att.FunctionID = ev.FunctionID;
                        att.SYSUserProfileID = stud.SYSUserProfileID;
                        db.Attendances.Add(att);
                    }
                    else if (ev.LectureID != null)
                    {
                        att.LectureID = ev.LectureID;
                        att.SYSUserProfileID = stud.SYSUserProfileID;
                        db.Attendances.Add(att);
                    }
                    else
                    {
                        att.ComEngID = ev.ComEngID;
                        att.SYSUserProfileID = stud.SYSUserProfileID;
                        db.Attendances.Add(att);
                    }
                }
                db.SaveChanges();

                TempData["Attend"] = "You have successfully logged the event attendance of: " + stud.FirstName;
            }
            catch (Exception)
            {
                TempData["Attend"] = "There was an error in the attempt of logging the attendance of the student";
            }
            return RedirectToAction("LogAttendance");
        }

        public ActionResult RSVPdMembers(int? id, string sortOrder, string searchString)
        {
            TRWLASchedule tRWLASchedule = db.TRWLASchedules.Find(id);


            

            TempData["NewStudent"] = id;

            if (tRWLASchedule.FunctionID != null)
            {
                var evnt = from s in db.SYSUserProfiles
                           join r in db.RSVP_Event on s.SYSUserProfileID equals r.SYSUserProfileID
                           join t in db.TRWLASchedules on r.FunctionID equals t.FunctionID
                           where r.FunctionID == tRWLASchedule.FunctionID
                           select r;

                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.SurnameSortParm = String.IsNullOrEmpty(sortOrder) ? "sur_desc" : "Surname";

                if (!String.IsNullOrEmpty(searchString))
                {
                    evnt = evnt.Where(s => s.SYSUserProfile.FirstName.Contains(searchString));
                }

                switch (sortOrder)
                {
                    case "name_desc":
                        evnt = evnt.OrderByDescending(s => s.SYSUserProfile.FirstName.Contains(searchString));
                        break;
                    case "sur_desc":
                        evnt = evnt.OrderByDescending(s => s.SYSUserProfile.FirstName.Contains(searchString));
                        break;
                    case "Surname":
                        evnt = evnt.OrderByDescending(s => s.SYSUserProfile.FirstName.Contains(searchString));
                        break;
                    default:
                        evnt = evnt.OrderByDescending(s => s.SYSUserProfile.FirstName.Contains(searchString));
                        break;
                }


                return View(evnt.ToList());
            }
            else if (tRWLASchedule.LectureID != null)
            {
                var evnt = from s in db.SYSUserProfiles
                           join r in db.RSVP_Event on s.SYSUserProfileID equals r.SYSUserProfileID
                           join t in db.TRWLASchedules on r.LectureID equals t.LectureID
                           where r.LectureID == tRWLASchedule.LectureID
                           select r;
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.SurnameSortParm = String.IsNullOrEmpty(sortOrder) ? "sur_desc" : "Surname";

                if (!String.IsNullOrEmpty(searchString))
                {
                    evnt = evnt.Where(s => s.SYSUserProfile.FirstName.Contains(searchString));
                }

                switch (sortOrder)
                {
                    case "name_desc":
                        evnt = evnt.OrderByDescending(s => s.SYSUserProfile.FirstName.Contains(searchString));
                        break;
                    case "sur_desc":
                        evnt = evnt.OrderByDescending(s => s.SYSUserProfile.FirstName.Contains(searchString));
                        break;
                    case "Surname":
                        evnt = evnt.OrderByDescending(s => s.SYSUserProfile.FirstName.Contains(searchString));
                        break;
                    default:
                        evnt = evnt.OrderByDescending(s => s.SYSUserProfile.FirstName.Contains(searchString));
                        break;
                }


                return View(evnt.ToList());
            }
            else
            {
                var evnt = from s in db.SYSUserProfiles
                           join r in db.RSVP_Event on s.SYSUserProfileID equals r.SYSUserProfileID
                           join t in db.TRWLASchedules on r.ComEngID equals t.ComEngID
                           where r.ComEngID == tRWLASchedule.ComEngID
                           select r;
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.SurnameSortParm = String.IsNullOrEmpty(sortOrder) ? "sur_desc" : "Surname";

                if (!String.IsNullOrEmpty(searchString))
                {
                    evnt = evnt.Where(s => s.SYSUserProfile.FirstName.Contains(searchString));
                }

                switch (sortOrder)
                {
                    case "name_desc":
                        evnt = evnt.OrderByDescending(s => s.SYSUserProfile.FirstName.Contains(searchString));
                        break;
                    case "sur_desc":
                        evnt = evnt.OrderByDescending(s => s.SYSUserProfile.FirstName.Contains(searchString));
                        break;
                    case "Surname":
                        evnt = evnt.OrderByDescending(s => s.SYSUserProfile.FirstName.Contains(searchString));
                        break;
                    default:
                        evnt = evnt.OrderByDescending(s => s.SYSUserProfile.FirstName.Contains(searchString));
                        break;
                }


                return View(evnt.ToList());
            }
        }

        public ActionResult ConfirmAttendance(int? studid, int? evid)
        {
            if (studid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SYSUserProfile student = db.SYSUserProfiles.Find(studid);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        [HttpPost, ActionName("ConfirmAttendance")]
        [ValidateAntiForgeryToken]
        public ActionResult AttendanceConfirmed(Attendance att, int evid, int studid)
        {

            SYSUserProfile stud = db.SYSUserProfiles.Find(studid);
            RSVP_Event ev = db.RSVP_Event.Find(evid);


            int i = db.Attendances.Count();

            try
            {
                if (i == 0)
                {
                    if (ev.FunctionID != null)
                    {
                        att.FunctionID = ev.FunctionID;
                        att.SYSUserProfileID = stud.SYSUserProfileID;
                        ev.Attended = 1;
                        db.Attendances.Add(att);
                    }
                    else if (ev.LectureID != null)
                    {
                        att.LectureID = ev.LectureID;
                        att.SYSUserProfileID = stud.SYSUserProfileID;
                        ev.Attended = 1;
                        db.Attendances.Add(att);
                    }
                    else
                    {
                        att.ComEngID = ev.ComEngID;
                        att.SYSUserProfileID = stud.SYSUserProfileID;
                        ev.Attended = 1;
                        db.Attendances.Add(att);
                    }

                }
                else
                {
                    int max = db.Attendances.Max(p => p.attendanceID);
                    int l = max + 1;

                    att.attendanceID = l;

                    if (ev.FunctionID != null)
                    {
                        att.FunctionID = ev.FunctionID;
                        att.SYSUserProfileID = stud.SYSUserProfileID;
                        ev.Attended = 1;
                        db.Attendances.Add(att);
                    }
                    else if (ev.LectureID != null)
                    {
                        att.LectureID = ev.LectureID;
                        att.SYSUserProfileID = stud.SYSUserProfileID;
                        ev.Attended = 1;
                        db.Attendances.Add(att);
                    }
                    else
                    {
                        att.ComEngID = ev.ComEngID;
                        att.SYSUserProfileID = stud.SYSUserProfileID;
                        ev.Attended = 1;
                        db.Attendances.Add(att);
                    }
                }
                db.SaveChanges();

                TempData["Attend"] = "You have successfully logged the event attendance of: " + stud.FirstName;
            }
            catch (Exception)
            {
                TempData["Attend"] = "There was an error in the attempt of logging the attendance of the student";
            }
            return RedirectToAction("LogAttendance");
        }

        public ActionResult WriteReview(int? id)
        {

            TRWLASchedule tRWLASchedule = db.TRWLASchedules.Find(id);
            Lecture lec = db.Lectures.Find(tRWLASchedule.LectureID);

            ViewBag.LectureName = lec.Lecture_Name;
            ViewBag.RatingID = new SelectList(db.RatingTypes, "RatingID", "Rating");
            return View();
        }

        [HttpPost, ActionName("WriteReview")]
        [ValidateAntiForgeryToken]
        public ActionResult WriteReviewConfirmed([Bind(Include = "reviewID, Review, RatingID, StudentID, VolunteerID, LectureID")] LectureReview LecRev, int id)
        {
            var user = (int)Session["User"];

            if (ModelState.IsValid)
            {
                int i = db.LectureReviews.Count();
                TRWLASchedule tRWLASchedule = db.TRWLASchedules.Find(id);
                Lecture lec = db.Lectures.Find(tRWLASchedule.LectureID);

                if (i == 0)
                {
                    LecRev.LectureID = lec.LectureID;
                    LecRev.SYSUserProfileID = user;

                    db.LectureReviews.Add(LecRev);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                else if (i != 0)
                {
                    int max = db.LectureReviews.Max(p => p.reviewID);
                    int k = max + 1;
                    LecRev.LectureID = lec.LectureID;
                    LecRev.reviewID = k;
                    LecRev.SYSUserProfileID = user;

                    db.LectureReviews.Add(LecRev);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }

                
            }
            ViewBag.RatingID = new SelectList(db.RatingTypes, "RatingID", "Rating");
            return View(LecRev);
        }




        // GET: TRWLASchedules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TRWLASchedule tRWLASchedule = db.TRWLASchedules.Find(id);
            if (tRWLASchedule == null)
            {
                return HttpNotFound();
            }
            return View(tRWLASchedule);
        }

        public ActionResult StudentEventDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TRWLASchedule tRWLASchedule = db.TRWLASchedules.Find(id);
            TempData["guestspeaker"] = tRWLASchedule.FunctionEvent.GuestSpeakerID;
            TempData["EventIdNeeded"] = tRWLASchedule.ScheduleID;

            if (tRWLASchedule == null)
            {
                return HttpNotFound();
            }
            return View(tRWLASchedule);
        }

        public ActionResult StudentGuestSpeaker(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GuestSpeaker guestSpeaker = db.GuestSpeakers.Find(id);
            if (guestSpeaker == null)
            {
                return HttpNotFound();
            }
            return View(guestSpeaker);
        }

        //RSVP to an EVENT
        public ActionResult RSVP(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TRWLASchedule tRWLASchedule = db.TRWLASchedules.Find(id);
            if (tRWLASchedule == null)
            {
                return HttpNotFound();
            }

            if (tRWLASchedule.FunctionID != null)
            {
                ViewBag.Name = tRWLASchedule.FunctionEvent.Function_Name;
            }
            else if (tRWLASchedule.LectureID != null)
            {
                ViewBag.Name = tRWLASchedule.Lecture.Lecture_Name;
            }
            else if (tRWLASchedule.ComEngID != null)
            {
                ViewBag.Name = tRWLASchedule.ComEngEvent.ComEng_Name;
            }
            return View(tRWLASchedule);
        }

        [HttpPost, ActionName("RSVP")]
        [ValidateAntiForgeryToken]
        public ActionResult RSVPConfirmed(RSVP_Event @event, int id)
        {
            var user = (int)Session["User"];
            TRWLASchedule tRWLASchedule = db.TRWLASchedules.Find(id);
            //Counts if the table has data in it
            int i = db.RSVP_Event.Count();
            if (i == 0)
            {
                //this is what adds data to the table
                @event.SYSUserProfileID = user; 

                if (tRWLASchedule.FunctionID != null)
                {
                    @event.FunctionID = tRWLASchedule.FunctionID;

                    RSVPSchedule mysched = new RSVPSchedule();
                    mysched.rsvpID = @event.rsvpID;
                    mysched.ScheduleID = id;
                    mysched.SYSUserProfileID = user;
                    db.RSVPSchedules.Add(mysched);
                    db.RSVP_Event.Add(@event);

                    foreach (var s in db.RSVP_Event.Where(p => p.FunctionID == tRWLASchedule.FunctionID))
                    {
                        if (s.Attended == null)
                        {
                            TempData["Attended"] = "You have already attended this event and cannot RSVP again.";
                            return RedirectToAction("Index");
                        }
                        if (s.SYSUserProfileID == user)
                        {
                            TempData["rsvpFAIL"] = "You have already RSVPd to this event";
                            return RedirectToAction("Index");
                        }
                        else if (s.SYSUserProfileID == user)
                        {
                            TempData["rsvp"] = "You have successfully RSVPd to the event: " + tRWLASchedule.FunctionEvent.Function_Name;
                        }
                    }

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else if (tRWLASchedule.ComEngID != null)
                {
                    @event.ComEngID = tRWLASchedule.ComEngID;

                    RSVPSchedule mysched = new RSVPSchedule();
                    mysched.rsvpID = @event.rsvpID;
                    mysched.ScheduleID = id;
                    mysched.SYSUserProfileID = user;
                    db.RSVPSchedules.Add(mysched);
                    db.RSVP_Event.Add(@event);

                    foreach (var s in db.RSVP_Event.Where(p => p.ComEngID == tRWLASchedule.ComEngID))
                    {
                        if (s.Attended != null)
                        {
                            TempData["Attended"] = "You have already attended this event and cannot RSVP again.";
                            return RedirectToAction("Index");
                        }
                        if (s.SYSUserProfileID == user)
                        {
                            TempData["rsvpFAIL"] = "You have already RSVPd to this event";
                            return RedirectToAction("Index");
                        }
                        else if (s.SYSUserProfileID != user)
                        {

                            TempData["rsvp"] = "You have successfully RSVPd to the event: " + tRWLASchedule.Lecture.Lecture_Name;
                        }
                    }
                    
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else if (tRWLASchedule.LectureID != null)
                {
                    @event.LectureID = tRWLASchedule.LectureID;

                    RSVPSchedule mysched = new RSVPSchedule();
                    mysched.rsvpID = @event.rsvpID;
                    mysched.ScheduleID = id;
                    mysched.SYSUserProfileID = user;
                    db.RSVPSchedules.Add(mysched);
                    db.RSVP_Event.Add(@event);

                    foreach (var s in db.RSVP_Event.Where(p => p.LectureID == tRWLASchedule.LectureID))
                    {

                        if (s.Attended != null)
                        {
                            TempData["Attended"] = "You have already attended this event and cannot RSVP again.";
                            return RedirectToAction("Index");
                        }
                        if (s.SYSUserProfileID == user)
                        {
                            TempData["rsvpFAIL"] = "You have already RSVPd to this event";
                            return RedirectToAction("Index");
                        }
                        else if (s.SYSUserProfileID == user)
                        {

                            TempData["rsvp"] = "You have successfully RSVPd to the event: " + tRWLASchedule.ComEngEvent.ComEng_Name;
                        }
                    }
                    
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }
            else
            {
                int max = db.RSVP_Event.Max(p => p.rsvpID);
                int l = max + 1;
                @event.SYSUserProfileID = user;

                if (tRWLASchedule.FunctionID != null)
                {
                    @event.FunctionID = tRWLASchedule.FunctionID;
                    @event.rsvpID = l;

                    RSVPSchedule mysched = new RSVPSchedule();
                    mysched.rsvpID = @event.rsvpID;
                    mysched.ScheduleID = id;
                    mysched.SYSUserProfileID = user;
                    db.RSVPSchedules.Add(mysched);
                    db.RSVP_Event.Add(@event);

                    foreach (var s in db.RSVP_Event.Where(p => p.FunctionID == tRWLASchedule.FunctionID))
                    {
                        
                        if (s.Attended != null)
                        {
                            TempData["Attended"] = "You have already attended this event and cannot RSVP again.";
                            return RedirectToAction("Index");
                        }
                        if (s.SYSUserProfileID == user)
                        {
                            TempData["rsvpFAIL"] = "You have already RSVPd to this event";
                            return RedirectToAction("Index");
                        }
                        else if (s.SYSUserProfileID == user)
                        {
                            TempData["rsvp"] = "You have successfully RSVPd to the event: " + tRWLASchedule.FunctionEvent.Function_Name;
                        }

                    }
                    
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else if (tRWLASchedule.ComEngID != null)
                {
                    @event.ComEngID = tRWLASchedule.ComEngID;
                    @event.rsvpID = l;

                    RSVPSchedule mysched = new RSVPSchedule();
                    mysched.rsvpID = @event.rsvpID;
                    mysched.ScheduleID = id;
                    mysched.SYSUserProfileID = user;
                    db.RSVPSchedules.Add(mysched);
                    db.RSVP_Event.Add(@event);

                    foreach (var s in db.RSVP_Event.Where(p => p.ComEngID == tRWLASchedule.ComEngID))
                    {

                        if (s.Attended != null)
                        {
                            TempData["Attended"] = "You have already attended this event and cannot RSVP again.";
                            return RedirectToAction("Index");
                        }
                        if (s.SYSUserProfileID == user)
                        {
                            TempData["rsvpFAIL"] = "You have already RSVPd to this event";
                            return RedirectToAction("Index");
                        }
                        else if (s.SYSUserProfileID == user)
                        {

                            TempData["rsvp"] = "You have successfully RSVPd to the event: " + tRWLASchedule.ComEngEvent.ComEng_Name;
                        }

                    }

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else if (tRWLASchedule.LectureID != null)
                {
                    @event.LectureID = tRWLASchedule.LectureID;
                    @event.rsvpID = l;

                    RSVPSchedule mysched = new RSVPSchedule();
                    mysched.rsvpID = @event.rsvpID;
                    mysched.ScheduleID = id;
                    mysched.SYSUserProfileID = user;
                    db.RSVPSchedules.Add(mysched);
                    db.RSVP_Event.Add(@event);

                    foreach (var s in db.RSVP_Event.Where(p => p.LectureID == tRWLASchedule.LectureID))
                    {

                        if (s.Attended != null)
                        {
                            TempData["Attended"] = "You have already attended this event and cannot RSVP again.";
                            return RedirectToAction("Index");
                        }
                        if (s.SYSUserProfileID == user)
                        {
                            TempData["rsvpFAIL"] = "You have already RSVPd to this event";
                            return RedirectToAction("Index");
                        }
                        else if (s.SYSUserProfileID == user)
                        {
                            TempData["rsvp"] = "You have successfully RSVPd to the event: " + tRWLASchedule.Lecture.Lecture_Name;
                        }

                    }
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return RedirectToAction("Index");
            }
        }

        public ActionResult EventType(int? id)
        {
            return View();
        }

        

        // GET: TRWLASchedules/Create
        public ActionResult Create()
        {
            ViewBag.ComEngID = new SelectList(db.ComEngEvents, "ComEngID", "ComEng_Name");
            ViewBag.FunctionID = new SelectList(db.FunctionEvents, "FunctionID", "Function_Name");
            ViewBag.LectureID = new SelectList(db.Lectures, "LectureID", "Lecture_Name");
            return View();
        }

        // POST: TRWLASchedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ScheduleID,FunctionID,LectureID,ComEngID")] TRWLASchedule tRWLASchedule)
        {
            if (ModelState.IsValid)
            {
                db.TRWLASchedules.Add(tRWLASchedule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ComEngID = new SelectList(db.ComEngEvents, "ComEngID", "ComEng_Name", tRWLASchedule.ComEngID);
            ViewBag.FunctionID = new SelectList(db.FunctionEvents, "FunctionID", "Function_Name", tRWLASchedule.FunctionID);
            ViewBag.LectureID = new SelectList(db.Lectures, "LectureID", "Lecture_Name", tRWLASchedule.LectureID);
            return View(tRWLASchedule);
        }



        // GET: TRWLASchedules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TRWLASchedule tRWLASchedule = db.TRWLASchedules.Find(id);
            if (tRWLASchedule == null)
            {
                return HttpNotFound();
            }
            ViewBag.ComEngID = new SelectList(db.ComEngEvents, "ComEngID", "ComEng_Name", tRWLASchedule.ComEngID);
            ViewBag.FunctionID = new SelectList(db.FunctionEvents, "FunctionID", "Function_Name", tRWLASchedule.FunctionID);
            ViewBag.LectureID = new SelectList(db.Lectures, "LectureID", "Lecture_Name", tRWLASchedule.LectureID);

            if (tRWLASchedule.LectureID != null)
            {
                ViewBag.ContentID = new SelectList(db.Contents, "ContentID", "Content_Name", tRWLASchedule.Lecture.ContentID);
                ViewBag.ResidenceID = new SelectList(db.Residences, "ResID", "Res_Name", tRWLASchedule.Lecture.ResidenceID);
                ViewBag.VenueID = new SelectList(db.Venues, "VenueID", "Venue_Name", tRWLASchedule.Lecture.VenueID);
            }
            else if (tRWLASchedule.FunctionID != null)
            {
                ViewBag.GuestSpeakerID = new SelectList(db.GuestSpeakers, "GuestSpeakerID", "GuestSpeaker_Name", tRWLASchedule.FunctionEvent.GuestSpeakerID);
                ViewBag.VenueID = new SelectList(db.Venues, "VenueID", "Venue_Name", tRWLASchedule.FunctionEvent.VenueID);
            }
            else if (tRWLASchedule.ComEngID != null)
            {
                ViewBag.ContentID = new SelectList(db.Contents, "ContentID", "Content_Name", tRWLASchedule.ComEngEvent.ContentID);
                ViewBag.VenueID = new SelectList(db.Venues, "VenueID", "Venue_Name", tRWLASchedule.ComEngEvent.VenueID);
            }
            return View(tRWLASchedule);
        }

        // POST: TRWLASchedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ScheduleID,FunctionID,LectureID,ComEngID")] TRWLASchedule tRWLASchedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tRWLASchedule).State = EntityState.Modified;

                if (tRWLASchedule.FunctionID != null)
                {
                    FunctionEvent func = db.FunctionEvents.Find(tRWLASchedule.FunctionID);


                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ComEngID = new SelectList(db.ComEngEvents, "ComEngID", "ComEng_Name", tRWLASchedule.ComEngID);
            ViewBag.FunctionID = new SelectList(db.FunctionEvents, "FunctionID", "Function_Name", tRWLASchedule.FunctionID);
            ViewBag.LectureID = new SelectList(db.Lectures, "LectureID", "Lecture_Name", tRWLASchedule.LectureID);

            if (tRWLASchedule.LectureID != null)
            {
                ViewBag.ContentID = new SelectList(db.Contents, "ContentID", "Content_Name", tRWLASchedule.Lecture.ContentID);
                ViewBag.ResidenceID = new SelectList(db.Residences, "ResID", "Res_Name", tRWLASchedule.Lecture.ResidenceID);
                ViewBag.VenueID = new SelectList(db.Venues, "VenueID", "Venue_Name", tRWLASchedule.Lecture.VenueID);
            }
            else if (tRWLASchedule.FunctionID != null)
            {
                ViewBag.GuestSpeakerID = new SelectList(db.GuestSpeakers, "GuestSpeakerID", "GuestSpeaker_Name", tRWLASchedule.FunctionEvent.GuestSpeakerID);
                ViewBag.VenueID = new SelectList(db.Venues, "VenueID", "Venue_Name", tRWLASchedule.FunctionEvent.VenueID);
            }
            else if (tRWLASchedule.ComEngID != null)
            {
                ViewBag.ContentID = new SelectList(db.Contents, "ContentID", "Content_Name", tRWLASchedule.ComEngEvent.ContentID);
                ViewBag.VenueID = new SelectList(db.Venues, "VenueID", "Venue_Name", tRWLASchedule.ComEngEvent.VenueID);
            }

            return View(tRWLASchedule);
        }

        // GET: TRWLASchedules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TRWLASchedule tRWLASchedule = db.TRWLASchedules.Find(id);
            if (tRWLASchedule == null)
            {
                return HttpNotFound();
            }
            return View(tRWLASchedule);
        }



        // POST: TRWLASchedules/Delete/5
        [HttpPost, ActionName("Details")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            //Finds the row in the table where the scheduleID is equal to this
            TRWLASchedule tRWLASchedule = db.TRWLASchedules.Find(id);

            //Finds the row/s in the table where the scheduleID found is equal to this
            RSVPSchedule rsvp = db.RSVPSchedules.FirstOrDefault(p => p.ScheduleID == id);



            if (tRWLASchedule.FunctionID != null)
            {

                FunctionEvent functions = db.FunctionEvents.Find(tRWLASchedule.FunctionID);


                var email = from s in db.RSVP_Event
                            where s.FunctionID == tRWLASchedule.FunctionID
                            select s.SYSUserProfileID;

                foreach (var s in email)
                {
                    try
                    {
                        SYSUserProfile recipient = db.SYSUserProfiles.Find(s);

                        FunctionEvent func = db.FunctionEvents.Find(Convert.ToInt32(tRWLASchedule.FunctionID));

                        MailMessage msg = new MailMessage();
                        msg.From = new MailAddress("u15213626@tuks.co.za");
                        msg.To.Add(recipient.Email);
                        msg.Subject = func.Function_Name + " Cancellation";
                        msg.Body = "Dear " + recipient.FirstName + "\n\n Please note that the event, " + func.Function_Name + " has been cancelled until further notice. Thank you for your understanding in this matter. \n\n Regards, \n TRWLA Management.";

                        SmtpClient smtp = new SmtpClient();

                        smtp.Host = "smtp.gmail.com";
                        smtp.Port = 587;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new System.Net.NetworkCredential("u15213626@tuks.co.za", "Rootsms4");
                        smtp.EnableSsl = true;
                        smtp.Send(msg);

                        ModelState.Clear();
                    }
                    catch (Exception)
                    {
                        ViewBag.Status = "Problem while sending email, Please check details.";
                    }
                }

                RSVP_Event function = db.RSVP_Event.FirstOrDefault(l => l.FunctionID == tRWLASchedule.FunctionID);

                db.RSVP_Event.Remove(function);
                db.TRWLASchedules.Remove(tRWLASchedule);
                db.RSVPSchedules.Remove(rsvp);
                db.FunctionEvents.Remove(functions);

            }
            else if (tRWLASchedule.LectureID != null)
            {

                Lecture lectures = db.Lectures.Find(Convert.ToInt32(tRWLASchedule.LectureID));

                var email = from s in db.RSVP_Event
                            where s.LectureID == tRWLASchedule.LectureID
                            select s.SYSUserProfileID;

                foreach (var s in email)
                {
                    try
                    {
                        SYSUserProfile recipient = db.SYSUserProfiles.Find(s);
                        Lecture lec = db.Lectures.Find(tRWLASchedule.LectureID);

                        MailMessage msg = new MailMessage();
                        msg.From = new MailAddress("u15213626@tuks.co.za");
                        msg.To.Add(recipient.Email);
                        msg.Subject = lec.Lecture_Name + " Cancellation";
                        msg.Body = "Dear " + recipient.FirstName + "\n\n Please note that the event, " + lec.Lecture_Name + " has been cancelled until further notice. Thank you for your understanding in this matter. \n\n Regards, \n TRWLA Management.";

                        SmtpClient smtp = new SmtpClient();

                        smtp.Host = "smtp.gmail.com";
                        smtp.Port = 587;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new System.Net.NetworkCredential("u15213626@tuks.co.za", "Rootsms4");
                        smtp.EnableSsl = true;
                        smtp.Send(msg);

                        ModelState.Clear();
                    }
                    catch (Exception)
                    {
                        ViewBag.Status = "Problem while sending email, Please check details.";
                    }
                }

                RSVP_Event lecture = db.RSVP_Event.FirstOrDefault(l => l.LectureID == tRWLASchedule.LectureID);

                db.TRWLASchedules.Remove(tRWLASchedule);
                db.RSVPSchedules.Remove(rsvp);
                db.RSVP_Event.Remove(lecture);
                db.Lectures.Remove(lectures);




            }
            else if (tRWLASchedule.ComEngID != null)
            {


                ComEngEvent comeng = db.ComEngEvents.Find(Convert.ToInt32(tRWLASchedule.ComEngID));

                var email = from s in db.RSVP_Event
                            where s.ComEngID == tRWLASchedule.ComEngID
                            select s.SYSUserProfileID;

                foreach (var s in email)
                {
                    try
                    {
                        SYSUserProfile recipient = db.SYSUserProfiles.Find(s);
                        ComEngEvent com = db.ComEngEvents.Find(tRWLASchedule.ComEngID);

                        MailMessage msg = new MailMessage();
                        msg.From = new MailAddress("u15213626@tuks.co.za");
                        msg.To.Add(recipient.Email);
                        msg.Subject = com.ComEng_Name + " Cancellation";
                        msg.Body = "Dear " + recipient.FirstName + "\n\n Please note that the event, " + com.ComEng_Name + ", has been cancelled until further notice. Thank you for your understanding in this matter. \n\n Regards, \n TRWLA Management.";

                        SmtpClient smtp = new SmtpClient();

                        smtp.Host = "smtp.gmail.com";
                        smtp.Port = 587;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new System.Net.NetworkCredential("u15213626@tuks.co.za", "Rootsms4");
                        smtp.EnableSsl = true;
                        smtp.Send(msg);

                        ModelState.Clear();
                    }
                    catch (Exception)
                    {
                        ViewBag.Status = "Problem while sending email, Please check details.";
                    }

                    RSVP_Event comu = db.RSVP_Event.FirstOrDefault(l => l.ComEngID == tRWLASchedule.ComEngID);

                    db.TRWLASchedules.Remove(tRWLASchedule);
                    db.RSVPSchedules.Remove(rsvp);
                    db.RSVP_Event.Remove(comu);
                    db.ComEngEvents.Remove(comeng);





                }

                //Note: Write code to send email to all students who have RSVP'd to the event so that they get the notification. 


            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult StudentContent(string sortOrder, string searchString, string locked, string unlocked, string all)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.SurnameSortParm = String.IsNullOrEmpty(sortOrder) ? "sur_desc" : "Surname";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";


            var content = from s in db.Contents
                          select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                content = content.Where(s => s.Content_Name.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(locked))
            {

                content = content.Where(s => s.Content_Status.Equals(1));
            }

            if (!String.IsNullOrEmpty(unlocked))
            {

                content = content.Where(s => s.Content_Status.Equals(0));
            }

            if (!String.IsNullOrEmpty(all))
            {

                content = content.Where(s => s.Content_Status.Equals(0) || s.Content_Status.Equals(1));
            }


            switch (sortOrder)
            {
                case "name_desc":
                    content = content.OrderByDescending(s => s.Content_Name);
                    break;
                case "sur_desc":
                    content = content.OrderByDescending(s => s.Content_Name);
                    break;
                case "Surname":
                    content = content.OrderByDescending(s => s.Content_Name);
                    break;
                default:
                    content = content.OrderBy(s => s.Content_Name);
                    break;
            }

            return View(content.ToList());
        }

        public ActionResult StudentContentDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.DB.Content content = db.Contents.Find(id);
            if (content == null)
            {
                return HttpNotFound();
            }

            TempData["Image"] = id;
            return View(content);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

