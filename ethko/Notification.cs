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
    
    public partial class Notification
    {
        public int NotificationId { get; set; }
        public string UserId { get; set; }
        public int N1 { get; set; }
        public string LstUser { get; set; }
        public System.DateTime LstDate { get; set; }
        public string FstUser { get; set; }
        public System.DateTime InsDate { get; set; }
        public byte[] RowVersion { get; set; }
    }
}