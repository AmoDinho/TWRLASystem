﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TRWLASystemMaster.Models
{
    public class SendMessage
    {
        [Required]
        public string Message
        { get; set; }
    }
}