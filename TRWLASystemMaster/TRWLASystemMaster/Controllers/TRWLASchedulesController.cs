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
using Microsoft.SqlServer.Server;
using System.Data.SqlClient;
using System.Xml;
using System.Web.Script.Services;
using System.Web.Services;


namespace TRWLASystemMaster.Controllers
{
    public class TRWLASchedulesController : Controller
    {
        private TWRLADB_Staging_V2Entities8 db = new TWRLADB_Staging_V2Entities8();
        

        public ActionResult ExportData()
        {
            return View();
        }

        public ActionResult BackupConfirmed()
        {
            

            return View();
        }

        

        public ActionResult ErrorPage()
        {
            return View();
        }

        public ActionResult StudentDashboard(int? id)
        {
            try
            {
                var Master = from s in db.RSVP_Event
                             where s.SYSUserProfileID == id
                             select s;
                var user = (int)Session["User"];

                SYSUserProfile myuser = db.SYSUserProfiles.Find(id);

                try
                {
                    progressbar myprog = db.progressbars.FirstOrDefault(p => p.SYSUserProfileID == user);
                    MasterData mymaster = db.MasterDatas.Find(9);

                    if (myprog != null)
                    {

                        var lec = myprog.LecProg;
                        var func = myprog.FuncProg;
                        var com = myprog.ComProg;
                        var gen = myprog.GenProg;


                        ViewBag.Lecture = ((Convert.ToDecimal(lec) / Convert.ToDecimal(mymaster.LecAttend)) * 100);
                        ViewBag.Function = ((Convert.ToDecimal(func) / Convert.ToDecimal(mymaster.FuncAttend)) * 100);
                        ViewBag.Com = ((Convert.ToDecimal(com) / Convert.ToDecimal(mymaster.ComAttend)) * 100);
                        ViewBag.Gen = ((Convert.ToDecimal(gen) / Convert.ToDecimal(mymaster.GenAttend)) * 100);
                    }
                    else
                    {
                        ViewBag.Lecture = 0;
                        ViewBag.Function = 0;
                        ViewBag.Com = 0;
                        ViewBag.Gen = 0;
                    }
                }
                catch
                {
                    TempData["NoEvent"] = "You have yet to attend an event";
                }

                ViewBag.Name = myuser.FirstName;

                return View(Master.ToList());
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }
        //AuditLog View
        // GET: TRWLASchedules
        [AllowAnonymous]
        public ActionResult AuditLog(string sortOrder, string searchString, string Create, string Update, string Delete, string all)
        {
            try
            {

                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.SurnameSortParm = String.IsNullOrEmpty(sortOrder) ? "sur_desc" : "Surname";
                ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

                var auditLog = from s in db.AuditLogs
                               select s;


                if (!String.IsNullOrEmpty(searchString))
                {
                    auditLog = auditLog.Where(s => s.TypeTran.Contains(searchString));
                }



                if (!String.IsNullOrEmpty(Create))
                {
                    auditLog = auditLog.Where(s => s.TypeTran.Contains("Create"));
                }

                if (!String.IsNullOrEmpty(Update))
                {
                    auditLog = auditLog.Where(s => s.TypeTran.Contains("Update"));
                }

                if (!String.IsNullOrEmpty(Delete))
                {
                    auditLog = auditLog.Where(s => s.TypeTran.Contains("Delete"));
                }

                if (!String.IsNullOrEmpty(all))
                {
                    auditLog = auditLog.Where(s => s.TypeTran.Contains("Create")
                            || s.TableAff.Contains("Update")
                            || s.TableAff.Contains("Delete"));
                }

                switch (sortOrder)
                {
                    case "name_desc":
                        auditLog = auditLog.OrderByDescending(s => s.TypeTran.Contains(searchString));
                        break;
                    case "sur_desc":
                        auditLog = auditLog.OrderByDescending(s => s.TypeTran.Contains(searchString));
                        break;
                    case "Surname":
                        auditLog = auditLog.OrderByDescending(s => s.TypeTran.Contains(searchString));
                        break;
                    default:
                        auditLog = auditLog.OrderByDescending(s => s.TypeTran.Contains(searchString));
                        break;
                }
                
                return View(auditLog.ToList());
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        public ActionResult AuditLogXML()
        {


            IEnumerable < AuditLog > dataList = db.AuditLogs;
            return new CsvFileResult<AuditLog>(dataList, "AuditLog.txt");

            
        }

        public ActionResult StudentMainMenu(string sortOrder, string searchString, string F, string CO, string L, string G, string all)
        {
            try
            {

                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.SurnameSortParm = String.IsNullOrEmpty(sortOrder) ? "sur_desc" : "Surname";
                ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

                var tRWLASchedules = db.TRWLASchedules.Include(t => t.ComEngEvent).Include(t => t.FunctionEvent).Include(t => t.Lecture).Include(t => t.GenEvent);


                if (!String.IsNullOrEmpty(searchString))
                {
                    tRWLASchedules = tRWLASchedules.Where(s => s.FunctionEvent.Function_Name.Contains(searchString)
                            || s.Lecture.Lecture_Name.Contains(searchString)
                            || s.ComEngEvent.ComEng_Name.Contains(searchString)
                            || s.GenEvent.Gen_Name.Contains(searchString));
                }



                if (!String.IsNullOrEmpty(F))
                {
                    tRWLASchedules = tRWLASchedules.Where(s => s.FunctionEvent.Type == 1);
                }

                if (!String.IsNullOrEmpty(CO))
                {
                    tRWLASchedules = tRWLASchedules.Where(s => s.ComEngEvent.Type == 3 );
                }

                if (!String.IsNullOrEmpty(L))
                {
                    tRWLASchedules = tRWLASchedules.Where(s => s.Lecture.Type == 2);
                }

                if (!String.IsNullOrEmpty(G))
                {
                    tRWLASchedules = tRWLASchedules.Where(s => s.GenEvent.Type == 4);
                }

                if (!String.IsNullOrEmpty(all))
                {
                    tRWLASchedules = tRWLASchedules.Where(s => s.GenEvent.Type == 4
                            || s.Lecture.Type == 2
                            || s.FunctionEvent.Type == 1
                            || s.ComEngEvent.Type == 3);
                }
                
                DateTime mydate = DateTime.Now.AddDays(-1);

                tRWLASchedules = tRWLASchedules.Where(p => p.Lecture.Lecture_Date >= mydate || p.FunctionEvent.Function_Date >= mydate || p.ComEngEvent.ComEng_Date >= mydate || p.GenEvent.Gen_Date >= mydate);

                return View(tRWLASchedules.ToList());
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }
        // GET: TRWLASchedules
        [AllowAnonymous]
        public ActionResult Index(string sortOrder, string searchString, string F, string CO, string L, string G, string all)
        {
            try
            {

                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.SurnameSortParm = String.IsNullOrEmpty(sortOrder) ? "sur_desc" : "Surname";
                ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

                var tRWLASchedules = db.TRWLASchedules.Include(t => t.ComEngEvent).Include(t => t.FunctionEvent).Include(t => t.Lecture).Include(t => t.GenEvent);


                if (!String.IsNullOrEmpty(searchString))
                {
                    tRWLASchedules = tRWLASchedules.Where(s => s.FunctionEvent.Function_Name.Contains(searchString)
                            || s.Lecture.Lecture_Name.Contains(searchString)
                            || s.ComEngEvent.ComEng_Name.Contains(searchString)
                            || s.GenEvent.Gen_Name.Contains(searchString));
                }



                if (!String.IsNullOrEmpty(F))
                {
                    tRWLASchedules = tRWLASchedules.Where(s => s.FunctionEvent.Type == 1);
                }

                if (!String.IsNullOrEmpty(CO))
                {
                    tRWLASchedules = tRWLASchedules.Where(s => s.ComEngEvent.Type == 3);
                }

                if (!String.IsNullOrEmpty(L))
                {
                    tRWLASchedules = tRWLASchedules.Where(s => s.Lecture.Type == 2);
                }

                if (!String.IsNullOrEmpty(G))
                {
                    tRWLASchedules = tRWLASchedules.Where(s => s.GenEvent.Type == 4);
                }

                if (!String.IsNullOrEmpty(all))
                {
                    tRWLASchedules = tRWLASchedules.Where(s => s.GenEvent.Type == 4
                            || s.Lecture.Type == 2
                            || s.FunctionEvent.Type == 1
                            || s.ComEngEvent.Type == 3);
                }



                DateTime mydate = DateTime.Now.AddDays(-1);

                tRWLASchedules = tRWLASchedules.Where(p => p.Lecture.Lecture_Date >= mydate || p.FunctionEvent.Function_Date >= mydate || p.ComEngEvent.ComEng_Date >= mydate|| p.GenEvent.Gen_Date >= mydate);

                return View(tRWLASchedules.ToList());
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        //Begginning of Report Generation 

        public IList<ClassAttendanceReport> GetClassAttendance()
        {
                var at = (from l in db.RSVP_Event
                          join s in db.SYSUserProfiles on l.SYSUserProfileID equals s.SYSUserProfileID
                          select new ClassAttendanceReport
                          {
                              Name = s.FirstName,
                              Surname = s.LastName,
                              EventCount = db.RSVP_Event.Count(),
                              StudentID = s.SYSUserProfileID,
                              PersonalCount = (db.RSVP_Event.Distinct().Where(p => p.SYSUserProfileID == s.SYSUserProfileID).Count()) / (db.RSVP_Event.Count()) * 100
                          }).ToList();

                return at;
        }

        public IList<Demographic> GetDemo(string namesearchString, string resname)
        {
            var at = (from l in db.SYSUserProfiles
                      join r in db.Residences on l.ResID equals r.ResID
                      where l.UserTypeID == 1
                      select new Demographic
                      {
                          StudNo = l.StudentNumber,
                          Name = l.FirstName,
                          Surname = l.LastName,
                          DoB = l.DoB,
                          Degree = l.Degree,
                          Res = r.Res_Name,
                          email = l.Email
                      }).ToList();

            int count =  at.Count();
            ViewBag.StudentCount = count;

            if (namesearchString == null)
            {
                TempData["name"] = "";
            }
            else if (namesearchString != null)
            {
                TempData["name"] = namesearchString;
            }

            if (resname == null)
            {

                TempData["res"] = "";
            }
            else if (resname != null)
            {

                TempData["res"] = resname;
            }



            if (!String.IsNullOrEmpty(namesearchString))
            {
                var newl = from s in at
                           where s.Name.Contains(namesearchString)
                           select s;

                return newl.ToList();
            }

            if (!String.IsNullOrEmpty(resname))
            {
                var newl = from s in at
                           where s.Res.Contains(resname)
                           select s;

                return newl.ToList();
            }





            return at;
        }

        public IList<AttendanceViewModel> GetLectureAttendance(string namesearchString, string resname)
        {
            var at = (from l in db.Attendances
                      join s in db.SYSUserProfiles on l.SYSUserProfileID equals s.SYSUserProfileID
                      join le in db.Lectures on l.LectureID equals le.LectureID
                      where l.LectureID != null
                      select new AttendanceViewModel
                      {
                          EventName = l.Lecture.Lecture_Name,
                          EventDate = l.Lecture.Lecture_Date,
                          StartTime = le.Lecture_StartTime,
                          EndTime = le.Lecture_EndTime,
                          Residence = le.Residence.Res_Name,
                          specific = le.Content.Content_Name,
                          StudentNp = s.StudentNumber,
                          Student_Name = l.SYSUserProfile.FirstName,
                          LastName = s.LastName,

                      }).ToList();

            ViewBag.Count = at.GroupBy(p => p.StudentNp).Distinct().Count();
            if (namesearchString == null)
            {
                TempData["name"] = "";
            }
            else if (namesearchString != null)
            {
                TempData["name"] = namesearchString;
            }

            if (resname == null)
            {

                TempData["res"] = "";
            }
            else if (resname != null)
            {

                TempData["res"] = resname;
            }



            if (!String.IsNullOrEmpty(namesearchString))
            {
                var newl = from s in at
                           where s.EventName.Contains(namesearchString)
                           select s;

                return newl.ToList();
            }

            if (!String.IsNullOrEmpty(resname))
            {
                var newl = from s in at
                           where s.Student_Name.Contains(resname)
                           select s;

                return newl.ToList();
            }

            return at;
        }
        public IList<AttendanceViewModel> GetFunctionAttendance(string namesearchString, string resname)
        {
            var attendance = (from s in db.Attendances
                              where s.FunctionID != null
                              select new AttendanceViewModel
                              {
                                  EventName = s.FunctionEvent.Function_Name,
                                  EventDate = s.FunctionEvent.Function_Date,
                                  StartTime = s.FunctionEvent.Function_StartTime,
                                  EndTime = s.FunctionEvent.Function_EndTime,
                                  Residence = s.FunctionEvent.Venue.Venue_Name,
                                  specific = s.FunctionEvent.GuestSpeaker.GuestSpeaker_Name,
                                  Student_Name = s.SYSUserProfile.FirstName,
                                  LastName = s.SYSUserProfile.LastName,
                                  StudentNp = s.SYSUserProfile.StudentNumber
                              }).ToList();


            ViewBag.Count = attendance.GroupBy(p => p.StudentNp).Distinct().Count();

            if (namesearchString == null)
            {
                TempData["name"] = "";
            }
            else if (namesearchString != null)
            {
                TempData["name"] = namesearchString;
            }

            if (resname == null)
            {

                TempData["res"] = "";
            }
            else if (resname != null)
            {

                TempData["res"] = resname;
            }



            if (!String.IsNullOrEmpty(namesearchString))
            {
                var newl = from s in attendance
                           where s.EventName.Contains(namesearchString)
                           select s;

                return newl.ToList();
            }

            if (!String.IsNullOrEmpty(resname))
            {
                var newl = from s in attendance
                           where s.Student_Name.Contains(resname)
                           select s;

                return newl.ToList();
            }
            return attendance;
        }

        public IList<GeneralEventList> GetGenEventAttendance(string namesearchString, string resname)
        {
            var attendance = (from s in db.Attendances
                              where s.GenID != null
                              select new GeneralEventList
                              {
                                  EventName = s.GenEvent.Gen_Name,
                                  Date = s.GenEvent.Gen_Date,
                                  Start = s.GenEvent.Gen_StartTime,
                                  End = s.GenEvent.Gen_EndTime,
                                  Residence = s.SYSUserProfile.Residence.Res_Name,
                                  StudentName = s.SYSUserProfile.FirstName,
                                  StudentSurname = s.SYSUserProfile.LastName,
                                  StudentNumber = s.SYSUserProfile.StudentNumber
                              }).ToList();

            ViewBag.Count = attendance.GroupBy(p => p.StudentNumber).Distinct().Count();
            if (namesearchString == null)
            {
                TempData["name"] = "";
            }
            else if (namesearchString != null)
            {
                TempData["name"] = namesearchString;
            }

            if (resname == null)
            {

                TempData["res"] = "";
            }
            else if (resname != null)
            {

                TempData["res"] = resname;
            }



            if (!String.IsNullOrEmpty(namesearchString))
            {
                var newl = from s in attendance
                           where s.EventName.Contains(namesearchString)
                           select s;

                return newl.ToList();
            }

            if (!String.IsNullOrEmpty(resname))
            {
                var newl = from s in attendance
                           where s.StudentName.Contains(resname)
                           select s;

                return newl.ToList();
            }
            return attendance;
        }

        public ActionResult GeneralEventReport(string namesearchString, string resname)
        {
            try
            {
                return View(this.GetGenEventAttendance(namesearchString, resname));
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        public ActionResult ExportToExcel6()
        {
            try
            {
                var gv = new GridView();
                gv.DataSource = this.GetGenEventAttendance(TempData["name"].ToString(), TempData["res"].ToString());
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                string name = "General Event Attendance Report " + Convert.ToString(DateTime.Now);
                Response.AddHeader("content-disposition", "attachment; filename=" + name + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter objStringWriter = new StringWriter();
                HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
                gv.RenderControl(objHtmlTextWriter);
                Response.Output.Write(objStringWriter.ToString());
                Response.Flush();
                Response.End();

                AuditLog myAudit = new AuditLog();
                myAudit.DateDone = DateTime.Now;
                myAudit.TypeTran = "ReportDownload";
                myAudit.SYSUserProfileID = (int)Session["User"];
                myAudit.TableAff = "RSVP_Event";
                db.AuditLogs.Add(myAudit);
                db.SaveChanges();

                return View("Index");
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }


        public IList<AttendanceViewModel> GetComAttendance(string namesearchString, string resname)
        {
            var attend = (from s in db.Attendances
                          where s.ComEngID != null
                          select new AttendanceViewModel
                          {
                              EventName = s.ComEngEvent.ComEng_Name,
                              EventDate = s.ComEngEvent.ComEng_Date,
                              StartTime = s.ComEngEvent.ComEnge_StartTime,
                              EndTime = s.ComEngEvent.ComEng_EndTime,
                              Residence = s.ComEngEvent.Venue.Venue_Name,
                              specific = s.ComEngEvent.Content.Content_Name,
                              Student_Name = s.SYSUserProfile.FirstName,
                              LastName = s.SYSUserProfile.LastName,
                              StudentNp = s.SYSUserProfile.StudentNumber
                          }).ToList();

            ViewBag.Count = attend.GroupBy(p => p.StudentNp).Distinct().Count();

            if (namesearchString == null)
            {
                TempData["name"] = "";
            }
            else if (namesearchString != null)
            {
                TempData["name"] = namesearchString;
            }

            if (resname == null)
            {

                TempData["res"] = "";
            }
            else if (resname != null)
            {

                TempData["res"] = resname;
            }



            if (!String.IsNullOrEmpty(namesearchString))
            {
                var newl = from s in attend
                           where s.EventName.Contains(namesearchString)
                           select s;

                return newl.ToList();
            }

            if (!String.IsNullOrEmpty(resname))
            {
                var newl = from s in attend
                           where s.Student_Name.Contains(resname)
                           select s;

                return newl.ToList();
            }


            return attend;
        }

        public IList<NoAttend> GetNoAttendance(string namesearchString, string resname)
        {
            DateTime mydate = DateTime.Now;

            var attend = (from m in db.RSVP_Event
                          join s in db.SYSUserProfiles on m.SYSUserProfileID equals s.SYSUserProfileID
                          where m.Attended == null && (m.FunctionEvent.Function_Date < mydate || m.ComEngEvent.ComEng_Date < mydate || m.GenEvent.Gen_Date < mydate || m.Lecture.Lecture_Date < mydate)
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
                              ComEngDate = m.ComEngEvent.ComEng_Date,
                              GenDate = m.GenEvent.Gen_Date,
                              FuncName = m.FunctionEvent.Function_Name,
                              LecName = m.Lecture.Lecture_Name,
                              ComName = m.ComEngEvent.ComEng_Name,
                              GenName = m.GenEvent.Gen_Name
                          }).ToList();


            int count = (from n in db.RSVP_Event
                         where n.Attended != null
                         select n).Count();

            ViewBag.Attend = count;
            ViewBag.Count = attend.GroupBy(p => p.StudNo).Distinct().Count();
            if (namesearchString == null)
            {
                TempData["name"] = "";
            }
            else if (namesearchString != null)
            {
                TempData["name"] = namesearchString;
            }

            if (resname == null)
            {

                TempData["res"] = "";
            }
            else if (resname != null)
            {

                TempData["res"] = resname;
            }



            if (!String.IsNullOrEmpty(namesearchString))
            {
                var newl = from s in attend
                           where s.Name.Contains(namesearchString)
                           select s;

                return newl.ToList();
            }

            if (!String.IsNullOrEmpty(resname))
            {
                var newl = from s in attend
                           where s.Res.Contains(resname)
                           select s;

                return newl.ToList();
            }



            return attend;
        }

        //Generate the Excel Spreadsheets for the data
        public ActionResult ExportToExcel1()
        {
            try
            {
                var gv = new GridView();
                gv.DataSource = this.GetFunctionAttendance(TempData["name"].ToString(), TempData["res"].ToString());
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

                AuditLog myAudit = new AuditLog();
                myAudit.DateDone = DateTime.Now;
                myAudit.TypeTran = "ReportDownload";
                myAudit.SYSUserProfileID = (int)Session["User"];
                myAudit.TableAff = "FunctionEvents";
                db.AuditLogs.Add(myAudit);
                db.SaveChanges();

                return View("Index");
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        public ActionResult ExportToExcel2()
        {
            try {

                var gv = new GridView();
                gv.DataSource = this.GetLectureAttendance(TempData["name"].ToString(), TempData["res"].ToString());
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

                AuditLog myAudit = new AuditLog();
                myAudit.DateDone = DateTime.Now;
                myAudit.TypeTran = "ReportDownload";
                myAudit.SYSUserProfileID = (int)Session["User"];
                myAudit.TableAff = "Lecture";
                db.AuditLogs.Add(myAudit);
                db.SaveChanges();

                return View("Index");
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        public ActionResult ExportToExcel3()
        {
            try
            {
                var gv = new GridView();
                gv.DataSource = this.GetComAttendance(TempData["name"].ToString(), TempData["res"].ToString());
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

                AuditLog myAudit = new AuditLog();
                myAudit.DateDone = DateTime.Now;
                myAudit.TypeTran = "ReportDownload";
                myAudit.SYSUserProfileID = (int)Session["User"];
                myAudit.TableAff = "ComEngEvents";
                db.AuditLogs.Add(myAudit);
                db.SaveChanges();

                return View("Index");
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        public ActionResult ExportToExcel4()
        {
            try
            {
                var gv = new GridView();
                gv.DataSource = this.GetDemo(TempData["name"].ToString(), TempData["res"].ToString());
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

                AuditLog myAudit = new AuditLog();
                myAudit.DateDone = DateTime.Now;
                myAudit.TypeTran = "ReportDownload";
                myAudit.SYSUserProfileID = (int)Session["User"];
                myAudit.TableAff = "SYSUserProfile";
                db.AuditLogs.Add(myAudit);
                db.SaveChanges();

                return View("Index");
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        public ActionResult ExportToExcel5()
        {
            try
            {
                var gv = new GridView();
                gv.DataSource = this.GetNoAttendance(TempData["name"].ToString(), TempData["res"].ToString());
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

                AuditLog myAudit = new AuditLog();
                myAudit.DateDone = DateTime.Now;
                myAudit.TypeTran = "ReportDownload";
                myAudit.SYSUserProfileID = (int)Session["User"];
                myAudit.TableAff = "RSVP_Event";
                db.AuditLogs.Add(myAudit);
                db.SaveChanges();

                return View("Index");
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        public ActionResult MainReportPage()
        {
            return View();
        }

        public ActionResult StudentDemographic(string namesearchString, string resname)
        {
            try
            {
                return View(this.GetDemo(namesearchString, resname));
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        public ActionResult NoAttend(string namesearchString, string resname)
        {
            try
            {
                return View(this.GetNoAttendance(namesearchString, resname));
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        public ActionResult ClassAttendance()
        {
            try
            {
                int s = (from n in db.RSVP_Event
                         select n).Count();

                ClassAttendance myfunc = db.ClassAttendances.Find(1);
                ClassAttendance mylec = db.ClassAttendances.Find(2);
                ClassAttendance mycom = db.ClassAttendances.Find(3);
                ClassAttendance mygen = db.ClassAttendances.Find(4);
                
                int func = myfunc.attend;
                int lec = mylec.attend;
                int com = mycom.attend;
                int gen = mygen.attend;

                ViewBag.Function = Convert.ToDecimal(func) / s * 100;
                ViewBag.Lecture = Convert.ToDecimal(lec) / s * 100;
                ViewBag.Com = Convert.ToDecimal(com) / s * 100;
                ViewBag.Gen = Convert.ToDecimal(gen) / s * 100;

                return View();

                
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        public ActionResult FunctionAttendance(string namesearchString, string resname)
        {
            try
            {
                return View(this.GetFunctionAttendance(namesearchString, resname));
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        public ActionResult ComAttendance(string namesearchString, string resname)
        {
            try
            {
                return View(this.GetComAttendance(namesearchString, resname));
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        public ActionResult LecAttendance(string namesearchString, string resname)
        {
            try
            {
                return View(this.GetLectureAttendance(namesearchString, resname));
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        public ActionResult SelectRecip()
        {
            try
            {
                var rsvp = from s in db.TRWLASchedules
                           select s;


                DateTime mydate = DateTime.Now.AddDays(-1);

                rsvp = rsvp.Where(p => p.Lecture.Lecture_Date >= mydate || p.FunctionEvent.Function_Date >= mydate || p.ComEngEvent.ComEng_Date >= mydate || p.GenEvent.Gen_Date >= mydate);

                return View(rsvp.ToList());
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        public ActionResult SendNotification(int? id)
        {
            try
            {
                TRWLASchedule sched = db.TRWLASchedules.Find(id);

                if (sched.FunctionID != null)
                {
                    ViewBag.Name = sched.FunctionEvent.Function_Name;
                }
                else if (sched.LectureID != null)
                {
                    ViewBag.Name = sched.Lecture.Lecture_Name;
                }
                else if (sched.ComEngID != null)
                {
                    ViewBag.Name = sched.ComEngEvent.ComEng_Name;
                }
                else if (sched.GenID != null)
                {
                    ViewBag.Name = sched.GenEvent.Gen_Name;
                }

                return View();
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        [HttpPost, ActionName("SendNotification")]
        [ValidateAntiForgeryToken]
        public ActionResult SendNotificationConfirmed(EventMessage mess,int id)
        {
            try
            {
                int i = db.EventMessages.Count();

                TRWLASchedule sched = db.TRWLASchedules.Find(id);

                if (sched.FunctionID != null)
                {
                    
                        RSVP_Event rsvp = db.RSVP_Event.FirstOrDefault(p => p.FunctionID == sched.FunctionID);
                    var choose = from s in db.RSVP_Event
                                 where s.FunctionID == rsvp.FunctionID
                                 select s;
                    if (i == 0)
                    {

                        if (rsvp != null)
                    {

                    int count = choose.ToList().Count();

                        if (count != 0)
                        {

                            
                                foreach (var s in choose)
                                {
                                    mess.SYSUserProfileID = Convert.ToInt32(s.SYSUserProfileID);
                                    mess.TimeMes = DateTime.Now.TimeOfDay;

                                    try
                                    {
                                        int k = Convert.ToInt32(s.SYSUserProfileID);

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
                                        smtp.Credentials = new System.Net.NetworkCredential("u15213626@tuks.co.za", "Coakes12345");
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
                            TempData["mess"] = 1;
                            return RedirectToAction("Index", "TRWLASchedules");
                        }
                    }
                    else
                    {
                        int max = db.EventMessages.Max(p => p.MessID);
                        int l = max + 1;
                        var select = from s in db.RSVP_Event
                                     where s.FunctionID == rsvp.FunctionID
                                     select s;


                        mess.MessID = l;

                        if (rsvp != null)

                        {
                            int count = select.ToList().Count();

                            if (count != 0)
                            {
                                foreach (var s in select)
                                {
                                    mess.SYSUserProfileID = Convert.ToInt32(s.SYSUserProfileID);
                                    mess.TimeMes = DateTime.Now.TimeOfDay;

                                    try
                                    {
                                        int k = Convert.ToInt32(s.SYSUserProfileID);

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
                                        smtp.Credentials = new System.Net.NetworkCredential("u15213626@tuks.co.za", "Coakes12345");
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
                            TempData["mess"] = 1;
                            return RedirectToAction("Index", "TRWLASchedules");
                        }
                    }
                }
                else if (sched.LectureID != null)
                {
                    if (i == 0)
                    {
                        RSVP_Event rsvp = db.RSVP_Event.FirstOrDefault(p => p.LectureID == sched.LectureID);
                        var choose = from s in db.RSVP_Event
                                     where s.LectureID == rsvp.LectureID
                                     select s;

                        if (rsvp != null)
                        {

                            int count = choose.ToList().Count();

                            if (count != 0)
                            {

                                foreach (var s in choose)
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
                                        smtp.Credentials = new System.Net.NetworkCredential("u15213626@tuks.co.za", "Coakes12345");
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
                            TempData["mess"] = 1;
                            return RedirectToAction("Index", "TRWLASchedules");
                        }
                    }
                    else
                    {
                        int max = db.EventMessages.Max(p => p.MessID);
                        int l = max + 1;

                        mess.MessID = l;

                        RSVP_Event rsvp = db.RSVP_Event.FirstOrDefault(p => p.LectureID == sched.LectureID);
                        var choose = from s in db.RSVP_Event
                                     where s.LectureID == rsvp.LectureID
                                     select s;

                        if (rsvp != null)
                        {

                            int count = choose.ToList().Count();

                            if (count != 0)
                            {
                                foreach (var s in choose)
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
                                        smtp.Credentials = new System.Net.NetworkCredential("u15213626@tuks.co.za", "Coakes12345");
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
                            TempData["mess"] = 1;
                            return RedirectToAction("Index", "TRWLASchedules");
                        }
                    }
                }
                else if (sched.GenID != null)
                {
                    if (i == 0)
                    {
                        RSVP_Event rsvp = db.RSVP_Event.FirstOrDefault(p => p.GenID == sched.GenID);
                        var choose = from s in db.RSVP_Event
                                     where s.GenID == rsvp.GenID
                                     select s;

                        if (rsvp != null)
                        {

                            int count = choose.ToList().Count();

                            if (count != 0)
                            {
                                foreach (var s in choose)
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
                                        msg.Subject = rsvp.GenEvent.Gen_Name + " Notification";
                                        msg.Body = "Dear " + myStu.FirstName + "\n\n " + mess.Msg;

                                        SmtpClient smtp = new SmtpClient();

                                        smtp.Host = "smtp.gmail.com";
                                        smtp.Port = 587;
                                        smtp.UseDefaultCredentials = false;
                                        smtp.Credentials = new System.Net.NetworkCredential("u15213626@tuks.co.za", "Coakes12345");
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
                            TempData["mess"] = 1;
                            return RedirectToAction("Index", "TRWLASchedules");
                        }
                    }
                    else
                    {
                        int max = db.EventMessages.Max(p => p.MessID);
                        int l = max + 1;

                        mess.MessID = l;

                        RSVP_Event rsvp = db.RSVP_Event.FirstOrDefault(p => p.GenID == sched.GenID);
                        var choose = from s in db.RSVP_Event
                                     where s.GenID == rsvp.GenID
                                     select s;

                        if (rsvp != null)
                        {

                            int count = choose.ToList().Count();

                            if (count != 0)
                            {

                                foreach (var s in choose)
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
                                        msg.Subject = rsvp.GenEvent.Gen_Name + " Notification";
                                        msg.Body = "Dear " + myStu.FirstName + "\n\n " + mess.Msg;

                                        SmtpClient smtp = new SmtpClient();

                                        smtp.Host = "smtp.gmail.com";
                                        smtp.Port = 587;
                                        smtp.UseDefaultCredentials = false;
                                        smtp.Credentials = new System.Net.NetworkCredential("u15213626@tuks.co.za", "Coakes12345");
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
                            TempData["mess"] = 1;
                            return RedirectToAction("Index", "TRWLASchedules");
                        }
                    }
                }
                else if (sched.ComEngID != null)
                {
                    if (i == 0)
                    {
                        RSVP_Event rsvp = db.RSVP_Event.FirstOrDefault(p => p.ComEngID == sched.ComEngID);

                        var choose = from s in db.RSVP_Event
                                     where s.ComEngID == rsvp.ComEngID
                                     select s;

                        if (rsvp != null)
                        {

                        int count = choose.ToList().Count();

                            if (count != 0)
                            {
                                foreach (var s in choose)
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
                                        smtp.Credentials = new System.Net.NetworkCredential("u15213626@tuks.co.za", "Coakes12345");
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
                            else
                            {
                                TempData["mess"] = 1;
                                return RedirectToAction("Index", "TRWLASchedules");
                            }
                        }
                    }
                    else
                    {
                        int max = db.EventMessages.Max(p => p.MessID);
                        int l = max + 1;

                        mess.MessID = l;

                        RSVP_Event rsvp = db.RSVP_Event.FirstOrDefault(p => p.ComEngID == sched.ComEngID);

                        if (rsvp != null)
                        {


                            var choose = from s in db.RSVP_Event
                                         where s.ComEngID == rsvp.ComEngID
                                         select s;

                            int count = choose.ToList().Count();

                            if (count != 0)
                            {
                                foreach (var s in choose)
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
                                        smtp.Credentials = new System.Net.NetworkCredential("u15213626@tuks.co.za", "Coakes12345");
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
                            TempData["mess"] = 1;
                            return RedirectToAction("Index", "TRWLASchedules");
                        }
                    }
                }
            
                    AuditLog myAudit = new AuditLog();
                    myAudit.DateDone = DateTime.Now;
                    myAudit.TypeTran = "Create";
                    myAudit.SYSUserProfileID = (int)Session["User"];
                    myAudit.TableAff = "EventMessages";
                    db.AuditLogs.Add(myAudit);

                db.SaveChanges();
                return RedirectToAction("Index", "TRWLASchedules");
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }

        }

        public ActionResult ViewNotification()
        {
            try
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
            catch
            {
                return RedirectToAction("ErrorPage");
            }


        }

        public ActionResult LogAttendance()
        {
                
                    try
            {
                        var trwla = from s in db.TRWLASchedules
                                    select s;

                        DateTime mydate = DateTime.Now.AddDays(-1);
                        DateTime myfuture = DateTime.Now;

                        trwla = trwla.Where(p => (p.ComEngEvent.ComEng_Date >= mydate || p.FunctionEvent.Function_Date >= mydate || p.Lecture.Lecture_Date >= mydate || p.GenEvent.Gen_Date >= mydate) && (p.ComEngEvent.ComEng_Date < myfuture || p.FunctionEvent.Function_Date < myfuture || p.Lecture.Lecture_Date < myfuture  ||  p.GenEvent.Gen_Date < myfuture));

                        return View(trwla.ToList());
                    }
                    catch
                    {
                        return RedirectToAction("ErrorPage");
                    }

        }

        public ActionResult LogAttendancePast()
        {
            try
            {
                var trwla = from s in db.TRWLASchedules
                            select s;

                trwla = trwla.Where(p => p.ComEngEvent.ComEng_Date <= DateTime.Now || p.FunctionEvent.Function_Date <= DateTime.Now || p.Lecture.Lecture_Date <= DateTime.Now);




                return View(trwla.ToList());
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        public ActionResult AddNewStudent(string sortOrder, string searchString)
        {
            try
            {
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.SurnameSortParm = String.IsNullOrEmpty(sortOrder) ? "sur_desc" : "Surname";

                int eventid = (int)TempData["NewStudentAdd"];
                TempData["NewStudentAdd"] = eventid;

                TRWLASchedule mysc = db.TRWLASchedules.Find(eventid);

                if (mysc.FunctionID != null)
                {
                    var rsvp = (from r in db.RSVP_Event
                                join s in db.TRWLASchedules on r.FunctionID equals s.FunctionID
                                join l in db.SYSUserProfiles on r.SYSUserProfileID equals l.SYSUserProfileID
                                where r.FunctionID == mysc.FunctionID
                                select l).ToList();

                    var students = (from s in db.SYSUserProfiles
                                    where s.UserTypeID == 1
                                    select s).ToList();

                    students.RemoveAll(x => rsvp.Contains(x));

                    if (!String.IsNullOrEmpty(searchString))
                    {
                        var t = (from l in students
                                 where l.FirstName.Contains(searchString)
                                 select l).ToList();

                        return View(t);
                    }


                    return View(students);
                }
                else if (mysc.LectureID != null)
                {
                    var rsvple = (from r in db.RSVP_Event
                                  join s in db.TRWLASchedules on r.FunctionID equals s.FunctionID
                                  join l in db.SYSUserProfiles on r.SYSUserProfileID equals l.SYSUserProfileID
                                  where r.LectureID == mysc.LectureID
                                  select l).ToList();

                    var studle = (from s in db.SYSUserProfiles
                                  where s.UserTypeID == 1
                                  select s).ToList();

                    studle.RemoveAll(x => rsvple.Contains(x));




                    if (!String.IsNullOrEmpty(searchString))
                    {
                        var t = (from l in studle
                                 where l.FirstName.Contains(searchString)
                                 select l).ToList();

                        return View(t);
                    }


                    return View(studle);
                }
                else if (mysc.ComEngID != null)
                {
                    var rsvple = (from r in db.RSVP_Event
                                  join s in db.TRWLASchedules on r.FunctionID equals s.FunctionID
                                  join l in db.SYSUserProfiles on r.SYSUserProfileID equals l.SYSUserProfileID
                                  where r.ComEngID == mysc.ComEngID
                                  select l).ToList();

                    var studle = (from s in db.SYSUserProfiles
                                  where s.UserTypeID == 1
                                  select s).ToList();

                    studle.RemoveAll(x => rsvple.Contains(x));




                    if (!String.IsNullOrEmpty(searchString))
                    {
                        var t = (from l in studle
                                 where l.FirstName.Contains(searchString)
                                 select l).ToList();

                        return View(t);
                    }


                    return View(studle);
                }
                else if (mysc.GenID != null)
                {
                    var rsvple = (from r in db.RSVP_Event
                                  join s in db.TRWLASchedules on r.FunctionID equals s.FunctionID
                                  join l in db.SYSUserProfiles on r.SYSUserProfileID equals l.SYSUserProfileID
                                  where r.GenID == mysc.GenID
                                  select l).ToList();

                    var studle = (from s in db.SYSUserProfiles
                                  where s.UserTypeID == 1
                                  select s).ToList();

                    studle.RemoveAll(x => rsvple.Contains(x));




                    if (!String.IsNullOrEmpty(searchString))
                    {
                        var t = (from l in studle
                                 where l.FirstName.Contains(searchString)
                                 select l).ToList();

                        return View(t);
                    }


                    return View(studle);
                }
                else
                {
                    return RedirectToAction("Index");
                }


            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        public ActionResult StudentDetails(int? id)
        {
            try
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
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        [HttpPost, ActionName("StudentDetails")]
        [ValidateAntiForgeryToken]
        public ActionResult AttendanceConfirmed(Attendance att, int id)
        {
            try
            {
                

                int stude = Convert.ToInt32(TempData["NewStudent"]);

                SYSUserProfile stud = db.SYSUserProfiles.Find(id);
                TRWLASchedule ev = db.TRWLASchedules.Find(stude);

                
                var FindUser = (int)Session["User"];

                progressbar prog = new progressbar();

                int i = db.Attendances.Count();

                try
                {
                    if (i == 0)
                    {
                        if (ev.FunctionID != null)
                        {
                            att.FunctionID = ev.FunctionID;
                            att.SYSUserProfileID = stud.SYSUserProfileID;

                            progressbar myProgress = db.progressbars.FirstOrDefault(p => p.SYSUserProfileID == stud.SYSUserProfileID);


                            int count = db.RSVP_Event.Max(p => p.rsvpID);
                            int k = count + 1;
                            RSVP_Event @event = new RSVP_Event();
                            @event.SYSUserProfileID = stud.SYSUserProfileID;
                            @event.FunctionID = ev.FunctionID;
                            @event.rsvpID = k;
                            @event.Attended = 1;

                            RSVPSchedule mysched = new RSVPSchedule();
                            mysched.rsvpID = @event.rsvpID;
                            mysched.ScheduleID = stude;
                            mysched.SYSUserProfileID = stud.SYSUserProfileID;
                            db.RSVPSchedules.Add(mysched);
                            db.RSVP_Event.Add(@event);


                            if (myProgress != null)
                            {
                                myProgress.FuncProg = myProgress.FuncProg + 1;

                                MasterData master = db.MasterDatas.Find(9);

                                if (myProgress.LecProg == master.LecAttend && myProgress.FuncProg == master.FuncAttend && myProgress.GenProg == master.GenAttend && myProgress.ComProg == master.ComAttend)
                                {
                                    SYSUserProfile myuser = db.SYSUserProfiles.Find(FindUser);
                                    myuser.Graduate = "Graduate";
                                }
                            }
                            else
                            {
                                prog.SYSUserProfileID = stud.SYSUserProfileID;
                                prog.FuncProg = 0;
                                prog.LecProg = 0;
                                prog.ComProg = 0;
                                prog.GenProg = 0;

                                prog.FuncProg = prog.FuncProg + 1;

                                db.progressbars.Add(prog);

                            }

                            ClassAttendance myclass = db.ClassAttendances.Find(1);
                            myclass.attend = myclass.attend + 1;
                            
                           

                            db.Attendances.Add(att);
                        }
                        else if (ev.LectureID != null)
                        {
                            att.LectureID = ev.LectureID;
                            att.SYSUserProfileID = stud.SYSUserProfileID;
                            progressbar myProgress = db.progressbars.FirstOrDefault(p => p.SYSUserProfileID == stud.SYSUserProfileID);

                            int count = db.RSVP_Event.Max(p => p.rsvpID);
                            int k = count + 1;
                            RSVP_Event @event = new RSVP_Event();
                            @event.SYSUserProfileID = stud.SYSUserProfileID;
                            @event.LectureID = ev.LectureID;
                            @event.rsvpID = k;
                            @event.Attended = 1;

                            RSVPSchedule mysched = new RSVPSchedule();
                            mysched.rsvpID = @event.rsvpID;
                            mysched.ScheduleID = stude;
                            mysched.SYSUserProfileID = stud.SYSUserProfileID;
                            db.RSVPSchedules.Add(mysched);
                            db.RSVP_Event.Add(@event);

                            if (myProgress != null)
                            {
                                myProgress.LecProg = myProgress.LecProg + 1;

                                MasterData master = db.MasterDatas.Find(9);

                                if (myProgress.LecProg == master.LecAttend && myProgress.FuncProg == master.FuncAttend && myProgress.GenProg == master.GenAttend && myProgress.ComProg == master.ComAttend)
                                {
                                    SYSUserProfile myuser = db.SYSUserProfiles.Find(FindUser);
                                    myuser.Graduate = "Graduate";
                                }
                            }
                            else
                            {
                                prog.SYSUserProfileID = stud.SYSUserProfileID;
                                prog.FuncProg = 0;
                                prog.LecProg = 0;
                                prog.ComProg = 0;
                                prog.GenProg = 0;

                                prog.LecProg = prog.LecProg + 1;

                                db.progressbars.Add(prog);

                            }

                            ClassAttendance myclass = db.ClassAttendances.Find(2);
                            myclass.attend = myclass.attend + 1;

                            db.Attendances.Add(att);
                        }
                        else if (ev.ComEngID != null)
                        {
                            att.ComEngID = ev.ComEngID;
                            att.SYSUserProfileID = stud.SYSUserProfileID;

                            int count = db.RSVP_Event.Max(p => p.rsvpID);
                            int k = count + 1;
                            RSVP_Event @event = new RSVP_Event();
                            @event.SYSUserProfileID = stud.SYSUserProfileID;
                            @event.ComEngID = ev.ComEngID;
                            @event.rsvpID = k;
                            @event.Attended = 1;

                            RSVPSchedule mysched = new RSVPSchedule();
                            mysched.rsvpID = @event.rsvpID;
                            mysched.ScheduleID = stude;
                            mysched.SYSUserProfileID = stud.SYSUserProfileID;
                            db.RSVPSchedules.Add(mysched);
                            db.RSVP_Event.Add(@event);


                            progressbar myProgress = db.progressbars.FirstOrDefault(p => p.SYSUserProfileID == stud.SYSUserProfileID);

                            if (myProgress != null)
                            {
                                myProgress.ComProg = myProgress.ComProg + 1;

                                MasterData master = db.MasterDatas.Find(9);

                                if (myProgress.LecProg == master.LecAttend && myProgress.FuncProg == master.FuncAttend && myProgress.GenProg == master.GenAttend && myProgress.ComProg == master.ComAttend)
                                {
                                    SYSUserProfile myuser = db.SYSUserProfiles.Find(FindUser);
                                    myuser.Graduate = "Graduate";
                                }
                            }
                            else
                            {
                                prog.SYSUserProfileID = stud.SYSUserProfileID;
                                prog.FuncProg = 0;
                                prog.LecProg = 0;
                                prog.ComProg = 0;
                                prog.GenProg = 0;

                                prog.ComProg = prog.ComProg + 1;

                                db.progressbars.Add(prog);

                            }

                            ClassAttendance myclass = db.ClassAttendances.Find(3);
                            myclass.attend = myclass.attend + 1;

                            db.Attendances.Add(att);
                        }
                        else
                        {
                            att.GenID = ev.GenID;
                            att.SYSUserProfileID = stud.SYSUserProfileID;

                            int count = db.RSVP_Event.Max(p => p.rsvpID);
                            int k = count + 1;
                            RSVP_Event @event = new RSVP_Event();
                            @event.SYSUserProfileID = stud.SYSUserProfileID;
                            @event.GenID = ev.GenID;
                            @event.rsvpID = k;
                            @event.Attended = 1;

                            RSVPSchedule mysched = new RSVPSchedule();
                            mysched.rsvpID = @event.rsvpID;
                            mysched.ScheduleID = stude;
                            mysched.SYSUserProfileID = stud.SYSUserProfileID;
                            db.RSVPSchedules.Add(mysched);
                            db.RSVP_Event.Add(@event);

                            progressbar myProgress = db.progressbars.FirstOrDefault(p => p.SYSUserProfileID == stud.SYSUserProfileID);

                            if (myProgress != null)
                            {
                                myProgress.GenProg = myProgress.GenProg + 1;

                                MasterData master = db.MasterDatas.Find(9);

                                if (myProgress.LecProg == master.LecAttend && myProgress.FuncProg == master.FuncAttend && myProgress.GenProg == master.GenAttend && myProgress.ComProg == master.ComAttend)
                                {
                                    SYSUserProfile myuser = db.SYSUserProfiles.Find(FindUser);
                                    myuser.Graduate = "Graduate";
                                }
                            }
                            else
                            {
                                prog.SYSUserProfileID = stud.SYSUserProfileID;
                                prog.FuncProg = 0;
                                prog.LecProg = 0;
                                prog.ComProg = 0;
                                prog.GenProg = 0;

                                prog.GenProg = prog.GenProg + 1;

                                db.progressbars.Add(prog);

                            }

                            ClassAttendance myclass = db.ClassAttendances.Find(4);
                            myclass.attend = myclass.attend + 1;


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

                            progressbar myProgress = db.progressbars.FirstOrDefault(p => p.SYSUserProfileID == stud.SYSUserProfileID);

                            int count = db.RSVP_Event.Max(p => p.rsvpID);
                            int k = count + 1;
                            RSVP_Event @event = new RSVP_Event();
                            @event.SYSUserProfileID = stud.SYSUserProfileID;
                            @event.FunctionID = ev.FunctionID;
                            @event.rsvpID = k;
                            @event.Attended = 1;

                            RSVPSchedule mysched = new RSVPSchedule();
                            mysched.rsvpID = @event.rsvpID;
                            mysched.ScheduleID = stude;
                            mysched.SYSUserProfileID = stud.SYSUserProfileID;
                            db.RSVPSchedules.Add(mysched);
                            db.RSVP_Event.Add(@event);
                            

                            if (myProgress != null)
                            {
                                myProgress.FuncProg = myProgress.FuncProg + 1;

                                MasterData master = db.MasterDatas.Find(9);

                                if (myProgress.LecProg == master.LecAttend && myProgress.FuncProg == master.FuncAttend && myProgress.GenProg == master.GenAttend && myProgress.ComProg == master.ComAttend)
                                {
                                    SYSUserProfile myuser = db.SYSUserProfiles.Find(FindUser);
                                    myuser.Graduate = "Graduate";
                                }
                            }
                            else
                            {
                                prog.SYSUserProfileID = stud.SYSUserProfileID;
                                prog.FuncProg = 0;
                                prog.LecProg = 0;
                                prog.ComProg = 0;
                                prog.GenProg = 0;

                                prog.FuncProg = prog.FuncProg + 1;

                                db.progressbars.Add(prog);

                            }
                            ClassAttendance myclass = db.ClassAttendances.Find(1);
                            myclass.attend = myclass.attend + 1;


                            db.Attendances.Add(att);
                        }
                        else if (ev.LectureID != null)
                        {
                            att.LectureID = ev.LectureID;
                            att.SYSUserProfileID = stud.SYSUserProfileID;

                            int count = db.RSVP_Event.Max(p => p.rsvpID);
                            int k = count + 1;
                            RSVP_Event @event = new RSVP_Event();
                            @event.SYSUserProfileID = stud.SYSUserProfileID;
                            @event.LectureID = ev.LectureID;
                            @event.rsvpID = k;
                            @event.Attended = 1;

                            RSVPSchedule mysched = new RSVPSchedule();
                            mysched.rsvpID = @event.rsvpID;
                            mysched.ScheduleID = stude;
                            mysched.SYSUserProfileID = stud.SYSUserProfileID;
                            db.RSVPSchedules.Add(mysched);
                            db.RSVP_Event.Add(@event);

                            progressbar myProgress = db.progressbars.FirstOrDefault(p => p.SYSUserProfileID == stud.SYSUserProfileID);

                            if (myProgress != null)
                            {
                                myProgress.LecProg = myProgress.LecProg + 1;

                                MasterData master = db.MasterDatas.Find(9);

                                if (myProgress.LecProg == master.LecAttend && myProgress.FuncProg == master.FuncAttend && myProgress.GenProg == master.GenAttend && myProgress.ComProg == master.ComAttend)
                                {
                                    SYSUserProfile myuser = db.SYSUserProfiles.Find(FindUser);
                                    myuser.Graduate = "Graduate";
                                }
                            }
                            else
                            {
                                prog.SYSUserProfileID = stud.SYSUserProfileID;
                                prog.FuncProg = 0;
                                prog.LecProg = 0;
                                prog.ComProg = 0;
                                prog.GenProg = 0;

                                prog.LecProg = prog.LecProg + 1;

                                db.progressbars.Add(prog);

                            }
                            ClassAttendance myclass = db.ClassAttendances.Find(2);
                            myclass.attend = myclass.attend + 1;

                            db.Attendances.Add(att);
                        }
                        else if (ev.ComEngID != null)
                        {
                            att.ComEngID = ev.ComEngID;
                            att.SYSUserProfileID = stud.SYSUserProfileID;

                            int count = db.RSVP_Event.Max(p => p.rsvpID);
                            int k = count + 1;
                            RSVP_Event @event = new RSVP_Event();
                            @event.SYSUserProfileID = stud.SYSUserProfileID;
                            @event.ComEngID = ev.ComEngID;
                            @event.rsvpID = k;
                            @event.Attended = 1;

                            RSVPSchedule mysched = new RSVPSchedule();
                            mysched.rsvpID = @event.rsvpID;
                            mysched.ScheduleID = stude;
                            mysched.SYSUserProfileID = stud.SYSUserProfileID;
                            db.RSVPSchedules.Add(mysched);
                            db.RSVP_Event.Add(@event);

                            progressbar myProgress = db.progressbars.FirstOrDefault(p => p.SYSUserProfileID == stud.SYSUserProfileID);

                            if (myProgress != null)
                            {
                                myProgress.ComProg = myProgress.ComProg + 1;

                                MasterData master = db.MasterDatas.Find(9);

                                if (myProgress.LecProg == master.LecAttend && myProgress.FuncProg == master.FuncAttend && myProgress.GenProg == master.GenAttend && myProgress.ComProg == master.ComAttend)
                                {
                                    SYSUserProfile myuser = db.SYSUserProfiles.Find(FindUser);
                                    myuser.Graduate = "Graduate";
                                }
                            }
                            else
                            {
                                prog.SYSUserProfileID = stud.SYSUserProfileID;
                                prog.FuncProg = 0;
                                prog.LecProg = 0;
                                prog.ComProg = 0;
                                prog.GenProg = 0;

                                prog.ComProg = prog.ComProg + 1;

                                db.progressbars.Add(prog);

                            }
                            ClassAttendance myclass = db.ClassAttendances.Find(3);
                            myclass.attend = myclass.attend + 1;

                            db.Attendances.Add(att);
                        }
                        else
                        {
                            att.GenID = ev.GenID;
                            att.SYSUserProfileID = stud.SYSUserProfileID;

                            int count = db.RSVP_Event.Max(p => p.rsvpID);
                            int k = count + 1;
                            RSVP_Event @event = new RSVP_Event();
                            @event.SYSUserProfileID = stud.SYSUserProfileID;
                            @event.GenID = ev.GenID;
                            @event.rsvpID = k;
                            @event.Attended = 1;

                            RSVPSchedule mysched = new RSVPSchedule();
                            mysched.rsvpID = @event.rsvpID;
                            mysched.ScheduleID = stude;
                            mysched.SYSUserProfileID = stud.SYSUserProfileID;
                            db.RSVPSchedules.Add(mysched);
                            db.RSVP_Event.Add(@event);

                            progressbar myProgress = db.progressbars.FirstOrDefault(p => p.SYSUserProfileID == stud.SYSUserProfileID);

                            if (myProgress != null)
                            {
                                myProgress.GenProg = myProgress.GenProg + 1;

                                MasterData master = db.MasterDatas.Find(9);

                                if (myProgress.LecProg == master.LecAttend && myProgress.FuncProg == master.FuncAttend && myProgress.GenProg == master.GenAttend && myProgress.ComProg == master.ComAttend)
                                {
                                    SYSUserProfile myuser = db.SYSUserProfiles.Find(FindUser);
                                    myuser.Graduate = "Graduate";
                                }
                            }
                            else
                            {
                                prog.SYSUserProfileID = stud.SYSUserProfileID;
                                prog.FuncProg = 0;
                                prog.LecProg = 0;
                                prog.ComProg = 0;
                                prog.GenProg = 0;

                                prog.GenProg = prog.GenProg + 1;

                                db.progressbars.Add(prog);

                            }

                            ClassAttendance myclass = db.ClassAttendances.Find(4);
                            myclass.attend = myclass.attend + 1;
                            db.Attendances.Add(att);
                        }
                    }
                    AuditLog myAudit = new AuditLog();
                    myAudit.DateDone = DateTime.Now;
                    myAudit.TypeTran = "Create";
                    myAudit.SYSUserProfileID = (int)Session["User"];
                    myAudit.TableAff = "Attendance";
                    db.AuditLogs.Add(myAudit);

                    myAudit.DateDone = DateTime.Now;
                    myAudit.TypeTran = "Update";
                    myAudit.SYSUserProfileID = (int)Session["User"];
                    myAudit.TableAff = "RSVP_Event";
                    db.AuditLogs.Add(myAudit);

                    db.SaveChanges();
                    
                }
                catch (Exception)
                {
                    TempData["Attend"] = "There was an error in the attempt of logging the attendance of the student";
                }
                return RedirectToAction("LogAttendance");
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        public ActionResult RSVPdMembers(int? id, string sortOrder, string searchString)
        {
            try
            {
                

                MasterData mydata = db.MasterDatas.Find(9);
                int time = Convert.ToInt32(mydata.LogAttendTime);

                int mytime = DateTime.Now.TimeOfDay.Hours;

                TRWLASchedule tRWLASchedule = db.TRWLASchedules.Find(id);
                if (tRWLASchedule.FunctionID != null)
                {
                    int m = tRWLASchedule.FunctionEvent.Function_StartTime.Hours;
                    ViewBag.Name = tRWLASchedule.FunctionEvent.Function_Name;
                    int diff = m - mytime;

                    if (diff > time)
                    {
                        TempData["Log"] = "You cannot log event attendance until it is " + time.ToString() + " hour/s before the event starts";
                        return RedirectToAction("LogAttendance");
                    }

                }
                else if (tRWLASchedule.LectureID != null)
                {
                    int m = tRWLASchedule.Lecture.Lecture_StartTime.Hours;
                    int diff = m - mytime;
                    ViewBag.Name = tRWLASchedule.Lecture.Lecture_Name;
                    if (diff > time)
                    {
                        TempData["Log"] = "You cannot log event attendance until it is " + time.ToString() + " hour/s before the event starts";
                        return RedirectToAction("LogAttendance");
                    }

                }
                else if (tRWLASchedule.GenID != null)
                {
                    int m = tRWLASchedule.GenEvent.Gen_StartTime.Hours;
                    int diff = m - mytime;
                    ViewBag.Name = tRWLASchedule.GenEvent.Gen_Name;
                    if (diff > time)
                    {
                        TempData["Log"] = "You cannot log event attendance until it is " + time.ToString() + " hour/s before the event starts";
                        return RedirectToAction("LogAttendance");
                    }
                }
                else if (tRWLASchedule.ComEngID != null)
                {
                    int m = tRWLASchedule.ComEngEvent.ComEnge_StartTime.Hours;
                    int diff = m - mytime;
                    ViewBag.Name = tRWLASchedule.ComEngEvent.ComEng_Name;
                    if (diff > time)
                    {
                        TempData["Log"] = "You cannot log event attendance until it is " + time.ToString() + " hour/s before the event starts";
                        return RedirectToAction("LogAttendance");
                    }
                }

                try
                {

                    TempData["NewStudent"] = id;

                    if (tRWLASchedule.FunctionID != null)
                    {
                        var evnt = from s in db.SYSUserProfiles
                                   join r in db.RSVP_Event on s.SYSUserProfileID equals r.SYSUserProfileID
                                   join t in db.TRWLASchedules on r.FunctionID equals t.FunctionID
                                   where r.FunctionID == tRWLASchedule.FunctionID
                                   select r;
                        RSVP_Event myevent = db.RSVP_Event.FirstOrDefault(p => p.FunctionID == id);

                        TempData["NewStudentAdd"] = tRWLASchedule.ScheduleID;


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
                        

                        TempData["NewStudentAdd"] = tRWLASchedule.ScheduleID;

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
                    else if (tRWLASchedule.ComEngID != null)
                    {
                        var evnt = from s in db.SYSUserProfiles
                                   join r in db.RSVP_Event on s.SYSUserProfileID equals r.SYSUserProfileID
                                   join t in db.TRWLASchedules on r.ComEngID equals t.ComEngID
                                   where r.ComEngID == tRWLASchedule.ComEngID
                                   select r;
                        ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                        ViewBag.SurnameSortParm = String.IsNullOrEmpty(sortOrder) ? "sur_desc" : "Surname";

                        RSVP_Event myevent = db.RSVP_Event.FirstOrDefault(p => p.ComEngID == id);

                        TempData["NewStudentAdd"] = tRWLASchedule.ScheduleID;

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
                    else if (tRWLASchedule.GenID != null)
                    {
                        var evnt = from s in db.SYSUserProfiles
                                   join r in db.RSVP_Event on s.SYSUserProfileID equals r.SYSUserProfileID
                                   join t in db.TRWLASchedules on r.GenID equals t.GenID
                                   where r.GenID == tRWLASchedule.GenID
                                   select r;
                        ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                        ViewBag.SurnameSortParm = String.IsNullOrEmpty(sortOrder) ? "sur_desc" : "Surname";

                        RSVP_Event myevent = db.RSVP_Event.FirstOrDefault(p => p.GenID == id);

                        TempData["NewStudentAdd"] = tRWLASchedule.ScheduleID;

                        if (!String.IsNullOrEmpty(searchString))
                        {
                            evnt = evnt.Where(s => s.SYSUserProfile.FirstName.Contains(searchString));
                        }

                        return View(evnt.ToList());
                    }

                    return RedirectToAction("Index");
                }
                catch
                {
                    return RedirectToAction("ErrorPage");
                }

            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }



        public ActionResult ConfirmAttendance(int? studid, int? evid)
        {
            try
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
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        [HttpPost, ActionName("ConfirmAttendance")]
        [ValidateAntiForgeryToken]
        public ActionResult AttendanceConfirmed(Attendance att, int evid, int studid)
        {
            try
            {
                SYSUserProfile stud = db.SYSUserProfiles.Find(studid);
                RSVP_Event ev = db.RSVP_Event.Find(evid);
                var FindUser = (int)Session["User"];

                progressbar prog = new progressbar();

                ev.Attended = 1;

                int i = db.Attendances.Count();

                try
                {
                    if (i == 0)
                    {
                        if (ev.FunctionID != null)
                        {
                            att.FunctionID = ev.FunctionID;
                            att.SYSUserProfileID = stud.SYSUserProfileID;
                            progressbar myProgress = db.progressbars.FirstOrDefault(p => p.SYSUserProfileID == studid);

                            if (myProgress != null)
                            {
                                myProgress.FuncProg = myProgress.FuncProg + 1;
                            }
                            else
                            {
                                prog.SYSUserProfileID = studid;
                                prog.FuncProg = 0;
                                prog.LecProg = 0;
                                prog.ComProg = 0;
                                prog.GenProg = 0;

                                prog.FuncProg = prog.FuncProg + 1;

                                db.progressbars.Add(prog);

                            }
                            ClassAttendance myclass = db.ClassAttendances.Find(1);
                            myclass.attend = myclass.attend + 1;

                            ev.Attended = 1;
                            db.Attendances.Add(att);
                        }
                        else if (ev.LectureID != null)
                        {
                            att.LectureID = ev.LectureID;
                            att.SYSUserProfileID = stud.SYSUserProfileID;
                            progressbar myProgress = db.progressbars.FirstOrDefault(p => p.SYSUserProfileID == studid);

                            if (myProgress != null)
                            {
                                myProgress.LecProg = myProgress.LecProg + 1;
                            }
                            else
                            {
                                prog.SYSUserProfileID = studid;
                                prog.FuncProg = 0;
                                prog.LecProg = 0;
                                prog.ComProg = 0;
                                prog.GenProg = 0;

                                prog.LecProg = prog.LecProg + 1;

                                db.progressbars.Add(prog);

                            }
                            ClassAttendance myclass = db.ClassAttendances.Find(2);
                            myclass.attend = myclass.attend + 1;
                            db.Attendances.Add(att);
                        }
                        else if (ev.ComEngID != null)
                        {
                            att.ComEngID = ev.ComEngID;
                            att.SYSUserProfileID = stud.SYSUserProfileID;
                            progressbar myProgress = db.progressbars.FirstOrDefault(p => p.SYSUserProfileID == studid);

                            if (myProgress != null)
                            {
                                myProgress.ComProg = myProgress.ComProg + 1;
                            }
                            else
                            {
                                prog.SYSUserProfileID = studid;
                                prog.FuncProg = 0;
                                prog.LecProg = 0;
                                prog.ComProg = 0;
                                prog.GenProg = 0;

                                prog.ComProg = prog.ComProg + 1;

                                db.progressbars.Add(prog);

                            }
                            ClassAttendance myclass = db.ClassAttendances.Find(3);
                            myclass.attend = myclass.attend + 1;

                            db.Attendances.Add(att);
                        }
                        else
                        {
                            att.GenID = ev.GenID;
                            att.SYSUserProfileID = stud.SYSUserProfileID;
                            progressbar myProgress = db.progressbars.FirstOrDefault(p => p.SYSUserProfileID == studid);

                            if (myProgress != null)
                            {
                                myProgress.GenProg = myProgress.GenProg + 1;
                            }
                            else
                            {
                                prog.SYSUserProfileID = studid;
                                prog.FuncProg = 0;
                                prog.LecProg = 0;
                                prog.ComProg = 0;
                                prog.GenProg = 0;

                                prog.GenProg = prog.GenProg + 1;

                                db.progressbars.Add(prog);

                            }
                            ClassAttendance myclass = db.ClassAttendances.Find(4);
                            myclass.attend = myclass.attend + 1;
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

                            progressbar myProgress = db.progressbars.FirstOrDefault(p => p.SYSUserProfileID == studid);

                            if (myProgress != null)
                            {
                                myProgress.FuncProg = myProgress.FuncProg + 1;
                            }
                            else
                            {
                                prog.SYSUserProfileID = studid;
                                prog.FuncProg = 0;
                                prog.LecProg = 0;
                                prog.ComProg = 0;
                                prog.GenProg = 0;

                                prog.FuncProg = prog.FuncProg + 1;

                                db.progressbars.Add(prog);

                            }
                            ClassAttendance myclass = db.ClassAttendances.Find(1);
                            myclass.attend = myclass.attend + 1;

                            db.Attendances.Add(att);
                        }
                        else if (ev.LectureID != null)
                        {
                            att.LectureID = ev.LectureID;
                            att.SYSUserProfileID = stud.SYSUserProfileID;
                            progressbar myProgress = db.progressbars.FirstOrDefault(p => p.SYSUserProfileID == studid);

                            if (myProgress != null)
                            {
                                myProgress.LecProg = myProgress.LecProg + 1;
                            }
                            else
                            {
                                prog.SYSUserProfileID = studid;
                                prog.FuncProg = 0;
                                prog.LecProg = 0;
                                prog.ComProg = 0;
                                prog.GenProg = 0;

                                prog.LecProg = prog.LecProg + 1;

                                db.progressbars.Add(prog);

                            }
                            ClassAttendance myclass = db.ClassAttendances.Find(2);
                            myclass.attend = myclass.attend + 1;
                            db.Attendances.Add(att);
                        }
                        else if (ev.ComEngID != null)
                        {
                            att.ComEngID = ev.ComEngID;
                            att.SYSUserProfileID = stud.SYSUserProfileID;
                            progressbar myProgress = db.progressbars.FirstOrDefault(p => p.SYSUserProfileID == studid);

                            if (myProgress != null)
                            {
                                myProgress.ComProg = myProgress.ComProg + 1;
                            }
                            else
                            {
                                prog.SYSUserProfileID = studid;
                                prog.FuncProg = 0;
                                prog.LecProg = 0;
                                prog.ComProg = 0;
                                prog.GenProg = 0;

                                prog.ComProg = prog.ComProg + 1;

                                db.progressbars.Add(prog);

                            }
                            ClassAttendance myclass = db.ClassAttendances.Find(3);
                            myclass.attend = myclass.attend + 1;
                            db.Attendances.Add(att);
                        }
                        else
                        {
                            att.GenID = ev.GenID;
                            att.SYSUserProfileID = stud.SYSUserProfileID;
                            progressbar myProgress = db.progressbars.FirstOrDefault(p => p.SYSUserProfileID == studid);

                            if (myProgress != null)
                            {
                                myProgress.GenProg = myProgress.GenProg + 1;
                            }
                            else
                            {
                                prog.SYSUserProfileID = studid;
                                prog.FuncProg = 0;
                                prog.LecProg = 0;
                                prog.ComProg = 0;
                                prog.GenProg = 0;

                                prog.GenProg = prog.GenProg + 1;

                                db.progressbars.Add(prog);

                            }
                            ClassAttendance myclass = db.ClassAttendances.Find(4);
                            myclass.attend = myclass.attend + 1;
                            db.Attendances.Add(att);
                        }
                    }
                    AuditLog myAudit = new AuditLog();
                    myAudit.DateDone = DateTime.Now;
                    myAudit.TypeTran = "Create";
                    myAudit.SYSUserProfileID = (int)Session["User"];
                    myAudit.TableAff = "Attendance";
                    db.AuditLogs.Add(myAudit);

                    myAudit.DateDone = DateTime.Now;
                    myAudit.TypeTran = "Update";
                    myAudit.SYSUserProfileID = (int)Session["User"];
                    myAudit.TableAff = "RSVP_Event";
                    db.AuditLogs.Add(myAudit);

                    db.SaveChanges();
                    
                }
                catch (Exception)
                {
                    TempData["Attend"] = "There was an error in the attempt of logging the attendance of the student";
                }
                return RedirectToAction("LogAttendance");
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        public ActionResult WriteReview(int? id)
        {
            try

            {
                RSVP_Event tRWLASchedule = db.RSVP_Event.Find(id);
                Lecture lec = db.Lectures.Find(tRWLASchedule.LectureID);

                ViewBag.LectureName = lec.Lecture_Name;
                ViewBag.RatingID = new SelectList(db.RatingTypes, "RatingID", "Rating");
                return View();
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        [HttpPost, ActionName("WriteReview")]
        [ValidateAntiForgeryToken]
        public ActionResult WriteReviewConfirmed([Bind(Include = "reviewID, Review, RatingID, StudentID, VolunteerID, LectureID")] LectureReview LecRev, int id)
        {
            try
            {
                var user = (int)Session["User"];

                if (ModelState.IsValid)
                {
                    int i = db.LectureReviews.Count();
                    RSVP_Event tRWLASchedule = db.RSVP_Event.Find(id);
                    Lecture lec = db.Lectures.Find(tRWLASchedule.LectureID);

                    if (i == 0)
                    {
                        LecRev.LectureID = lec.LectureID;
                        LecRev.SYSUserProfileID = user;

                        db.LectureReviews.Add(LecRev);

                        AuditLog myAudit = new AuditLog();
                        myAudit.DateDone = DateTime.Now;
                        myAudit.TypeTran = "Create";
                        myAudit.SYSUserProfileID = (int)Session["User"];
                        myAudit.TableAff = "LectureReview";
                        db.AuditLogs.Add(myAudit);

                        db.SaveChanges();
                        return RedirectToAction("StudentMainMenu");
                    }

                    else if (i != 0)
                    {
                        int max = db.LectureReviews.Max(p => p.reviewID);
                        int k = max + 1;
                        LecRev.LectureID = lec.LectureID;
                        LecRev.reviewID = k;
                        LecRev.SYSUserProfileID = user;

                        AuditLog myAudit = new AuditLog();
                        myAudit.DateDone = DateTime.Now;
                        myAudit.TypeTran = "Create";
                        myAudit.SYSUserProfileID = (int)Session["User"];
                        myAudit.TableAff = "LectureReview";
                        db.AuditLogs.Add(myAudit);

                        db.LectureReviews.Add(LecRev);
                        db.SaveChanges();
                        return RedirectToAction("StudentMainMenu");

                    }


                }
                ViewBag.RatingID = new SelectList(db.RatingTypes, "RatingID", "Rating");
                return View(LecRev);
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }




        // GET: TRWLASchedules/Details/5
        public ActionResult Details(int? id)
        {
            try
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
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        public ActionResult StudentEventDetails(int? id)
        {
            try {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                TRWLASchedule tRWLASchedule = db.TRWLASchedules.Find(id);

                if (tRWLASchedule.FunctionID != null)
                {
                    TempData["guestspeaker"] = tRWLASchedule.FunctionEvent.GuestSpeakerID;
                }

                TempData["EventIdNeeded"] = tRWLASchedule.ScheduleID;

                if (tRWLASchedule == null)
                {
                    return HttpNotFound();
                }
                return View(tRWLASchedule);
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        public ActionResult StudentGuestSpeaker(int? id)
        {
            try
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
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        //RSVP to an EVENT
        public ActionResult RSVP(int? id)
        {
            try
            {
                
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                TRWLASchedule tRWLASchedule = db.TRWLASchedules.Find(id);
                TempData["EventIdNeeded"] = id;
                if (tRWLASchedule == null)
                {
                    return HttpNotFound();
                }

                if (tRWLASchedule.FunctionID != null)
                {
                    ViewBag.Name = tRWLASchedule.FunctionEvent.Function_Name;
                    TempData["Guest"] = tRWLASchedule.FunctionEvent.GuestSpeakerID;
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
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        [HttpPost, ActionName("RSVP")]
        [ValidateAntiForgeryToken]
        public ActionResult RSVPConfirmed(RSVP_Event @event, int id)
        {
            try
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
                            //if (s.Attended == null)
                            //{
                            //    TempData["Attended"] = "You have already attended this event and cannot RSVP again.";
                            //    return RedirectToAction("StudentMainMenu");
                            //}
                            if (s.SYSUserProfileID == user)
                            {
                                TempData["rsvpFAIL"] = "You have already RSVPd to this event";
                                return RedirectToAction("StudentMainMenu");
                            }
                            else if (s.SYSUserProfileID == user)
                            {
                                TempData["rsvp"] = "You have successfully RSVPd to the event: " + tRWLASchedule.FunctionEvent.Function_Name;
                            }
                        }
                        AuditLog myAudit = new AuditLog();
                        myAudit.DateDone = DateTime.Now;
                        myAudit.TypeTran = "Create";
                        myAudit.SYSUserProfileID = (int)Session["User"];
                        myAudit.TableAff = "RSVP_Event";
                        db.AuditLogs.Add(myAudit);

                        db.SaveChanges();
                        return RedirectToAction("StudentMainMenu");
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
                            //if (s.Attended != null)
                            //{
                            //    TempData["Attended"] = "You have already attended this event and cannot RSVP again.";
                            //    return RedirectToAction("StudentMainMenu");
                            //}
                            if (s.SYSUserProfileID == user)
                            {
                                TempData["rsvpFAIL"] = "You have already RSVPd to this event";
                                return RedirectToAction("StudentMainMenu");
                            }
                            else if (s.SYSUserProfileID != user)
                            {

                                TempData["rsvp"] = "You have successfully RSVPd to the event: " + tRWLASchedule.Lecture.Lecture_Name;
                            }
                        }
                        AuditLog myAudit = new AuditLog();
                        myAudit.DateDone = DateTime.Now;
                        myAudit.TypeTran = "Create";
                        myAudit.SYSUserProfileID = (int)Session["User"];
                        myAudit.TableAff = "RSVP_Event";
                        db.AuditLogs.Add(myAudit);
                        db.SaveChanges();
                        return RedirectToAction("StudentMainMenu");
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

                            //if (s.Attended != null)
                            //{
                            //    TempData["Attended"] = "You have already attended this event and cannot RSVP again.";
                            //    return RedirectToAction("StudentMainMenu");
                            //}
                            if (s.SYSUserProfileID == user)
                            {
                                TempData["rsvpFAIL"] = "You have already RSVPd to this event";
                                return RedirectToAction("StudentMainMenu");
                            }
                            else if (s.SYSUserProfileID == user)
                            {

                                TempData["rsvp"] = "You have successfully RSVPd to the event: " + tRWLASchedule.ComEngEvent.ComEng_Name;
                            }
                        }
                        AuditLog myAudit = new AuditLog();
                        myAudit.DateDone = DateTime.Now;
                        myAudit.TypeTran = "Create";
                        myAudit.SYSUserProfileID = (int)Session["User"];
                        myAudit.TableAff = "RSVP_Event";
                        db.AuditLogs.Add(myAudit);
                        db.SaveChanges();
                        return RedirectToAction("StudentMainMenu");
                    }
                    else if (tRWLASchedule.GenID != null)
                    {
                        @event.GenID = tRWLASchedule.GenID;

                        RSVPSchedule mysched = new RSVPSchedule();
                        mysched.rsvpID = @event.rsvpID;
                        mysched.ScheduleID = id;
                        mysched.SYSUserProfileID = user;
                        db.RSVPSchedules.Add(mysched);
                        db.RSVP_Event.Add(@event);

                        foreach (var s in db.RSVP_Event.Where(p => p.GenID == tRWLASchedule.GenID))
                        {

                            //if (s.Attended != null)
                            //{
                            //    TempData["Attended"] = "You have already attended this event and cannot RSVP again.";
                            //    return RedirectToAction("StudentMainMenu");
                            //}
                            if (s.SYSUserProfileID == user)
                            {
                                TempData["rsvpFAIL"] = "You have already RSVPd to this event";
                                return RedirectToAction("StudentMainMenu");
                            }
                            else if (s.SYSUserProfileID == user)
                            {

                                TempData["rsvp"] = "You have successfully RSVPd to the event: " + tRWLASchedule.GenEvent.Gen_Name;
                            }
                        }
                        AuditLog myAudit = new AuditLog();
                        myAudit.DateDone = DateTime.Now;
                        myAudit.TypeTran = "Create";
                        myAudit.SYSUserProfileID = (int)Session["User"];
                        myAudit.TableAff = "RSVP_Event";
                        db.AuditLogs.Add(myAudit);
                        db.SaveChanges();
                        return RedirectToAction("StudentMainMenu");
                    }
                    return RedirectToAction("StudentMainMenu");
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
                            if (s.SYSUserProfileID == user)
                            {
                                TempData["rsvpFAIL"] = "You have already RSVPd to this event";
                                return RedirectToAction("StudentMainMenu");
                            }
                            else if (s.SYSUserProfileID == user)
                            {
                                TempData["rsvp"] = "You have successfully RSVPd to the event: " + tRWLASchedule.FunctionEvent.Function_Name;
                            }

                        }
                        AuditLog myAudit = new AuditLog();
                        myAudit.DateDone = DateTime.Now;
                        myAudit.TypeTran = "Create";
                        myAudit.SYSUserProfileID = (int)Session["User"];
                        myAudit.TableAff = "RSVP_Event";
                        db.AuditLogs.Add(myAudit);
                        db.SaveChanges();
                        return RedirectToAction("StudentMainMenu");
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
                            
                            if (s.SYSUserProfileID == user)
                            {
                                TempData["rsvpFAIL"] = "You have already RSVPd to this event";
                                return RedirectToAction("StudentMainMenu");
                            }
                            else if (s.SYSUserProfileID == user)
                            {

                                TempData["rsvp"] = "You have successfully RSVPd to the event: " + tRWLASchedule.ComEngEvent.ComEng_Name;
                            }

                        }
                        AuditLog myAudit = new AuditLog();
                        myAudit.DateDone = DateTime.Now;
                        myAudit.TypeTran = "Create";
                        myAudit.SYSUserProfileID = (int)Session["User"];
                        myAudit.TableAff = "RSVP_Event";
                        db.AuditLogs.Add(myAudit);
                        db.SaveChanges();
                        return RedirectToAction("StudentMainMenu");
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
                            
                            if (s.SYSUserProfileID == user)
                            {
                                TempData["rsvpFAIL"] = "You have already RSVPd to this event";
                                return RedirectToAction("StudentMainMenu");
                            }
                            else if (s.SYSUserProfileID == user)
                            {
                                TempData["rsvp"] = "You have successfully RSVPd to the event: " + tRWLASchedule.Lecture.Lecture_Name;
                            }

                        }
                        AuditLog myAudit = new AuditLog();
                        myAudit.DateDone = DateTime.Now;
                        myAudit.TypeTran = "Create";
                        myAudit.SYSUserProfileID = (int)Session["User"];
                        myAudit.TableAff = "RSVP_Event";
                        db.AuditLogs.Add(myAudit);

                        db.SaveChanges();
                        return RedirectToAction("StudentMainMenu");
                    }
                    else if (tRWLASchedule.GenID != null)
                    {
                        @event.GenID = tRWLASchedule.GenID;

                        RSVPSchedule mysched = new RSVPSchedule();
                        mysched.rsvpID = @event.rsvpID;
                        mysched.ScheduleID = id;
                        mysched.SYSUserProfileID = user;
                        db.RSVPSchedules.Add(mysched);
                        db.RSVP_Event.Add(@event);

                        foreach (var s in db.RSVP_Event.Where(p => p.GenID == tRWLASchedule.GenID))
                        {
                            
                            if (s.SYSUserProfileID == user)
                            {
                                TempData["rsvpFAIL"] = "You have already RSVPd to this event";
                                return RedirectToAction("StudentMainMenu");
                            }
                            else if (s.SYSUserProfileID == user)
                            {

                                TempData["rsvp"] = "You have successfully RSVPd to the event: " + tRWLASchedule.GenEvent.Gen_Name;
                            }
                        }
                        AuditLog myAudit = new AuditLog();
                        myAudit.DateDone = DateTime.Now;
                        myAudit.TypeTran = "Create";
                        myAudit.SYSUserProfileID = (int)Session["User"];
                        myAudit.TableAff = "RSVP_Event";
                        db.AuditLogs.Add(myAudit);
                        db.SaveChanges();
                        return RedirectToAction("StudentMainMenu");
                    }
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        public ActionResult EventType(int? id)
        {
            try
            {
                return View();
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        

        // GET: TRWLASchedules/Create
        public ActionResult Create()
        {
            try
            {
                ViewBag.ComEngID = new SelectList(db.ComEngEvents, "ComEngID", "ComEng_Name");
                ViewBag.FunctionID = new SelectList(db.FunctionEvents, "FunctionID", "Function_Name");
                ViewBag.LectureID = new SelectList(db.Lectures, "LectureID", "Lecture_Name");
                return View();
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        // POST: TRWLASchedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ScheduleID,FunctionID,LectureID,ComEngID")] TRWLASchedule tRWLASchedule)
        {
            try
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
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }



        // GET: TRWLASchedules/Edit/5
        public ActionResult Edit(int? id)
        {
            try
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
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        // POST: TRWLASchedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ScheduleID,FunctionID,LectureID,ComEngID")] TRWLASchedule tRWLASchedule)
        {
            try
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
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        // GET: TRWLASchedules/Delete/5
        public ActionResult Delete(int? id)
        {



            try
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
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }



        // POST: TRWLASchedules/Delete/5
        [HttpPost, ActionName("Details")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                MasterData mydata = db.MasterDatas.Find(9);
                int time = Convert.ToInt32(mydata.CancelEvent);

                int mytime = DateTime.Now.AddDays(-time).DayOfYear;

                TRWLASchedule tRWLASchedule = db.TRWLASchedules.Find(id);


                if (tRWLASchedule.FunctionID != null)
                {
                    int m = tRWLASchedule.FunctionEvent.Function_Date.DayOfYear;

                    int diff = m - mytime;

                    if (diff <= time)
                    {
                        TempData["Log"] = "Days to event: " + mydata.CancelEvent + ". You cannot delete this event";

                        return View(tRWLASchedule);
                    }

                }
                else if (tRWLASchedule.LectureID != null)
                {
                    int m = tRWLASchedule.Lecture.Lecture_Date.DayOfYear;
                    int diff = m - mytime;

                    if (diff <= time)
                    {
                        TempData["Log"] = "Days to event: " + mydata.CancelEvent + ". You cannot delete this event";
                        return View(tRWLASchedule);
                    }

                }
                else if (tRWLASchedule.GenID != null)
                {
                    int m = tRWLASchedule.GenEvent.Gen_Date.DayOfYear;
                    int diff = m - mytime;

                    if (diff <= time)
                    {
                        TempData["Log"] = "Days to event: " + mydata.CancelEvent + ". You cannot delete this event";
                        return View(tRWLASchedule);
                    }
                }
                else if (tRWLASchedule.ComEngID != null)
                {
                    int m = tRWLASchedule.ComEngEvent.ComEng_Date.DayOfYear;
                    int diff = m - mytime;

                    if (diff <= time)
                    {
                        TempData["Log"] = "Days to event: " + mydata.CancelEvent +". You cannot delete this event";
                        return View(tRWLASchedule);
                    }
                }



                try
                {

                    //Finds the row/s in the table where the scheduleID found is equal to this
                    RSVPSchedule rsvp = db.RSVPSchedules.FirstOrDefault(p => p.ScheduleID == id);



                    if (tRWLASchedule.FunctionID != null)
                    {

                        FunctionEvent functions = db.FunctionEvents.Find(tRWLASchedule.FunctionID);

                        AuditLog myAudit = new AuditLog();
                        myAudit.DateDone = DateTime.Now;
                        myAudit.TypeTran = "Delete";
                        myAudit.SYSUserProfileID = (int)Session["User"];
                        myAudit.TableAff = "FunctiionEvents";
                        db.AuditLogs.Add(myAudit);

                        var email = from s in db.RSVP_Event
                                    where s.FunctionID == functions.FunctionID
                                    select s;


                        db.FunctionEvents.Remove(functions);
                        db.TRWLASchedules.Remove(tRWLASchedule);
                        if (email != null)
                        {
                            foreach (var s in email)
                            {
                                try
                                {
                                    SYSUserProfile recipient = db.SYSUserProfiles.Find(s.SYSUserProfileID);
                                    

                                    MailMessage msg = new MailMessage();
                                    msg.From = new MailAddress("u15213626@tuks.co.za");
                                    msg.To.Add(recipient.Email);
                                    msg.Subject = functions.Function_Name + " Cancellation";
                                    msg.Body = "Dear " + recipient.FirstName + "\n\n Please note that the event, " + functions.Function_Name + " has been cancelled until further notice. Thank you for your understanding in this matter. \n\n Regards, \n TRWLA Management.";

                                    SmtpClient smtp = new SmtpClient();

                                    smtp.Host = "smtp.gmail.com";
                                    smtp.Port = 587;
                                    smtp.UseDefaultCredentials = false;
                                    smtp.Credentials = new System.Net.NetworkCredential("u15213626@tuks.co.za", "Coakes12345");
                                    smtp.EnableSsl = true;
                                    smtp.Send(msg);

                                    ModelState.Clear();
                                }
                                catch (Exception)
                                {
                                    ViewBag.Status = "Problem while sending email, Please check details.";
                                }
                                db.RSVP_Event.Remove(s);
                            }



                        }




                        var sched = from n in db.RSVPSchedules
                                    where n.ScheduleID == tRWLASchedule.ScheduleID
                                    select n;

                        if (sched != null)
                        {
                            foreach (var n in sched)
                            {
                                db.RSVPSchedules.Remove(n);
                            }
                        }


                    }
                    else if (tRWLASchedule.LectureID != null)
                    {

                        Lecture lectures = db.Lectures.Find(Convert.ToInt32(tRWLASchedule.LectureID));
                        AuditLog myAudit = new AuditLog();
                        myAudit.DateDone = DateTime.Now;
                        myAudit.TypeTran = "Delete";
                        myAudit.SYSUserProfileID = (int)Session["User"];
                        myAudit.TableAff = "Lectures";
                        db.AuditLogs.Add(myAudit);

                        var email = from s in db.RSVP_Event
                                    where s.LectureID == lectures.LectureID
                                    select s;

                        db.Lectures.Remove(lectures);
                        db.TRWLASchedules.Remove(tRWLASchedule);
                        if (email != null)
                        {
                            foreach (var s in email)
                            {
                                try
                                {
                                    SYSUserProfile recipient = db.SYSUserProfiles.Find(s.SYSUserProfileID);

                                    MailMessage msg = new MailMessage();
                                    msg.From = new MailAddress("u15213626@tuks.co.za");
                                    msg.To.Add(recipient.Email);
                                    msg.Subject = lectures.Lecture_Name + " Cancellation";
                                    msg.Body = "Dear " + recipient.FirstName + "\n\n Please note that the event, " + lectures.Lecture_Name + " has been cancelled until further notice. Thank you for your understanding in this matter. \n\n Regards, \n TRWLA Management.";

                                    SmtpClient smtp = new SmtpClient();

                                    smtp.Host = "smtp.gmail.com";
                                    smtp.Port = 587;
                                    smtp.UseDefaultCredentials = false;
                                    smtp.Credentials = new System.Net.NetworkCredential("u15213626@tuks.co.za", "Coakes12345");
                                    smtp.EnableSsl = true;
                                    smtp.Send(msg);

                                    ModelState.Clear();
                                }
                                catch (Exception)
                                {
                                    ViewBag.Status = "Problem while sending email, Please check details.";
                                }

                                db.RSVP_Event.Remove(s);
                            }


                        }




                        var sched = from n in db.RSVPSchedules
                                    where n.ScheduleID == tRWLASchedule.ScheduleID
                                    select n;

                        if (sched != null)
                        {
                            foreach (var n in sched)
                            {
                                db.RSVPSchedules.Remove(n);
                            }
                        }



                    }

                    else if (tRWLASchedule.ComEngID != null)
                    {
                        AuditLog myAudit = new AuditLog();
                        myAudit.DateDone = DateTime.Now;
                        myAudit.TypeTran = "Delete";
                        myAudit.SYSUserProfileID = (int)Session["User"];
                        myAudit.TableAff = "ComEngEvent";
                        db.AuditLogs.Add(myAudit);

                        ComEngEvent comeng = db.ComEngEvents.Find(Convert.ToInt32(tRWLASchedule.ComEngID));
                        

                        var email = from s in db.RSVP_Event
                                    where s.ComEngID == comeng.ComEngID
                                    select s;

                        db.ComEngEvents.Remove(comeng);
                        db.TRWLASchedules.Remove(tRWLASchedule);
                        if (email != null)
                        {

                            foreach (var s in email)
                            {
                                try
                                {
                                    SYSUserProfile recipient = db.SYSUserProfiles.Find(s.SYSUserProfileID);

                                    MailMessage msg = new MailMessage();
                                    msg.From = new MailAddress("u15213626@tuks.co.za");
                                    msg.To.Add(recipient.Email);
                                    msg.Subject = comeng.ComEng_Name + " Cancellation";
                                    msg.Body = "Dear " + recipient.FirstName + "\n\n Please note that the event, " + comeng.ComEng_Name + ", has been cancelled until further notice. Thank you for your understanding in this matter. \n\n Regards, \n TRWLA Management.";

                                    SmtpClient smtp = new SmtpClient();

                                    smtp.Host = "smtp.gmail.com";
                                    smtp.Port = 587;
                                    smtp.UseDefaultCredentials = false;
                                    smtp.Credentials = new System.Net.NetworkCredential("u15213626@tuks.co.za", "Coakes12345");
                                    smtp.EnableSsl = true;
                                    smtp.Send(msg);

                                    ModelState.Clear();
                                }
                                catch (Exception)
                                {
                                    ViewBag.Status = "Problem while sending email, Please check details.";
                                }
                                db.RSVP_Event.Remove(s);
                            }


                        }

                        var sched = from n in db.RSVPSchedules
                                    where n.ScheduleID == tRWLASchedule.ScheduleID
                                    select n;

                        if (sched != null)
                        {
                            foreach (var n in sched)
                            {
                                db.RSVPSchedules.Remove(n);
                            }
                        }
                        //Note: Write code to send email to all students who have RSVP'd to the event so that they get the notification. 


                    }
                    else if (tRWLASchedule.GenID != null)
                    {
                        AuditLog myAudit = new AuditLog();
                        myAudit.DateDone = DateTime.Now;
                        myAudit.TypeTran = "Delete";
                        myAudit.SYSUserProfileID = (int)Session["User"];
                        myAudit.TableAff = "GenEvent";
                        db.AuditLogs.Add(myAudit);

                        GenEvent gen = db.GenEvents.Find(Convert.ToInt32(tRWLASchedule.GenID));
                        

                        var email = from s in db.RSVP_Event
                                    where s.GenID == gen.GenID
                                    select s;

                        db.GenEvents.Remove(gen);
                        db.TRWLASchedules.Remove(tRWLASchedule);

                        if (email != null)
                        {

                            foreach (var s in email)
                            {
                                try
                                {
                                    SYSUserProfile recipient = db.SYSUserProfiles.Find(s.SYSUserProfileID);

                                    MailMessage msg = new MailMessage();
                                    msg.From = new MailAddress("u15213626@tuks.co.za");
                                    msg.To.Add(recipient.Email);
                                    msg.Subject = gen.Gen_Name + " Cancellation";
                                    msg.Body = "Dear " + recipient.FirstName + "\n\n Please note that the event, " + gen.Gen_Name + ", has been cancelled until further notice. Thank you for your understanding in this matter. \n\n Regards, \n TRWLA Management.";

                                    SmtpClient smtp = new SmtpClient();

                                    smtp.Host = "smtp.gmail.com";
                                    smtp.Port = 587;
                                    smtp.UseDefaultCredentials = false;
                                    smtp.Credentials = new System.Net.NetworkCredential("u15213626@tuks.co.za", "Coakes12345");
                                    smtp.EnableSsl = true;
                                    smtp.Send(msg);

                                    ModelState.Clear();
                                }
                                catch (Exception)
                                {
                                    ViewBag.Status = "Problem while sending email, Please check details.";
                                }
                                db.RSVP_Event.Remove(s);
                            }






                        }

                        var sched = from n in db.RSVPSchedules
                                where n.ScheduleID == tRWLASchedule.ScheduleID
                                select n;

                        if (sched != null)
                        {
                            foreach (var n in sched)
                            {
                                db.RSVPSchedules.Remove(n);
                            }
                        }

                        

                        //Note: Write code to send email to all students who have RSVP'd to the event so that they get the notification. 


                    }

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    return RedirectToAction("ErrorPage");
                }
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }
        public ActionResult StudentContent(string sortOrder, string searchString, string locked, string unlocked, string all)
        {
            try
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
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        public ActionResult StudentContentDetails(int? id)
        {
            try
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
            catch
            {
                return RedirectToAction("ErrorPage");
            }
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

