//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Case
    {
        public int CaseId { get; set; }
        public int ContactId { get; set; }
        public int BillingContactId { get; set; }
        public string LeadAttorneyId { get; set; }
        public string CaseName { get; set; }
        public string CaseNumber { get; set; }
        public int PracticeAreaId { get; set; }
        public int BillingMethodId { get; set; }
        public int OfficeId { get; set; }
        public int CaseStageId { get; set; }
        public int DateOpened { get; set; }
        public int Statute { get; set; }
        public string Description { get; set; }
        public string FstUser { get; set; }
        public int InsDate { get; set; }
        public int LstDate { get; set; }
        public string LstUser { get; set; }
        public byte[] RowVersion { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual BillingMethod BillingMethod { get; set; }
        public virtual CaseStage CaseStage { get; set; }
        public virtual Contact Contact { get; set; }
        public virtual Contact Contact1 { get; set; }
        public virtual Office Office { get; set; }
        public virtual PracticeArea PracticeArea { get; set; }
    }
}
