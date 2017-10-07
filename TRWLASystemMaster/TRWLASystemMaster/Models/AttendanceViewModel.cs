using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace TRWLASystemMaster.Models
{
    public class AttendanceViewModel
    {
        public string EventName
        { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}", ApplyFormatInEditMode = true)]
        [CheckDateRange]
        public Nullable<System.DateTime> EventDate
        { get; set; }

        [Required(ErrorMessage = "A start time is required")]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public Nullable<System.TimeSpan> StartTime
        {
            get; set;
        }

        [Required(ErrorMessage = "A start time is required")]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
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