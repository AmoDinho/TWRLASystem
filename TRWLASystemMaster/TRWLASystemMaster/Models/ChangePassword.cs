using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TRWLASystemMaster.Models
{
    public class ChangePassword
    {


        [Required(ErrorMessage = "Your Old Password is required.")]
        [Display(Name = "Old Password")]
        public string OldPassword { get; set; }


        
        [Display(Name = "New Password")]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Required(ErrorMessage = "Your new Password is required is required.")]
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        
    }
}