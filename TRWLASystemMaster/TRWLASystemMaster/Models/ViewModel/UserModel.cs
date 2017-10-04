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


        [Required(ErrorMessage = "A Username is Required")]
        [Display(Name = "Username")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string LoginName { get; set; }

        
        [Display(Name = "Password")]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Your First Name is required")]
        [Display(Name = "First Name")]
        [StringLength(35, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string FirstName { get; set; }
        [StringLength(35, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Required(ErrorMessage = "Your Last Name is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public int UserTypeID { get; set; }

   
        [Display(Name = " StudentNumber")]
        [DataType(DataType.PhoneNumber)] 
        [Required(ErrorMessage = "Student Number Required!")]
        [RegularExpression(@"^\(?([0-8]{3})\)?[-. ]?([0-8]{3})[-. ]?([0-8]{4})$", ErrorMessage = "Entered student number is not valid.")]
        public string StudentNumber { get; set; }

       
        [Required(ErrorMessage = "The degree field is required")]
        [Display(Name = "Degree")]
        [StringLength(35, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Degree { get; set; }


        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "Academic Commencement Date")]
        [DataType(DataType.Date)]
        public DateTime YearOfStudy { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "Residence")]
        public int ResID { get; set; }

        [Required(ErrorMessage = "Your Email address is required")]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "DoB")]
        [DataType(DataType.Date)]
        public DateTime DoB { get; set; }
       

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone Number Required!")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered phone format is not valid.")]
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
        [Display(Name = "Username")]
        public string LoginName { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Your First Name is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Your Last Name is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public int UserTypeID { get; set; }

        [Required(ErrorMessage = "Your Last Name is required")]
        public int SecurityAnswerID { get; set; }



        [Required(ErrorMessage = "Your Email address is required")]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }


        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "DoB")]
        [DataType(DataType.Date)]
        public DateTime DoB { get; set; }


        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone Number Required!")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered phone format is not valid.")]
        public string Phonenumber { get; set; }
    }

    //UserLoginView
    
    public class UserLoginView
    {
        [Key]
        public int SYSUserID { get; set; }
        [StringLength(54, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        [Required(ErrorMessage = "The Login ID Field is Required")]
        [Display(Name = "Login ID")]
        public string LoginName { get; set; }

        [Required(ErrorMessage = "The password Field is required")]
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


    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string ReturnToken { get; set; }
    }


    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
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

   

