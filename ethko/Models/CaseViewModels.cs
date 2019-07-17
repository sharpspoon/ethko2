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
        public string CaseContact { get; set; }

        [Display(Name = "Case Name")]
        public string CaseName { get; set; }

        [Display(Name = "Case Number")]
        public string CaseNumber { get; set; }

        [Display(Name = "Practice Area")]
        public string PracticeArea { get; set; }

        [Display(Name = "Practice Area Id")]
        public string PracticeAreaId { get; set; }

        [Display(Name = "Case Stage")]
        public string CaseStage { get; set; }

        [Display(Name = "Date Opened")]
        public string DateOpened { get; set; }

        [Display(Name = "Office")]
        public string Office { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Statute of Limitations")]
        public string Statute { get; set; }
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
