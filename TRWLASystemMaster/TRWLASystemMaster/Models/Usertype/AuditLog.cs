//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TRWLASystemMaster.Models.Usertype
{
    using System;
    using System.Collections.Generic;
    
    public partial class AuditLog
    {
        public int AuditID { get; set; }
        public Nullable<int> PersonID { get; set; }
        public System.DateTime LoginDate { get; set; }
        public System.TimeSpan LoginTime { get; set; }
        public decimal LoginDuration { get; set; }
    
        public virtual Person Person { get; set; }
    }
}