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

        //View List
        [HttpGet]
        public ActionResult Index()
        {
            using (ethko_dbEntities entities = new ethko_dbEntities())
            {
                var cases = from c in entities.Cases
                               join u in entities.AspNetUsers on c.FstUser equals u.Id //into users
                               //from cs in entities.CaseStages.DefaultIfEmpty()
                               //join 
                               //where c.Archived == 0
                               select new GetCaseListViewModel() { CaseId = c.CaseId, CaseNumber = c.CaseNumber, CaseName = c.CaseName, CaseStageId = c.CaseStageId, UserName = u.UserName, InsDate = c.InsDate.ToString() };
                return View(cases.ToList());
            }
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

        //New

        public ActionResult New()
        {
            using (ethko_dbEntities entities = new ethko_dbEntities())
            {
                var contactResult = (from contacts in entities.Contacts select contacts).ToList();
                var companyResult = (from companies in entities.Companies select companies).ToList();
                //var totalResults = contactResult.AddRange(companyResult);
                if (contactResult != null)
                {
                    ViewBag.contactList = contactResult.Select(N => new SelectListItem { Text = N.FullName, Value = N.ContactId.ToString() });
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
            var contactList = new SelectList(entities.Contacts.ToList(), "FullName", "FullName");
            ViewData["DBContacts"] = contactList;
            return View();
        }

        public Case ConvertViewModelToModel(AddCaseViewModel vm)
        {
            return new Case()
            {
                ContactId = vm.ContactId,
                CaseName = vm.CaseName,
                CaseNumber = vm.CaseNumber,
                PracticeAreaId = vm.PracticeAreaId,
                BillingMethodId = vm.BillingMethodId,
                CaseStageId = vm.CaseStageId,
                DateOpened = vm.DateOpened,
                OfficeId = vm.OfficeId,
                Description = vm.Description
            };
        }

        [HttpPost]
        public ActionResult New(AddCaseViewModel model)
        {
            var user = User.Identity.GetUserName().ToString();
            var caseModel = ConvertViewModelToModel(model);

            using (ethko_dbEntities entities = new ethko_dbEntities())
            {
                entities.Cases.Add(caseModel);
                caseModel.InsDate = DateTime.Now;
                caseModel.DateOpened = DateTime.Now;
                string contactName = Request.Form["Contacts"].ToString();
                string practiceArea = Request.Form["PracticeAreas"].ToString();
                string caseStage = Request.Form["CaseStages"].ToString();
                string office = Request.Form["Offices"].ToString();
                string billingMethod = Request.Form["BillingMethods"].ToString();
                caseModel.ContactId = entities.Contacts.Where(m => m.FullName == contactName).Select(m => m.ContactId).FirstOrDefault();
                caseModel.PracticeAreaId = entities.PracticeAreas.Where(m => m.PracticeAreaName == practiceArea).Select(m => m.PracticeAreaId).FirstOrDefault();
                caseModel.CaseStageId = entities.CaseStages.Where(m => m.CaseStageName == caseStage).Select(m => m.CaseStageId).FirstOrDefault();
                caseModel.OfficeId = entities.Offices.Where(m => m.OfficeName == office).Select(m => m.OfficeId).FirstOrDefault();
                caseModel.BillingMethodId = entities.BillingMethods.Where(m => m.BillingMethodName == billingMethod).Select(m => m.BillingMethodId).FirstOrDefault();
                caseModel.FstUser = entities.AspNetUsers.Where(m => m.Email == user).Select(m => m.Id).First();
                entities.SaveChanges();
            }
            return RedirectToAction("Index");
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

        //public ActionResult DeleteConfirmed(int? PracticeAreaId)
        //{
        //    if (PracticeAreaId == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    PracticeArea practiceAreas = entities.PracticeAreas.Find(PracticeAreaId);
        //    entities.PracticeAreas.Remove(practiceAreas);
        //    entities.SaveChanges();
        //    DeleteConfirmedCaseViewModel delete = new DeleteConfirmedCaseViewModel
        //    {
        //        PracticeAreaId = PracticeAreaId
        //    };
        //    return RedirectToAction("PracticeAreas", "Cases");
        //}

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

        // GET: /Cases/EditPracticeArea
        [HttpGet]
        public ActionResult EditPracticeArea(int? PracticeAreaId)
        {
            if (PracticeAreaId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PracticeArea practiceAreas = entities.PracticeAreas.Where(m => m.PracticeAreaId == PracticeAreaId).Single();
            return View(practiceAreas);
        }

        [HttpPost]
        public JsonResult AutoComplete(string prefix)
        {
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

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //////////////////////////////////////////////////////////////
        //GLOBAL//////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        // POST: /Cases/EditSave
        [HttpPost]
        public ActionResult EditSave(int? PracticeAreaId)
        {
            if (PracticeAreaId != null)
            {
                PracticeArea practiceAreas = entities.PracticeAreas.Find(PracticeAreaId);
                string newPracticeAreaName = Request.Form["NewPracticeArea"].ToString();
                practiceAreas.PracticeAreaName = newPracticeAreaName;
                entities.SaveChanges();
                return RedirectToAction("PracticeAreas", "Cases");
            }

            return RedirectToAction("Index", "Cases");
        }

        // POST: /Cases/DeleteConfirmed
        [HttpPost]
        public ActionResult DeleteConfirmed(int? PracticeAreaId)
        {
            if (PracticeAreaId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (PracticeAreaId != null)
            {
                PracticeArea practiceAreas = entities.PracticeAreas.Find(PracticeAreaId);
                entities.PracticeAreas.Remove(practiceAreas);
                entities.SaveChanges();
                DeleteConfirmedCaseViewModel delete = new DeleteConfirmedCaseViewModel
                {
                    PracticeAreaId = PracticeAreaId
                };
                return RedirectToAction("PracticeAreas", "Cases");
            }
            return RedirectToAction("Index", "Contacts");
        }

        //View Specific Case
        [HttpGet]
        public ActionResult ViewCase(int? CaseId)
        {
            using (ethko_dbEntities entities = new ethko_dbEntities())
            {
                var contacts = (from c in entities.Cases
                                //join cg in entities.ContactGroups on c.ContactGroupId equals cg.ContactGroupId
                                //join u in entities.AspNetUsers on c.UserId equals u.Id
                                join pa in entities.PracticeAreas on c.PracticeAreaId equals pa.PracticeAreaId
                                join cs in entities.CaseStages on c.CaseStageId equals cs.CaseStageId
                                where c.CaseId == CaseId
                                select new GetIndividualCaseViewModel()
                                {
                                    CaseId = c.CaseId,
                                    CaseName = c.CaseName,
                                    CaseNumber = c.CaseNumber,
                                    PracticeAreaName = pa.PracticeAreaName,
                                    CaseStageName = cs.CaseStageName
                                }).FirstOrDefault();


                //IEnumerable<Contact> contacts = entities.Contacts.Where(m => m.Archived == 0).ToList();
                //var contactModel = ConvertViewModelToModel(contacts);
                return View(contacts);
            }
        }
    }
}
