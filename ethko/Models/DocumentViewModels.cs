using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ethko.Models
{
    public class GetDocumentsViewModel
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
}
