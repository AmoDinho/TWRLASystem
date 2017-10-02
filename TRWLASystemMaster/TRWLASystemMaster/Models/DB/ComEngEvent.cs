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
    
    public partial class ComEngEvent
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ComEngEvent()
        {
            this.Attendances = new HashSet<Attendance>();
            this.RSVP_Event = new HashSet<RSVP_Event>();
            this.TRWLASchedules = new HashSet<TRWLASchedule>();
        }
    
        public int ComEngID { get; set; }
        public string ComEng_Name { get; set; }
        public string ComEng_Summary { get; set; }
        public string ComEng_Description { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}", ApplyFormatInEditMode = true)]

        public System.DateTime ComEng_Date { get; set; }
        public System.TimeSpan ComEnge_StartTime { get; set; }
        public System.TimeSpan ComEng_EndTime { get; set; }
        public string ComEng_Theme { get; set; }
        public Nullable<int> VenueID { get; set; }
        public Nullable<int> ContentID { get; set; }
        public int Type { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Attendance> Attendances { get; set; }
        public virtual Content Content { get; set; }
        public virtual Venue Venue { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RSVP_Event> RSVP_Event { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TRWLASchedule> TRWLASchedules { get; set; }
    }
}
