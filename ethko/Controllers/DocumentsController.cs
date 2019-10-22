using ethko.Models;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ethko.Controllers
{
    [Authorize]
    public class DocumentsController : Controller
    {
        static readonly string subscriptionKey = Environment.GetEnvironmentVariable("07c8c872b1844e49ac5db5258dc53dc3");
        static readonly string endpoint = Environment.GetEnvironmentVariable("https://ethko.cognitiveservices.azure.com/");

        public ActionResult Index()
        {
            using (ethko_dbEntities entities = new ethko_dbEntities())
            {
                var documentList = from d in entities.Documents
                                   join c in entities.Cases on d.CaseId equals c.CaseId
                                    join u in entities.AspNetUsers on d.FstUser equals u.Id
                                    join da in entities.DimDates on d.InsDate equals da.DateKey
                                   where d.DocumentTypeId == 1
                                    select new GetDocumentsViewModel() { DocumentId = d.DocumentId, DocumentName = d.DocumentName, InsDate = da.FullDateUSA.ToString(), UserId = u.UserName, FullName = u.FName + " " + u.LName, CaseName = c.CaseName };
                return View(documentList.ToList());
            }
        }

        //View Specific Document
        [HttpGet]
        public ActionResult ViewDocument(int DocumentId)
        {
            using (ethko_dbEntities entities = new ethko_dbEntities())
            {
                var document = (from d in entities.Documents
                                join dd in entities.DimDates on d.InsDate equals dd.DateKey into dateopened
                                from x in dateopened.DefaultIfEmpty()
                                where d.DocumentId == DocumentId
                                select new GetIndividualDocumentViewModel()
                                {
                                    DocumentId = d.DocumentId,
                                    DocumentName = d.DocumentName
                                }).FirstOrDefault();

                return View(document);
            }
        }

        public static ComputerVisionClient Authenticate(string endpoint, string key)
        {
            ComputerVisionClient client =
                new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
                { Endpoint = endpoint };
            return client;
        }

        [HttpPost]
        public ActionResult OCR()
        {
            ComputerVisionClient client = Authenticate(endpoint, subscriptionKey);
            return RedirectToAction("Index", "Documents"); ;
        }
    }
}