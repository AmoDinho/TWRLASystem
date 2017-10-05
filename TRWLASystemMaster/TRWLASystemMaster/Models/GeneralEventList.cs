using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TRWLASystemMaster.Models
{
    public class GeneralEventList
    {
        public string EventName
        {
            get;
            set;
        }

        public DateTime Date
        {
            get;
            set;
        }

        public TimeSpan Start
        {
            get;
            set;
        }

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