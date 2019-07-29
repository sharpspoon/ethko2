using ethko.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Data.Entity;

namespace ethko.Controllers
{
    [Authorize]
    public class CasesController : Controller
    {
        ethko_dbEntities entities = new ethko_dbEntities();
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PracticeAreas()
        {
            using (ethko_dbEntities entities = new ethko_dbEntities())
            {
                var practiceAreas = from p in entities.PracticeAreas
                                    join u in entities.AspNetUsers on p.FstUser equals u.Id
                                    select new GetPracticeAreasViewModel() { PracticeAreaId = p.PracticeAreaId.ToString(), PracticeAreaName = p.PracticeAreaName, InsDate = p.InsDate.ToString(), UserId = u.UserName };
                return View(practiceAreas.ToList());
            }
        }

        public ActionResult CaseInsights()
        {
            return View();
        }

        public ActionResult Closed()
        {
            return View();
        }

        public ActionResult New()
        {
            using (ethko_dbEntities entities = new ethko_dbEntities())
            {
                var contactResult = (from contacts in entities.Contacts select contacts).ToList();
                var companyResult = (from companies in entities.Companies select companies).ToList();
                if (contactResult != null)
                {
                    ViewBag.contactList = contactResult.Select(N => new SelectListItem { Text = N.FName+" "+N.MName+" "+N.LName, Value = N.ContactId.ToString() });
                }
                if (companyResult != null)
                {
                    ViewBag.companyList = companyResult.Select(N => new SelectListItem { Text = N.Name, Value = N.CompanyId.ToString() });
                }
            }
            var practiceAreas = new SelectList(entities.PracticeAreas.ToList(), "PracticeAreaName", "PracticeAreaName");
            ViewData["DBContactGroupsPracticeArea"] = practiceAreas;
            var caseStages = new SelectList(entities.CaseStages.ToList(), "CaseStageName", "CaseStageName");
            ViewData["DBContactGroupsCaseStage"] = caseStages;
            var offices = new SelectList(entities.Offices.ToList(), "OfficeName", "OfficeName");
            ViewData["DBOffice"] = offices;
            var billingMethods = new SelectList(entities.BillingMethods.ToList(), "BillingMethodName", "BillingMethodName");
            ViewData["DBBillingMethods"] = billingMethods;
            return View();
        }

        public ActionResult NewPracticeArea()
        {
            return View();
        }

        public ActionResult DeletePracticeArea(int? PracticeAreaId)
        {
            if (PracticeAreaId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PracticeArea practiceAreas = entities.PracticeAreas.Where(m => m.PracticeAreaId == PracticeAreaId).Single();
            return View(practiceAreas);
        }

        public ActionResult DeleteConfirmed(int? PracticeAreaId)
        {
            if (PracticeAreaId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PracticeArea practiceAreas = entities.PracticeAreas.Find(PracticeAreaId);
            entities.PracticeAreas.Remove(practiceAreas);
            entities.SaveChanges();
            DeleteConfirmedCaseViewModel delete = new DeleteConfirmedCaseViewModel
            {
                PracticeAreaId = PracticeAreaId
            };
            return RedirectToAction("PracticeAreas", "Cases");
        }

        public PracticeArea ConvertViewModelToModel(AddPracticeAreaViewModel vm)
        {
            return new PracticeArea()
            {
                PracticeAreaName = vm.PracticeAreaName
            };
        }

        [HttpPost]
        public ActionResult NewPracticeArea(AddPracticeAreaViewModel model)
        {
            var user = User.Identity.GetUserName().ToString();
            var practiceAreaModel = ConvertViewModelToModel(model);

            using (ethko_dbEntities entities = new ethko_dbEntities())
            {
                entities.PracticeAreas.Add(practiceAreaModel);
                practiceAreaModel.InsDate = DateTime.Now;
                practiceAreaModel.FstUser = entities.AspNetUsers.Where(m => m.Email == user).Select(m => m.Id).First();
                entities.SaveChanges();
            }
            return RedirectToAction("PracticeAreas");
        }

        [HttpPost]
        public JsonResult AutoComplete(string prefix)
        {
            ethko_dbEntities entities = new ethko_dbEntities();
            var contacts = (from c in entities.Contacts
                             where (c.FName.Contains(prefix) || c.LName.Contains(prefix))
                            select new
                             {
                                 label = c.FName + " " + c.LName,
                                 val = c.ContactId
                             }).Concat(from co in entities.Companies
                                       where (co.Name.Contains(prefix) || co.Address.Contains(prefix))
                                       select new
                                       {
                                           label = co.Name,
                                           val = co.CompanyId
                                       }).ToList();

            return Json(contacts);
        }
    }
}
