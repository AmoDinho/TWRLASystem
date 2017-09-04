using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TRWLASystemMaster.Models.DB;
using System.ComponentModel.DataAnnotations;

namespace TRWLASystemMaster.Models
{
    public class LectureReviewModel
    {
        public int reviewID { get; set; }
        public string Review { get; set; }
        public int RatingID { get; set; }
        public int LectureID { get; set; }
        public Nullable<int> SYSUserProfileID { get; set; }

        public virtual Lecture Lecture { get; set; }
        public virtual RatingType RatingType { get; set; }
        public virtual SYSUserProfile SYSUserProfile { get; set; }
    }
}