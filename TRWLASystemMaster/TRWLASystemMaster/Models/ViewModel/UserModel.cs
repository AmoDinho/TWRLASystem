using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TRWLASystemMaster.Models.ViewModel
{

    /// <summary>
    /// to do:
    /// Add full error message of attribute
    /// </summary>
    /// 

        //User Sign up view
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

    //UserSignUpViewVol
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

    //UserLoginView
    
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

    /// <summary>
    /// 
    ///              USER PROFILE VIEW:
    ///              THIS IS TO ALLOW FUNCTIONALITY TO ENABLE ADMIN TO MAANGE IE:UPDATE THE THEIR INFO
    ///              
    /// 
    /// 
    /// </summary>

    //User Profile View Models

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



    //Look Up the User Role

    public class LOOKUPAvailableRole
    {
        [Key]
        public int LOOKUPRoleID { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
    }


    //Look up user type
    public class LookUpUserType
    {
        [Key]
        public int UserTypeID { get; set; }

        public string Description { get; set; }
        public string AccessRight { get; set; }
    }

    //Look up SecurityAnswer
    public class LookupSecurityAnswer
    {
        [Key]
        public int SecurityAnswerID { get; set; }

        public string Security_Question { get; set; }

        public string Security_Answer { get; set; }
    }


    //LookUpRes

    public class LookUpRes
    {
        [Key]
        public int ResID { get; set; }

        public string Res_Name { get; set; }


    }


    /// <summary>
    /// Collections
    /// </summary>

    //UserRoles
    public class UserRoles
    {
        public int? SelectedRoleID { get; set; }
        public IEnumerable<LOOKUPAvailableRole> UserRoleList { get; set; }
    }

    //User_UserTypeID
    public class User_UserTypeID
    {
        public int? SelectedUserType { get; set; }
        public IEnumerable<LookUpUserType> UserTypeList { get; set; }
    }
    //UserSecurityAnswerID
    public class UserSecurityAnswerID
    {
        public int? SelectedSecurityAnswer { get; set; }
        public IEnumerable<LookupSecurityAnswer> SecurityAnswerList { get; set; }


    }
    //UserResID
    public class UserResID
    {
        public int? SelectedRes { get; set; }
        public IEnumerable<LookUpRes> ResList { get; set; }
    }

    //UserDataView
    public class UserDataView
    {
        public IEnumerable<UserProfileView> UserProfile { get; set; }
        public UserRoles UserRoles { get; set; }
        public User_UserTypeID User_UserTypeID { get; set; }

        public UserSecurityAnswerID UserSecurityAnswerID { get; set; }
        public UserResID UserResID { get; set; }


    }










}

   

