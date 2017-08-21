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
using System.Web.UI.DataVisualization.Charting;
using System.Text;
using System.IO;
using System.Drawing;
using ClosedXML.Excel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TRWLASystemMaster.Controllers
{
    public class TRWLASchedulesController : Controller
    {
        private TWRLADB_Staging_V2Entities5 db = new TWRLADB_Staging_V2Entities5();

        // GET: TRWLASchedules
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


            return View(tRWLASchedules.ToList());
        }

        //Begginning of Report Generation 

        public IList<ClassAttendance> GetClassAttendance()
        {
            var at = (from l in db.RSVP_Event
                      join s in db.Students on l.StudentID equals s.StudentID
                      select new ClassAttendance
                      {
                          Name = s.Student_Name,
                          Surname = s.Student_Surname,
                          EventCount = db.RSVP_Event.Count(),
                          StudentID = s.StudentID,
                          PersonalCount = (db.RSVP_Event.Distinct().Where(p => p.StudentID == s.StudentID).Count())/(db.RSVP_Event.Count())*100
                      }).ToList();

            return at;
        }

        public IList<Demographic> GetDemo()
        {
            var at = (from l in db.Students
                      join r in db.Residences on l.ResID equals r.ResID
                      join st in db.StudentTypes on l.StudentTypeID equals st.StudentTypeID
                      select new Demographic
                      {
                          StudNo = l.StudentNumber,
                          Name = l.Student_Name,
                          Surname = l.Student_Surname,
                          DoB = l.Student_DoB,
                          Degree = l.Degree,
                          Grad = l.Graduate,
                          Res = r.Res_Name,
                          StudType = st.StudentTypeDescription
                      }).ToList();

            return at;
        }

        public IList<AttendanceViewModel> GetLectureAttendance()
        {
            var at = (from l in db.RSVP_Event
                      where l.LectureID != null
                              select new AttendanceViewModel
                              {
                                  EventName = l.Lecture.Lecture_Name,
                                  EventDate = l.Lecture.Lecture_Date,
                                  Student_Name = l.Student.Student_Name
                              }).ToList();
            return at;
        }
        public IList<AttendanceViewModel> GetFunctionAttendance()
        {
            var attendance = (from s in db.RSVP_Event
                              where s.FunctionID != null
                              select new AttendanceViewModel
                                {
                                    EventName = s.FunctionEvent.Function_Name,
                                    EventDate = s.FunctionEvent.Function_Date,
                                    Student_Name = s.Student.Student_Name
                                }).ToList();
            return attendance;
        }

       

        public IList<AttendanceViewModel> GetComAttendance()
        {
            var attend = (from m in db.RSVP_Event
                          where m.ComEngID != null
                              select new AttendanceViewModel
                              {
                                  EventName = m.ComEngEvent.ComEng_Name,
                                  EventDate = m.ComEngEvent.ComEng_Date,
                                  Student_Name = m.Student.Student_Name
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
            Response.AddHeader("content-disposition", "attachment; filename="+name+ ".xls");
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

        public ActionResult StudentDemographic()
        {
            return View(this.GetDemo());
        }

        public ActionResult ClassAttendance()
        {
            return View(this.GetClassAttendance());
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

        //public ActionResult ClassAttendance(SeriesChartType chartType)
        //{
        //    IList<Attendance> attendances = Attendance.GetResults();
        //    System.Web.UI.DataVisualization.Charting.Chart chart = new System.Web.UI.DataVisualization.Charting.Chart();
        //    chart.Width = 700;
        //    chart.Height = 300;
        //    chart.BackColor = Color.FromArgb(211, 223, 240);
        //    chart.BorderlineDashStyle = ChartDashStyle.Solid;
        //    chart.BackSecondaryColor = Color.White;
        //    chart.BackGradientStyle = GradientStyle.TopBottom;
        //    chart.BorderlineWidth = 1;
        //    chart.Palette = ChartColorPalette.BrightPastel;
        //    chart.BorderlineColor = Color.FromArgb(26, 59, 105);
        //    chart.RenderType = RenderType.BinaryStreaming;
        //    chart.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
        //    chart.AntiAliasing = AntiAliasingStyles.All;
        //    chart.TextAntiAliasingQuality = TextAntiAliasingQuality.Normal;
        //    chart.Titles.Add(CreateTitle());
        //    chart.Legends.Add(CreateLegend());
        //    chart.Series.Add(CreateSeries(attendances, chartType));
        //    chart.ChartAreas.Add(CreateChartArea());

        //    MemoryStream ms = new MemoryStream();
        //    chart.SaveImage(ms);
        //    return File(ms.GetBuffer(), @"image/png");
        //}

        //public Title CreateTitle()
        //{
        //    Title title = new Title();
        //    title.Text = "Result Chart";
        //    title.ShadowColor = Color.FromArgb(32, 0, 0, 0);
        //    title.Font = new Font("Trebuchet MS", 14F, FontStyle.Bold);
        //    title.ShadowOffset = 3;
        //    title.ForeColor = Color.FromArgb(26, 59, 105);
        //    return title;
        //}

        //private Legend CreateLegend()
        //{
        //    Legend legend = new Legend();
        //    legend.Enabled = true;
        //    legend.ShadowColor = Color.FromArgb(32, 0, 0, 0);
        //    legend.Font = new Font("Trebuchet MS", 14F, FontStyle.Bold);
        //    legend.ShadowOffset = 3;
        //    legend.ForeColor = Color.FromArgb(26, 59, 105);
        //    legend.Title = "Legend";
        //    return legend;
        //}


        //public Series CreateSeries(IList<Attendance> results, SeriesChartType chartType)
        //{
        //    Series seriesDetail = new Series();
        //    seriesDetail.Name = "Result Chart";
        //    seriesDetail.IsValueShownAsLabel = false;
        //    seriesDetail.Color = Color.FromArgb(198, 99, 99);
        //    seriesDetail.ChartType = chartType;
        //    seriesDetail.BorderWidth = 2;
        //    DataPoint point;

        //    foreach (Attendance result in results)
        //    {
        //        point = new DataPoint();
        //        point.AxisLabel = result.Student.Student_Name ;
        //        point.YValues = new double[] { double.Parse(result.) };
        //        seriesDetail.Points.Add(point);
        //    }
        //    seriesDetail.ChartArea = "Result Chart";
        //    return seriesDetail;
        //}

        //public ChartArea CreateChartArea()
        //{
        //    ChartArea chartArea = new ChartArea();
        //    chartArea.Name = "Result Chart";
        //    chartArea.BackColor = Color.Transparent;
        //    chartArea.AxisX.IsLabelAutoFit = false;
        //    chartArea.AxisY.IsLabelAutoFit = false;
        //    chartArea.AxisX.LabelStyle.Font =
        //       new Font("Verdana,Arial,Helvetica,sans-serif",
        //                8F, FontStyle.Regular);
        //    chartArea.AxisY.LabelStyle.Font =
        //       new Font("Verdana,Arial,Helvetica,sans-serif",
        //                8F, FontStyle.Regular);
        //    chartArea.AxisY.LineColor = Color.FromArgb(64, 64, 64, 64);
        //    chartArea.AxisX.LineColor = Color.FromArgb(64, 64, 64, 64);
        //    chartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
        //    chartArea.AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
        //    chartArea.AxisX.Interval = 1;
        //}

        public ActionResult LogAttendance()
        {
            return View(db.TRWLASchedules.ToList());
        }

        public ActionResult RSVPdMembers(int? id)
        {
            TRWLASchedule tRWLASchedule = db.TRWLASchedules.Find(id);

            if (tRWLASchedule.FunctionID != null)
            {
                var evnt = from s in db.Students
                           join r in db.RSVP_Event on s.StudentID equals r.StudentID
                           join t in db.TRWLASchedules on r.FunctionID equals t.FunctionID
                           where r.FunctionID == tRWLASchedule.FunctionID
                           select r;
                

                return View(evnt.ToList());
            }
            else if (tRWLASchedule.LectureID != null)
            {
                var evnt = from s in db.Students
                           join r in db.RSVP_Event on s.StudentID equals r.StudentID
                           join t in db.TRWLASchedules on r.LectureID equals t.LectureID
                           where r.LectureID == tRWLASchedule.LectureID
                           select r;
                

                return View(evnt.ToList());
            }
            else
            {
                var evnt = from s in db.Students
                           join r in db.RSVP_Event on s.StudentID equals r.StudentID
                           join t in db.TRWLASchedules on r.ComEngID equals t.ComEngID
                           where r.ComEngID == tRWLASchedule.ComEngID
                           select r;
                

                return View(evnt.ToList());
            }
        }

        public ActionResult ConfirmAttendance(int? id, int? idd)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        [HttpPost, ActionName("ConfirmAttendance")]
        [ValidateAntiForgeryToken]
        public ActionResult AttendanceConfirmed(Attendance att ,int id, int idd)
        {

            Student stud = db.Students.Find(id);
            TRWLASchedule ev = db.TRWLASchedules.Find(idd);


            int i = db.Attendances.Count();

            try
            {
                if (i == 0)
                {
                    if (ev.FunctionID != null)
                    {
                        att.FunctionID = ev.FunctionID;
                        att.StudentID = stud.StudentID;
                        db.Attendances.Add(att);
                    }
                    else if (ev.LectureID != null)
                    {
                        att.LectureID = ev.LectureID;
                        att.StudentID = stud.StudentID;
                        db.Attendances.Add(att);
                    }
                    else
                    {
                        att.ComEngID = ev.ComEngID;
                        att.StudentID = stud.StudentID;
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
                        att.StudentID = stud.StudentID;
                        db.Attendances.Add(att);
                    }
                    else if (ev.LectureID != null)
                    {
                        att.LectureID = ev.LectureID;
                        att.StudentID = stud.StudentID;
                        db.Attendances.Add(att);
                    }
                    else
                    {
                        att.ComEngID = ev.ComEngID;
                        att.StudentID = stud.StudentID;
                        db.Attendances.Add(att);
                    }
                }
                db.SaveChanges();

                TempData["Attend"] = "You have successfully logged the event attendance of: " + stud.Student_Name;
            }
            catch (Exception)
            {
                TempData["Attend"] = "There was an error in the attempt of logging the attendance of the student";
            }
            return View();
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
            return View(tRWLASchedule);
        }

        public ActionResult EventType(int? id)
        {
            return View();
        }

        [HttpPost, ActionName("EventType")]
        [ValidateAntiForgeryToken]
        public ActionResult EventTypeLoad(int id)
        {
            if (id == 1)
            {
                return View("../FunctionEvents/Create");
            }
            else if (id == 2)
            {
                return View("../ComEngEvents/Create");
            }
            else
            {
                return View("../Lectures/Create");
            }
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

        [HttpPost, ActionName("RSVP")]
        [ValidateAntiForgeryToken]
        public ActionResult RSVPConfirmed(RSVP_Event @event, int id)
        {

            TRWLASchedule tRWLASchedule = db.TRWLASchedules.Find(id);
            //Counts if the table has data in it
            int i = db.RSVP_Event.Count();
            if (i == 0)
            {
                //this is what adds data to the table
                @event.StudentID = 2;

                if (tRWLASchedule.FunctionID != null)
                {
                    @event.FunctionID = tRWLASchedule.FunctionID;

                    RSVPSchedule mysched = new RSVPSchedule();
                    mysched.rsvpID = @event.rsvpID;
                    mysched.ScheduleID = id;
                    mysched.StudentID = @event.StudentID = 2;
                    db.RSVPSchedules.Add(mysched);
                    db.RSVP_Event.Add(@event);

                    foreach (var s in db.RSVP_Event.Where(p => p.StudentID == 2))
                    {
                        if (db.RSVP_Event.Where(p => p.FunctionID == tRWLASchedule.FunctionID) == null)
                        {
                            TempData["rsvpFAIL"] = "You have already RSVPd to this event";
                            return RedirectToAction("Index");
                        }

                        else
                        {

                            TempData["rsvp"] = "You have successfully RSVPd to the event: " + tRWLASchedule.FunctionEvent.Function_Name;
                        }
                    }

                    TempData["rsvp"] = "You have successfully RSVPd to the event: " + tRWLASchedule.FunctionEvent.Function_Name;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else if (tRWLASchedule.ComEngID != null)
                {
                    @event.ComEngID = tRWLASchedule.ComEngID;

                    RSVPSchedule mysched = new RSVPSchedule();
                    mysched.rsvpID = @event.rsvpID;
                    mysched.ScheduleID = id;
                    mysched.StudentID = @event.StudentID = 2;
                    db.RSVPSchedules.Add(mysched);
                    db.RSVP_Event.Add(@event);

                    foreach (var s in db.RSVP_Event.Where(p => p.StudentID == 2))
                    {
                        if (db.RSVP_Event.Where(p => p.ComEngID == tRWLASchedule.ComEngID) == null)
                        {
                            TempData["rsvpFAIL"] = "You have already RSVPd to this event";
                            return RedirectToAction("Index");
                        }

                        else
                        {

                            TempData["rsvp"] = "You have successfully RSVPd to the event: " + tRWLASchedule.Lecture.Lecture_Name;
                        }
                    }

                    TempData["rsvp"] = "You have successfully RSVPd to the event: " + tRWLASchedule.Lecture.Lecture_Name;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else if (tRWLASchedule.LectureID != null)
                {
                    @event.LectureID = tRWLASchedule.LectureID;

                    RSVPSchedule mysched = new RSVPSchedule();
                    mysched.rsvpID = @event.rsvpID;
                    mysched.ScheduleID = id;
                    mysched.StudentID = @event.StudentID = 2;
                    db.RSVPSchedules.Add(mysched);
                    db.RSVP_Event.Add(@event);

                    foreach (var s in db.RSVP_Event.Where(p => p.StudentID == 2))
                    {
                        if (db.RSVP_Event.Where(p => p.LectureID == tRWLASchedule.LectureID) == null)
                        {
                            TempData["rsvpFAIL"] = "You have already RSVPd to this event";
                            return RedirectToAction("Index");
                        }

                        else
                        {

                            TempData["rsvp"] = "You have successfully RSVPd to the event: " + tRWLASchedule.ComEngEvent.ComEng_Name;
                        }
                    }

                    TempData["rsvp"] = "You have successfully RSVPd to the event: " + tRWLASchedule.ComEngEvent.ComEng_Name;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }
            else
            {
                int max = db.RSVP_Event.Max(p => p.rsvpID);
                int l = max + 1;
                @event.StudentID = 2;

                if (tRWLASchedule.FunctionID != null)
                {
                    @event.FunctionID = tRWLASchedule.FunctionID;
                    @event.rsvpID = l;

                    RSVPSchedule mysched = new RSVPSchedule();
                    mysched.rsvpID = @event.rsvpID;
                    mysched.ScheduleID = id;
                    mysched.StudentID = @event.StudentID = 2;
                    db.RSVPSchedules.Add(mysched);
                    db.RSVP_Event.Add(@event);

                    foreach (var s in db.RSVP_Event.Where(p => p.StudentID == 2))
                    {
                        if (db.RSVP_Event.Where(p => p.FunctionID == tRWLASchedule.FunctionID) == null)
                        {
                            TempData["rsvpFAIL"] = "You have already RSVPd to this event";
                            return RedirectToAction("Index");
                        }

                        else
                        {

                            TempData["rsvp"] = "You have successfully RSVPd to the event: " + tRWLASchedule.FunctionEvent.Function_Name;
                        }
                    }

                    TempData["rsvp"] = "You have successfully RSVPd to the event: " + tRWLASchedule.FunctionEvent.Function_Name;
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
                    mysched.StudentID = @event.StudentID = 2;
                    db.RSVPSchedules.Add(mysched);
                    db.RSVP_Event.Add(@event);

                    foreach (var s in db.RSVP_Event.Where(p => p.StudentID == 2))
                    {
                        if (db.RSVP_Event.Where(p => p.ComEngID == tRWLASchedule.ComEngID) == null)
                        {
                            TempData["rsvpFAIL"] = "You have already RSVPd to this event";
                            return RedirectToAction("Index");
                        }

                        else
                        {

                            TempData["rsvp"] = "You have successfully RSVPd to the event: " + tRWLASchedule.ComEngEvent.ComEng_Name;
                        }
                    }

                    TempData["rsvp"] = "You have successfully RSVPd to the event: " + tRWLASchedule.ComEngEvent.ComEng_Name;
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
                    mysched.StudentID = @event.StudentID = 2;
                    db.RSVPSchedules.Add(mysched);
                    db.RSVP_Event.Add(@event);

                    foreach (var s in db.RSVP_Event.Where(p => p.StudentID == 2))
                    {
                        if (db.RSVP_Event.Where(p => p.LectureID == tRWLASchedule.LectureID) == null)
                        {
                            TempData["rsvpFAIL"] = "You have already RSVPd to this event";
                            return RedirectToAction("Index");
                        }

                        else
                        {

                            TempData["rsvp"] = "You have successfully RSVPd to the event: " + tRWLASchedule.Lecture.Lecture_Name;
                        }
                    }
                    TempData["rsvp"] = "You have successfully RSVPd to the event: " + tRWLASchedule.Lecture.Lecture_Name;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                
                return RedirectToAction("Index");
            }
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
                            select s.StudentID;

                foreach (var s in email)
                {
                    try
                    {
                        Student recipient = db.Students.Find(s);

                        FunctionEvent func = db.FunctionEvents.Find(Convert.ToInt32(tRWLASchedule.FunctionID));

                        MailMessage msg = new MailMessage();
                        msg.From = new MailAddress("u15213626@tuks.co.za");
                        msg.To.Add(recipient.AspNetUser.Email);
                        msg.Subject = func.Function_Name + " Cancellation";
                        msg.Body = "Dear " + recipient.Student_Name + "\n\n Please note that the event, " + func.Function_Name + " has been cancelled until further notice. Thank you for your understanding in this matter. \n\n Regards, \n TRWLA Management.";

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
                            select s.StudentID;

                foreach (var s in email)
                {
                    try
                    {
                        Student recipient = db.Students.Find(s);
                        Lecture lec = db.Lectures.Find(tRWLASchedule.LectureID);

                        MailMessage msg = new MailMessage();
                        msg.From = new MailAddress("u15213626@tuks.co.za");
                        msg.To.Add(recipient.AspNetUser.Email);
                        msg.Subject = lec.Lecture_Name + " Cancellation";
                        msg.Body = "Dear " + recipient.Student_Name + "\n\n Please note that the event, " + lec.Lecture_Name + " has been cancelled until further notice. Thank you for your understanding in this matter. \n\n Regards, \n TRWLA Management.";

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
                            select s.StudentID;

                foreach (var s in email)
                {
                    try
                    {
                        Student recipient = db.Students.Find(s);
                        ComEngEvent com = db.ComEngEvents.Find(tRWLASchedule.ComEngID);

                        MailMessage msg = new MailMessage();
                        msg.From = new MailAddress("u15213626@tuks.co.za");
                        msg.To.Add(recipient.AspNetUser.Email);
                        msg.Subject = com.ComEng_Name + " Cancellation";
                        msg.Body = "Dear " + recipient.Student_Name + "\n\n Please note that the event, " + com.ComEng_Name + ", has been cancelled until further notice. Thank you for your understanding in this matter. \n\n Regards, \n TRWLA Management.";

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