using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ethko.Models
{
    //Add
    public class AddContactIndividualViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FName { get; set; }

        [Display(Name = "Middle Name")]
        public string MName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Client Portal")]
        public string EnableClientPortal { get; set; }

        [Display(Name = "Contact Group")]
        public string ContactGroupId { get; set; }

        [Display(Name = "Cell Phone")]
        public string CellPhone { get; set; }

        [Display(Name = "Work Phone")]
        public string WorkPhone { get; set; }

        [Display(Name = "Home Phone")]
        public string HomePhone { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Address2")]
        public string Address2 { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "State")]
        public string State { get; set; }

        [Display(Name = "Zip")]
        public string Zip { get; set; }

        [Display(Name = "Country")]
        public string Country { get; set; }
    }
    public class AddCompanyViewModel
    {
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Website")]
        public string Website { get; set; }

        [Display(Name = "Main Phone")]
        public string MainPhone { get; set; }

        [Display(Name = "Fax Number")]
        public string FaxNumber { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Address2")]
        public string Address2 { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "State")]
        public string State { get; set; }

        [Display(Name = "Zip")]
        public string Zip { get; set; }

        [Display(Name = "Country")]
        public string Country { get; set; }
    }
    public class AddContactGroupViewModel
    {
        [Required]
        [Display(Name = "Contact Group")]
        public string ContactGroupName { get; set; }
    }

    public class DeleteContactIndividualViewModel
    {
        [Display(Name = "First Name")]
        public string FName { get; set; }

        [Display(Name = "Middle Name")]
        public string MName { get; set; }
        
        [Display(Name = "Last Name")]
        public string LName { get; set; }
        public string ContactId { get; set; }
    }

    
    [Table("Contacts")]
    public class GetContactListViewModel
    {
        [Required]
        [Display(Name = "ContactId")]
        public string ContactId { get; set; }
        [Display(Name = "First Name")]
        public string FName { get; set; }
        [Display(Name = "Last Name")]
        public string LName { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "InsDate")]
        public string InsDate { get; set; }
        [Display(Name = "UserId")]
        public string UserId { get; set; }
        [Display(Name = "ContactGroupList")]
        public string ContactGroupList { get; set; }
        public Contact Contact { get; set; }
        public ContactGroup ContactGroup { get; set; }
    }

    [Table("Contacts")]
    public class GetContactArchiveListViewModel
    {
        [Required]
        [Display(Name = "ContactId")]
        public string ContactId { get; set; }
        [Display(Name = "First Name")]
        public string FName { get; set; }
        [Display(Name = "Last Name")]
        public string LName { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "InsDate")]
        public string InsDate { get; set; }
        [Display(Name = "UserId")]
        public string UserId { get; set; }
        [Display(Name = "ContactGroupList")]
        public string ContactGroupList { get; set; }
        public Contact Contact { get; set; }
        public ContactGroup ContactGroup { get; set; }
    }

    [Table("Companies")]
    public class GetCompanyListViewModel
    {
        [Required]
        [Display(Name = "ContactId")]
        public string CompanyId { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "InsDate")]
        public string InsDate { get; set; }
        [Display(Name = "UserId")]
        public string FstUser { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
    }

    [Table("ContactGroups")]
    public class GetContactGroupListViewModel
    {
        [Required]
        [Display(Name = "ContactGroupId")]
        public string ContactGroupId { get; set; }
        [Display(Name = "ContactGroupName")]
        public string ContactGroupName { get; set; }
        [Display(Name = "InsDate")]
        public string InsDate { get; set; }
        [Display(Name = "UserId")]
        public string FstUser { get; set; }
        [Display(Name = "ContactCount")]
        public int ContactCount { get; set; }
    }

    public class GetIndividualContactViewModel
    {
        [Display(Name = "ContactId")]
        public int ContactId { get; set; }

        [Display(Name = "CompanyId")]
        public int CompanyId { get; set; }

        [Display(Name = "First Name")]
        public string FName { get; set; }

        [Display(Name = "Middle Name")]
        public string MName { get; set; }

        [Display(Name = "Last Name")]
        public string LName { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Client Portal")]
        public short EnableClientPortal { get; set; }

        [Display(Name = "Contact Group")]
        public string ContactGroupId { get; set; }

        [Display(Name = "Cell Phone")]
        public string CellPhone { get; set; }

        [Display(Name = "Work Phone")]
        public string WorkPhone { get; set; }

        [Display(Name = "Home Phone")]
        public string HomePhone { get; set; }

        [Display(Name = "Fax")]
        public string Fax { get; set; }

        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }

        [Display(Name = "Birthday")]
        public DateTime? Birthday { get; set; }

        [Display(Name = "License")]
        public string License { get; set; }

        [Display(Name = "Website")]
        public string Website { get; set; }

        [Display(Name = "Notes")]
        public string Notes { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Address2")]
        public string Address2 { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "State")]
        public string State { get; set; }

        [Display(Name = "Zip")]
        public string Zip { get; set; }

        [Display(Name = "Country")]
        public string Country { get; set; }

        [Display(Name = "Archived")]
        public int Archived { get; set; }

        [Display(Name = "UserId")]
        public string UserId { get; set; }

        [Display(Name = "InsDate")]
        public string InsDate { get; set; }

        [Display(Name = "contactGroupList")]
        public string ContactGroupList { get; set; }
    }

    public class EditConfirmedContactViewModel
    {
        [Display(Name = "Contact Group Id")]
        public int? ContactGroupId { get; set; }
    }

    public class DeleteConfirmedContactViewModel
    {
        [Display(Name = "Contact Group Id")]
        public int? ContactGroupId { get; set; }
    }
}