using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TRWLASystemMaster.Models
{
    public class VerifyCode
    {
        public int UniID { get; set; }

        public int Code { get; set; }

        public DateTime stamptime { get; set; }


    }
}