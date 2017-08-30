using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TRWLASystemMaster.Models.ViewModel
{
    public class UserSignUpView
    {
        [Key]
        public int SYSUserID { get; set; }
        public int LOOKUPRoleID { get; set; }
        public string RoleName { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Login ID")]
        public string LoginName { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public int UserTypeID { get; set; }


        public int SecurityAnswerID { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = " StudentNumber")]
        public string StudentNumber { get; set; }

       
        [Required(ErrorMessage = "*")]
        [Display(Name = "Degree")]
        public string Degree { get; set; }
        public DateTime YearOfStudy { get; set; }
        public int ResID { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        public DateTime DoB { get; set; }

        [Display(Name = "Phone Number")]
        public string Phonenumber { get; set; }
    }


    public class UserSignUpViewVol
    {
        [Key]
        public int SYSUserID { get; set; }
        public int LOOKUPRoleID { get; set; }
        public string RoleName { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Login ID")]
        public string LoginName { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public int UserTypeID { get; set; }


        public int SecurityAnswerID { get; set; }

        public int ResID { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        public DateTime DoB { get; set; }

        [Display(Name = "Phone Number")]
        public string Phonenumber { get; set; }
    }

    public class UserLoginView
    {
        [Key]
        public int SYSUserID { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Login ID")]
        public string LoginName { get; set; }
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }


    public class UserProfileView
    {
       
            [Key]
            public int SYSUserID { get; set; }
            public int LOOKUPRoleID { get; set; }
            public string RoleName { get; set; }
            public bool? IsRoleActive { get; set; }
            [Required(ErrorMessage = "*")]
            [Display(Name = "Login ID")]
            public string LoginName { get; set; }
            [Required(ErrorMessage = "*")]
            [Display(Name = "Password")]
            public string Password { get; set; }
            [Required(ErrorMessage = "*")]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }
            [Required(ErrorMessage = "*")]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }
            public string Gender { get; set; }
        }

        public class LOOKUPAvailableRole
        {
            [Key]
            public int LOOKUPRoleID { get; set; }
            public string RoleName { get; set; }
            public string RoleDescription { get; set; }
        }

        public class UserTypeID
        {
            public string Text { get; set; }
            public string Value { get; set; }
        }


    public class SecurityAnswerID
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }

    public class ResID
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }

    public class UserRoles
        {
            public int? SelectedRoleID { get; set; }
            public IEnumerable<LOOKUPAvailableRole> UserRoleList { get; set; }
    }

    public class User_UserTypeID
    {
        public string SelectedUserType { get; set; }
        public IEnumerable<UserTypeID> UserType { get; set; }
}

    public class UserSecurityAnswerID
    {
        public string SelectedSecurityAnswer { get; set; }
        public IEnumerable<SecurityAnswerID> SecurityAnswer { get; set; }
    }

    public class UserResID
    {
        public string SelectedRes { get; set; }
        public IEnumerable<ResID> ResID { get; set; }
    }


    public class UserDataView
{
    public IEnumerable<UserProfileView> UserProfile { get; set; }
public UserRoles UserRoles { get; set; }
public User_UserTypeID User_UserTypeID { get; set; }

        public UserSecurityAnswerID UserSecurityAnswerID { get; set; }
        public UserResID UserResID { get; set; }


    }


    }

   

