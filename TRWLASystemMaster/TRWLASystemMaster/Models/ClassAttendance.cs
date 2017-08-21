using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TRWLASystemMaster.Models
{
    public class ClassAttendance
    {
        public string Name
        { get; set; }

        public string Surname
        { get; set; }

        public string Email
        { get; set; }

        public int PersonalCount
        { get; set; }

        public int EventCount
        { get; set; }

        public int StudentID
        { get; set; }
    }
}