//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ethko
{
    using System;
    using System.Collections.Generic;
    
    public partial class ToDo
    {
        public int ToDoId { get; set; }
        public string ToDoName { get; set; }
        public int ToDoPriorityId { get; set; }
        public Nullable<int> CaseId { get; set; }
        public Nullable<int> LeadId { get; set; }
        public int AssignedTo { get; set; }
        public int DueDate { get; set; }
        public short Archived { get; set; }
        public string FstUser { get; set; }
        public int InsDate { get; set; }
        public int LstDate { get; set; }
        public string LstUser { get; set; }
        public byte[] RowVersion { get; set; }
    
        public virtual Case Case { get; set; }
        public virtual Priority Priority { get; set; }
    }
}
