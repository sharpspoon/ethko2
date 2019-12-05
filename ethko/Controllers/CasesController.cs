using ethko.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ethko.Controllers
{
    //[Authorize]
    public class CasesController : Controller
    {
        ethko_dbEntities1 entities = new ethko_dbEntities1();
        readonly string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=ethko;AccountKey=Onwb/R0jWlYKaiPT6Nypnea6++vkZMVRKUp1eq97Rvpn25QKvtJrUMEPJPQyQcg/kOwpYCMaqaVF1rTot7VEJw==;EndpointSuffix=core.windows.net";

        // GET: /Cases/Index
        [HttpGet]
        public ActionResult Index()
        {
            var cases = from c in entities.Cases
                        join u in entities.AspNetUsers on c.FstUser equals u.Id //into users
                        join cs in entities.CaseStages on c.CaseStageId equals cs.CaseStageId
                        join d in entities.DimDates on c.InsDate equals d.DateKey
                        select new GetCaseListViewModel() { CaseId = c.CaseId, CaseNumber = c.CaseNumber, CaseName = c.CaseName, CaseStageId = c.CaseStageId, UserName = u.UserName, InsDate = d.FullDateUSA, CaseStageName = cs.CaseStageName, FullName = u.FName + " " + u.LName };
            return View(cases.ToList());
        }

        // GET: /Cases/PracticeAreas
        [HttpGet]
        public ActionResult PracticeAreas()
        {
            var practiceAreas = from p in entities.PracticeAreas
                                join u in entities.AspNetUsers on p.FstUser equals u.Id
                                join d in entities.DimDates on p.InsDate equals d.DateKey
                                select new GetPracticeAreasViewModel() { PracticeAreaId = p.PracticeAreaId.ToString(), 
                                    PracticeAreaName = p.PracticeAreaName, 
                                    InsDate = d.FullDateUSA.ToString(), 
                                    UserId = u.UserName, 
                                    FullName = u.FName + " " + u.LName };

            //count logic
            int i = 0;
            foreach (var item in practiceAreas.ToList())
            {
                i++;
                var totalCases = entities.Cases.Where(x => x.PracticeAreaId.Equals(i)).Count();
                ViewBag.totalCases = totalCases;
            }

            return View(practiceAreas.ToList());
        }

        // GET: /Cases/CaseInsights
        public ActionResult CaseInsights()
        {
            return View();
        }

        // GET: /Cases/Closed
        public ActionResult Closed()
        {
            return View();
        }

        // GET: /Cases/New
        public ActionResult NewCaseModal()
        {
            var contactResult = (from contacts in entities.Contacts select contacts).ToList();
            var companyResult = (from companies in entities.Companies select companies).ToList();
            var months = (from dimdates in entities.DimDates select new SelectListItem { Text = dimdates.MonthName }).Distinct().ToList();
            if (contactResult != null)
            {
                ViewBag.contactList = contactResult.Select(N => new SelectListItem { Text = N.FullName, Value = N.ContactId.ToString() });
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
            var AspNetUserList = new SelectList(entities.AspNetUsers.ToList(), "FName", "FName");
            ViewData["DBAspNetUsers"] = AspNetUserList;
            return PartialView("_AddCaseModal");
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
                Description = vm.Description,
                BillingContactId = vm.BillingContactId,
                LeadAttorneyId = vm.LeadAttorneyId
            };
        }

        // POST: /Cases/New
        [HttpPost]
        public ActionResult NewCaseModal(AddCaseViewModel model)
        {
            var user = User.Identity.GetUserName().ToString();
            var caseModel = ConvertViewModelToModel(model);
            DateTime date = DateTime.Now;
            int intDate = int.Parse(date.ToString("yyyyMMdd"));
            entities.Cases.Add(caseModel);
            caseModel.InsDate = intDate;
            caseModel.LstDate = intDate;
            string contactName = Request.Form["Contacts"].ToString();
            string practiceArea = Request.Form["PracticeAreas"].ToString();
            string caseStage = Request.Form["CaseStages"].ToString();
            string office = Request.Form["Offices"].ToString();
            string billingMethod = Request.Form["BillingMethods"].ToString();
            string billingContact = Request.Form["BillingContacts"].ToString();
            string leadAttorney = Request.Form["LeadAttorney"].ToString();
            string statuteMonth = Request.Form["StatuteMonth"].ToString();
            string statuteDay = Request.Form["StatuteDay"].ToString();
            string statuteYear = Request.Form["StatuteYear"].ToString();
            string statuteDate = statuteYear + statuteMonth + statuteDay;
            int statuteDateInt = Int32.Parse(statuteDate);
            string openedMonth = Request.Form["OpenedMonth"].ToString();
            string openedDay = Request.Form["OpenedDay"].ToString();
            string openedYear = Request.Form["OpenedYear"].ToString();
            string openedDate = openedYear + openedMonth + openedDay;
            int openedDateInt = Int32.Parse(openedDate);
            caseModel.ContactId = entities.Contacts.Where(m => m.FullName == contactName).Select(m => m.ContactId).FirstOrDefault();
            caseModel.PracticeAreaId = entities.PracticeAreas.Where(m => m.PracticeAreaName == practiceArea).Select(m => m.PracticeAreaId).FirstOrDefault();
            caseModel.CaseStageId = entities.CaseStages.Where(m => m.CaseStageName == caseStage).Select(m => m.CaseStageId).FirstOrDefault();
            caseModel.OfficeId = entities.Offices.Where(m => m.OfficeName == office).Select(m => m.OfficeId).FirstOrDefault();
            caseModel.BillingMethodId = entities.BillingMethods.Where(m => m.BillingMethodName == billingMethod).Select(m => m.BillingMethodId).FirstOrDefault();
            caseModel.FstUser = entities.AspNetUsers.Where(m => m.Email == user).Select(m => m.Id).First();
            caseModel.LstUser = entities.AspNetUsers.Where(m => m.Email == user).Select(m => m.Id).First();
            caseModel.BillingContactId = entities.Contacts.Where(m => m.FullName == billingContact).Select(m => m.ContactId).FirstOrDefault();
            caseModel.Statute = statuteDateInt;
            caseModel.DateOpened = openedDateInt;
            caseModel.LeadAttorneyId = entities.AspNetUsers.Where(m => m.Email == user).Select(m => m.Id).First();//need to fix this
            entities.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: /Cases/NewPracticeAreaModal
        public ActionResult NewPracticeAreaModal()
        {
            return PartialView("_AddPracticeAreaModal");
        }

        // GET: /Cases/DeletePracticeAreaModal
        public ActionResult DeletePracticeAreaModal(int PracticeAreaId)
        {
            PracticeArea practiceAreas = entities.PracticeAreas.Where(m => m.PracticeAreaId == PracticeAreaId).Single();
            return PartialView("_DeletePracticeAreaModal", practiceAreas);
        }

        public PracticeArea ConvertViewModelToModel(AddPracticeAreaViewModel vm)
        {
            return new PracticeArea()
            {
                PracticeAreaName = vm.PracticeAreaName
            };
        }

        // POST: /Cases/NewPracticeArea
        [HttpPost]
        public ActionResult NewPracticeArea(AddPracticeAreaViewModel model)
        {
            var user = User.Identity.GetUserName().ToString();
            var practiceAreaModel = ConvertViewModelToModel(model);
            DateTime date = DateTime.Now;
            int intDate = int.Parse(date.ToString("yyyyMMdd"));
            entities.PracticeAreas.Add(practiceAreaModel);
            practiceAreaModel.InsDate = intDate;
            practiceAreaModel.FstUser = entities.AspNetUsers.Where(m => m.Email == user).Select(m => m.Id).First();
            practiceAreaModel.LstDate = intDate;
            practiceAreaModel.LstUser = entities.AspNetUsers.Where(m => m.Email == user).Select(m => m.Id).First();
            entities.SaveChanges();
            return RedirectToAction("PracticeAreas");
        }

        // GET: /Cases/EditPracticeAreaModal
        [HttpGet]
        public ActionResult EditPracticeAreaModal(int PracticeAreaId)
        {
            PracticeArea practiceAreas = entities.PracticeAreas.Where(m => m.PracticeAreaId == PracticeAreaId).Single();
            return PartialView("_EditPracticeAreaModal", practiceAreas);
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

        // GET: /Cases/ViewCase
        [HttpGet]
        public ActionResult ViewCase(int CaseId)
        {
            var contacts = (from c in entities.Cases
                            join u in entities.AspNetUsers on c.LeadAttorneyId equals u.Id
                            join pa in entities.PracticeAreas on c.PracticeAreaId equals pa.PracticeAreaId
                            join cs in entities.CaseStages on c.CaseStageId equals cs.CaseStageId
                            join dd in entities.DimDates on c.DateOpened equals dd.DateKey into dateopened
                            from x in dateopened.DefaultIfEmpty()
                            join dd2 in entities.DimDates on c.Statute equals dd2.DateKey into statute
                            from y in statute.DefaultIfEmpty()
                            join dd3 in entities.DimDates on c.InsDate equals dd3.DateKey into create
                            from z in create.DefaultIfEmpty()
                            where c.CaseId == CaseId
                            select new GetIndividualCaseViewModel()
                            {
                                CaseId = c.CaseId,
                                CaseName = c.CaseName,
                                CaseNumber = c.CaseNumber,
                                PracticeAreaName = pa.PracticeAreaName,
                                CaseStageName = cs.CaseStageName,
                                Description = c.Description,
                                DateOpened = x.FullDateUSA,
                                Statute = y.FullDateUSA,
                                DateCreated = z.FullDateUSA,
                                LeadAttorney = u.FName + " " + u.LName
                            }).FirstOrDefault();
            return View(contacts);
        }

        public Document ConvertViewModelToModel(AddCaseDocumentsViewModel vm)
        {
            return new Document()
            {
                CaseId = vm.CaseId
            };
        }

        // GET: /Cases/UploadDocument
        [HttpPost]
        public ActionResult UploadDocument(AddCaseDocumentsViewModel model, int CaseId)
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(storageConnectionString);
            CloudBlobClient serviceClient = account.CreateCloudBlobClient();
            string cid = Request.Form["CaseId"].ToString();
            var container = serviceClient.GetContainerReference("case"+model.CaseId.ToString());
            container.CreateIfNotExistsAsync().Wait();

            // write a blob to the container
            string path = Request.Form["file"].ToString();
            string file = Path.GetFileName(path);
            CloudBlockBlob blob = container.GetBlockBlobReference(file);
            blob.UploadFromFileAsync(path);

            //write db entry on new file
            var user = User.Identity.GetUserName().ToString();
            var documentModel = ConvertViewModelToModel(model);
            DateTime date = DateTime.Now;
            int intDate = int.Parse(date.ToString("yyyyMMdd"));
            //var documentId = (from d in entities.Documents
            //                select new GetDocumentsViewModel()
            //                {
            //                    DocumentId = d.DocumentId
            //                }).FirstOrDefault();

            entities.Documents.Add(documentModel);
            documentModel.InsDate = intDate;
            documentModel.LstDate = intDate;
            documentModel.LstUser = entities.AspNetUsers.Where(m => m.Email == user).Select(m => m.Id).First();
            documentModel.FstUser = entities.AspNetUsers.Where(m => m.Email == user).Select(m => m.Id).First();
            documentModel.DocumentName = file;
            documentModel.DocumentTypeId = 1;
            documentModel.CaseId = CaseId;
            entities.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: /Cases/CaseDocuments
        [HttpGet]
        public PartialViewResult CaseDocuments(GetCaseDocumentsViewModel model)
        {
            var caseDocuments = from c in entities.Cases
                                select new GetCaseDocumentsViewModel() { CaseName = c.CaseName.ToString() };
            //return RedirectToAction("Cases", "Index");
            return PartialView(caseDocuments.ToList());
        }
    }
}