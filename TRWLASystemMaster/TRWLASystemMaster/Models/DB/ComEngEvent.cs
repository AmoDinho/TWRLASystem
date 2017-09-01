//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
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
        [Required]
        [StringLength(35, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "ComEng_Name")]
        public string ComEng_Name { get; set; }
        [Required]
        [StringLength(35, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "ComEng_Summary")]
        public string ComEng_Summary { get; set; }
        [Required]
        [StringLength(35, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "ComEng_Description")]
        public string ComEng_Description { get; set; }
        [Required]
        public System.DateTime ComEng_Date { get; set; }
        [Required]
        public System.TimeSpan ComEnge_StartTime { get; set; }
        [Required]
        public System.TimeSpan ComEng_EndTime { get; set; }
        [Required]
        [StringLength(35, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "ComEng_Theme")]
        public string ComEng_Theme { get; set; }
        [Required]
        public Nullable<int> VenueID { get; set; }
        [Required]
        public Nullable<int> ContentID { get; set; }
    
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
