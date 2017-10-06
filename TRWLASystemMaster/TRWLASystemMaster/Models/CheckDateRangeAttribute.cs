using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace TRWLASystemMaster.Models
{
    public class CheckDateRangeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if ((DateTime)value != null)
            {

                DateTime dt = (DateTime)value;
                if (dt >= DateTime.UtcNow.AddDays(-1))
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(ErrorMessage ?? "You cannot make an event in the past");
            }
            else
            {
                return new ValidationResult(ErrorMessage ?? "Please select a date");
            }
        }

    }
}