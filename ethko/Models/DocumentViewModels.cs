using System.ComponentModel.DataAnnotations;

namespace ethko.Models
{
    public class GetDocumentsViewModel
    {
        [Display(Name = "Document Id")]
        public int DocumentId { get; set; }

        [Display(Name = "Document Name")]
        public string DocumentName { get; set; }

        [Display(Name = "Case Name")]
        public string CaseName { get; set; }

        [Display(Name = "UserId")]
        public string UserId { get; set; }

        [Display(Name = "InsDate")]
        public string InsDate { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }
    }

    public class GetIndividualDocumentViewModel
    {
        [Display(Name = "Document Id")]
        public int DocumentId { get; set; }

        [Display(Name = "Document Name")]
        public string DocumentName { get; set; }

        [Display(Name = "Case Name")]
        public string CaseName { get; set; }

        [Display(Name = "UserId")]
        public string UserId { get; set; }

        [Display(Name = "InsDate")]
        public string InsDate { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }
    }

    public class AddDocumentsViewModel
    {
        [Display(Name = "Document Name")]
        public string DocumentName { get; set; }
    }

    public class OCRViewModel
    {
        [Display(Name = "Document Name")]
        public string DocumentName { get; set; }
    }
}
