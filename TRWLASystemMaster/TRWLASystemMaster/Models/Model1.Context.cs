﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TWRLADB_Staging_V2Entities : DbContext
    {
        public TWRLADB_Staging_V2Entities()
            : base("name=TWRLADB_Staging_V2Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<ComEngEvent> ComEngEvents { get; set; }
        public virtual DbSet<Content> Contents { get; set; }
        public virtual DbSet<FunctionEvent> FunctionEvents { get; set; }
        public virtual DbSet<GuestSpeaker> GuestSpeakers { get; set; }
        public virtual DbSet<Lecture> Lectures { get; set; }
        public virtual DbSet<Milestone> Milestones { get; set; }
        public virtual DbSet<Residence> Residences { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<StudentMilestone> StudentMilestones { get; set; }
        public virtual DbSet<StudentType> StudentTypes { get; set; }
        public virtual DbSet<TRWLASchedule> TRWLASchedules { get; set; }
        public virtual DbSet<UserType> UserTypes { get; set; }
        public virtual DbSet<Venue> Venues { get; set; }
        public virtual DbSet<VenueType> VenueTypes { get; set; }
        public virtual DbSet<Volunteer> Volunteers { get; set; }
        public virtual DbSet<VolunteerFeedback> VolunteerFeedbacks { get; set; }
        public virtual DbSet<VolunteerType> VolunteerTypes { get; set; }
        public virtual DbSet<Attendance> Attendances { get; set; }
        public virtual DbSet<RSVP_Event> RSVP_Event { get; set; }
    }
}