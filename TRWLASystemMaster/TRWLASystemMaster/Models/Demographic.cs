using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TRWLASystemMaster.Models
{
    public class Demographic
    {
        public string StudNo
        { get; set; }

        public string Name
        { get; set; }

        public string Surname
        { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DoB
        { get; set; }

        public string Degree
        { get; set; }

        public string email
        { get; set; }

        public string Res
        { get; set; }
        
    }
}