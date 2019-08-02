using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [Display(Name = "Billing MEthod Id")]
        public int BillingMethodId { get; set; }

        [Display(Name = "Case Stage")]
        public int CaseStageId { get; set; }

        [Display(Name = "Date Opened")]
        public DateTime DateOpened { get; set; }

        [Display(Name = "Office")]
        public int OfficeId { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Statute of Limitations")]
        public DateTime Statute { get; set; }
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
}
