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
    public class ContactsController : Controller
    {
        //private readonly Entities db = new Entities();
        ethko_dbEntities entities = new ethko_dbEntities();

        //////////
        //CONTACTS
        //////////
        //New
        public ActionResult New()
        {
            var contactGroups = new SelectList(entities.ContactGroups.ToList(), "ContactGroupName", "ContactGroupName");
            ViewData["DBContactGroups"] = contactGroups;
            return View();
        }

        public Contact ConvertViewModelToModel(AddContactIndividualViewModel vm)
        {
            return new Contact()
            {
                FName = vm.FName,
                MName = vm.MName,
                LName = vm.LName,
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
        public ActionResult New(AddContactIndividualViewModel model)
        {
            var contactModel = ConvertViewModelToModel(model);
            var user = User.Identity.GetUserName().ToString();
            using (ethko_dbEntities entities = new ethko_dbEntities())
            {
                entities.Contacts.Add(contactModel);
                contactModel.InsDate = DateTime.Now;
                string contactGroupName = Request.Form["ContactGroups"].ToString();
                contactModel.ContactGroupId = entities.ContactGroups.Where(m => m.ContactGroupName == contactGroupName).Select(m => m.ContactGroupId).FirstOrDefault();
                contactModel.UserId = entities.AspNetUsers.Where(m => m.Email == user).Select(m => m.Id).First();
                entities.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        //View List
        [HttpGet]
        public ActionResult Index()
        {
            using(ethko_dbEntities entities = new ethko_dbEntities())
            {
                var contacts = from c in entities.Contacts
                               join cg in entities.ContactGroups on c.ContactGroupId equals cg.ContactGroupId
                               join u in entities.AspNetUsers on c.UserId equals u.Id
                               where c.Archived == 0
                               select new GetContactListViewModel() { ContactId = c.ContactId.ToString(), FName = c.FName, LName = c.LName, Email = c.Email, UserId = u.UserName, InsDate = c.InsDate.ToString(), ContactGroupList = cg.ContactGroupName};
                return View(contacts.ToList());
            }
        }

        //View Archive List
        [HttpGet]
        public ActionResult ContactsArchive()
        {
            using (ethko_dbEntities entities = new ethko_dbEntities())
            {
                var contacts = from c in entities.Contacts
                               join cg in entities.ContactGroups on c.ContactGroupId equals cg.ContactGroupId
                               join u in entities.AspNetUsers on c.UserId equals u.Id
                               where c.Archived == 1
                               select new GetContactArchiveListViewModel() { ContactId = c.ContactId.ToString(), FName = c.FName, LName = c.LName, Email = c.Email, UserId = u.UserName, InsDate = c.InsDate.ToString(), ContactGroupList = cg.ContactGroupName };
                return View(contacts.ToList());
            }
        }

        //View Specific Contact
        [HttpGet]
        public ActionResult ViewContact(int? ContactId)
        {
            using (ethko_dbEntities entities = new ethko_dbEntities())
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


                //IEnumerable<Contact> contacts = entities.Contacts.Where(m => m.Archived == 0).ToList();
                //var contactModel = ConvertViewModelToModel(contacts);
                return View(contacts);
            }
        }

        //Edit Specific Contact
        [HttpGet]
        public ActionResult EditContact(int? ContactId)
        {
            ethko_dbEntities entities = new ethko_dbEntities();
            Contact contacts = entities.Contacts.Where(m => m.ContactId == ContactId).SingleOrDefault();
            return View(contacts);
        }

        [HttpPost]
        public ActionResult EditContact([Bind(Include="FName")]Contact contact)
        {
            ethko_dbEntities entities = new ethko_dbEntities();
            if (ModelState.IsValid)
            {
                entities.Entry(contact).State = EntityState.Modified;
                entities.SaveChanges();
                //return RedirectToAction("Index");
            }
            
            //Contact contacts = entities.Contacts.Where(m => m.ContactId == 1).SingleOrDefault();
            return RedirectToAction("Index");
        }

        //Delete Specific Contact
        [HttpGet]
        public ActionResult Delete(int? ContactId)
        {
            if (ContactId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ethko_dbEntities entities = new ethko_dbEntities();
            Contact contacts = entities.Contacts.Where(m => m.ContactId == ContactId).Single();
            return View(contacts);
        }

        //[HttpPost]
        public ActionResult DeleteConfirmed(int? ContactId)
        {
            if (ContactId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contacts = entities.Contacts.Find(ContactId);//works if hardcode in the contactid
            entities.Contacts.Remove(contacts);
            entities.SaveChanges();
            return View();
        }

        //Archive Specific Contact
        [HttpGet]
        public ActionResult Archive(int? ContactId)
        {
            if (ContactId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
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
        public ActionResult NewCompany()
        {
            return View();
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

            using (ethko_dbEntities entities = new ethko_dbEntities())
            {
                entities.Companies.Add(companyModel);
                //var user = User.Identity.GetUserName().ToString();
                companyModel.InsDate = DateTime.Now;
                companyModel.FstUser = entities.AspNetUsers.Where(m => m.Email == user).Select(m => m.Id).First();
                entities.SaveChanges();
            }
            return RedirectToAction("Companies");
        }

        //View List
        [HttpGet]
        public ActionResult Companies()
        {
            using (ethko_dbEntities entities = new ethko_dbEntities())
            {
                var companies = from c in entities.Companies
                               join u in entities.AspNetUsers on c.FstUser equals u.Id
                               where c.Archived == 0
                               select new GetCompanyListViewModel() { CompanyId = c.CompanyId.ToString(),  Email = c.Email, FstUser = u.UserName, InsDate = c.InsDate.ToString() };
                return View(companies.ToList());
            }
        }

        //View Archive List
        public ActionResult CompaniesArchive()
        {
            ethko_dbEntities entities = new ethko_dbEntities();
            IEnumerable<Company> companies = entities.Companies.Where(m => m.Archived == 1).ToList();
            //var contactModel = ConvertViewModelToModel(contacts);
            return View(companies.AsEnumerable());
        }


        //////////
        //GROUPS
        //////////
        //New
        public ActionResult NewGroup()
        {
            return View();
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

            using (ethko_dbEntities entities = new ethko_dbEntities())
            {
                entities.ContactGroups.Add(contactGroupModel);
                contactGroupModel.InsDate = DateTime.Now;
                contactGroupModel.FstUser = entities.AspNetUsers.Where(m => m.Email == user).Select(m => m.Id).First();
                entities.SaveChanges();
            }
            return RedirectToAction("ContactGroups");
        }

        //View List
        [HttpGet]
        public ActionResult ContactGroups()
        {
            ethko_dbEntities entities = new ethko_dbEntities();
            IEnumerable<ContactGroup> contactGroups = entities.ContactGroups.ToList();
            //var contactModel = ConvertViewModelToModel(contacts);
            return View(contactGroups.AsEnumerable());
        }
    }
}
