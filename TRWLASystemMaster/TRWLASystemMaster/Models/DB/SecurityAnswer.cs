//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TRWLASystemMaster.Models.DB
{
    using Microsoft.SqlServer.Dac.Model;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class SecurityAnswer
    {
        public int SecurityAnswerID { get; set; }

        [Required(ErrorMessage = "An Answer is Required")]
        public string Security_Answer { get; set; }

        public int SYSUserProfileID { get; set; }
        public int QuestionID { get; set; }
    
        public virtual SYSUserProfile SYSUserProfile { get; set; }
        public virtual SecurityQuestion SecurityQuestion { get; set; }
    }
}
