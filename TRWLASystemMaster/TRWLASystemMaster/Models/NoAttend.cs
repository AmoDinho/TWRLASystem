using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TRWLASystemMaster.Models
{
    public class NoAttend
    {
        public string StudNo
        { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> FunctionDate
        {
            get; set;
        }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> LectureDate
        {
            get; set;
        }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> ComEngDate
        {
            get; set;
        }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> GenDate
        {
            get; set;
        }

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

        public string Grad
        { get; set; }

        public string Res
        { get; set; }

        public string FuncName
        { get; set; }

        public string ComName
        { get; set; }

        public string LecName
        { get; set; }

        public string GenName
        { get; set; }
    }
}