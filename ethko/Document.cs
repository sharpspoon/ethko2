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
    
    public partial class Document
    {
        public int DocumentId { get; set; }
        public int DocumentTypeId { get; set; }
        public int CaseId { get; set; }
        public string DocumentName { get; set; }
        public string FstUser { get; set; }
        public int InsDate { get; set; }
        public int LstDate { get; set; }
        public string LstUser { get; set; }
        public byte[] RowVersion { get; set; }
        public short Archived { get; set; }
    
        public virtual Case Case { get; set; }
        public virtual DocumentType DocumentType { get; set; }
    }
}
