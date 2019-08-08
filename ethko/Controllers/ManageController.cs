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
    
    [Authorize]
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
                
                notifications.N1 = c1;
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
                //firmUserModel.InsDate = DateTime.Now;
                //officeModel.FstUser = entities.AspNetUsers.Where(m => m.Email == user).Select(m => m.Id).First();
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
                              select new GetFirmSettingsViewModel() { OfficeId = o.OfficeId.ToString(), OfficeName = o.OfficeName, FstUser = x.UserName, InsDate = o.InsDate.ToString() };
                return View(offices.ToList());
            }
        }

        // GET: /Manage/NewOffice
        [HttpGet]
        public ActionResult NewOffice()
        {
            return View();
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

            using (ethko_dbEntities entities = new ethko_dbEntities())
            {
                entities.Offices.Add(officeModel);
                officeModel.InsDate = DateTime.Now;
                officeModel.FstUser = entities.AspNetUsers.Where(m => m.Email == user).Select(m => m.Id).First();
                entities.SaveChanges();
            }
            return RedirectToAction("FirmSettings");
        }

        // GET: /Manage/DeleteOffice
        [HttpGet]
        public ActionResult DeleteOffice(int? OfficeId)
        {
            if (OfficeId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ethko_dbEntities entities = new ethko_dbEntities();
            Office offices = entities.Offices.Where(m => m.OfficeId == OfficeId).Single();
            return View(offices);
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
            Office offices = entities.Offices.Where(m => m.OfficeId == OfficeId).Single();
            return View(offices);
        }

        //############
        //User Types//
        //############

        // GET: /Manage/NewUserType
        [HttpGet]
        public ActionResult NewUserType()
        {
            return View();
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

            using (ethko_dbEntities entities = new ethko_dbEntities())
            {
                entities.UserTypes.Add(userTypeModel);
                userTypeModel.InsDate = DateTime.Now;
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
                                select new GetUserTypesViewModel() { UserTypeId = ut.UserTypeId.ToString(), UserTypeName = ut.UserTypeName, InsDate = ut.InsDate.ToString() };
                return View(userTypes.ToList());
            }
        }

        // GET: /Manage/DeleteUserType
        [HttpGet]
        public ActionResult DeleteUserType(int? UserTypeId)
        {
            if (UserTypeId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ethko_dbEntities entities = new ethko_dbEntities();
            UserType userTypes = entities.UserTypes.Where(m => m.UserTypeId == UserTypeId).Single();
            return View(userTypes);
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
                                     from x in gj.DefaultIfEmpty()
                                     select new GetClientBillingViewModel() { BillingMethodId = bm.BillingMethodId.ToString(), BillingMethodName = bm.BillingMethodName, InsDate = bm.InsDate.ToString(), FstUser = x.UserName };
                return View(billingMethods.ToList());
            }
        }

        // GET: /Manage/NewBillingMethod
        [HttpGet]
        public ActionResult NewBillingMethod()
        {
            return View();
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

            using (ethko_dbEntities entities = new ethko_dbEntities())
            {
                entities.BillingMethods.Add(billingMthodModel);
                billingMthodModel.InsDate = DateTime.Now;
                billingMthodModel.FstUser = entities.AspNetUsers.Where(m => m.Email == user).Select(m => m.Id).First();
                entities.SaveChanges();
            }
            return RedirectToAction("ClientBilling");
        }

        // GET: /Manage/DeleteBillingMethod
        [HttpGet]
        public ActionResult DeleteBillingMethod(int? BillingMethodId)
        {
            if (BillingMethodId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ethko_dbEntities entities = new ethko_dbEntities();
            BillingMethod billingMethods = entities.BillingMethods.Where(m => m.BillingMethodId == BillingMethodId).Single();
            return View(billingMethods);
        }

        // GET: /Manage/EditBillingMethod
        [HttpGet]
        public ActionResult EditBillingMethod(int? BillingMethodId)
        {
            if (BillingMethodId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BillingMethod billingMethods = entities.BillingMethods.Where(m => m.BillingMethodId == BillingMethodId).Single();
            return View(billingMethods);
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
                                 join u in entities.AspNetUsers on c.FstUser equals u.Id
                                 select new GetCaseStagesViewModel() { CaseStageId = c.CaseStageId.ToString(), CaseStageName = c.CaseStageName, InsDate = c.InsDate.ToString(), UserId = u.UserName };
                return View(caseStages.ToList());
            }
        }

        // GET: /Manage/NewCaseStage
        [HttpGet]
        public ActionResult NewCaseStage()
        {
            return View();
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

            using (ethko_dbEntities entities = new ethko_dbEntities())
            {
                entities.CaseStages.Add(caseStageModel);
                caseStageModel.InsDate = DateTime.Now;
                caseStageModel.FstUser = entities.AspNetUsers.Where(m => m.Email == user).Select(m => m.Id).First();
                entities.SaveChanges();
            }
            return RedirectToAction("CaseStages");
        }

        // GET: /Manage/DeleteCaseStage
        [HttpGet]
        public ActionResult DeleteCaseStage(int? CaseStageId)
        {
            if (CaseStageId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ethko_dbEntities entities = new ethko_dbEntities();
            CaseStage caseStages = entities.CaseStages.Where(m => m.CaseStageId == CaseStageId).Single();
            return View(caseStages);
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
                                          select new GetLeadReferralSourcesViewModel() { ReferralSourceId = lrs.ReferralSourceId.ToString(), ReferralSourceName = lrs.ReferralSourceName, InsDate = lrs.InsDate.ToString() };
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
                                   select new GetLeadStatusesViewModel() { LeadStatusId = ls.LeadStatusId.ToString(), LeadStatusName = ls.LeadStatusName, InsDate = ls.InsDate.ToString() };
                return View(leadStatuses.ToList());
            }
        }

        // GET: /Manage/NewLeadStatus
        [HttpGet]
        public ActionResult NewLeadStatus()
        {
            return View();
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

            using (ethko_dbEntities entities = new ethko_dbEntities())
            {
                entities.LeadStatuses.Add(leadStatusModel);
                leadStatusModel.InsDate = DateTime.Now;
                leadStatusModel.FstUser = entities.AspNetUsers.Where(m => m.Email == user).Select(m => m.Id).First();
                entities.SaveChanges();
            }
            return RedirectToAction("LeadStatus");
        }

        // GET: /Manage/DeleteLeadStatus
        [HttpGet]
        public ActionResult DeleteLeadStatus(int? LeadStatusId)
        {
            if (LeadStatusId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ethko_dbEntities entities = new ethko_dbEntities();
            LeadStatus leadStatuses = entities.LeadStatuses.Where(m => m.LeadStatusId == LeadStatusId).Single();
            return View(leadStatuses);
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
        public ActionResult NewLeadReferralSource()
        {
            return View();
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

            using (ethko_dbEntities entities = new ethko_dbEntities())
            {
                entities.LeadReferralSources.Add(referralSourceModel);
                referralSourceModel.InsDate = DateTime.Now;
                referralSourceModel.FstUser = entities.AspNetUsers.Where(m => m.Email == user).Select(m => m.Id).First();
                entities.SaveChanges();
            }
            return RedirectToAction("Leads");
        }

        // GET: /Manage/DeleteReferralSource
        [HttpGet]
        public ActionResult DeleteLeadReferralSource(int? ReferralSourceId)
        {
            if (ReferralSourceId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ethko_dbEntities entities = new ethko_dbEntities();
            LeadReferralSource referralSources = entities.LeadReferralSources.Where(m => m.ReferralSourceId == ReferralSourceId).Single();
            return View(referralSources);
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