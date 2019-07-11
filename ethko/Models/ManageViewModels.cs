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
    }

    public class DeleteConfirmedViewModel
    {
        [Display(Name = "Billing Method Id")]
        public int? BillingMethodId { get; set; }

        [Display(Name = "Office Id")]
        public int? OfficeId { get; set; }

        [Display(Name = "User Type Id")]
        public int? UserTypeId { get; set; }
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
}