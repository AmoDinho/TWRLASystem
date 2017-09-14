use TWRLADB_Staging_V2
go


select SYSUser.LoginName, SYSUser.PasswordEncryptedText,SYSUserProfile.StudentNumber,SYSUserProfile.FirstName,SYSUserProfile.LastName,SYSUserProfile.UserTypeID,SYSUserProfile.Email,SYSUserProfile.DoB,SYSUserProfile.Phonenumber,SYSUserProfile.SecurityAnswerID,SYSUserProfile.Graduate,SYSUserProfile.Degree,SYSUserProfile.YearOfStudy,SYSUserRole.LOOKUPRoleID
from SYSUser
full outer join SYSUserProfile on
SYSUser.SYSUserID = SYSUserProfile.SYSUserID
full outer join SYSUserRole on
SYSUser.SYSUserID = SYSUserRole.SYSUserRoleID
where SYSUser.SYSUserID = 1;