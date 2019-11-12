using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ethko.Models
{
    //Add
    public class AddToDoViewModel
    {
        [Required]
        [Display(Name = "To-Do Name")]
        public string ToDoName { get; set; }
    }
}