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
    using System.Collections.Generic;
    
    public partial class TRWLASchedule
    {
        public int ScheduleID { get; set; }
        public Nullable<int> FunctionID { get; set; }
        public Nullable<int> LectureID { get; set; }
        public Nullable<int> ComEngID { get; set; }
    
        public virtual ComEngEvent ComEngEvent { get; set; }
        public virtual FunctionEvent FunctionEvent { get; set; }
        public virtual Lecture Lecture { get; set; }
    }
}
