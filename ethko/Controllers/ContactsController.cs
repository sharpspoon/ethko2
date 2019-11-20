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
    public class ContactsController : Controller
    {
        ethko_dbEntities1 entities = new ethko_dbEntities1();

        //////////
        //CONTACTS
        //////////
        //New

        public ActionResult NewContactModal()
        {
            var contactGroups = new SelectList(entities.ContactGroups.ToList(), "ContactGroupName", "ContactGroupName");
            ViewData["DBContactGroups"] = contactGroups;
            return PartialView("_AddContactModal");
        }

        public Contact ConvertViewModelToModel(AddContactIndividualViewModel vm)
        {
            return new Contact()
            {
                FName = vm.FName,
                MName = vm.MName,
                LName = vm.LName,
                FullName = vm.FName+" "+vm.MName+" "+vm.LName,
                CellPhone = vm.CellPhone,
                WorkPhone = vm.WorkPhone,
                HomePhone = vm.HomePhone,
                Address = vm.Address,
                Address2 = vm.Address2,
                City = vm.City,
                State = vm.State,
                Zip = vm.Zip,
                Country = vm.Country,
                Email = vm.Email
            };
        }

        [HttpPost]
        public ActionResult NewContactModal(AddContactIndividualViewModel model)
        {
            var contactModel = ConvertViewModelToModel(model);
            var user = User.Identity.GetUserName().ToString();
            DateTime date = DateTime.Now;
            int intDate = int.Parse(date.ToString("yyyyMMdd"));
            entities.Contacts.Add(contactModel);
            contactModel.InsDate = intDate;
            string contactGroupName = Request.Form["ContactGroups"].ToString();
            contactModel.ContactGroupId = entities.ContactGroups.Where(m => m.ContactGroupName == contactGroupName).Select(m => m.ContactGroupId).FirstOrDefault();
            contactModel.UserId = entities.AspNetUsers.Where(m => m.Email == user).Select(m => m.Id).First();
            entities.SaveChanges();
            return RedirectToAction("Index");
        }

        //View List
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

        //View Archive List
        [HttpGet]
        public ActionResult ContactsArchive()
        {
            var contacts = from c in entities.Contacts
                           join cg in entities.ContactGroups on c.ContactGroupId equals cg.ContactGroupId
                           join u in entities.AspNetUsers on c.UserId equals u.Id
                           where c.Archived == 1
                           select new GetContactArchiveListViewModel() { ContactId = c.ContactId.ToString(), FName = c.FName, LName = c.LName, Email = c.Email, UserId = u.UserName, InsDate = c.InsDate.ToString(), ContactGroupList = cg.ContactGroupName };
            return View(contacts.ToList());
        }

        //View Specific Contact
        [HttpGet]
        public ActionResult ViewContact(int ContactId)
        {
                        var contacts = (from c in entities.Contacts
                            join cg in entities.ContactGroups on c.ContactGroupId equals cg.ContactGroupId
                            join u in entities.AspNetUsers on c.UserId equals u.Id
                            where c.ContactId == ContactId
                            select new GetIndividualContactViewModel() { ContactId = c.ContactId
                            , FName = c.FName
                            , MName = c.MName
                            , LName = c.LName
                            , Email = c.Email
                            , EnableClientPortal = c.EnableClientPortal
                            , UserId = u.UserName
                            , InsDate = c.InsDate.ToString()
                            , ContactGroupList = cg.ContactGroupName
                            , CellPhone = c.CellPhone
                            , WorkPhone = c.WorkPhone
                            , HomePhone = c.HomePhone
                            , Fax = c.Fax
                            , JobTitle = c.JobTitle
                            , Birthday = c.Birthday
                            , License = c.License
                            , Website = c.Website
                            , Notes = c.Notes
                            , Address = c.Address
                            , Address2 = c.Address2
                            , City = c.City
                            , State = c.State
                            , Zip = c.Zip
                            , Country = c.Country
                            , Archived = c.Archived
                            }).FirstOrDefault();
            return View(contacts);
        }

        //Edit Specific Contact
        [HttpGet]
        public ActionResult EditContactModal(int ContactId)
        {
            Contact contacts = entities.Contacts.Where(m => m.ContactId == ContactId).SingleOrDefault();
            return PartialView("_EditContactModal", contacts);
        }

        [HttpPost]
        public ActionResult EditContact([Bind(Include="FName")]Contact contact)
        {
            if (ModelState.IsValid)
            {
                entities.Entry(contact).State = EntityState.Modified;
                entities.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        //Delete Specific Contact
        [HttpGet]
        public ActionResult Delete(int ContactId)
        {
            Contact contacts = entities.Contacts.Where(m => m.ContactId == ContactId).Single();
            return View(contacts);
        }

        //Archive Specific Contact
        [HttpGet]
        public ActionResult Archive(int ContactId)
        {
            var result = entities.Contacts.SingleOrDefault(m => m.ContactId == ContactId);
            if (result != null)
            {
                if (result.Archived == 1)
                {
                    result.Archived = 0;
                    entities.SaveChanges();
                }
                else
                {
                    result.Archived = 1;
                    entities.SaveChanges();
                }

            }
            return View(result);
        }

        //////////
        //COMPANIES
        //////////
        //New
        [HttpGet]
        public ActionResult NewCompanyModal()
        {
            return PartialView("_AddCompanyModal");
        }

        public Company ConvertViewModelToModel(AddCompanyViewModel vm)
        {
            return new Company()
            {
                Name = vm.Name,
                Email = vm.Email,
                Website = vm.Website,
                MainPhone = vm.MainPhone,
                FaxNumber = vm.FaxNumber,
                Address = vm.Address,
                Address2 = vm.Address2,
                City = vm.City,
                State = vm.State,
                Zip = vm.Zip,
                Country = vm.Country
            };
        }

        [HttpPost]
        public ActionResult NewCompany(AddCompanyViewModel model)
        {
            var user = User.Identity.GetUserName().ToString();
            var companyModel = ConvertViewModelToModel(model);
            DateTime date = DateTime.Now;
            int intDate = int.Parse(date.ToString("yyyyMMdd"));
            entities.Companies.Add(companyModel);
            companyModel.InsDate = intDate;
            companyModel.FstUser = entities.AspNetUsers.Where(m => m.Email == user).Select(m => m.Id).First();
            entities.SaveChanges();
            return RedirectToAction("Companies");
        }

        //View List
        [HttpGet]
        public ActionResult Companies()
        {
            var companies = from c in entities.Companies
                            join u in entities.AspNetUsers on c.FstUser equals u.Id
                            where c.Archived == 0
                            select new GetCompanyListViewModel() { CompanyId = c.CompanyId.ToString(), Email = c.Email, FstUser = u.UserName, InsDate = c.InsDate.ToString(), Name = c.Name };
            return View(companies.ToList());
        }

        //View Archive List
        public ActionResult CompaniesArchive()
        {
            IEnumerable<Company> companies = entities.Companies.Where(m => m.Archived == 1).ToList();
            return View(companies.AsEnumerable());
        }


        //////////
        //GROUPS
        //////////
        //New
        [HttpGet]
        public ActionResult NewContactGroupModal()
        {
            return PartialView("_AddContactGroupModal");
        }

        public ContactGroup ConvertViewModelToModel(AddContactGroupViewModel vm)
        {
            return new ContactGroup()
            {
                ContactGroupName = vm.ContactGroupName
            };
        }

        [HttpPost]
        public ActionResult NewGroup(AddContactGroupViewModel model)
        {
            var user = User.Identity.GetUserName().ToString();
            var contactGroupModel = ConvertViewModelToModel(model);
            DateTime date = DateTime.Now;
            int intDate = int.Parse(date.ToString("yyyyMMdd"));
            entities.ContactGroups.Add(contactGroupModel);
            contactGroupModel.InsDate = intDate;
            contactGroupModel.FstUser = entities.AspNetUsers.Where(m => m.Email == user).Select(m => m.Id).First();
            contactGroupModel.LstDate = intDate;
            contactGroupModel.LstUser = entities.AspNetUsers.Where(m => m.Email == user).Select(m => m.Id).First();
            entities.SaveChanges();
            return RedirectToAction("ContactGroups");
        }

        // GET: /Contacts/ContactGroups
        [HttpGet]
        public ActionResult ContactGroups()
        {
            var contactGroups = from cg in entities.ContactGroups
                                join u in entities.AspNetUsers on cg.FstUser equals u.Id into lj
                                from x in lj.DefaultIfEmpty()
                                join d in entities.DimDates on cg.InsDate equals d.DateKey
                                select new GetContactGroupListViewModel() { ContactGroupId = cg.ContactGroupId.ToString(), ContactGroupName = cg.ContactGroupName, FstUser = x.FName + " " + x.LName, InsDate = d.FullDateUSA.ToString() };
            return View(contactGroups.ToList());
        }

        // GET: /Contacts/EditContactGroupModal
        [HttpGet]
        public ActionResult EditContactGroupModal(int ContactGroupId)
        {
            ContactGroup contactGroups = entities.ContactGroups.Where(m => m.ContactGroupId == ContactGroupId).Single();
            return PartialView("_EditContactGroupModal", contactGroups);
        }

        // GET: /Contacts/DeleteGroupModal
        [HttpGet]
        public ActionResult DeleteContactGroupModal(int ContactGroupId)
        {
            ContactGroup contactGroups = entities.ContactGroups.Where(m => m.ContactGroupId == ContactGroupId).Single();
            return PartialView("_DeleteContactGroupModal", contactGroups);
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //////////////////////////////////////////////////////////////
        //GLOBAL//////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        // POST: /Contacts/EditSave
        [HttpPost]
        public ActionResult EditSave(int? ContactGroupId)
        {
            if (ContactGroupId != null)
            {
                ContactGroup contactGroups = entities.ContactGroups.Find(ContactGroupId);
                string newContactGroupName = Request.Form["NewContactGroup"].ToString();
                contactGroups.ContactGroupName = newContactGroupName;
                entities.SaveChanges();
                return RedirectToAction("ContactGroups", "Contacts");
            }

            return RedirectToAction("Index", "Contacts");
        }

        // POST: /Contacts/DeleteConfirmed
        [HttpPost]
        public ActionResult DeleteConfirmed(int? ContactGroupId)
        {
            if (ContactGroupId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (ContactGroupId != null)
            {
                ContactGroup contactGroups = entities.ContactGroups.Find(ContactGroupId);
                entities.ContactGroups.Remove(contactGroups);
                entities.SaveChanges();
                DeleteConfirmedContactViewModel delete = new DeleteConfirmedContactViewModel
                {
                    ContactGroupId = ContactGroupId
                };
                return RedirectToAction("ContactGroups", "Contacts");
            }
            return RedirectToAction("Index", "Contacts");
        }
    }
}