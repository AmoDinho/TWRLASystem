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

        public string Student_Name
        { get; set; }
    }
}