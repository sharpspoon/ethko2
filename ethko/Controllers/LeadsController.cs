﻿using ethko.Models;
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
            //var contactResult = (from contacts in entities.Contacts select contacts).ToList();
            //var companyResult = (from companies in entities.Companies select companies).ToList();

            var referralSources = new SelectList(entities.LeadReferralSources.ToList(), "ReferralSourceName", "ReferralSourceName");
            ViewData["DBReferralSources"] = referralSources;

            var contacts = new SelectList(entities.Contacts.ToList(), "FullName", "FullName");
            ViewData["DBContacts"] = contacts;

            var contactGroups = new SelectList(entities.ContactGroups.ToList(), "ContactGroupName", "ContactGroupName");
            ViewData["DBContactGroups"] = contactGroups;

            return PartialView("_AddLeadModal");
        }

        public Lead ConvertViewModelToModel(AddLeadViewModel vm)
        {
            return new Lead()
            {
                FName = vm.FName
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
            string contactGroupName = Request.Form["ContactGroups"].ToString();
            //leadModel.ContactGroupId = entities.ContactGroups.Where(m => m.ContactGroupName == contactGroupName).Select(m => m.ContactGroupId).FirstOrDefault();
            //leadModel.UserId = entities.AspNetUsers.Where(m => m.Email == user).Select(m => m.Id).First();
            entities.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: /Leads/Index
        [HttpGet]
        public ActionResult Index()
        {
            var contacts = from c in entities.Contacts
                           join cg in entities.ContactGroups on c.ContactGroupId equals cg.ContactGroupId
                           join d in entities.DimDates on c.InsDate equals d.DateKey
                           join u in entities.AspNetUsers on c.UserId equals u.Id into lj
                           from x in lj.DefaultIfEmpty()
                           where c.Archived == 0
                           select new GetContactListViewModel() { ContactId = c.ContactId.ToString(), FName = c.FName, LName = c.LName, Email = c.Email, FullName = x.FName + " " + x.LName, InsDate = d.FullDateUSA.ToString(), ContactGroupList = cg.ContactGroupName };
            return View(contacts.ToList());
        }
    }
}