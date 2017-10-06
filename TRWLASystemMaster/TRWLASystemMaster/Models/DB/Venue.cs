//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TRWLASystemMaster.Models.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class Venue
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Venue()
        {
            this.ComEngEvents = new HashSet<ComEngEvent>();
            this.FunctionEvents = new HashSet<FunctionEvent>();
            this.GenEvents = new HashSet<GenEvent>();
            this.Lectures = new HashSet<Lecture>();
        }
    
        public int VenueID { get; set; }

        [Required(ErrorMessage = "A venue name is required")]
        [Display(Name = "venue")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Venue_Name { get; set; }

        [Required(ErrorMessage = "A street number is required")]
        [Display(Name = "street number")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        public string StreeNumber { get; set; }

        [Required(ErrorMessage = "A street name is required")]
        [Display(Name = "street name")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        public string StreetName { get; set; }

        [Required(ErrorMessage = "A suburb is required")]
        [Display(Name = "suburb")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        public string Suburb { get; set; }

        [Required(ErrorMessage = "A city is required")]
        [Display(Name = "city")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        public string City { get; set; }

        [Required(ErrorMessage = "A province is required")]
        [Display(Name = "province")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        public string Province { get; set; }

        [Required(ErrorMessage = "A postcode is required")]
        [Display(Name = "postcode")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        public string PostCode { get; set; }
        public int VenueTypeID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ComEngEvent> ComEngEvents { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FunctionEvent> FunctionEvents { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GenEvent> GenEvents { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Lecture> Lectures { get; set; }
        public virtual VenueType VenueType { get; set; }
    }
}
