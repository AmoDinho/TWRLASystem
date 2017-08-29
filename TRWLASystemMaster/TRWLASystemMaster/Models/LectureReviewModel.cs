using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TRWLASystemMaster.Models.DB;

namespace TRWLASystemMaster.Models
{
    public class LectureReviewModel
    {
        public Lecture Lecture { get; set; }
        public LectureReview LectureReview { get; set; }
    }
}