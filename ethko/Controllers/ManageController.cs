using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ethko.Models;
using System.Net;
using System.Diagnostics;

namespace ethko.Controllers
{
    
    //[Authorize]
    public class ManageController : Controller
    {
        ethko_dbEntities entities = new ethko_dbEntities();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            return View(model);
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }

        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //////////////////////////////////////////////////////////////
        //SETTINGS DASHBOARD//////////////////////////////////////////
        //////////////////////////////////////////////////////////////
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //////////////////////////////////////////////////////////////
        //MY PROFILE//////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        // GET: /Manage/MyProfile
        public ActionResult MyProfile()
        {
            return View();
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //////////////////////////////////////////////////////////////
        //MY SETTINGS/////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        // POST: /Manage/MySettings
        public ActionResult MySettings()
        {
            return View();
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //////////////////////////////////////////////////////////////
        //MY NOTIFICATIONS////////////////////////////////////////////
        //////////////////////////////////////////////////////////////
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        // GET: /Manage/MyNotifications
        [HttpGet]
        public ActionResult MyNotifications()
        {
            var c1 = Request.Form["c1"];
            Debug.WriteLine("c1="+c1);

            using (ethko_dbEntities entities = new ethko_dbEntities())
            {
                var notifications = from n in entities.Notifications
                                        //join ut in entities.UserTypes on fu.UserTypeId equals ut.UserTypeId
                                    select new GetEditNotificationsViewModel()
                                    {   N1 = (n.N1 == "on") ? "checked" : null,
                                        N2 = (n.N2 == "on") ? "checked" : null,
                                        N3 = (n.N3 == "on") ? "checked" : null,
                                        N4 = (n.N4 == "on") ? "checked" : null,
                                        N5 = (n.N5 == "on") ? "checked" : null,
                                        N6 = (n.N6 == "on") ? "checked" : null,
                                        N7 = (n.N7 == "on") ? "checked" : null,
                                        N8 = (n.N8 == "on") ? "checked" : null,
                                        N9 = (n.N9 == "on") ? "checked" : null,
                                        N10 = (n.N10 == "on") ? "checked" : null,
                                        N11 = (n.N11 == "on") ? "checked" : null,
                                        N12 = (n.N12 == "on") ? "checked" : null,
                                        N13 = (n.N13 == "on") ? "checked" : null,
                                        N14 = (n.N14 == "on") ? "checked" : null,
                                        N15 = (n.N15 == "on") ? "checked" : null,
                                        N16 = (n.N16 == "on") ? "checked" : null,
                                        N17 = (n.N17 == "on") ? "checked" : null,
                                        N18 = (n.N18 == "on") ? "checked" : null,
                                        N19 = (n.N19 == "on") ? "checked" : null,
                                        N20 = (n.N20 == "on") ? "checked" : null,
                                        N21 = (n.N21 == "on") ? "checked" : null,
                                        N22 = (n.N22 == "on") ? "checked" : null,
                                        N23 = (n.N23 == "on") ? "checked" : null,
                                        N24 = (n.N24 == "on") ? "checked" : null,
                                        N25 = (n.N25 == "on") ? "checked" : null,
                                        N26 = (n.N26 == "on") ? "checked" : null,
                                        N27 = (n.N27 == "on") ? "checked" : null,
                                        N28 = (n.N28 == "on") ? "checked" : null,
                                        N29 = (n.N29 == "on") ? "checked" : null,
                                        N30 = (n.N30 == "on") ? "checked" : null,
                                        N31 = (n.N31 == "on") ? "checked" : null,
                                        N32 = (n.N32 == "on") ? "checked" : null,
                                        N33 = (n.N33 == "on") ? "checked" : null,
                                        N34 = (n.N34 == "on") ? "checked" : null,
                                        N35 = (n.N35 == "on") ? "checked" : null,
                                        N36 = (n.N36 == "on") ? "checked" : null,
                                        N37 = (n.N37 == "on") ? "checked" : null,
                                        N38 = (n.N38 == "on") ? "checked" : null,
                                        N39 = (n.N39 == "on") ? "checked" : null,
                                        N40 = (n.N40 == "on") ? "checked" : null,
                                        N41 = (n.N41 == "on") ? "checked" : null,
                                        N42 = (n.N42 == "on") ? "checked" : null,
                                        N43 = (n.N43 == "on") ? "checked" : null,
                                        N44 = (n.N44 == "on") ? "checked" : null,
                                        N45 = (n.N45 == "on") ? "checked" : null,
                                        N46 = (n.N46 == "on") ? "checked" : null,
                                        N47 = (n.N47 == "on") ? "checked" : null,
                                        N48 = (n.N48 == "on") ? "checked" : null,
                                        N49 = (n.N49 == "on") ? "checked" : null,
                                        N50 = (n.N50 == "on") ? "checked" : null,
                                        N51 = (n.N51 == "on") ? "checked" : null,
                                        N52 = (n.N52 == "on") ? "checked" : null,
                                        N53 = (n.N53 == "on") ? "checked" : null,
                                        N54 = (n.N54 == "on") ? "checked" : null,
                                        N55 = (n.N55 == "on") ? "checked" : null,
                                        N56 = (n.N56 == "on") ? "checked" : null,
                                        N57 = (n.N57 == "on") ? "checked" : null,
                                        N58 = (n.N58 == "on") ? "checked" : null,
                                        N59 = (n.N59 == "on") ? "checked" : null,
                                        N60 = (n.N60 == "on") ? "checked" : null,
                                        N61 = (n.N61 == "on") ? "checked" : null,
                                        N62 = (n.N62 == "on") ? "checked" : null,
                                        N63 = (n.N63 == "on") ? "checked" : null,
                                        N64 = (n.N64 == "on") ? "checked" : null,
                                        N65 = (n.N65 == "on") ? "checked" : null,
                                        N66 = (n.N66 == "on") ? "checked" : null,
                                        N67 = (n.N67 == "on") ? "checked" : null,
                                        N68 = (n.N68 == "on") ? "checked" : null,
                                        N69 = (n.N69 == "on") ? "checked" : null,
                                        N70 = (n.N70 == "on") ? "checked" : null,
                                        N71 = (n.N71 == "on") ? "checked" : null,
                                        N72 = (n.N72 == "on") ? "checked" : null,
                                        N73 = (n.N73 == "on") ? "checked" : null,
                                        N74 = (n.N74 == "on") ? "checked" : null,
                                        N75 = (n.N75 == "on") ? "checked" : null,
                                        N76 = (n.N76 == "on") ? "checked" : null,
                                        N77 = (n.N77 == "on") ? "checked" : null,
                                        N78 = (n.N78 == "on") ? "checked" : null,
                                        N79 = (n.N79 == "on") ? "checked" : null,
                                        N80 = (n.N80 == "on") ? "checked" : null,
                                        N81 = (n.N81 == "on") ? "checked" : null,
                                        N82 = (n.N82 == "on") ? "checked" : null,
                                        N83 = (n.N83 == "on") ? "checked" : null,
                                        N84 = (n.N84 == "on") ? "checked" : null,
                                        N85 = (n.N85 == "on") ? "checked" : null,
                                        N86 = (n.N86 == "on") ? "checked" : null,
                                        N87 = (n.N87 == "on") ? "checked" : null,
                                        N88 = (n.N88 == "on") ? "checked" : null,
                                        N89 = (n.N89 == "on") ? "checked" : null,
                                        N90 = (n.N90 == "on") ? "checked" : null,
                                        N91 = (n.N91 == "on") ? "checked" : null,
                                        N92 = (n.N92 == "on") ? "checked" : null,
                                        N93 = (n.N93 == "on") ? "checked" : null,
                                        N94 = (n.N94 == "on") ? "checked" : null,
                                        N95 = (n.N95 == "on") ? "checked" : null,
                                        N96 = (n.N96 == "on") ? "checked" : null,
                                        N97 = (n.N97 == "on") ? "checked" : null,
                                        N98 = (n.N98 == "on") ? "checked" : null,
                                        N99 = (n.N99 == "on") ? "checked" : null,
                                        N100 = (n.N100 == "on") ? "checked" : null,
                                        N101 = (n.N101 == "on") ? "checked" : null,
                                        N102 = (n.N102 == "on") ? "checked" : null,
                                        N103 = (n.N103 == "on") ? "checked" : null,
                                        N104 = (n.N104 == "on") ? "checked" : null,
                                        N105 = (n.N105 == "on") ? "checked" : null,
                                        N106 = (n.N106 == "on") ? "checked" : null,
                                        N107 = (n.N107 == "on") ? "checked" : null,
                                        N108 = (n.N108 == "on") ? "checked" : null,
                                        N109 = (n.N109 == "on") ? "checked" : null,
                                        N110 = (n.N110 == "on") ? "checked" : null,
                                        N111 = (n.N111 == "on") ? "checked" : null,
                                        N112 = (n.N112 == "on") ? "checked" : null,
                                        N113 = (n.N113 == "on") ? "checked" : null,
                                        N114 = (n.N114 == "on") ? "checked" : null,
                                        N115 = (n.N115 == "on") ? "checked" : null,
                                        N116 = (n.N116 == "on") ? "checked" : null,
                                        N117 = (n.N117 == "on") ? "checked" : null,
                                        N118 = (n.N118 == "on") ? "checked" : null,
                                        N119 = (n.N119 == "on") ? "checked" : null,
                                        N120 = (n.N120 == "on") ? "checked" : null,
                                        N121 = (n.N121 == "on") ? "checked" : null,
                                        N122 = (n.N122 == "on") ? "checked" : null,
                                        N123 = (n.N123 == "on") ? "checked" : null,
                                        N124 = (n.N124 == "on") ? "checked" : null,
                                        N125 = (n.N125 == "on") ? "checked" : null,
                                        N126 = (n.N126 == "on") ? "checked" : null,
                                        N127 = (n.N127 == "on") ? "checked" : null,
                                        N128 = (n.N128 == "on") ? "checked" : null,
                                        N129 = (n.N129 == "on") ? "checked" : null,
                                        N130 = (n.N130 == "on") ? "checked" : null,
                                        N131 = (n.N131 == "on") ? "checked" : null,
                                        N132 = (n.N132 == "on") ? "checked" : null,
                                        N133 = (n.N133 == "on") ? "checked" : null,
                                        N134 = (n.N134 == "on") ? "checked" : null,
                                        N135 = (n.N135 == "on") ? "checked" : null,
                                        N136 = (n.N136 == "on") ? "checked" : null,
                                        N137 = (n.N137 == "on") ? "checked" : null,
                                        N138 = (n.N138 == "on") ? "checked" : null,
                                        N139 = (n.N139 == "on") ? "checked" : null,
                                        N140 = (n.N140 == "on") ? "checked" : null,
                                        N141 = (n.N141 == "on") ? "checked" : null,
                                        N142 = (n.N142 == "on") ? "checked" : null,
                                        N143 = (n.N143 == "on") ? "checked" : null,
                                        N144 = (n.N144 == "on") ? "checked" : null,
                                        N145 = (n.N145 == "on") ? "checked" : null,
                                        N146 = (n.N146 == "on") ? "checked" : null,
                                        N147 = (n.N147 == "on") ? "checked" : null,
                                        N148 = (n.N148 == "on") ? "checked" : null,
                                        N149 = (n.N149 == "on") ? "checked" : null,
                                        N150 = (n.N150 == "on") ? "checked" : null
                                    };
                return View(notifications.ToList());
            }
        }

        [HttpGet]
        public Notification ConvertViewModelToModel(GetEditNotificationsViewModel vm)
        {
            string c1 = Request.Form["c1"];
            Debug.WriteLine("c1=" + c1);
            return new Notification()
            {
                N1 = c1
            };
        }

        // POST: /Manage/MyNotifications
        [HttpPost]
        public ActionResult MyNotifications(GetEditNotificationsViewModel model)
        {
            var user = User.Identity.GetUserName().ToString();
            var userid = entities.AspNetUsers.Where(m => m.Email == user).Select(m => m.Id).First();
            var notificationId = entities.Notifications.Where(m => m.UserId == userid).Select(m => m.NotificationId).First();

            if (userid != null)
            {
                Notification notifications = entities.Notifications.Find(notificationId);
                string c1 = Request.Form["c1"];
                string c2 = Request.Form["c2"];
                string c3 = Request.Form["c3"];
                string c4 = Request.Form["c4"];
                string c5 = Request.Form["c5"];
                string c6 = Request.Form["c6"];
                string c7 = Request.Form["c7"];
                string c8 = Request.Form["c8"];
                string c9 = Request.Form["c9"];
                string c10 = Request.Form["c10"];
                string c11 = Request.Form["c11"];
                string c12 = Request.Form["c12"];
                string c13 = Request.Form["c13"];
                string c14 = Request.Form["c14"];
                string c15 = Request.Form["c15"];
                string c16 = Request.Form["c16"];
                string c17 = Request.Form["c17"];
                string c18 = Request.Form["c18"];
                string c19 = Request.Form["c19"];
                string c20 = Request.Form["c20"];
                string c21 = Request.Form["c21"];
                string c22 = Request.Form["c22"];
                string c23 = Request.Form["c23"];
                string c24 = Request.Form["c24"];
                string c25 = Request.Form["c25"];
                string c26 = Request.Form["c26"];
                string c27 = Request.Form["c27"];
                string c28 = Request.Form["c28"];
                string c29 = Request.Form["c29"];
                string c30 = Request.Form["c30"];
                string c31 = Request.Form["c31"];
                string c32 = Request.Form["c32"];
                string c33 = Request.Form["c33"];
                string c34 = Request.Form["c34"];
                string c35 = Request.Form["c35"];
                string c36 = Request.Form["c36"];
                string c37 = Request.Form["c37"];
                string c38 = Request.Form["c38"];
                string c39 = Request.Form["c39"];
                string c40 = Request.Form["c40"];
                string c41 = Request.Form["c41"];
                string c42 = Request.Form["c42"];
                string c43 = Request.Form["c43"];
                string c44 = Request.Form["c44"];
                string c45 = Request.Form["c45"];
                string c46 = Request.Form["c46"];
                string c47 = Request.Form["c47"];
                string c48 = Request.Form["c48"];
                string c49 = Request.Form["c49"];
                string c50 = Request.Form["c50"];
                string c51 = Request.Form["c51"];
                string c52 = Request.Form["c52"];
                string c53 = Request.Form["c53"];
                string c54 = Request.Form["c54"];
                string c55 = Request.Form["c55"];
                string c56 = Request.Form["c56"];
                string c57 = Request.Form["c57"];
                string c58 = Request.Form["c58"];
                string c59 = Request.Form["c59"];
                string c60 = Request.Form["c60"];
                string c61 = Request.Form["c61"];
                string c62 = Request.Form["c62"];
                string c63 = Request.Form["c63"];
                string c64 = Request.Form["c64"];
                string c65 = Request.Form["c65"];
                string c66 = Request.Form["c66"];
                string c67 = Request.Form["c67"];
                string c68 = Request.Form["c68"];
                string c69 = Request.Form["c69"];
                string c70 = Request.Form["c70"];
                string c71 = Request.Form["c71"];
                string c72 = Request.Form["c72"];
                string c73 = Request.Form["c73"];
                string c74 = Request.Form["c74"];
                string c75 = Request.Form["c75"];
                string c76 = Request.Form["c76"];
                string c77 = Request.Form["c77"];
                string c78 = Request.Form["c78"];
                string c79 = Request.Form["c79"];
                string c80 = Request.Form["c80"];
                string c81 = Request.Form["c81"];
                string c82 = Request.Form["c82"];
                string c83 = Request.Form["c83"];
                string c84 = Request.Form["c84"];
                string c85 = Request.Form["c85"];
                string c86 = Request.Form["c86"];
                string c87 = Request.Form["c87"];
                string c88 = Request.Form["c88"];
                string c89 = Request.Form["c89"];
                string c90 = Request.Form["c90"];
                string c91 = Request.Form["c91"];
                string c92 = Request.Form["c92"];
                string c93 = Request.Form["c93"];
                string c94 = Request.Form["c94"];
                string c95 = Request.Form["c95"];
                string c96 = Request.Form["c96"];
                string c97 = Request.Form["c97"];
                string c98 = Request.Form["c98"];
                string c99 = Request.Form["c99"];
                string c100 = Request.Form["c100"];
                string c101 = Request.Form["c101"];
                string c102 = Request.Form["c102"];
                string c103 = Request.Form["c103"];
                string c104 = Request.Form["c104"];
                string c105 = Request.Form["c105"];
                string c106 = Request.Form["c106"];
                string c107 = Request.Form["c107"];
                string c108 = Request.Form["c108"];
                string c109 = Request.Form["c109"];
                string c110 = Request.Form["c110"];
                string c111 = Request.Form["c111"];
                string c112 = Request.Form["c112"];
                string c113 = Request.Form["c113"];
                string c114 = Request.Form["c114"];
                string c115 = Request.Form["c115"];
                string c116 = Request.Form["c116"];
                string c117 = Request.Form["c117"];
                string c118 = Request.Form["c118"];
                string c119 = Request.Form["c119"];
                string c120 = Request.Form["c120"];
                string c121 = Request.Form["c121"];
                string c122 = Request.Form["c122"];
                string c123 = Request.Form["c123"];
                string c124 = Request.Form["c124"];
                string c125 = Request.Form["c125"];
                string c126 = Request.Form["c126"];
                string c127 = Request.Form["c127"];
                string c128 = Request.Form["c128"];
                string c129 = Request.Form["c129"];
                string c130 = Request.Form["c130"];
                string c131 = Request.Form["c131"];
                string c132 = Request.Form["c132"];
                string c133 = Request.Form["c133"];
                string c134 = Request.Form["c134"];
                string c135 = Request.Form["c135"];
                string c136 = Request.Form["c136"];
                string c137 = Request.Form["c137"];
                string c138 = Request.Form["c138"];
                string c139 = Request.Form["c139"];
                string c140 = Request.Form["c140"];
                string c141 = Request.Form["c141"];
                string c142 = Request.Form["c142"];
                string c143 = Request.Form["c143"];
                string c144 = Request.Form["c144"];
                string c145 = Request.Form["c145"];
                string c146 = Request.Form["c146"];
                string c147 = Request.Form["c147"];
                string c148 = Request.Form["c148"];
                string c149 = Request.Form["c149"];
                string c150 = Request.Form["c150"];


                notifications.N1 = c1;
                notifications.N2 = c2;
                notifications.N3 = c3;
                notifications.N4 = c4;
                notifications.N5 = c5;
                notifications.N6 = c6;
                notifications.N7 = c7;
                notifications.N8 = c8;
                notifications.N9 = c9;
                notifications.N10 = c10;
                notifications.N11 = c11;
                notifications.N12 = c12;
                notifications.N13 = c13;
                notifications.N14 = c14;
                notifications.N15 = c15;
                notifications.N16 = c16;
                notifications.N17 = c17;
                notifications.N18 = c18;
                notifications.N19 = c19;
                notifications.N20 = c20;
                notifications.N21 = c21;
                notifications.N22 = c22;
                notifications.N23 = c23;
                notifications.N24 = c24;
                notifications.N25 = c25;
                notifications.N26 = c26;
                notifications.N27 = c27;
                notifications.N28 = c28;
                notifications.N29 = c29;
                notifications.N30 = c30;
                notifications.N31 = c31;
                notifications.N32 = c32;
                notifications.N33 = c33;
                notifications.N34 = c34;
                notifications.N35 = c35;
                notifications.N36 = c36;
                notifications.N37 = c37;
                notifications.N38 = c38;
                notifications.N39 = c39;
                notifications.N40 = c40;
                notifications.N41 = c41;
                notifications.N42 = c42;
                notifications.N43 = c43;
                notifications.N44 = c44;
                notifications.N45 = c45;
                notifications.N46 = c46;
                notifications.N47 = c47;
                notifications.N48 = c48;
                notifications.N49 = c49;
                notifications.N50 = c50;
                notifications.N51 = c51;
                notifications.N52 = c52;
                notifications.N53 = c53;
                notifications.N54 = c54;
                notifications.N55 = c55;
                notifications.N56 = c56;
                notifications.N57 = c57;
                notifications.N58 = c58;
                notifications.N59 = c59;
                notifications.N60 = c60;
                notifications.N61 = c61;
                notifications.N62 = c62;
                notifications.N63 = c63;
                notifications.N64 = c64;
                notifications.N65 = c65;
                notifications.N66 = c66;
                notifications.N67 = c67;
                notifications.N68 = c68;
                notifications.N69 = c69;
                notifications.N70 = c70;
                notifications.N71 = c71;
                notifications.N72 = c72;
                notifications.N73 = c73;
                notifications.N74 = c74;
                notifications.N75 = c75;
                notifications.N76 = c76;
                notifications.N77 = c77;
                notifications.N78 = c78;
                notifications.N79 = c79;
                notifications.N80 = c80;
                notifications.N81 = c81;
                notifications.N82 = c82;
                notifications.N83 = c83;
                notifications.N84 = c84;
                notifications.N85 = c85;
                notifications.N86 = c86;
                notifications.N87 = c87;
                notifications.N88 = c88;
                notifications.N89 = c89;
                notifications.N90 = c90;
                notifications.N91 = c91;
                notifications.N92 = c92;
                notifications.N93 = c93;
                notifications.N94 = c94;
                notifications.N95 = c95;
                notifications.N96 = c96;
                notifications.N97 = c97;
                notifications.N98 = c98;
                notifications.N99 = c99;
                notifications.N100 = c100;
                notifications.N101 = c101;
                notifications.N102 = c102;
                notifications.N103 = c103;
                notifications.N104 = c104;
                notifications.N105 = c105;
                notifications.N106 = c106;
                notifications.N107 = c107;
                notifications.N108 = c108;
                notifications.N109 = c109;
                notifications.N110 = c110;
                notifications.N111 = c111;
                notifications.N112 = c112;
                notifications.N113 = c113;
                notifications.N114 = c114;
                notifications.N115 = c115;
                notifications.N116 = c116;
                notifications.N117 = c117;
                notifications.N118 = c118;
                notifications.N119 = c119;
                notifications.N120 = c120;
                notifications.N121 = c121;
                notifications.N122 = c122;
                notifications.N123 = c123;
                notifications.N124 = c124;
                notifications.N125 = c125;
                notifications.N126 = c126;
                notifications.N127 = c127;
                notifications.N128 = c128;
                notifications.N129 = c129;
                notifications.N130 = c130;
                notifications.N131 = c131;
                notifications.N132 = c132;
                notifications.N133 = c133;
                notifications.N134 = c134;
                notifications.N135 = c135;
                notifications.N136 = c136;
                notifications.N137 = c137;
                notifications.N138 = c138;
                notifications.N139 = c139;
                notifications.N140 = c140;
                notifications.N141 = c141;
                notifications.N142 = c142;
                notifications.N143 = c143;
                notifications.N144 = c144;
                notifications.N145 = c145;
                notifications.N146 = c146;
                notifications.N147 = c147;
                notifications.N148 = c148;
                notifications.N149 = c149;
                notifications.N150 = c150;
                entities.SaveChanges();
            }
            return RedirectToAction("MyNotifications", "Manage");
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //////////////////////////////////////////////////////////////
        //FIRM USERS//////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        // GET: /Manage/FirmUsers
        [HttpGet]
        public ActionResult FirmUsers()
        {
            using (ethko_dbEntities entities = new ethko_dbEntities())
            {
                var firmUsers = from fu in entities.AspNetUsers
                                join ut in entities.UserTypes on fu.UserTypeId equals ut.UserTypeId
                                select new GetFirmUsersViewModel() { Name = fu.FName + " " + fu.LName, UserType = ut.UserTypeName };
                return View(firmUsers.ToList());
            }
        }

        // GET: /Manage/FirmUsersArchive
        [HttpGet]
        public ActionResult FirmUsersArchive()
        {
            return View();
        }

        // GET: /Manage/NewFirmUser
        [HttpGet]
        public ActionResult NewFirmUser()
        {
            return View();
        }

        public AspNetUser ConvertViewModelToModel(AddFirmUserViewModel vm)
        {
            return new AspNetUser()
            {
                FName = vm.FName
            };
        }

        // POST: /Manage/NewFirmUser
        [HttpPost]
        public ActionResult NewFirmUser(AddFirmUserViewModel model)
        {
            var user = User.Identity.GetUserName().ToString();
            var firmUserModel = ConvertViewModelToModel(model);

            using (ethko_dbEntities entities = new ethko_dbEntities())
            {
                entities.AspNetUsers.Add(firmUserModel);
                entities.SaveChanges();
            }
            return RedirectToAction("FirmUsers");
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //////////////////////////////////////////////////////////////
        //FIRM SETTINGS///////////////////////////////////////////////
        //////////////////////////////////////////////////////////////
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        //#########
        //Offices//
        //#########

        // GET: /Manage/FirmSettings
        [HttpGet]
        public ActionResult FirmSettings()
        {
            using (ethko_dbEntities entities = new ethko_dbEntities())
            {
                var offices = from o in entities.Offices
                              join u in entities.AspNetUsers on o.FstUser equals u.Id into gj
                              from x in gj.DefaultIfEmpty()
                              join d in entities.DimDates on o.InsDate equals d.DateKey
                              join u in entities.AspNetUsers on o.LstUser equals u.Id into lst
                              from y in lst.DefaultIfEmpty()
                              select new GetFirmSettingsViewModel() { OfficeId = o.OfficeId.ToString(), OfficeName = o.OfficeName, InsDate = d.FullDateUSA.ToString(), FullName = x.FName + " " + x.LName, LstUser=y.FName+" "+y.LName };
                return View(offices.ToList());
            }
        }

        // GET: /Manage/NewOffice
        [HttpGet]
        public ActionResult NewOfficeModal()
        {
            return PartialView("_AddOfficeModal");
        }
        [HttpGet]
        public Office ConvertViewModelToModel(AddOfficeViewModel vm)
        {
            return new Office()
            {
                OfficeName = vm.OfficeName
            };
        }

        // POST: /Manage/NewOffice
        [HttpPost]
        public ActionResult NewOffice(AddOfficeViewModel model)
        {
            var user = User.Identity.GetUserName().ToString();
            var officeModel = ConvertViewModelToModel(model);
            DateTime date = DateTime.Now;
            int intDate = int.Parse(date.ToString("yyyyMMdd"));

            using (ethko_dbEntities entities = new ethko_dbEntities())
            {
                entities.Offices.Add(officeModel);
                officeModel.InsDate = intDate;
                officeModel.LstDate = intDate;
                officeModel.FstUser = entities.AspNetUsers.Where(m => m.Email == user).Select(m => m.Id).First();
                officeModel.LstUser = entities.AspNetUsers.Where(m => m.Email == user).Select(m => m.Id).First();
                entities.SaveChanges();
            }
            return RedirectToAction("FirmSettings");
        }

        // GET: /Manage/DeleteOffice
        [HttpGet]
        public ActionResult DeleteOfficeModal(int OfficeId)
        {
            ethko_dbEntities entities = new ethko_dbEntities();
            Office offices = entities.Offices.Where(m => m.OfficeId == OfficeId).Single();
            return PartialView("_DeleteOfficeModal", offices);
        }

        // GET: /Manage/EditOffice
        [HttpGet]
        public ActionResult EditOffice(int? OfficeId)
        {
            if (OfficeId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ethko_dbEntities entities = new ethko_dbEntities();
            //Office offices = entities.Offices.GroupJoin(entities.DimDates.DefaultIfEmpty(), oid => oid.OfficeId ).Where(m => m.OfficeId == OfficeId).Single();
            var offices = from o in entities.Offices
                          join u in entities.AspNetUsers on o.FstUser equals u.Id into gj
                          from x in gj.DefaultIfEmpty()
                          join d in entities.DimDates on o.InsDate equals d.DateKey
                          join u in entities.AspNetUsers on o.LstUser equals u.Id into lst
                          from y in lst.DefaultIfEmpty()
                          select new EditOfficeModel() { OfficeName = o.OfficeName, InsDate = d.FullDateUSA.ToString(), FullName = x.FName + " " + x.LName, LstUser = y.FName + " " + y.LName };
            return View(offices);
        }

        //############
        //User Types//
        //############

        // GET: /Manage/NewUserTypeModal
        [HttpGet]
        public ActionResult NewUserTypeModal()
        {
            return PartialView("_AddUserTypeModal");
        }

        public UserType ConvertViewModelToModel(AddUserTypeViewModel vm)
        {
            return new UserType()
            {
                UserTypeName = vm.UserTypeName
            };
        }

        // POST: /Manage/NewUserType
        [HttpPost]
        public ActionResult NewUserType(AddUserTypeViewModel model)
        {
            var user = User.Identity.GetUserName().ToString();
            var userTypeModel = ConvertViewModelToModel(model);
            DateTime date = DateTime.Now;
            int intDate = int.Parse(date.ToString("yyyyMMdd"));

            using (ethko_dbEntities entities = new ethko_dbEntities())
            {
                entities.UserTypes.Add(userTypeModel);
                userTypeModel.InsDate = intDate;
                entities.SaveChanges();
            }
            return RedirectToAction("UserTypes");
        }

        // GET: /Manage/UserTypes
        [HttpGet]
        public ActionResult UserTypes()
        {
            using (ethko_dbEntities entities = new ethko_dbEntities())
            {
                var userTypes = from ut in entities.UserTypes
                                join d in entities.DimDates on ut.InsDate equals d.DateKey
                                select new GetUserTypesViewModel() { UserTypeId = ut.UserTypeId.ToString(), UserTypeName = ut.UserTypeName, InsDate = d.FullDateUSA.ToString() };
                return View(userTypes.ToList());
            }
        }

        // GET: /Manage/DeleteUserTypeModal
        [HttpGet]
        public ActionResult DeleteUserTypeModal(int UserTypeId)
        {
            ethko_dbEntities entities = new ethko_dbEntities();
            UserType userTypes = entities.UserTypes.Where(m => m.UserTypeId == UserTypeId).Single();
            return PartialView("_DeleteUserTypeModal", userTypes);
        }

        // GET: /Manage/EditUserType
        [HttpGet]
        public ActionResult EditUserType(int? UserTypeId)
        {
            if (UserTypeId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserType userTypes = entities.UserTypes.Where(m => m.UserTypeId == UserTypeId).Single();
            return View(userTypes);
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //////////////////////////////////////////////////////////////
        //CLIENT BILLING//////////////////////////////////////////////
        //////////////////////////////////////////////////////////////
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        // GET: /Manage/ClientBilling
        [HttpGet]
        public ActionResult ClientBilling()
        {
            using (ethko_dbEntities entities = new ethko_dbEntities())
            {
                var billingMethods = from bm in entities.BillingMethods
                                     join u in entities.AspNetUsers on bm.FstUser equals u.Id into gj
                                     join d in entities.DimDates on bm.InsDate equals d.DateKey
                                     from x in gj.DefaultIfEmpty()
                                     select new GetClientBillingViewModel() { BillingMethodId = bm.BillingMethodId.ToString(), BillingMethodName = bm.BillingMethodName, InsDate = d.FullDateUSA.ToString(), FullName = x.FName + " " + x.LName };
                return View(billingMethods.ToList());
            }
        }

        // GET: /Manage/NewBillingMethodModal
        [HttpGet]
        public ActionResult NewBillingMethodModal()
        {
            return PartialView("_AddBillingMethodModal");
        }
        
        [HttpGet]
        public BillingMethod ConvertViewModelToModel(AddBillingMethodViewModel vm)
        {
            return new BillingMethod()
            {
                BillingMethodName = vm.BillingMethodName
            };
        }

        // POST: /Manage/NewBillingMethod
        [HttpPost]
        public ActionResult NewBillingMethod(AddBillingMethodViewModel model)
        {
            var user = User.Identity.GetUserName().ToString();
            var billingMthodModel = ConvertViewModelToModel(model);
            DateTime date = DateTime.Now;
            int intDate = int.Parse(date.ToString("yyyyMMdd"));

            using (ethko_dbEntities entities = new ethko_dbEntities())
            {
                entities.BillingMethods.Add(billingMthodModel);
                billingMthodModel.InsDate = intDate;
                billingMthodModel.FstUser = entities.AspNetUsers.Where(m => m.Email == user).Select(m => m.Id).First();
                billingMthodModel.LstDate = intDate;
                billingMthodModel.LstUser = entities.AspNetUsers.Where(m => m.Email == user).Select(m => m.Id).First();
                entities.SaveChanges();
            }
            return RedirectToAction("ClientBilling");
        }

        // GET: /Manage/DeleteBillingMethodModal
        [HttpGet]
        public ActionResult DeleteBillingMethodModal(int BillingMethodId)
        {
            ethko_dbEntities entities = new ethko_dbEntities();
            BillingMethod billingMethods = entities.BillingMethods.Where(m => m.BillingMethodId == BillingMethodId).Single();
            return PartialView("_DeleteBillingMethodModal", billingMethods);
        }

        // GET: /Manage/EditBillingMethodModal
        [HttpGet]
        public ActionResult EditBillingMethodModal(int BillingMethodId)
        {
            BillingMethod billingMethods = entities.BillingMethods.Where(m => m.BillingMethodId == BillingMethodId).Single();
            return PartialView("_EditBillingMethodModal", billingMethods);
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //////////////////////////////////////////////////////////////
        //IMPORT/EXPORT///////////////////////////////////////////////
        //////////////////////////////////////////////////////////////
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        // GET: /Manage/ImportExport
        [HttpGet]
        public ActionResult ImportExport()
        {
            return View();
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //////////////////////////////////////////////////////////////
        //CUSTOM FIELDS///////////////////////////////////////////////
        //////////////////////////////////////////////////////////////
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        // GET: /Manage/CustomFields
        [HttpGet]
        public ActionResult CustomFields()
        {
            return View();
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //////////////////////////////////////////////////////////////
        //INTAKE FORMS////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        // GET: /Manage/IntakeForms
        [HttpGet]
        public ActionResult IntakeForms()
        {
            return View();
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //////////////////////////////////////////////////////////////
        //WORKFLOWS///////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        // GET: /Manage/Workflows
        [HttpGet]
        public ActionResult Workflows()
        {
            return View();
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //////////////////////////////////////////////////////////////
        //CASE STAGES/////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        // GET: /Manage/CaseStages
        [HttpGet]
        public ActionResult CaseStages()
        {
            using (ethko_dbEntities entities = new ethko_dbEntities())
            {
                var caseStages = from c in entities.CaseStages
                                 join u in entities.AspNetUsers on c.FstUser equals u.Id into lj
                                 from x in lj.DefaultIfEmpty()
                                 join d in entities.DimDates on c.InsDate equals d.DateKey
                                 select new GetCaseStagesViewModel() { CaseStageId = c.CaseStageId.ToString(), CaseStageName = c.CaseStageName, InsDate = d.FullDateUSA.ToString(), FullName = x.FName + " " + x.LName };
                return View(caseStages.ToList());
            }
        }

        // GET: /Manage/NewCaseStage
        [HttpGet]
        public ActionResult NewCaseStageModal()
        {
            return PartialView("_AddCaseStageModal");
        }

        // GET: /Manage/NewCaseStage
        public CaseStage ConvertViewModelToModel(AddCaseStageViewModel vm)
        {
            return new CaseStage()
            {
                CaseStageName = vm.CaseStageName
            };
        }

        // POST: /Manage/NewCaseStage
        [HttpPost]
        public ActionResult NewCaseStage(AddCaseStageViewModel model)
        {
            var user = User.Identity.GetUserName().ToString();
            var caseStageModel = ConvertViewModelToModel(model);
            DateTime date = DateTime.Now;
            int intDate = int.Parse(date.ToString("yyyyMMdd"));

            using (ethko_dbEntities entities = new ethko_dbEntities())
            {
                entities.CaseStages.Add(caseStageModel);
                caseStageModel.InsDate = intDate;
                caseStageModel.FstUser = entities.AspNetUsers.Where(m => m.Email == user).Select(m => m.Id).First();
                caseStageModel.LstDate = intDate;
                caseStageModel.LstUser = entities.AspNetUsers.Where(m => m.Email == user).Select(m => m.Id).First();
                entities.SaveChanges();
            }
            return RedirectToAction("CaseStages");
        }

        // GET: /Manage/DeleteCaseStageModal
        [HttpGet]
        public ActionResult DeleteCaseStageModal(int CaseStageId)
        {
            ethko_dbEntities entities = new ethko_dbEntities();
            CaseStage caseStages = entities.CaseStages.Where(m => m.CaseStageId == CaseStageId).Single();
            return PartialView("_DeleteCaseStageModal", caseStages);
        }

        // GET: /Manage/EditCaseStage
        [HttpGet]
        public ActionResult EditCaseStage(int? CaseStageId)
        {
            if (CaseStageId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CaseStage caseStages = entities.CaseStages.Where(m => m.CaseStageId == CaseStageId).Single();
            return View(caseStages);
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //////////////////////////////////////////////////////////////
        //LEADS///////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        // GET: /Manage/Leads
        [HttpGet]
        public ActionResult Leads()
        {
            using (ethko_dbEntities entities = new ethko_dbEntities())
            {
                var leadReferralSources = from lrs in entities.LeadReferralSources
                                          join u in entities.AspNetUsers on lrs.FstUser equals u.Id into lj
                                          from x in lj.DefaultIfEmpty()
                                          join d in entities.DimDates on lrs.InsDate equals d.DateKey
                                          select new GetLeadReferralSourcesViewModel() { ReferralSourceId = lrs.ReferralSourceId.ToString(), ReferralSourceName = lrs.ReferralSourceName, InsDate = d.FullDateUSA.ToString(), FullName = x.FName + " " + x.LName };
                return View(leadReferralSources.ToList());
            }
        }

        // GET: /Manage/LeadStatus
        [HttpGet]
        public ActionResult LeadStatus()
        {
            using (ethko_dbEntities entities = new ethko_dbEntities())
            {
                var leadStatuses = from ls in entities.LeadStatuses
                                   join u in entities.AspNetUsers on ls.FstUser equals u.Id into lj
                                   from x in lj.DefaultIfEmpty()
                                   join d in entities.DimDates on ls.InsDate equals d.DateKey
                                   select new GetLeadStatusesViewModel() { LeadStatusId = ls.LeadStatusId.ToString(), LeadStatusName = ls.LeadStatusName, InsDate = d.FullDateUSA.ToString(), FullName = x.FName + " " + x.LName };
                return View(leadStatuses.ToList());
            }
        }

        // GET: /Manage/NewLeadStatus
        [HttpGet]
        public ActionResult NewLeadStatusModal()
        {
            return PartialView("_AddLeadStatusModal");
        }

        // GET: /Manage/NewLeadStatus
        public LeadStatus ConvertViewModelToModel(AddLeadStatusViewModel vm)
        {
            return new LeadStatus()
            {
                LeadStatusName = vm.LeadStatusName
            };
        }

        // POST: /Manage/NewLeadStatus
        [HttpPost]
        public ActionResult NewLeadStatus(AddLeadStatusViewModel model)
        {
            var user = User.Identity.GetUserName().ToString();
            var leadStatusModel = ConvertViewModelToModel(model);
            DateTime date = DateTime.Now;
            int intDate = int.Parse(date.ToString("yyyyMMdd"));

            using (ethko_dbEntities entities = new ethko_dbEntities())
            {
                entities.LeadStatuses.Add(leadStatusModel);
                leadStatusModel.InsDate = intDate;
                leadStatusModel.FstUser = entities.AspNetUsers.Where(m => m.Email == user).Select(m => m.Id).First();
                leadStatusModel.LstDate = intDate;
                leadStatusModel.LstUser = entities.AspNetUsers.Where(m => m.Email == user).Select(m => m.Id).First();
                entities.SaveChanges();
            }
            return RedirectToAction("LeadStatus");
        }

        // GET: /Manage/DeleteLeadStatus
        [HttpGet]
        public ActionResult DeleteLeadStatusModal(int LeadStatusId)
        {
            ethko_dbEntities entities = new ethko_dbEntities();
            LeadStatus leadStatuses = entities.LeadStatuses.Where(m => m.LeadStatusId == LeadStatusId).Single();
            return PartialView("_DeleteLeadStatusModal", leadStatuses);
        }

        // GET: /Manage/EditLeadStatus
        [HttpGet]
        public ActionResult EditLeadStatus(int? LeadStatusId)
        {
            if (LeadStatusId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeadStatus leadStatuses = entities.LeadStatuses.Where(m => m.LeadStatusId == LeadStatusId).Single();
            return View(leadStatuses);
        }

        // GET: /Manage/NewReferralSource
        [HttpGet]
        public ActionResult NewLeadReferralSourceModal()
        {
            return PartialView("_AddLeadReferralSourceModal");
        }

        // GET: /Manage/NewReferralSource
        public LeadReferralSource ConvertViewModelToModel(AddLeadReferralSourceViewModel vm)
        {
            return new LeadReferralSource()
            {
                ReferralSourceName = vm.ReferralSourceName
            };
        }

        // POST: /Manage/NewReferralSource
        [HttpPost]
        public ActionResult NewLeadReferralSource(AddLeadReferralSourceViewModel model)
        {
            var user = User.Identity.GetUserName().ToString();
            var referralSourceModel = ConvertViewModelToModel(model);
            DateTime date = DateTime.Now;
            int intDate = int.Parse(date.ToString("yyyyMMdd"));

            using (ethko_dbEntities entities = new ethko_dbEntities())
            {
                entities.LeadReferralSources.Add(referralSourceModel);
                referralSourceModel.InsDate = intDate;
                referralSourceModel.FstUser = entities.AspNetUsers.Where(m => m.Email == user).Select(m => m.Id).First();
                referralSourceModel.LstDate = intDate;
                referralSourceModel.LstUser = entities.AspNetUsers.Where(m => m.Email == user).Select(m => m.Id).First();
                entities.SaveChanges();
            }
            return RedirectToAction("Leads");
        }

        // GET: /Manage/DeleteReferralSourceModal
        [HttpGet]
        public ActionResult DeleteLeadReferralSourceModal(int ReferralSourceId)
        {
            ethko_dbEntities entities = new ethko_dbEntities();
            LeadReferralSource referralSources = entities.LeadReferralSources.Where(m => m.ReferralSourceId == ReferralSourceId).Single();
            return PartialView("_DeleteLeadReferralSourceModal", referralSources);
        }

        // GET: /Manage/EditReferralSource
        [HttpGet]
        public ActionResult EditLeadReferralSource(int? ReferralSourceId)
        {
            if (ReferralSourceId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeadReferralSource referralSources = entities.LeadReferralSources.Where(m => m.ReferralSourceId == ReferralSourceId).Single();
            return View(referralSources);
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //////////////////////////////////////////////////////////////
        //GLOBAL//////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        // POST: /Manage/EditSave
        [HttpPost]
        public ActionResult EditSave(int? OfficeId, int? UserTypeId, int? BillingMethodId, int? CaseStageId, int? LeadStatusId, int? ReferralSourceId)
        {
            if (OfficeId != null)
            {
                Office offices = entities.Offices.Find(OfficeId);
                string newOfficeName = Request.Form["NewOffice"].ToString();
                offices.OfficeName = newOfficeName;
                entities.SaveChanges();
                return RedirectToAction("FirmSettings", "Manage");
            }

            if (UserTypeId != null)
            {
                UserType userTypes = entities.UserTypes.Find(UserTypeId);
                string newUserTypeName = Request.Form["NewUserType"].ToString();
                userTypes.UserTypeName = newUserTypeName;
                entities.SaveChanges();
                return RedirectToAction("UserTypes", "Manage");
            }

            if (BillingMethodId != null)
            {
                BillingMethod billingMethods = entities.BillingMethods.Find(BillingMethodId);
                string newBillingMethodName = Request.Form["NewBillingMethod"].ToString();
                billingMethods.BillingMethodName = newBillingMethodName;
                entities.SaveChanges();
                return RedirectToAction("ClientBilling", "Manage");
            }

            if (CaseStageId != null)
            {
                CaseStage caseStages = entities.CaseStages.Find(CaseStageId);
                string newCastStageName = Request.Form["NewCaseStage"].ToString();
                caseStages.CaseStageName = newCastStageName;
                entities.SaveChanges();
                return RedirectToAction("CaseStages", "Manage");
            }

            if (LeadStatusId != null)
            {
                LeadStatus leadStatuses = entities.LeadStatuses.Find(LeadStatusId);
                string newLeadStatusName = Request.Form["NewLeadStatus"].ToString();
                leadStatuses.LeadStatusName = newLeadStatusName;
                entities.SaveChanges();
                return RedirectToAction("LeadStatus", "Manage");
            }

            if (ReferralSourceId != null)
            {
                LeadReferralSource leadReferralSources = entities.LeadReferralSources.Find(ReferralSourceId);
                string newReferralSourceName = Request.Form["NewLeadReferralSource"].ToString();
                leadReferralSources.ReferralSourceName = newReferralSourceName;
                entities.SaveChanges();
                return RedirectToAction("Leads", "Manage");
            }

            return RedirectToAction("Index", "Manage");
        }

        // POST: /Manage/DeleteConfirmed
        [HttpPost]
        public ActionResult DeleteConfirmed(int? CaseStageId, int? OfficeId, int? BillingMethodId, int? UserTypeId, int? LeadStatusId, int? ReferralSourceId)
        {
            if (CaseStageId == null && OfficeId == null && BillingMethodId == null && UserTypeId == null && LeadStatusId ==null && ReferralSourceId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (CaseStageId != null)
            {
                CaseStage caseStages = entities.CaseStages.Find(CaseStageId);
                entities.CaseStages.Remove(caseStages);
                entities.SaveChanges();
                DeleteConfirmedViewModel delete = new DeleteConfirmedViewModel
                {
                    CaseStageId = CaseStageId
                };
                return RedirectToAction("CaseStages", "Manage");
            }

            if (OfficeId != null)
            {
                Office offices = entities.Offices.Find(OfficeId);
                entities.Offices.Remove(offices);
                entities.SaveChanges();
                DeleteConfirmedViewModel delete = new DeleteConfirmedViewModel
                {
                    OfficeId = OfficeId
                };
                return RedirectToAction("FirmSettings", "Manage");
            }

            if (BillingMethodId != null)
            {
                BillingMethod billingMethods = entities.BillingMethods.Find(BillingMethodId);
                entities.BillingMethods.Remove(billingMethods);
                entities.SaveChanges();
                DeleteConfirmedViewModel delete = new DeleteConfirmedViewModel
                {
                    BillingMethodId = BillingMethodId
                };
                return RedirectToAction("ClientBilling", "Manage");
            }

            if (UserTypeId != null)
            {
                UserType userTypes = entities.UserTypes.Find(UserTypeId);
                entities.UserTypes.Remove(userTypes);
                entities.SaveChanges();
                DeleteConfirmedViewModel delete = new DeleteConfirmedViewModel
                {
                    UserTypeId = UserTypeId
                };
                return RedirectToAction("UserTypes", "Manage");
            }

            if (LeadStatusId != null)
            {
                LeadStatus leadStatuses = entities.LeadStatuses.Find(LeadStatusId);
                entities.LeadStatuses.Remove(leadStatuses);
                entities.SaveChanges();
                DeleteConfirmedViewModel delete = new DeleteConfirmedViewModel
                {
                    LeadStatusId = LeadStatusId
                };
                return RedirectToAction("LeadStatus", "Manage");
            }

            if (ReferralSourceId != null)
            {
                LeadReferralSource leadReferralSources = entities.LeadReferralSources.Find(ReferralSourceId);
                entities.LeadReferralSources.Remove(leadReferralSources);
                entities.SaveChanges();
                DeleteConfirmedViewModel delete = new DeleteConfirmedViewModel
                {
                    LeadReferralSourceId = ReferralSourceId
                };
                return RedirectToAction("Leads", "Manage");
            }
            return RedirectToAction("Index", "Manage");
        }


        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        public Office ConvertViewModelToModel(EditConfirmedViewModel vm)
        {
            return new Office()
            {
                OfficeName = vm.OfficeName
            };
        }



        #endregion
    }
}