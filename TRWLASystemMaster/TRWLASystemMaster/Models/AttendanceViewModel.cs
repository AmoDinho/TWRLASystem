using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TRWLASystemMaster.Models
{
    public class AttendanceViewModel
    {
        public string EventName
        { get; set; }

        public Nullable<System.DateTime> EventDate
        { get; set; }

        public Nullable<System.TimeSpan> StartTime
        {
            get; set;
        }
        
        public Nullable<System.TimeSpan> EndTime
        {
            get;
            set;
        }

        public string Residence
        {
            get;
            set;
        }
        
        

        public string specific
        {
            get;
            set;
        }
        

        public string Student_Name
        { get; set; }

        public string LastName
        {
            get;
            set;
        }

        public string StudentNp
        {
            get;
            set;
        }
    }
}