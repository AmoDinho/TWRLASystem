using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
namespace TRWLASystemMaster.Models
{
    public class GeneralEventList
    {
        public string EventName
        {
            get;
            set;
        }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}", ApplyFormatInEditMode = true)]
        [CheckDateRange]
        public DateTime Date
        {
            get;
            set;
        }

        [Required(ErrorMessage = "A start time is required")]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan Start
        {
            get;
            set;
        }

        [Required(ErrorMessage = "A start time is required")]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan End
        {
            get;
            set;
        }

        public string StudentName
        {
            get;
            set;
        }

        public string StudentSurname
        {
            get;
            set;
        }

        public string Residence
        {
            get;
            set;
        }

        public string StudentNumber
        {
            get;
            set;
        }
    }
}