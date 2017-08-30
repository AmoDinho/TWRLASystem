﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TRWLASystemMaster.Models.DB;
using TRWLASystemMaster.Models.ViewModel;

namespace TRWLASystemMaster.Models.EntityManager
{
    public class UserManager
    {
        //Student
        public void AddUserAccount(UserSignUpView user)
        {

            using (TWRLADB_Staging_V2Entities17 db = new TWRLADB_Staging_V2Entities17())
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

        //Volunteer
        public void AddUserAccount(UserSignUpViewVol user)
        {

            using (TWRLADB_Staging_V2Entities17 db = new TWRLADB_Staging_V2Entities17())
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

        public bool IsLoginNameExist(string loginName)
        {
            using (TWRLADB_Staging_V2Entities17 db = new TWRLADB_Staging_V2Entities17())
            {
                return db.SYSUsers.Where(o => o.LoginName.Equals(loginName)).Any();
            }
        }

        public string GetUserPassword(string loginName)
        {
            using (TWRLADB_Staging_V2Entities17 db = new TWRLADB_Staging_V2Entities17())
            {
                var user = db.SYSUsers.Where(o => o.LoginName.ToLower().Equals(loginName));
                if (user.Any())
                    return user.FirstOrDefault().PasswordEncryptedText;
                else
                    return string.Empty;
            }
        }




        public bool IsUserInRole(string loginName, string roleName)
        {
            using (TWRLADB_Staging_V2Entities17 db = new TWRLADB_Staging_V2Entities17())
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


        public List < LOOKUPAvailableRole > GetAllRoles()
        {
            using (TWRLADB_Staging_V2Entities17 db = new TWRLADB_Staging_V2Entities17())
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

        public int GetUserID(string loginName)
        {
            using (TWRLADB_Staging_V2Entities17 db = new TWRLADB_Staging_V2Entities17())
            {
                var user = db.SYSUsers.Where(o => o.LoginName.Equals(loginName));
                if (user.Any()) return user.FirstOrDefault().SYSUserID;
            }
            return 0;
        }

        public List<UserProfileView> GetAllUserProfiles()
        {
            List < UserProfileView > profiles = new List < UserProfileView > ();

            using (TWRLADB_Staging_V2Entities17 db = new TWRLADB_Staging_V2Entities17())
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
                        UPV.ResID = SUP.ResID;
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

        public UserDataView GetUserDataView(string loginName)
        {
            UserDataView UDV = new UserDataView();
            List < UserProfileView > profiles = GetAllUserProfiles();
            List <LOOKUPAvailableRole > roles = GetAllRoles();
            List<LookUpUserType> usertypes = new List<LookUpUserType>();

            List<LookUpRes> residences = new List<LookUpRes>();

            List<LookupSecurityAnswer> secanswers = new List<LookupSecurityAnswer>();


            int? userAssignedRoleID = 0, userID = 0;
            int? user_usertype = 0;
            int? user_resid = 0;
            int? user_secq= 0;

            userID = GetUserID(loginName);
            using (TWRLADB_Staging_V2Entities17 db = new TWRLADB_Staging_V2Entities17())
            {
                userAssignedRoleID = db.SYSUserRoles.Where(o => o.SYSUserID == userID)?.FirstOrDefault().LOOKUPRoleID;
                user_usertype = db.SYSUserProfiles.Where(o => o.SYSUserID == userID)?.FirstOrDefault().UserTypeID;

                user_resid = db.SYSUserProfiles.Where(o => o.SYSUserID == userID)?.FirstOrDefault().ResID;
                user_secq = db.SYSUserProfiles.Where(o => o.SYSUserID == userID)?.FirstOrDefault().SecurityAnswerID;
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
















    }

    
}

