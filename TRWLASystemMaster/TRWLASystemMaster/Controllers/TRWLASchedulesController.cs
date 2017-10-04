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
        private TWRLADB_Staging_V2Entities7 db = new TWRLADB_Staging_V2Entities7();
        

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
                    MasterData mymaster = db.MasterDatas.Find(1);

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

        public ActionResult StudentMainMenu(string sortOrder, string searchString, string F, string CO, string L, string all)
        {
            try
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
                DateTime mydate = DateTime.Now.AddDays(-1);

                tRWLASchedules = tRWLASchedules.Where(p => p.Lecture.Lecture_Date >= mydate || p.FunctionEvent.Function_Date >= mydate || p.ComEngEvent.ComEng_Date >= mydate);

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
                            || s.ComEngEvent.ComEng_Name.Contains(searchString));
                }



                if (!String.IsNullOrEmpty(F))
                {
                    tRWLASchedules = tRWLASchedules.Where(s => s.FunctionEvent.Type == 1);
                }

                if (!String.IsNullOrEmpty(G))
                {
                    tRWLASchedules = tRWLASchedules.Where(s => s.GenEvent.Type == 4);
                }

                if (!String.IsNullOrEmpty(CO))
                {
                    tRWLASchedules = tRWLASchedules.Where(s => s.ComEngEvent.Type == 3 );
                }

                if (!String.IsNullOrEmpty(L))
                {
                    tRWLASchedules = tRWLASchedules.Where(s => s.Lecture.Type == 2 );
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

        public IList<Demographic> GetDemo()
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

            return at;
        }

        public IList<AttendanceViewModel> GetLectureAttendance()
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
                                  StartTime = s.FunctionEvent.Function_StartTime,
                                  EndTime = s.FunctionEvent.Function_EndTime,
                                  Residence = s.FunctionEvent.Venue.Venue_Name,
                                  specific = s.FunctionEvent.GuestSpeaker.GuestSpeaker_Name,
                                  Student_Name = s.SYSUserProfile.FirstName,
                                  LastName = s.SYSUserProfile.LastName,
                                  StudentNp = s.SYSUserProfile.StudentNumber
                              }).ToList();
            return attendance;
        }



        public IList<AttendanceViewModel> GetComAttendance()
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
            try
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

        public ActionResult StudentDemographic()
        {
            try
            {
                return View(this.GetDemo());
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        public ActionResult NoAttend()
        {
            try
            {
                return View(this.GetNoAttendance());
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

        public ActionResult FunctionAttendance()
        {
            try
            {
                return View(this.GetFunctionAttendance());
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        public ActionResult ComAttendance()
        {
            try
            {
                return View(this.GetComAttendance());
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        public ActionResult LecAttendance()
        {
            try
            {
                return View(this.GetLectureAttendance());
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
                var rsvp = from s in db.RSVP_Event
                           select s;

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

                trwla = trwla.Where(p => (p.ComEngEvent.ComEng_Date >= mydate || p.FunctionEvent.Function_Date >= mydate || p.Lecture.Lecture_Date >= mydate) && (p.ComEngEvent.ComEng_Date < myfuture || p.FunctionEvent.Function_Date < myfuture || p.Lecture.Lecture_Date < myfuture));

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

                            if (myProgress != null)
                            {
                                myProgress.FuncProg = myProgress.FuncProg + 1;
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

                            if (myProgress != null)
                            {
                                myProgress.LecProg = myProgress.LecProg + 1;
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

                            progressbar myProgress = db.progressbars.FirstOrDefault(p => p.SYSUserProfileID == stud.SYSUserProfileID);

                            if (myProgress != null)
                            {
                                myProgress.ComProg = myProgress.ComProg + 1;
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

                            progressbar myProgress = db.progressbars.FirstOrDefault(p => p.SYSUserProfileID == stud.SYSUserProfileID);

                            if (myProgress != null)
                            {
                                myProgress.GenProg = myProgress.GenProg + 1;
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

                            if (myProgress != null)
                            {
                                myProgress.FuncProg = myProgress.FuncProg + 1;
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

                            if (myProgress != null)
                            {
                                myProgress.LecProg = myProgress.LecProg + 1;
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

                            progressbar myProgress = db.progressbars.FirstOrDefault(p => p.SYSUserProfileID == stud.SYSUserProfileID);

                            if (myProgress != null)
                            {
                                myProgress.ComProg = myProgress.ComProg + 1;
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

                            progressbar myProgress = db.progressbars.FirstOrDefault(p => p.SYSUserProfileID == stud.SYSUserProfileID);

                            if (myProgress != null)
                            {
                                myProgress.GenProg = myProgress.GenProg + 1;
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

                    TempData["Attend"] = "You have successfully logged the event attendance of: " + stud.FirstName;
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

                    TempData["Attend"] = "You have successfully logged the event attendance of: " + stud.FirstName;
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
                TRWLASchedule tRWLASchedule = db.TRWLASchedules.Find(id);
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
                    TRWLASchedule tRWLASchedule = db.TRWLASchedules.Find(id);
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
                        return RedirectToAction("Index");

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

                        foreach (var s in db.RSVP_Event.Where(p => p.LectureID == tRWLASchedule.GenID))
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

                        foreach (var s in db.RSVP_Event.Where(p => p.LectureID == tRWLASchedule.GenID))
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
                //Finds the row in the table where the scheduleID is equal to this
                TRWLASchedule tRWLASchedule = db.TRWLASchedules.Find(id);

                //Finds the row/s in the table where the scheduleID found is equal to this
                RSVPSchedule rsvp = db.RSVPSchedules.FirstOrDefault(p => p.ScheduleID == id);



                if (tRWLASchedule.FunctionID != null)
                {

                    FunctionEvent functions = db.FunctionEvents.Find(tRWLASchedule.FunctionID);

                    db.TRWLASchedules.Remove(tRWLASchedule);


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


                        RSVP_Event function = db.RSVP_Event.FirstOrDefault(l => l.FunctionID == tRWLASchedule.FunctionID);


                        db.RSVP_Event.Remove(function);
                        db.RSVPSchedules.Remove(rsvp);
                    }


                    
                    db.FunctionEvents.Remove(functions);

                }
                else if (tRWLASchedule.LectureID != null)
                {

                    Lecture lectures = db.Lectures.Find(Convert.ToInt32(tRWLASchedule.LectureID));
                    db.TRWLASchedules.Remove(tRWLASchedule);

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

                        RSVP_Event lecture = db.RSVP_Event.FirstOrDefault(l => l.LectureID == tRWLASchedule.LectureID);
                        db.RSVPSchedules.Remove(rsvp);
                        db.RSVP_Event.Remove(lecture);
                    }


                    
                    
                    db.Lectures.Remove(lectures);




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
                    db.TRWLASchedules.Remove(tRWLASchedule);

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
                        db.RSVPSchedules.Remove(rsvp);
                        db.RSVP_Event.Remove(comu);

                    }
                    

                    
                    
                    
                    db.ComEngEvents.Remove(comeng);
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

