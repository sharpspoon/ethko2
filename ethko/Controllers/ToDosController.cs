using ethko.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ethko.Controllers
{
    //[Authorize]
    public class ToDosController : Controller
    {
        ethko_dbEntities1 entities = new ethko_dbEntities1();
        [HttpGet]
        public ActionResult Index()
        {
            var todos = from t in entities.ToDos
                        join p in entities.Priorities on t.ToDoPriorityId equals p.PriorityId
                        join d in entities.DimDates on t.InsDate equals d.DateKey
                        join dd in entities.DimDates on t.DueDate equals dd.DateKey into d2
                        from y in d2.DefaultIfEmpty()
                        join u in entities.AspNetUsers on t.FstUser equals u.Id into lj
                        from x in lj.DefaultIfEmpty()
                            where t.Archived == 0
                        select new GetToDosViewModel() { ToDoId = t.ToDoId.ToString(), 
                            ToDoName = t.ToDoName, 
                            InsDate = d.FullDateUSA.ToString(), 
                            PriorityName = p.PriorityName,
                            DueDate = y.FullDateUSA};
            return View(todos.ToList());
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
            entities.ToDos.Add(todoModel);
            todoModel.InsDate = intDate;
            todoModel.LstDate = intDate;
            string priorityName = Request.Form["PriorityNames"].ToString();
            string dueDate = Request.Form["addtododatetimepicker"].ToString();
            todoModel.DueDate = entities.DimDates.Where(m => m.FullDateUSA == dueDate).Select(m => m.DateKey).FirstOrDefault();
            todoModel.ToDoPriorityId = entities.Priorities.Where(m => m.PriorityName == priorityName).Select(m => m.PriorityId).FirstOrDefault();
            todoModel.FstUser = entities.AspNetUsers.Where(m => m.Email == user).Select(m => m.Id).First();
            todoModel.LstUser = entities.AspNetUsers.Where(m => m.Email == user).Select(m => m.Id).First();
            entities.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}