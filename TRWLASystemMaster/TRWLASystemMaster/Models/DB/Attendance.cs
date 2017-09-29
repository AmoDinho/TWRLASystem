//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TRWLASystemMaster.Models.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class Attendance
    {
        public int attendanceID { get; set; }
        public Nullable<int> VolunteerID { get; set; }
        public Nullable<int> StudentID { get; set; }
        public Nullable<int> FunctionID { get; set; }
        public Nullable<int> LectureID { get; set; }
        public Nullable<int> ComEngID { get; set; }
        public Nullable<int> GenID { get; set; }
        public Nullable<int> SYSUserProfileID { get; set; }
    
        public virtual ComEngEvent ComEngEvent { get; set; }
        public virtual FunctionEvent FunctionEvent { get; set; }
        public virtual GenEvent GenEvent { get; set; }
        public virtual Lecture Lecture { get; set; }
        public virtual Student Student { get; set; }
        public virtual SYSUserProfile SYSUserProfile { get; set; }
        public virtual Volunteer Volunteer { get; set; }
    }
}
