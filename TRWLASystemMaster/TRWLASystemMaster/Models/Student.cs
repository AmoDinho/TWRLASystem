//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TRWLASystemMaster.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    
    public partial class Student
    {
        public int StudentID { get; set; }
       [Required]
        [StringLength(10, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Student number")]
        public string StudentNumber { get; set; }

        public string Graduate { get; set; }
        [Required]
        [StringLength(35, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Degree")]
        public string Degree { get; set; }

         [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public System.DateTime YearOfStudy { get; set; }

        [Required]
        [StringLength(35, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Student Name")]
        public string Student_Name { get; set; }

        [Required]
        [StringLength(35, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Student surname")]
        public string Student_Surname { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Student phone number")]
        public string Student_Phone { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public System.DateTime Student_DoB { get; set; }

   
        public string ActiveStatus { get; set; }
        public int Id { get; set; }
        public int ResID { get; set; }
        public int UserTypeID { get; set; }
        public int StudentTypeID { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual Residence Residence { get; set; }
        public virtual StudentType StudentType { get; set; }
        public virtual UserType UserType { get; set; }
    }
}
