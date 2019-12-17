using System.ComponentModel.DataAnnotations;

namespace ethko.Models
{
    //Add
    public class AddToDoViewModel
    {
        [Required]
        [Display(Name = "To-Do Name")]
        public string ToDoName { get; set; }
    }

    public class GetToDosViewModel
    {
        [Display(Name = "To-Do Name")]
        public string ToDoName { get; set; }
        public string ToDoId { get; set; }
        public string PriorityName { get; set; }
        public int PriorityId { get; set; }
        [Display(Name = "InsDate")]
        public string InsDate { get; set; }
        public string DueDate { get; set; }
    }
}