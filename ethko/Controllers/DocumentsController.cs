using ethko.Models;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ethko.Controllers
{
    [Authorize]
    public class DocumentsController : Controller
    {
        static string subscriptionKey = "07c8c872b1844e49ac5db5258dc53dc3";
        static string endpoint = "https://ethko.cognitiveservices.azure.com/";

        private const string EXTRACT_TEXT_URL_IMAGE = "https://moderatorsampleimages.blob.core.windows.net/samples/sample2.jpg";


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

        public static async Task ExtractTextUrl(ComputerVisionClient client, string urlImage)
        {
            Console.WriteLine("start of the ocr extracttexturl");
            BatchReadFileHeaders textHeaders = await client.BatchReadFileAsync(urlImage);
            string operationLocation = textHeaders.OperationLocation;
            // Retrieve the URI where the recognized text will be stored from the Operation-Location header. 
            // We only need the ID and not the full URL
            const int numberOfCharsInOperationId = 36;
            string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);

            // Extract the text 
            // Delay is between iterations and tries a maximum of 10 times.
            int i = 0;
            int maxRetries = 10;
            ReadOperationResult results;
            Console.WriteLine($"Extracting text from URL image {Path.GetFileName(urlImage)}...");
            Console.WriteLine();
            do
            {
                results = await client.GetReadOperationResultAsync(operationId);
                Console.WriteLine("Server status: {0}, waiting {1} seconds...", results.Status, i);
                await Task.Delay(1000);
            }
            while ((results.Status == TextOperationStatusCodes.Running ||
                    results.Status == TextOperationStatusCodes.NotStarted) && i++ < maxRetries);
            // Display the found text.
            Console.WriteLine();
            var recognitionResults = results.RecognitionResults;
            foreach (TextRecognitionResult result in recognitionResults)
            {
                foreach (Line line in result.Lines)
                {
                    Console.WriteLine(line.Text);
                }
            }
            Console.WriteLine();
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
            Console.WriteLine("start of the ocr post");
            ComputerVisionClient client = Authenticate(endpoint, subscriptionKey);
            ExtractTextUrl(client, EXTRACT_TEXT_URL_IMAGE).Wait();
            return RedirectToAction("Index", "Documents");
        }
    }
}