﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class TWRLADB_Staging_V2Entities2 : DbContext
    {
        public TWRLADB_Staging_V2Entities2()
            : base("name=TWRLADB_Staging_V2Entities2")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Attendance> Attendances { get; set; }
        public virtual DbSet<AuditLog> AuditLogs { get; set; }
        public virtual DbSet<ComEngEvent> ComEngEvents { get; set; }
        public virtual DbSet<Content> Contents { get; set; }
        public virtual DbSet<EventMessage> EventMessages { get; set; }
        public virtual DbSet<FunctionEvent> FunctionEvents { get; set; }
        public virtual DbSet<GuestSpeaker> GuestSpeakers { get; set; }
        public virtual DbSet<Lecture> Lectures { get; set; }
        public virtual DbSet<LectureReview> LectureReviews { get; set; }
        public virtual DbSet<LOOKUPRole> LOOKUPRoles { get; set; }
        public virtual DbSet<Milestone> Milestones { get; set; }
        public virtual DbSet<Progress> Progresses { get; set; }
        public virtual DbSet<RatingType> RatingTypes { get; set; }
        public virtual DbSet<Residence> Residences { get; set; }
        public virtual DbSet<RSVP_Event> RSVP_Event { get; set; }
        public virtual DbSet<RSVPSchedule> RSVPSchedules { get; set; }
        public virtual DbSet<SecurityAnswer> SecurityAnswers { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<StudentMilestone> StudentMilestones { get; set; }
        public virtual DbSet<StudentType> StudentTypes { get; set; }
        public virtual DbSet<SYSUser> SYSUsers { get; set; }
        public virtual DbSet<SYSUserProfile> SYSUserProfiles { get; set; }
        public virtual DbSet<SYSUserRole> SYSUserRoles { get; set; }
        public virtual DbSet<TRWLASchedule> TRWLASchedules { get; set; }
        public virtual DbSet<UniqueCode> UniqueCodes { get; set; }
        public virtual DbSet<UserType> UserTypes { get; set; }
        public virtual DbSet<Venue> Venues { get; set; }
        public virtual DbSet<VenueType> VenueTypes { get; set; }
        public virtual DbSet<Volunteer> Volunteers { get; set; }
        public virtual DbSet<VolunteerFeedback> VolunteerFeedbacks { get; set; }
        public virtual DbSet<VolunteerType> VolunteerTypes { get; set; }
        public virtual DbSet<GenEvent> GenEvents { get; set; }
        public virtual DbSet<MasterData> MasterDatas { get; set; }
        public virtual DbSet<progressbar> progressbars { get; set; }
    
        public virtual int BackUp(string path)
        {
            var pathParameter = path != null ?
                new ObjectParameter("path", path) :
                new ObjectParameter("path", typeof(string));
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("BackUp", pathParameter);
        }
    }
}
