using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ethko.Models
{
    public class AddCaseViewModel
    {
        [Required]
        [Display(Name = "Contact")]
        public int ContactId { get; set; }

        [Display(Name = "Case Name")]
        public string CaseName { get; set; }

        [Display(Name = "Case Number")]
        public string CaseNumber { get; set; }

        [Display(Name = "Practice Area")]
        public string PracticeArea { get; set; }

        [Display(Name = "Practice Area Id")]
        public int PracticeAreaId { get; set; }

        [Display(Name = "Billing Method Id")]
        public int BillingMethodId { get; set; }

        [Display(Name = "Billing Contact Id")]
        public int BillingContactId { get; set; }

        [Display(Name = "Case Stage")]
        public int CaseStageId { get; set; }

        [Display(Name = "Date Opened")]
        public int DateOpened { get; set; }

        [Display(Name = "Office")]
        public int OfficeId { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Statute of Limitations")]
        public int Statute { get; set; }

        [Display(Name = "Lead Attorney Id")]
        public string LeadAttorneyId { get; set; }
    }

    public class GetPracticeAreasViewModel
    {
        [Display(Name = "Practice Area")]
        public string PracticeAreaName { get; set; }

        [Display(Name = "Active Cases")]
        public string ActiveCases { get; set; }

        [Display(Name = "Practice Area Id")]
        public string PracticeAreaId { get; set; }

        [Display(Name = "Case Number")]
        public string CaseNumber { get; set; }

        [Display(Name = "UserId")]
        public string UserId { get; set; }

        [Display(Name = "InsDate")]
        public string InsDate { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }
    }

    [Table("Cases")]
    public class GetCaseListViewModel
    {
        [Required]
        [Display(Name = "CaseId")]
        public int CaseId { get; set; }

        [Display(Name = "CaseNumber")]
        public string CaseNumber { get; set; }

        [Display(Name = "CaseName")]
        public string CaseName { get; set; }

        [Display(Name = "CaseStageId")]
        public int CaseStageId { get; set; }

        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Display(Name = "FstUser")]
        public string FstUser { get; set; }

        [Display(Name = "InsDate")]
        public string InsDate { get; set; }

        [Display(Name = "Case Stage")]
        public string CaseStageName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }
    }

    public class AddPracticeAreaViewModel
    {
        [Display(Name = "Practice Area")]
        public string PracticeAreaName { get; set; }

        [Display(Name = "Active Cases")]
        public string ActiveCases { get; set; }

        [Display(Name = "Practice Area Id")]
        public string PracticeAreaId { get; set; }

        [Display(Name = "Case Number")]
        public string CaseNumber { get; set; }

        [Display(Name = "UserId")]
        public string UserId { get; set; }

        [Display(Name = "InsDate")]
        public string InsDate { get; set; }
    }

    public class DeleteConfirmedCaseViewModel
    {
        [Display(Name = "Practice Area Id")]
        public int? PracticeAreaId { get; set; }
    }

    public class GetIndividualCaseViewModel
    {
        [Display(Name = "CaseId")]
        public int CaseId { get; set; }

        [Display(Name = "CaseNumber")]
        public string CaseNumber { get; set; }

        [Display(Name = "CaseName")]
        public string CaseName { get; set; }

        [Display(Name = "Practice Area")]
        public string PracticeAreaName { get; set; }

        [Display(Name = "Case Stage")]
        public string CaseStageName { get; set; }

        [Display(Name = "Statute")]
        public string Statute { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Date Opened")]
        public string DateOpened { get; set; }
        
        [Display(Name = "Date Created")]
        public string DateCreated { get; set; }

        [Display(Name = "Lead Attorney")]
        public string LeadAttorney { get; set; }
        public string Files { get; set; }
        public GetCaseDocumentsViewModel Children { get; set; }
    }

    public class GetCaseDocumentsViewModel
    {
        [Display(Name = "CaseName")]
        public string CaseName { get; set; }
    }
}
