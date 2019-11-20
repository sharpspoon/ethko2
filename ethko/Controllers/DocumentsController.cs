using ethko.Models;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ethko.Controllers
{
    [Authorize]
    public class DocumentsController : Controller
    {
        ethko_dbEntities1 entities = new ethko_dbEntities1();
        //vision
        static string subscriptionKey = "07c8c872b1844e49ac5db5258dc53dc3";
        const string endpoint = @"https://ethko.cognitiveservices.azure.com/";
        const string uriBase = endpoint + "vision/v2.1/ocr";
        private const string imageFilePath = @"https://moderatorsampleimages.blob.core.windows.net/samples/sample2.jpg";


        [HttpPost]
        public ActionResult OCR()
        {
            MakeOCRRequest(imageFilePath).Wait();

            return RedirectToAction("Index", "Documents");
        }

        static async Task MakeOCRRequest(string imageFilePath)
        {
            try
            {
                HttpClient client = new HttpClient();

                // Request headers.
                client.DefaultRequestHeaders.Add(
                    "Ocp-Apim-Subscription-Key", subscriptionKey);

                // Request parameters. 
                // The language parameter doesn't specify a language, so the 
                // method detects it automatically.
                // The detectOrientation parameter is set to true, so the method detects and
                // and corrects text orientation before detecting text.
                string requestParameters = "language=unk&detectOrientation=true";

                // Assemble the URI for the REST API method.
                string uri = uriBase + "?" + requestParameters;

                HttpResponseMessage response;

                // Read the contents of the specified local image
                // into a byte array.
                byte[] byteData = GetImageAsByteArray(imageFilePath);

                // Add the byte array as an octet stream to the request body.
                using (ByteArrayContent content = new ByteArrayContent(byteData))
                {
                    // This example uses the "application/octet-stream" content type.
                    // The other content types you can use are "application/json"
                    // and "multipart/form-data".
                    content.Headers.ContentType =
                        new MediaTypeHeaderValue("application/octet-stream");

                    // Asynchronously call the REST API method.
                    response = await client.PostAsync(uri, content);
                }

                // Asynchronously get the JSON response.
                string contentString = await response.Content.ReadAsStringAsync();

                // Display the JSON response.
                Console.WriteLine("\nResponse:\n\n{0}\n",
                    JToken.Parse(contentString).ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.Message);
            }
        }

        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            // Open a read-only file stream for the specified file.
            using (FileStream fileStream =
                new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                // Read the file's contents into a byte array.
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }

        //vision



        [HttpGet]
        public ActionResult Index()
        {
            var documentList = from d in entities.Documents
                               join c in entities.Cases on d.CaseId equals c.CaseId
                               join u in entities.AspNetUsers on d.FstUser equals u.Id
                               join da in entities.DimDates on d.InsDate equals da.DateKey
                               where d.DocumentTypeId == 1
                               select new GetDocumentsViewModel() { DocumentId = d.DocumentId, DocumentName = d.DocumentName, InsDate = da.FullDateUSA.ToString(), UserId = u.UserName, FullName = u.FName + " " + u.LName, CaseName = c.CaseName };
            return View(documentList.ToList());
        }

        [HttpGet]
        public ActionResult Firm()
        {
            var documentList = from d in entities.Documents
                               join c in entities.Cases on d.CaseId equals c.CaseId
                               join u in entities.AspNetUsers on d.FstUser equals u.Id
                               join da in entities.DimDates on d.InsDate equals da.DateKey
                               where d.DocumentTypeId == 1
                               select new GetDocumentsViewModel() { DocumentId = d.DocumentId, DocumentName = d.DocumentName, InsDate = da.FullDateUSA.ToString(), UserId = u.UserName, FullName = u.FName + " " + u.LName, CaseName = c.CaseName };
            return View(documentList.ToList());
        }

        [HttpGet]
        public ActionResult Unread()
        {
            var documentList = from d in entities.Documents
                               join c in entities.Cases on d.CaseId equals c.CaseId
                               join u in entities.AspNetUsers on d.FstUser equals u.Id
                               join da in entities.DimDates on d.InsDate equals da.DateKey
                               where d.DocumentTypeId == 1
                               select new GetDocumentsViewModel() { DocumentId = d.DocumentId, DocumentName = d.DocumentName, InsDate = da.FullDateUSA.ToString(), UserId = u.UserName, FullName = u.FName + " " + u.LName, CaseName = c.CaseName };
            return View(documentList.ToList());
        }

        [HttpGet]
        public ActionResult Templates()
        {
            var documentList = from d in entities.Documents
                               join c in entities.Cases on d.CaseId equals c.CaseId
                               join u in entities.AspNetUsers on d.FstUser equals u.Id
                               join da in entities.DimDates on d.InsDate equals da.DateKey
                               where d.DocumentTypeId == 1
                               select new GetDocumentsViewModel() { DocumentId = d.DocumentId, DocumentName = d.DocumentName, InsDate = da.FullDateUSA.ToString(), UserId = u.UserName, FullName = u.FName + " " + u.LName, CaseName = c.CaseName };
            return View(documentList.ToList());
        }

        //View Specific Document
        [HttpGet]
        public ActionResult ViewDocument(int DocumentId)
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
}