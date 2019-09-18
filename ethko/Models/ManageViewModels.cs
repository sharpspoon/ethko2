using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace ethko.Models
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }

    public class AddCaseStageViewModel
    {
        [Required]
        [Display(Name = "Case Stage")]
        public string CaseStageName { get; set; }
    }

    public class GetCaseStagesViewModel
    {
        [Display(Name = "Case Stage")]
        public string CaseStageName { get; set; }

        [Display(Name = "Case Stage Id")]
        public string CaseStageId { get; set; }

        [Display(Name = "UserId")]
        public string UserId { get; set; }

        [Display(Name = "InsDate")]
        public string InsDate { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }
    }

    public class GetFirmSettingsViewModel
    {
        [Display(Name = "Office Name")]
        public string OfficeName { get; set; }

        [Display(Name = "Office Id")]
        public string OfficeId { get; set; }

        [Display(Name = "User Type")]
        public string UserTypeName { get; set; }

        [Display(Name = "User Type Id")]
        public string UserTypeId { get; set; }

        [Display(Name = "FstUser")]
        public string FstUser { get; set; }

        [Display(Name = "InsDate")]
        public string InsDate { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }
    }

    public class GetUserTypesViewModel
    {
        [Display(Name = "User Type Name")]
        public string UserTypeName { get; set; }

        [Display(Name = "User Type Id")]
        public string UserTypeId { get; set; }

        [Display(Name = "FstUser")]
        public string FstUser { get; set; }

        [Display(Name = "InsDate")]
        public string InsDate { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }
    }

    public class AddOfficeViewModel
    {
        [Required]
        [Display(Name = "Office")]
        public string OfficeName { get; set; }
    }

    public class AddBillingMethodViewModel
    {
        [Required]
        [Display(Name = "Billing Method")]
        public string BillingMethodName { get; set; }
    }

    public class AddLeadStatusViewModel
    {
        [Required]
        [Display(Name = "Lead Status")]
        public string LeadStatusName { get; set; }
    }

    public class AddLeadReferralSourceViewModel
    {
        [Required]
        [Display(Name = "Referral Source")]
        public string ReferralSourceName { get; set; }
    }

    public class GetClientBillingViewModel
    {
        [Display(Name = "Billing Method Name")]
        public string BillingMethodName { get; set; }

        [Display(Name = "Billing Method Id")]
        public string BillingMethodId { get; set; }

        [Display(Name = "FstUser")]
        public string FstUser { get; set; }

        [Display(Name = "InsDate")]
        public string InsDate { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }
    }

    public class DeleteConfirmedViewModel
    {
        [Display(Name = "Billing Method Id")]
        public int? BillingMethodId { get; set; }

        [Display(Name = "Office Id")]
        public int? OfficeId { get; set; }

        [Display(Name = "User Type Id")]
        public int? UserTypeId { get; set; }

        [Display(Name = "Case Stage Id")]
        public int? CaseStageId { get; set; }

        [Display(Name = "Lead Status Id")]
        public int? LeadStatusId { get; set; }

        [Display(Name = "Lead Referral Source Id")]
        public int? LeadReferralSourceId { get; set; }
    }

    public class GetFirmUsersViewModel
    {
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Display(Name = "USerTypes")]
        public string UserType { get; set; }
    }

    public class AddUserTypeViewModel
    {
        [Required]
        [Display(Name = "User Type")]
        public string UserTypeName { get; set; }
    }

    public class AddFirmUserViewModel
    {
        [Display(Name = "First Name")]
        public string FName { get; set; }

        [Display(Name = "Middle Name")]
        public string MName { get; set; }

        [Display(Name = "Last Name")]
        public string LName { get; set; }

        [Display(Name = "Email")]
        public string UserType { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }
    }

    public class GetLeadReferralSourcesViewModel
    {
        [Display(Name = "Referral Source Name")]
        public string ReferralSourceName { get; set; }

        [Display(Name = "Referral Source Id")]
        public string ReferralSourceId { get; set; }

        [Display(Name = "FstUser")]
        public string FstUser { get; set; }

        [Display(Name = "InsDate")]
        public string InsDate { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }
    }

    public class GetLeadStatusesViewModel
    {
        [Display(Name = "Lead Status Name")]
        public string LeadStatusName { get; set; }

        [Display(Name = "Lead Status Id")]
        public string LeadStatusId { get; set; }

        [Display(Name = "FstUser")]
        public string FstUser { get; set; }

        [Display(Name = "InsDate")]
        public string InsDate { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }
    }

    public class EditConfirmedViewModel
    {
        [Display(Name = "Billing Method Id")]
        public int? BillingMethodId { get; set; }

        [Display(Name = "Office Id")]
        public int? OfficeId { get; set; }

        [Display(Name = "User Type Id")]
        public int? UserTypeId { get; set; }

        [Display(Name = "Office")]
        public string OfficeName { get; set; }
    }

    public class GetEditNotificationsViewModel
    {
        public string N1 { get; set; }
        public string N2 { get; set; }
        public string N3 { get; set; }
        public string N4 { get; set; }
        public string N5 { get; set; }
        public string N6 { get; set; }
        public string N7 { get; set; }
        public string N8 { get; set; }
        public string N9 { get; set; }
        public string N10 { get; set; }
        public string N11 { get; set; }
        public string N12 { get; set; }
        public string N13 { get; set; }
        public string N14 { get; set; }
        public string N15 { get; set; }
        public string N16 { get; set; }
        public string N17 { get; set; }
        public string N18 { get; set; }
        public string N19 { get; set; }
        public string N20 { get; set; }
        public string N21 { get; set; }
        public string N22 { get; set; }
        public string N23 { get; set; }
        public string N24 { get; set; }
        public string N25 { get; set; }
        public string N26 { get; set; }
        public string N27 { get; set; }
        public string N28 { get; set; }
        public string N29 { get; set; }
        public string N30 { get; set; }
        public string N31 { get; set; }
        public string N32 { get; set; }
        public string N33 { get; set; }
        public string N34 { get; set; }
        public string N35 { get; set; }
        public string N36 { get; set; }
        public string N37 { get; set; }
        public string N38 { get; set; }
        public string N39 { get; set; }
        public string N40 { get; set; }
        public string N41 { get; set; }
        public string N42 { get; set; }
        public string N43 { get; set; }
        public string N44 { get; set; }
        public string N45 { get; set; }
        public string N46 { get; set; }
        public string N47 { get; set; }
        public string N48 { get; set; }
        public string N49 { get; set; }
        public string N50 { get; set; }
        public string N51 { get; set; }
        public string N52 { get; set; }
        public string N53 { get; set; }
        public string N54 { get; set; }
        public string N55 { get; set; }
        public string N56 { get; set; }
        public string N57 { get; set; }
        public string N58 { get; set; }
        public string N59 { get; set; }
        public string N60 { get; set; }
        public string N61 { get; set; }
        public string N62 { get; set; }
        public string N63 { get; set; }
        public string N64 { get; set; }
        public string N65 { get; set; }
        public string N66 { get; set; }
        public string N67 { get; set; }
        public string N68 { get; set; }
        public string N69 { get; set; }
        public string N70 { get; set; }
        public string N71 { get; set; }
        public string N72 { get; set; }
        public string N73 { get; set; }
        public string N74 { get; set; }
        public string N75 { get; set; }
        public string N76 { get; set; }
        public string N77 { get; set; }
        public string N78 { get; set; }
        public string N79 { get; set; }
        public string N80 { get; set; }
        public string N81 { get; set; }
        public string N82 { get; set; }
        public string N83 { get; set; }
        public string N84 { get; set; }
        public string N85 { get; set; }
        public string N86 { get; set; }
        public string N87 { get; set; }
        public string N88 { get; set; }
        public string N89 { get; set; }
        public string N90 { get; set; }
        public string N91 { get; set; }
        public string N92 { get; set; }
        public string N93 { get; set; }
        public string N94 { get; set; }
        public string N95 { get; set; }
        public string N96 { get; set; }
        public string N97 { get; set; }
        public string N98 { get; set; }
        public string N99 { get; set; }
        public string N100 { get; set; }
        public string N101 { get; set; }
        public string N102 { get; set; }
        public string N103 { get; set; }
        public string N104 { get; set; }
        public string N105 { get; set; }
        public string N106 { get; set; }
        public string N107 { get; set; }
        public string N108 { get; set; }
        public string N109 { get; set; }
        public string N110 { get; set; }
        public string N111 { get; set; }
        public string N112 { get; set; }
        public string N113 { get; set; }
        public string N114 { get; set; }
        public string N115 { get; set; }
        public string N116 { get; set; }
        public string N117 { get; set; }
        public string N118 { get; set; }
        public string N119 { get; set; }
        public string N120 { get; set; }
        public string N121 { get; set; }
        public string N122 { get; set; }
        public string N123 { get; set; }
        public string N124 { get; set; }
        public string N125 { get; set; }
        public string N126 { get; set; }
        public string N127 { get; set; }
        public string N128 { get; set; }
        public string N129 { get; set; }
        public string N130 { get; set; }
        public string N131 { get; set; }
        public string N132 { get; set; }
        public string N133 { get; set; }
        public string N134 { get; set; }
        public string N135 { get; set; }
        public string N136 { get; set; }
        public string N137 { get; set; }
        public string N138 { get; set; }
        public string N139 { get; set; }
        public string N140 { get; set; }
        public string N141 { get; set; }
        public string N142 { get; set; }
        public string N143 { get; set; }
        public string N144 { get; set; }
        public string N145 { get; set; }
        public string N146 { get; set; }
        public string N147 { get; set; }
        public string N148 { get; set; }
        public string N149 { get; set; }
        public string N150 { get; set; }
    }
}