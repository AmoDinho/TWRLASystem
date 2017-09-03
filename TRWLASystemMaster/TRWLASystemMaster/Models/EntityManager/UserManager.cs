using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TRWLASystemMaster.Models.DB;
using TRWLASystemMaster.Models.ViewModel;
using System.ComponentModel.DataAnnotations;

namespace TRWLASystemMaster.Models.EntityManager
{

    /// <summary>
    /// USEER MANAGEMENT CLASS
    /// 
    /// 
    /// </summary>
    public class UserManager
    {
        //Student sIGN uPP
        public void AddUserAccount(UserSignUpView user)
        {

            using (TWRLADB_Staging_V2Entities db = new TWRLADB_Staging_V2Entities())
            {

                SYSUser SU = new SYSUser();
                SU.LoginName = user.LoginName;
                SU.PasswordEncryptedText = user.Password;
                SU.RowCreatedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1;
                SU.RowModifiedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1; ;
                SU.RowCreatedDateTime = DateTime.Now;
                SU.RowModifiedDateTime = DateTime.Now;

                db.SYSUsers.Add(SU);
                db.SaveChanges();

                SYSUserProfile SUP = new SYSUserProfile();
                SUP.SYSUserID = SU.SYSUserID;
                SUP.FirstName = user.FirstName;
                SUP.LastName = user.LastName;
                SUP.UserTypeID = user.UserTypeID;
                SUP.Email = user.Email;
                SUP.DoB = user.DoB;
                SUP.ResID = user.ResID;
                SUP.YearOfStudy = user.YearOfStudy;
                SUP.Degree = user.Degree;
                SUP.SecurityAnswerID = user.SecurityAnswerID;
                SUP.StudentNumber = user.StudentNumber;
          
                SUP.Phonenumber = user.Phonenumber;
              

                SUP.RowCreatedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1;
                SUP.RowModifiedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1;
                SUP.RowCreatedDateTime = DateTime.Now;
                SUP.RowModifiedDateTime = DateTime.Now;

                db.SYSUserProfiles.Add(SUP);
                db.SaveChanges();


                if (user.LOOKUPRoleID > 0)
                {
                    SYSUserRole SUR = new SYSUserRole();
                    SUR.LOOKUPRoleID = user.LOOKUPRoleID;
                    SUR.SYSUserID = user.SYSUserID;
                    SUR.IsActive = true;
                    SUR.RowCreatedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1;
                    SUR.RowModifiedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1;
                    SUR.RowCreatedDateTime = DateTime.Now;
                    SUR.RowModifiedDateTime = DateTime.Now;

                    db.SYSUserRoles.Add(SUR);
                    db.SaveChanges();
                }
            }
        }

        //Volunteer Sign Up
        public void AddUserAccount(UserSignUpViewVol user)
        {

            using (TWRLADB_Staging_V2Entities db = new TWRLADB_Staging_V2Entities())
            {

                SYSUser SU = new SYSUser();
                SU.LoginName = user.LoginName;
                SU.PasswordEncryptedText = user.Password;
                SU.RowCreatedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1;
                SU.RowModifiedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1; ;
                SU.RowCreatedDateTime = DateTime.Now;
                SU.RowModifiedDateTime = DateTime.Now;

                db.SYSUsers.Add(SU);
                db.SaveChanges();

                SYSUserProfile SUP = new SYSUserProfile();
                SUP.SYSUserID = SU.SYSUserID;
                SUP.FirstName = user.FirstName;
                SUP.LastName = user.LastName;
                SUP.UserTypeID = user.UserTypeID;
                SUP.Email = user.Email;
                SUP.DoB = user.DoB;
                SUP.ResID = user.ResID;
           
                SUP.SecurityAnswerID = user.SecurityAnswerID;


                SUP.Phonenumber = user.Phonenumber;


                SUP.RowCreatedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1;
                SUP.RowModifiedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1;
                SUP.RowCreatedDateTime = DateTime.Now;
                SUP.RowModifiedDateTime = DateTime.Now;

                db.SYSUserProfiles.Add(SUP);
                db.SaveChanges();


                if (user.LOOKUPRoleID > 0)
                {
                    SYSUserRole SUR = new SYSUserRole();
                    SUR.LOOKUPRoleID = user.LOOKUPRoleID;
                    SUR.SYSUserID = user.SYSUserID;
                    SUR.IsActive = true;
                    SUR.RowCreatedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1;
                    SUR.RowModifiedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1;
                    SUR.RowCreatedDateTime = DateTime.Now;
                    SUR.RowModifiedDateTime = DateTime.Now;

                    db.SYSUserRoles.Add(SUR);
                    db.SaveChanges();
                }
            }
        }


        public void UpdateUserAccount(UserProfileView user)
        {

            using (TWRLADB_Staging_V2Entities db = new TWRLADB_Staging_V2Entities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {

                        SYSUser SU = db.SYSUsers.Find(user.SYSUserID);
                        SU.LoginName = user.LoginName;
                        SU.PasswordEncryptedText = user.Password;
                        SU.RowCreatedSYSUserID = user.SYSUserID;
                        SU.RowModifiedSYSUserID = user.SYSUserID;
                        SU.RowCreatedDateTime = DateTime.Now;
                        SU.RowModifiedDateTime = DateTime.Now;

                        db.SaveChanges();

                        var userProfile = db.SYSUserProfiles.Where(o => o.SYSUserID == user.SYSUserID);
                        if (userProfile.Any())
                        {
                            SYSUserProfile SUP = userProfile.FirstOrDefault();
                            SUP.SYSUserID = SU.SYSUserID;
                            SUP.FirstName = user.FirstName;
                            SUP.LastName = user.LastName;
                            //SUP.Gender = user.Gender;
                            SUP.UserTypeID = user.UserTypeID;
                            SUP.Email = user.Email;
                            SUP.DoB = user.DoB;
                            SUP.ResID = user.ResID;

                            SUP.YearOfStudy = user.YearOfStudy;
                            SUP.Degree = user.Degree;
                            SUP.SecurityAnswerID = user.SecurityAnswerID;
                            SUP.StudentNumber = user.StudentNumber;


                            SUP.Phonenumber = user.Phonenumber;


                            SUP.RowCreatedSYSUserID = user.SYSUserID;
                            SUP.RowModifiedSYSUserID = user.SYSUserID;
                            SUP.RowCreatedDateTime = DateTime.Now;
                            SUP.RowModifiedDateTime = DateTime.Now;

                            db.SaveChanges();
                        }

                        if (user.LOOKUPRoleID > 0)
                        {
                            var userRole = db.SYSUserRoles.Where(o => o.SYSUserID == user.SYSUserID);
                            SYSUserRole SUR = null;
                            if (userRole.Any())
                            {
                                SUR = userRole.FirstOrDefault();
                                SUR.LOOKUPRoleID = user.LOOKUPRoleID;
                                SUR.SYSUserID = user.SYSUserID;
                                SUR.IsActive = true;
                                SUR.RowCreatedSYSUserID = user.SYSUserID;
                                SUR.RowModifiedSYSUserID = user.SYSUserID;
                                SUR.RowCreatedDateTime = DateTime.Now;
                                SUR.RowModifiedDateTime = DateTime.Now;
                            }
                            else
                            {
                                SUR = new SYSUserRole();
                                SUR.LOOKUPRoleID = user.LOOKUPRoleID;
                                SUR.SYSUserID = user.SYSUserID;
                                SUR.IsActive = true;
                                SUR.RowCreatedSYSUserID = user.SYSUserID;
                                SUR.RowModifiedSYSUserID = user.SYSUserID;
                                SUR.RowCreatedDateTime = DateTime.Now;
                                SUR.RowModifiedDateTime = DateTime.Now;
                                db.SYSUserRoles.Add(SUR);
                            }

                            db.SaveChanges();
                        }
                        dbContextTransaction.Commit();
                    }
                    catch
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }
        }
        //MEHTOD:Does Login Exist?

        public bool IsLoginNameExist(string loginName)
        {
            using (TWRLADB_Staging_V2Entities db = new TWRLADB_Staging_V2Entities())
            {
                return db.SYSUsers.Where(o => o.LoginName.Equals(loginName)).Any();
            }
        }

        //Get User Password
        public string GetUserPassword(string loginName)
        {
            using (TWRLADB_Staging_V2Entities db = new TWRLADB_Staging_V2Entities())
            {
                var user = db.SYSUsers.Where(o => o.LoginName.ToLower().Equals(loginName));
                if (user.Any())
                    return user.FirstOrDefault().PasswordEncryptedText;
                else
                    return string.Empty;
            }
        }


        //What role is this user?

        public bool IsUserInRole(string loginName, string roleName)
        {
            using (TWRLADB_Staging_V2Entities db = new TWRLADB_Staging_V2Entities())
            {
                SYSUser SU = db.SYSUsers.Where(o => o.LoginName.ToLower().Equals(loginName))?.FirstOrDefault();
                if (SU != null)
                {
                    var roles = from q in db.SYSUserRoles
                                join r in db.LOOKUPRoles on q.LOOKUPRoleID equals r.LOOKUPRoleID
                                where r.RoleName.Equals(roleName) && q.SYSUserID.Equals(SU.SYSUserID)
                                select r.RoleName;

                    if (roles != null)
                    {
                        return roles.Any();
                    }
                }

                return false;
            }

        }

        //List for getting Roles
        public List < LOOKUPAvailableRole > GetAllRoles()
        {
            using (TWRLADB_Staging_V2Entities db = new TWRLADB_Staging_V2Entities())
            {
                var roles = db.LOOKUPRoles.Select(o => new LOOKUPAvailableRole
                {
                    LOOKUPRoleID = o.LOOKUPRoleID,
                    RoleName = o.RoleName,
                    RoleDescription = o.RoleDescription
                }).ToList();

                return roles;
            }
        }


        /// Get All User Types

        public List<LookUpUserType> GetAllUserTypes()
        {
            using (TWRLADB_Staging_V2Entities db = new TWRLADB_Staging_V2Entities())
            {
                var usertypes = db.UserTypes.Select(o => new LookUpUserType
                {
                    UserTypeID = o.UserTypeID,
                    AccessRight = o.AccessRight,
                    Description = o.Description

                }).ToList();

                return usertypes;
            }

        }

        //Get all Residences
        public List<LookUpRes> GetAllRes()
        {
            using (TWRLADB_Staging_V2Entities db = new TWRLADB_Staging_V2Entities())
            {
                var residences = db.Residences.Select(o => new LookUpRes
                {
                    ResID = o.ResID,
                    Res_Name = o.Res_Name

                }).ToList();

                return residences;
            }

        }

        //Get Answers

        public List<LookupSecurityAnswer> Getansers()
        {
            using (TWRLADB_Staging_V2Entities db = new TWRLADB_Staging_V2Entities())
            {
                var secanswers = db.SecurityAnswers.Select(o => new LookupSecurityAnswer
                {
                    SecurityAnswerID = o.SecurityAnswerID,
                    Security_Question = o.Security_Question,
                    Security_Answer = o.Security_Answer
                }).ToList();

                return secanswers;
            }

        }
        // get user ID 

        public int GetUserID(string loginName)
        {
            using (TWRLADB_Staging_V2Entities db = new TWRLADB_Staging_V2Entities())
            {
                var user = db.SYSUsers.Where(o => o.LoginName.Equals(loginName));
                if (user.Any()) return user.FirstOrDefault().SYSUserID;
            }
            return 0;
        }

        /// <summary>
        /// /COLLECTIONS 
        /// </summary>
        /// <returns></returns>


            //GET USER PROFILES
        public List<UserProfileView> GetAllUserProfiles()
        {
            List < UserProfileView > profiles = new List < UserProfileView > ();

            using (TWRLADB_Staging_V2Entities db = new TWRLADB_Staging_V2Entities())
            {
                UserProfileView UPV;
                var users = db.SYSUsers.ToList();

                foreach (SYSUser u in db.SYSUsers)
                {
                    UPV = new UserProfileView();
                    UPV.SYSUserID = u.SYSUserID;
                    UPV.LoginName = u.LoginName;
                    UPV.Password = u.PasswordEncryptedText;

                    var SUP = db.SYSUserProfiles.Find(u.SYSUserID);
                    if (SUP != null)
                    {
                        UPV.FirstName = SUP.FirstName;
                        UPV.LastName = SUP.LastName;
                        UPV.Degree = SUP.Degree;
                        UPV.DoB = SUP.DoB;
                        UPV.Email = SUP.Email;
                        UPV.Phonenumber = SUP.Phonenumber;
                        UPV.UserTypeID = SUP.UserTypeID;
                        UPV.SecurityAnswerID = SUP.SecurityAnswerID;
                        UPV.StudentNumber = SUP.StudentNumber;
                        UPV.Degree = SUP.Degree;
                        
                        //UPV.YearOfStudy = SUP.YearOfStudy;
                        UPV.ResID = Convert.ToInt32(SUP.ResID);
                    }

                    var SUR = db.SYSUserRoles.Where(o => o.SYSUserID.Equals(u.SYSUserID));
                    if (SUR.Any())
                    {
                        var userRole = SUR.FirstOrDefault();
                        UPV.LOOKUPRoleID = userRole.LOOKUPRoleID;
                        UPV.RoleName = userRole.LOOKUPRole.RoleName;
                        UPV.IsRoleActive = userRole.IsActive;
                    }

                    profiles.Add(UPV);
                }
            }

            return profiles;
        }

        //USER DATAVIEW 
        public UserDataView GetUserDataView(string loginName)
        {
            UserDataView UDV = new UserDataView();


            List < UserProfileView > profiles = GetAllUserProfiles();
            List <LOOKUPAvailableRole > roles = GetAllRoles();
            List<LookUpUserType> usertypes = GetAllUserTypes();
            List<LookUpRes> residences = GetAllRes();
            List<LookupSecurityAnswer> secanswers = Getansers();


            int? userAssignedRoleID = 0, userID = 0;
            int? user_usertype = 0;
            int? user_resid = 0;
            int? user_secq= 0;

            userID = GetUserID(loginName);
            using (TWRLADB_Staging_V2Entities db = new TWRLADB_Staging_V2Entities())
            {
                userAssignedRoleID = db.SYSUserRoles.Where(o => o.SYSUserID == userID)?.FirstOrDefault().LOOKUPRoleID;
                user_usertype = db.UserTypes.Where(o => o.UserTypeID == userID)?.FirstOrDefault().UserTypeID;

                user_resid = db.Residences.Where(o => o.ResID == userID)?.FirstOrDefault().ResID;
                user_secq = db.SecurityAnswers.Where(o => o.SecurityAnswerID== userID)?.FirstOrDefault().SecurityAnswerID;
            }

           

            UDV.UserProfile = profiles;
            UDV.UserRoles = new UserRoles
            {
                SelectedRoleID = userAssignedRoleID,
                UserRoleList = roles
            };

            UDV.User_UserTypeID = new User_UserTypeID
            {
                SelectedUserType = user_usertype,
                UserTypeList= usertypes
            };


            UDV.UserResID = new UserResID
            {

                SelectedRes = user_resid,
                ResList = residences


            };


            UDV.UserSecurityAnswerID = new UserSecurityAnswerID
            {

                SelectedSecurityAnswer = user_secq,
                SecurityAnswerList = secanswers


            };


            return UDV;
        }


        //GetUserProfile
        public UserProfileView GetUserProfile(int userID)
        {
            UserProfileView UPV = new UserProfileView();
            using (TWRLADB_Staging_V2Entities db = new TWRLADB_Staging_V2Entities())
            {
                var user = db.SYSUsers.Find(userID);
                if (user != null)
                {
                    UPV.SYSUserID = user.SYSUserID;
                    UPV.LoginName = user.LoginName;
                    UPV.Password = user.PasswordEncryptedText;

                    var SUP = db.SYSUserProfiles.Find(userID);
                    if (SUP != null)
                    {
                        UPV.FirstName = SUP.FirstName;
                        UPV.LastName = SUP.LastName;
                        UPV.Phonenumber = SUP.Phonenumber;
                        //UPV.YearOfStudy = SUP.YearOfStudy;
                        UPV.Degree = SUP.Degree;
                        UPV.DoB = SUP.DoB;
                        UPV.Email = SUP.Email;
                        UPV.SecurityAnswerID = SUP.SecurityAnswerID;
                        UPV.UserTypeID = SUP.UserTypeID;
                        //I added this here Amo because it would not convert ?int to int.
                        UPV.ResID = Convert.ToInt32(SUP.ResID);
                    }

                    var SUR = db.SYSUserRoles.Find(userID);
                    if (SUR != null)
                    {
                        UPV.LOOKUPRoleID = SUR.LOOKUPRoleID;
                        UPV.RoleName = SUR.LOOKUPRole.RoleName;
                        UPV.IsRoleActive = SUR.IsActive;
                    }
                }
            }

            return UPV;
        }













    }

    
}

