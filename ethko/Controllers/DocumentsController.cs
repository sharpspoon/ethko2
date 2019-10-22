using ethko.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ethko.Controllers
{
    [Authorize]
    public class DocumentsController : Controller
    {
        public ActionResult Index()
        {
            using (ethko_dbEntities entities = new ethko_dbEntities())
            {
                var documentList = from d in entities.Documents
                                    join u in entities.AspNetUsers on d.FstUser equals u.Id
                                    join da in entities.DimDates on d.InsDate equals da.DateKey
                                    select new GetDocumentsViewModel() { DocumentId = d.DocumentId, DocumentName = d.DocumentName, InsDate = da.FullDateUSA.ToString(), UserId = u.UserName, FullName = u.FName + " " + u.LName };
                return View(documentList.ToList());
            }
        }
    }
}