﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ethko
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ethko_dbEntities1 : DbContext
    {
        public ethko_dbEntities1()
            : base("name=ethko_dbEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<BillingMethod> BillingMethods { get; set; }
        public virtual DbSet<Case> Cases { get; set; }
        public virtual DbSet<CaseStage> CaseStages { get; set; }
        public virtual DbSet<ContactGroup> ContactGroups { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<DimDate> DimDates { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<DocumentType> DocumentTypes { get; set; }
        public virtual DbSet<LeadStatus> LeadStatuses { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Office> Offices { get; set; }
        public virtual DbSet<PracticeArea> PracticeAreas { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<UserType> UserTypes { get; set; }
        public virtual DbSet<database_firewall_rules> database_firewall_rules { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Lead> Leads { get; set; }
        public virtual DbSet<LeadReferralSource> LeadReferralSources { get; set; }
        public virtual DbSet<Priority> Priorities { get; set; }
        public virtual DbSet<ToDo> ToDos { get; set; }
    }
}
