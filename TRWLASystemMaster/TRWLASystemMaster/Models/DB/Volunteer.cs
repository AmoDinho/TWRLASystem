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
    
    public partial class Volunteer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Volunteer()
        {
            this.Attendances = new HashSet<Attendance>();
            this.EventMessages = new HashSet<EventMessage>();
            this.RSVP_Event = new HashSet<RSVP_Event>();
        }
    
        public int VolunteerID { get; set; }
        public string Volunteer_Name { get; set; }
        public string Volunteer_Surname { get; set; }
        public string Volunteer_Phone { get; set; }
        public System.DateTime Volunteer_DoB { get; set; }
        public string ActiveStatus { get; set; }
        public int UserTypeID { get; set; }
        public int VolunteerTypeID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Attendance> Attendances { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EventMessage> EventMessages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RSVP_Event> RSVP_Event { get; set; }
        public virtual UserType UserType { get; set; }
        public virtual VolunteerType VolunteerType { get; set; }
    }
}
