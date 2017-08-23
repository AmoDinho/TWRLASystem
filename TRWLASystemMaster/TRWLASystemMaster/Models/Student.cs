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
    
    public partial class Student
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Student()
        {
            this.Attendances = new HashSet<Attendance>();
            this.LectureReviews = new HashSet<LectureReview>();
            this.RSVP_Event = new HashSet<RSVP_Event>();
            this.RSVPSchedules = new HashSet<RSVPSchedule>();
            this.SecurityAnswers = new HashSet<SecurityAnswer>();
        }
    
        public int StudentID { get; set; }
        public string StudentNumber { get; set; }
        public string Graduate { get; set; }
        public string Degree { get; set; }
        public System.DateTime YearOfStudy { get; set; }
        public string Student_Name { get; set; }
        public string Student_Surname { get; set; }
        public string Student_Phone { get; set; }
        public System.DateTime Student_DoB { get; set; }
        public string ActiveStatus { get; set; }
        public int Id { get; set; }
        public int ResID { get; set; }
        public int UserTypeID { get; set; }
        public int StudentTypeID { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Attendance> Attendances { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LectureReview> LectureReviews { get; set; }
        public virtual Residence Residence { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RSVP_Event> RSVP_Event { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RSVPSchedule> RSVPSchedules { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SecurityAnswer> SecurityAnswers { get; set; }
        public virtual StudentType StudentType { get; set; }
        public virtual UserType UserType { get; set; }
    }
}
