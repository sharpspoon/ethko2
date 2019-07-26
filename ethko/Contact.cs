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
    
    public partial class Contact
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Contact()
        {
            this.Cases = new HashSet<Case>();
        }
    
        public int ContactId { get; set; }
        public string UserId { get; set; }
        public System.DateTime InsDate { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string MName { get; set; }
        public string Title { get; set; }
        public short Archived { get; set; }
        public string Email { get; set; }
        public Nullable<int> ContactGroupId { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public short EnableClientPortal { get; set; }
        public string CellPhone { get; set; }
        public string WorkPhone { get; set; }
        public string HomePhone { get; set; }
        public string Fax { get; set; }
        public string SSN { get; set; }
        public string JobTitle { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string License { get; set; }
        public string Website { get; set; }
        public string Notes { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public byte[] RowVersion { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Case> Cases { get; set; }
    }
}
