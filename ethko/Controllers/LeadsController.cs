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
    //[Authorize]
    public class LeadsController : Controller
    {
        ethko_dbEntities1 entities = new ethko_dbEntities1();

        //////////
        //CONTACTS
        //////////

        // GET: /Leads/NewContactModal
        public ActionResult NewLeadModal()
        {
            var referralSources = new SelectList(entities.LeadReferralSources.ToList(), "ReferralSourceName", "ReferralSourceName");
            ViewData["DBReferralSources"] = referralSources;
            var practiceAreas = new SelectList(entities.PracticeAreas.ToList(), "PracticeAreaName", "PracticeAreaName");
            ViewData["DBPracticeAreas"] = practiceAreas;
            var contacts = new SelectList(entities.Contacts.ToList(), "FullName", "FullName");
            ViewData["DBContacts"] = contacts;
            var companies = new SelectList(entities.Companies.ToList(), "Name", "Name");
            ViewData["DBCompanies"] = companies;
            var AspNetUserList = new SelectList(entities.AspNetUsers.ToList(), "FullName", "FullName");
            ViewData["DBAspNetUsers"] = AspNetUserList;
            var leadStatus = new SelectList(entities.LeadStatuses.ToList(), "LeadStatusName", "LeadStatusName");
            ViewData["DBLeadStatuses"] = leadStatus;
            return PartialView("_AddLeadModal");
        }

        public Lead ConvertViewModelToModel(AddLeadViewModel vm)
        {
            return new Lead()
            {
                FName = vm.FName,
                MName = vm.MName,
                LName = vm.LName,
                FullName = vm.FullName,
                Email = vm.Email,
                CellPhone = vm.CellPhone,
                LeadNotes = vm.LeadNotes,
                CaseNotes = vm.CaseNotes,
                PotentialValue = vm.PotentialValue
            };
        }

        // POST: /Leads/NewContactModal
        [HttpPost]
        public ActionResult NewLeadModal(AddLeadViewModel model)
        {
            var leadModel = ConvertViewModelToModel(model);
            var user = User.Identity.GetUserName().ToString();
            DateTime date = DateTime.Now;
            int intDate = int.Parse(date.ToString("yyyyMMdd"));
            entities.Leads.Add(leadModel);
            leadModel.InsDate = intDate;
            leadModel.LstDate = intDate;
            string referralSource = Request.Form["ReferralSources"].ToString();
            string practiceArea = Request.Form["PracticeAreas"].ToString();
            string assignTo = Request.Form["LeadAssignTo"].ToString();
            string leadStatus = Request.Form["LeadStatus"].ToString();
            string fName = Request.Form["LeadStatus"].ToString();
            string mName = Request.Form["LeadStatus"].ToString();
            string lName = Request.Form["LeadStatus"].ToString();
            leadModel.FullName = leadModel.FName+" "+leadModel.MName+" "+leadModel.LName;
            leadModel.ReferralSourceId = entities.LeadReferralSources.Where(m => m.ReferralSourceName == referralSource).Select(m => m.ReferralSourceId).FirstOrDefault();
            leadModel.PracticeAreaId = entities.PracticeAreas.Where(m => m.PracticeAreaName == practiceArea).Select(m => m.PracticeAreaId).FirstOrDefault();
            leadModel.AssignTo = entities.AspNetUsers.Where(m => m.FullName == assignTo).Select(m => m.Id).FirstOrDefault();
            leadModel.LeadStatusId = entities.LeadStatuses.Where(m => m.LeadStatusName == leadStatus).Select(m => m.LeadStatusId).FirstOrDefault();
            leadModel.FstUser = entities.AspNetUsers.Where(m => m.Email == user).Select(m => m.Id).First();
            leadModel.LstUser = entities.AspNetUsers.Where(m => m.Email == user).Select(m => m.Id).First();
            entities.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: /Leads/Index
        [HttpGet]
        public ActionResult Index()
        {
            var contacts = from l in entities.Leads
                           join ls in entities.LeadStatuses on l.LeadStatusId equals ls.LeadStatusId
                           join lrs in entities.LeadReferralSources on l.ReferralSourceId equals lrs.ReferralSourceId
                           join pa in entities.PracticeAreas on l.PracticeAreaId equals pa.PracticeAreaId
                           //join u in entities.AspNetUsers on c.FstUser equals u.Id into lj
                           //from x in lj.DefaultIfEmpty()
                           where l.Archived == 0
                           select new GetLeadListViewModel() { LeadId = l.LeadId.ToString(), 
                               Email = l.Email, 
                               FullName = l.FullName,
                               LeadStatus = ls.LeadStatusName,
                               ReferralSource = lrs.ReferralSourceName,
                               PracticeArea = pa.PracticeAreaName};
            return View(contacts.ToList());
        }
    }
}