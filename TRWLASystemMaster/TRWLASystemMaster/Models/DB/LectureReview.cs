//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TRWLASystemMaster.Models.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class LectureReview
    {
        public int reviewID { get; set; }

        [Required(ErrorMessage = "A review is Required")]
        [Display(Name = "Review")]
        public string Review { get; set; }
        public int RatingID { get; set; }
        public int LectureID { get; set; }
        public Nullable<int> SYSUserProfileID { get; set; }
    
        public virtual Lecture Lecture { get; set; }
        public virtual RatingType RatingType { get; set; }
        public virtual SYSUserProfile SYSUserProfile { get; set; }
    }
}
