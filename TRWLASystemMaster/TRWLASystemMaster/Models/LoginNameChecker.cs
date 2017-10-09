using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TRWLASystemMaster.Models.DB;
using TRWLASystemMaster.Models.ViewModel;
using System.ComponentModel.DataAnnotations;

namespace TRWLASystemMaster.Models
{
    public class LoginNameChecker : ValidationAttribute
    
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {

                string username = value.ToString();
                TWRLADB_Staging_V2Entities8 db = new TWRLADB_Staging_V2Entities8();

                SYSUser myuser = db.SYSUsers.FirstOrDefault(p => p.LoginName == username);
                if (myuser != null)
                {
                    return new ValidationResult(ErrorMessage ?? "Username already taken");
                }

                else
                {
                    return ValidationResult.Success;
                }
                
            }
            else
            {
                return new ValidationResult(ErrorMessage ?? "Please provide a username");
            }
        }
    }
}