//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TRWLASystemMaster.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class GuestSpeaker
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GuestSpeaker()
        {
            this.FunctionEvents = new HashSet<FunctionEvent>();
        }
    
        public int GuestSpeakerID { get; set; }
        public string GuestSpeaker_Name { get; set; }
        public string GuestSpeaker_Surname { get; set; }
        public string GuestSpeaker_Phone { get; set; }
        public string GuestSpeaker_Email { get; set; }
        public string GuestSpeaker_PictureLink { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FunctionEvent> FunctionEvents { get; set; }
    }
}
