﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TRWLASystemMaster.Models.DB;

namespace TRWLASystemMaster.Models
{
    public class SelectRecipients
    {
        public RSVP_Event RSVP_Event { get; set; }
        public Residence Residence { get; set; }
    }
}