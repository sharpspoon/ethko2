using ethko.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ethko.Controllers
{
    [Authorize]
    public class ToDosController : Controller
    {
        ethko_dbEntities entities = new ethko_dbEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NewToDoModal()
        {
            var priorityNames = new SelectList(entities.Priorities.ToList(), "PriorityName", "PriorityName");
            ViewData["DBPriorityNames"] = priorityNames;
            return PartialView("_AddToDoModal");
        }

        public ToDo ConvertViewModelToModel(AddToDoViewModel vm)
        {
            return new ToDo()
            {
                ToDoName = vm.ToDoName
            };
        }

        [HttpPost]
        public ActionResult NewToDoModal(AddToDoViewModel model)
        {
            var todoModel = ConvertViewModelToModel(model);
            var user = User.Identity.GetUserName().ToString();
            DateTime date = DateTime.Now;
            int intDate = int.Parse(date.ToString("yyyyMMdd"));

            using (ethko_dbEntities entities = new ethko_dbEntities())
            {
                entities.ToDos.Add(todoModel);
                todoModel.InsDate = intDate;
                todoModel.LstDate = intDate;
                string priorityName = Request.Form["PriorityNames"].ToString();
                todoModel.ToDoPriorityId = entities.Priorities.Where(m => m.PriorityName == priorityName).Select(m => m.PriorityId).FirstOrDefault();
                todoModel.FstUser = entities.AspNetUsers.Where(m => m.Email == user).Select(m => m.Id).First();
                todoModel.LstUser = entities.AspNetUsers.Where(m => m.Email == user).Select(m => m.Id).First();
                entities.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}