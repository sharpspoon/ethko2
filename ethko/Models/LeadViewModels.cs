using System.ComponentModel.DataAnnotations;

namespace ethko.Models
{
    public class AddLeadViewModel
    {
        public string FName { get; set; }

        public string MName { get; set; }

        public string LName { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }
        public string CellPhone { get; set; }
        public string LeadNotes { get; set; }
        public string CaseNotes { get; set; }
        public int PotentialValue { get; set; }
    }

    public class GetLeadListViewModel
    {
        public string ContactId { get; set; }
    }
}