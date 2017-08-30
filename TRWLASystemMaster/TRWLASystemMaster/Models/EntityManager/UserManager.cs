using System;
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

        }

    
}

