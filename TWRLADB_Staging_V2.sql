use Master
Go
if Exists (Select * from sys.databases where name = 'TWRLADB_Staging_V2')
DROP DATABASE TWRLADB_Staging_V2
Go
Create Database TWRLADB_Staging_V2
Go

use TWRLADB_Staging_V2
go


/*=================================================================================================================================
==================================================================                       =============================
========================   TWRLA Staging Database                                     =========================================================
+++++++++ This database contains the following Tables:                ===========================================
&&&&&&&&&&&&&&&   Milestone                         
                  Residence  
				   Student
				   StudentMilestone
				   StudentType
				   UserType
				   Volunteer
				   VolunteerFeedback
				   VolunteerType        
+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                     Address
					[dbo].[Attendance]  
					[dbo].[ComEngEvent]
					 [dbo].[Content]   
					 [dbo].[FunctionEvent]       
					 [dbo].[GuestSpeaker]      
					 [dbo].[Lecture]
					 [dbo].[Residence]
					 [dbo].[RSVP_Event]
					 [dbo].[Student]
					 [dbo].[StudentType]
					 [dbo].[TRWLASchedule]
					 [dbo].[Venue]
					 [dbo].[VenueType]
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^;
					 ^^^^^^;
					 
					*/


----------------------------------------  Start of tables --------------------------------------------------;

                 ---User Management --- 

				           --- Residence  ----
create table Residence
(
ResID int identity(1,1) primary key,
Res_Name varchar(100) not null
)
go

	  --- UserType---
 
create table UserType
(
UserTypeID int identity(1,1) primary key,
Description varchar(25) not null,
AccessRight char(25) not null
)
go





 ---- StudentType---

create table StudentType
(StudentTypeID int identity(1,1) primary key,
StudentTypeDescription varchar(25) not null
)
go

---Migration History Table----

CREATE TABLE [dbo].[__MigrationHistory] (
    [MigrationId]    NVARCHAR (150)  NOT NULL,
    [ContextKey]     NVARCHAR (300)  NOT NULL,
    [Model]          VARBINARY (MAX) NOT NULL,
    [ProductVersion] NVARCHAR (32)   NOT NULL,
    CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED ([MigrationId] ASC, [ContextKey] ASC)
);
go
--ASP.NET ROLES TABLES---
CREATE TABLE [dbo].[AspNetRoles] (
    [Id]   NVARCHAR (128) NOT NULL,
    [Name] NVARCHAR (256) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex]
    ON [dbo].[AspNetRoles]([Name] ASC);

		---ASPNET USERS----

	CREATE TABLE [dbo].[AspNetUsers] (
    [Id]                   int IDENTITY (1, 1) NOT NULL,
    [Email]                NVARCHAR (256) NULL,
    [EmailConfirmed]       BIT            NOT NULL,
    [PasswordHash]         NVARCHAR (MAX) NULL,
    [SecurityStamp]        NVARCHAR (MAX) NULL,
    [PhoneNumber]          NVARCHAR (MAX) NULL,
    [PhoneNumberConfirmed] BIT            NOT NULL,
    [TwoFactorEnabled]     BIT            NOT NULL,
    [LockoutEndDateUtc]    DATETIME       NULL,
    [LockoutEnabled]       BIT            NOT NULL,
    [AccessFailedCount]    INT            NOT NULL,
    [UserName]             NVARCHAR (256) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex]
    ON [dbo].[AspNetUsers]([UserName] ASC);


	---ASPNET User Claims---

CREATE TABLE [dbo].[AspNetUserClaims] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [UserId]     INT NOT NULL,
    [ClaimType]  NVARCHAR (MAX) NULL,
    [ClaimValue] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[AspNetUserClaims]([UserId] ASC);


	---ASPNET User loginss---
CREATE TABLE [dbo].[AspNetUserLogins] (
    [LoginProvider] NVARCHAR (128) NOT NULL,
    [ProviderKey]   NVARCHAR (128) NOT NULL,
    [UserId]        INT  NOT NULL,
    CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED ([LoginProvider] ASC, [ProviderKey] ASC, [UserId] ASC),
    CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[AspNetUserLogins]([UserId] ASC);


	---ASPNET USER ROLES---
	CREATE TABLE [dbo].[AspNetUserRoles] (
    [UserId] INT NOT NULL,
    [RoleId] NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC),
    CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[AspNetUserRoles]([UserId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RoleId]
    ON [dbo].[AspNetUserRoles]([RoleId] ASC);


  ------Student -----
create table Student
(
StudentID int IDENTITY(1,1) PRIMARY KEY not null,
StudentNumber varchar(10) not null,
Graduate varchar(10)  not null,
Degree varchar(35) not null,
YearOfStudy date not null,
Student_Name varchar(35) not null,
Student_Surname varchar(35) not null,
Student_Phone varchar(10) not null,
Student_DoB datetime not null,
ActiveStatus varchar(25)not null,
Id int FOREIGN KEY REFERENCES AspNetUsers(Id) not null,
 ResID int FOREIGN KEY REFERENCES Residence(ResID) not null,
 UserTypeID int FOREIGN KEY REFERENCES UserType(UserTypeID) not null,
StudentTypeID int FOREIGN KEY REFERENCES StudentType(StudentTypeID) not null

)
go

---SecurityAnswer---
create table SecurityAnswer
 (SecurityAnswerID int identity(1,1) primary key,
 Security_Question varchar(150) not null,
 Security_Answer varchar(25) not null,
StudentID int FOREIGN KEY REFERENCES Student(StudentID)
--StudentID int references Student(StudentID)
 )
 go


    ---Milestone---     
create table Milestone
(
MilestoneID int identity(1,1) primary key,
Milestone_Description varchar(35) not null
)
go

	  ---- StudentMilestone----

create table StudentMilestone
(

MilestoneID int references Milestone(MilestoneID),

primary key(/*StudentID*/ MilestoneID)
)
go

  --- VolunteerType  ----
create table VolunteerType
(VolunteerTypeID int identity(1,1) primary key,
VolunteerType_Description varchar(25) not null
)
go


	  --- Volunteer---

create table Volunteer
(
VolunteerID int IDENTITY(1,1) PRIMARY KEY,
Volunteer_Name varchar(35) not null,
Volunteer_Surname varchar(35) not null,
Volunteer_Phone varchar(10) not null,

Volunteer_DoB datetime not null,

ActiveStatus varchar(25)not null,
Id int FOREIGN KEY REFERENCES AspNetUsers(Id) not null,
UserTypeID int FOREIGN KEY REFERENCES UserType(UserTypeID) not null,
VolunteerTypeID int FOREIGN KEY REFERENCES VolunteerType(VolunteerTypeID) not null
)
go

/*

create table ResVolunteer
(

Facilitation_Year date not null,
VolunteerID int references Volunteer(VolunteerID),
Volunteer_Name varchar references Volunteer(Volunteer_Name)
ResID int references Residence(ResID),
primary key(VolunteerID, ResID)
)
go

*/


  --- VolunteerFeedback---

create table VolunteerFeedback
(
VolunteerFeedbackID int IDENTITY(1,1) PRIMARY KEY,
Feedback_Date datetime not null,
Enjoyment int not null,
Preparation int not null,
Expectation int not null,
Communication int not null,
Growth int not null,
Impact int not null,
Coping int not null,
FutureVision int not null,
Confident int not null,
Challenge int not null,
Improvement varchar(300) not null,
Criticism varchar(250) not null,
FutureMonths varchar(250) not null,
ExperienceImpact varchar(250) not null,

--VolunteerID int FOREIGN KEY REFERENCES Volunteer(VolunteerID)

)
go


--Left out until decision made
--create table AuditLog
--(
--AuditID int identity(1,1)
--)









				  /* TEST RECORDS!!! - 
				  
				  PLEASE START INSERTING INTO THE TABLES HERE :D :D :D
				  
				  
				  */
				  ---INSERT TEST RECORDS----

--User Tyupe--


insert into UserType(Description, AccessRight)
values('Admin','Flexiable')
GO

insert into UserType(Description, AccessRight)
values('Regular','Strict')
GO






--Student Type--

insert into StudentType(StudentTypeDescription)
values('Undergrad')
GO

insert into StudentType(StudentTypeDescription)
values('Postgrad')
GO


--Volunteer Type---

insert into VolunteerType(VolunteerType_Description)
values('Facilitator')
GO

insert into VolunteerType(VolunteerType_Description)
values('Mentor')
GO

insert into VolunteerType(VolunteerType_Description)
values('Boardmember')
GO



--Residence---

insert into Residence(Res_Name)
values('Nerina')
GO

insert into Residence(Res_Name)
values('Asterhof')
GO

insert into Residence(Res_Name)
values('Erika')
GO

insert into Residence(Res_Name)
values('Jasmyn')
GO

insert into Residence(Res_Name)
values('Klaradyn')
GO


insert into Residence(Res_Name)
values('Magritjie')
go

insert into Residence(Res_Name)
values('Klaradyn')
go

insert into Residence(Res_Name)
values('Vividus Ladies')
go

--Insert into Aspnetusers--



---Student---
/*
insert into Student(StudentNumber,Degree,YearOfStudy,Student_Name,Student_Surname,Student_Phone,Student_Email,Student_DoB,Student_Password,UserTypeID,StudentTypeID)
values('14284783','Informatics','2017/01/01','Siobhann','Tatum','07410298689','u14284783@tuks.co.za','1994/04/06','January','1','2')
GO


insert into Student(StudentNumber,Degree,YearOfStudy,Student_Name,Student_Surname,Student_Phone,Student_Email,Student_DoB,Student_Password,UserTypeID,StudentTypeID)
values('14284783','Informatics','2017/01/01','Siobhann','Tatum','07410298689','u14284783@tuks.co.za','1994/04/06','January','1','2')
GO


insert into Student(StudentNumber,Degree,YearOfStudy,Student_Name,Student_Surname,Student_Phone,Student_Email,Student_DoB,Student_Password,UserTypeID,StudentTypeID)
values('14284783','Informatics','2017/01/01','Siobhann','Tatum','07410298689','u14284783@tuks.co.za','1994/04/06','January','1','2')
GO


insert into Student(StudentNumber,Graduate,Degree,YearOfStudy,Student_Name,Student_Surname,Student_Phone,Student_Email,Student_DoB,Student_Password,ActiveStatus,UserTypeID,StudentTypeID,ResID)
values('14284783','1','Informatics','2017/01/01','Siobhann','Tatum','074100249','u14284783@tuks.co.za','1994/04/06','January','None active',1,2,2)
GO

insert into Student(StudentNumber,Graduate,Degree,YearOfStudy,Student_Name,Student_Surname,Student_Phone,Student_Email,Student_DoB,Student_Password,ActiveStatus,UserTypeID,StudentTypeID,ResID)
values('1422','2','BSC Zoology','2017/06/21','Manion','Flom','07784249','u1587985@tuks.co.za','1994/04/06','march','None active',1,2,2)
GO

*/




---Volunteer----
/*


insert into Volunteer(Volunteer_Name,Volunteer_Surname,Volunteer_Phone,Volunteer_DoB,Volunteer_Password,ActiveStatus)
values('Vuyo','Renene','0741258963','v@twrla','1994/06/12','myguy','None')
GO

insert into Volunteer(Volunteer_Name,Volunteer_Surname,Volunteer_Phone,Volunteer_DoB,Volunteer_Password,ActiveStatus)
values('Vuyo','Renene','0741258963','v@twrla','1994/06/12','myguy','None')
GO

insert into Volunteer(Volunteer_Name,Volunteer_Surname,Volunteer_Phone,Volunteer_DoB,Volunteer_Password,ActiveStatus)
values('Vuyo','Renene','0741258963','v@twrla','1994/06/12','myguy','None')
GO

insert into Volunteer(Volunteer_Name,Volunteer_Surname,Volunteer_Phone,Volunteer_DoB,Volunteer_Password,ActiveStatus)
values('Vuyo','Renene','0741258963','v@twrla','1994/06/12','myguy','None')
GO 



*/

insert into Volunteer(Volunteer_Name,Volunteer_Surname,Volunteer_Phone,Volunteer_DoB,ActiveStatus,Id,UserTypeID,VolunteerTypeID)
values('Vuyo','Renene','0741258963','1994/06/12','None',4,1,1)
GO


				  
				  /* TEST RECORDS!!! - 
				  
				  PLEASE END YOUR INSERTS HERERERERERERERERE :D - LIFE IS ALWAYS BEUTIFUL 
				  
				  
				  */



	-------------------------=== Events Management ===---------------------
		  
		  


		   -- Address

	
create table Address
(
	AddressID int identity(1,1) primary key,
	StreetNumber varchar(10) not null,
	StreetName varchar(35) not null,
	Suburb varchar(150) not null,
	City varchar(35) not null,
	Province varchar(35) null,
	PostCode varchar(9) null,
)
go
--vENUE tYPE
create table VenueType
(
	VenueTypeID int identity(1,1) primary key,
	VenueType_Description varchar(25) not null
)
GO

--vENUE ---
create table Venue
(
	VenueID int identity(1,1) primary key,
	Venue_Name varchar(35) not null,
	AddressID int not null,
	VenueTypeID int not null,
	FOREIGN KEY (AddressID) REFERENCES Address(AddressID),
	FOREIGN KEY (VenueTypeID) REFERENCES VenueType(VenueTypeID),
)
GO


	
			   
				---		 [dbo].[GuestSpeaker]   


create table GuestSpeaker
(
	GuestSpeakerID int identity(1,1) primary key,
	GuestSpeaker_Name varchar(35) not null,
	GuestSpeaker_Surname varchar(35) not null,
	GuestSpeaker_Phone varchar(20) null,
	GuestSpeaker_Email varchar(255) not null,
	GuestSpeaker_PictureLink varchar(100) null
)
GO
	---		 [dbo].[Content]   



			  
		
create table Content 
(
	ContentID int identity(1,1) primary key,
	Content_Name varchar(35) not null,
	Content_Link varchar(1000) not null,
	Content_Status int not null,
	Content_Description varchar(300) not null
)
GO

		
				
				   
				---		 [dbo].[Lecture]

create table Lecture
(
	LectureID int identity(1,1) primary key,
	Lecture_Name varchar(35) not null,
	Lecture_Summary varchar(100) not null,
	Lecture_Description varchar(300) not null,
	Lecture_Date datetime not null,
	Lecture_StartTime time not null,
	Lecture_EndTime time not null,
	Lecture_Theme varchar(25) null,
	VenueID int null,
	ResidenceID int null,
	ContentID int null,
	FOREIGN KEY (VenueID) REFERENCES Venue(VenueID),
	FOREIGN KEY (ResidenceID) REFERENCES Residence(ResID),
	FOREIGN KEY (ContentID) REFERENCES Content(ContentID)
)
GO
---	[dbo].[ComEngEvent]




create table ComEngEvent
(
	ComEngID int identity(1,1) primary key,
	ComEng_Name varchar(35) not null,
	ComEng_Summary varchar(100) not null,
	ComEng_Description varchar(300) not null,
	ComEng_Date datetime not null,
	ComEnge_StartTime time not null,
	ComEng_EndTime time not null,
	ComEng_Theme varchar(25) null,
	VenueID int null,
	ContentID int null,
	FOREIGN KEY (VenueID) REFERENCES Venue(VenueID),
	FOREIGN KEY (ContentID) REFERENCES Content(ContentID)
)
GO


			---		 [dbo].[FunctionEvent]  


create table FunctionEvent
(
	FunctionID int identity(1,1) primary key,
	Function_Name varchar(35) not null,
	Function_Summary varchar(100) not null,
	Function_Description varchar(300) not null,
	Function_Date datetime not null,
	Function_StartTime time not null,
	Function_EndTime time not null,
	Function_Theme varchar(25) null,
	GuestSpeakerID int null,
	VenueID int null,
	FOREIGN KEY (VenueID) REFERENCES Venue(VenueID),
	FOREIGN KEY (GuestSpeakerID) REFERENCES GuestSpeaker(GuestSpeakerID)
)
GO
					
				---		 [dbo].[TRWLASchedule]
create table TRWLASchedule
(
	ScheduleID int identity(1,1) primary key,
	FunctionID int null,
	LectureID int null,
	ComEngID int null,
	FOREIGN KEY (FunctionID) REFERENCES FunctionEvent(FunctionID),
	FOREIGN KEY (LectureID) REFERENCES Lecture(LectureID),
	FOREIGN KEY (ComEngID) REFERENCES ComEngEvent(ComEngID),
)
GO


---	[dbo].[Attendance]  

create table Attendance
(
		attendanceID int identity (1,1) primary key,
		VolunteerID int null,
		StudentID int null,
		FunctionID int null,
		LectureID int null,
		ComEngID int null,
		FOREIGN KEY (VolunteerID) REFERENCES Volunteer(VolunteerID),
		FOREIGN KEY (StudentID) REFERENCES Student(StudentID),
		FOREIGN KEY (FunctionID) REFERENCES FunctionEvent(FunctionID),
		FOREIGN KEY (LectureID) REFERENCES Lecture(LectureID),
		FOREIGN KEY (ComEngID) REFERENCES ComEngEvent(ComEngID)
)
GO	

						
				---		 [dbo].[RSVP_Event]
create table RSVP_Event
(
		rsvpID int identity (1,1) primary key,
		VolunteerID int null,
		StudentID int null,
		FunctionID int null,
		LectureID int null,
		ComEngID int null,
		Attended int null,
		FOREIGN KEY (VolunteerID) REFERENCES Volunteer(VolunteerID),
		FOREIGN KEY (StudentID) REFERENCES Student(StudentID),
		FOREIGN KEY (FunctionID) REFERENCES FunctionEvent(FunctionID),
		FOREIGN KEY (LectureID) REFERENCES Lecture(LectureID),
		FOREIGN KEY (ComEngID) REFERENCES ComEngEvent(ComEngID)
)	
GO	

create table RSVPSchedule
(
	RsvpScheduleID int identity (1,1) primary key,
	rsvpID int not null,
	ScheduleID int not null,
	StudentID int not null,
	FOREIGN KEY (StudentID) REFERENCES Student(StudentID),
	FOREIGN KEY (rsvpID) REFERENCES RSVP_Event(rsvpID),
	FOREIGN KEY (ScheduleID) REFERENCES TRWLASchedule(ScheduleID)
)
GO

create table RatingType
(
	RatingID int identity (1,1) primary key,
	Rating varchar(50) not null
)

create table LectureReview
(
	reviewID int identity (1,1) primary key,
	Review varchar(500) not null,
	RatingID int not null,
	StudentID int not null,
	LectureID int not null,
	FOREIGN KEY (StudentID) REFERENCES Student(StudentID),
	FOREIGN KEY (LectureID) REFERENCES Lecture(LectureID),
	FOREIGN KEY (RatingID) REFERENCES RatingType(RatingID)
)

create table EventMessage
(
	MessID int identity (1,1) primary key,
	Msg varchar(500) not null,
	VolunteerID int null,
	StudentID int null,
	FOREIGN KEY (StudentID) REFERENCES Student(StudentID),
	FOREIGN KEY (VolunteerID) REFERENCES Volunteer(VolunteerID)
)
go
					
				  /* TEST RECORDS!!! - 
				  
				  PLEASE START INSERTING INTO THE TABLES HERE :D :D :D
				  
				  
				  */


				  ---insert into: ADDRESS TABLE---


				  insert into Address(StreetNumber, StreetName, Suburb, City, Province, PostCode)
values('31', '22nd Street', 'Menlo Park', 'Pretori', 'Gauteng', '0081')
GO

insert into Address(StreetNumber, StreetName, Suburb, City, Province, PostCode)
values('5A', 'Arcadia Street', 'Arcadia', 'Pretoria', 'Gauteng', '2130')
GO

insert into Address(StreetNumber, StreetName, Suburb, City, Province, PostCode)
values('17', 'Chris Corner', 'Kichenbrandish', 'Pretoria', 'Gauteng', '2103')
GO

---insert into: RatingType Table---
insert into RatingType(Rating)
values('Excellent')
GO

insert into RatingType(Rating)
values('Good')
GO

insert into RatingType(Rating)
values('Average')
GO

insert into RatingType(Rating)
values('Bad')
GO

insert into RatingType(Rating)
values('Terrible')
GO


  ---insert into: VENUE TYPE TABLE---
insert into VenueType(VenueType_Description)
values('Resturant')
go

insert into VenueType(VenueType_Description)
values('Hall')
go

insert into VenueType(VenueType_Description)
values('Lecture Hall')
go

insert into VenueType(VenueType_Description)
values('House')
go


  ---insert into: VENUE TABLE---
insert into venue(Venue_Name, AddressID, VenueTypeID)
values('Duxbury Palace', 3, 4)
go

insert into venue(Venue_Name, AddressID, VenueTypeID)
values('Rautenbach Hall', 2, 2)
go

insert into venue(Venue_Name, AddressID, VenueTypeID)
values('IT 4-5', 1, 3)
go


  ---DELETE FROM: VENUETYPE TABLE---
delete from VenueType where VenueTypeID = 5
go

  ---insert into:CONTENT TABLE---
insert into Content(Content_Name, Content_Link, Content_Status, Content_Description)
values('Looking Forward', 'www.google.com', 1, 'Taking the plunge')
go

insert into Content(Content_Name, Content_Link, Content_Status, Content_Description)
values('Looking Backward', 'www.google.com', 1, 'Taking the plunge together')
go

insert into Content(Content_Name, Content_Link, Content_Status, Content_Description)
values('Strike a pose', 'www.google.com', 1, 'Making everything better for you')
go

--Guest Speaker
insert into GuestSpeaker(GuestSpeaker_Name,GuestSpeaker_Surname,GuestSpeaker_Phone,GuestSpeaker_Email,GuestSpeaker_PictureLink)
values('Bob' , 'Buider','0741258','me@me.co.za','jgjgfjjg/gjgj')
go

				  /* TEST RECORDS!!! - 
				  
				  PLEASE END YOUR INSERTS HERERERERERERERERE :D - LIFE IS ALWAYS BEUTIFUL 
				  
				  
				  */

insert into Address(StreetNumber, StreetName, Suburb, City, Province, PostCode)
values('31', '22nd Street', 'Menlo Park', 'Pretori', 'Gauteng', '0081')
GO

insert into Address(StreetNumber, StreetName, Suburb, City, Province, PostCode)
values('5A', 'Arcadia Street', 'Arcadia', 'Pretoria', 'Gauteng', '2130')
GO

insert into Address(StreetNumber, StreetName, Suburb, City, Province, PostCode)
values('17', 'Chris Corner', 'Kichenbrandish', 'Pretoria', 'Gauteng', '2103')
GO

insert into VenueType(VenueType_Description)
values('Resturant')
go

insert into VenueType(VenueType_Description)
values('Hall')
go

insert into VenueType(VenueType_Description)
values('Lecture Hall')
go

insert into VenueType(VenueType_Description)
values('House')
go

insert into venue(Venue_Name, AddressID, VenueTypeID)
values('Duxbury Palace', 3, 4)
go

insert into venue(Venue_Name, AddressID, VenueTypeID)
values('Rautenbach Hall', 2, 2)
go

insert into venue(Venue_Name, AddressID, VenueTypeID)
values('IT 4-5', 1, 3)
go

delete from VenueType where VenueTypeID = 5
go

insert into Residence(Res_Name)
values('Magritjie')
go

insert into Residence(Res_Name)
values('Madelief')
go

insert into Residence(Res_Name)
values('Klaradyn')
go

insert into Residence(Res_Name)
values('Vividus Ladies')
go

insert into Content(Content_Name, Content_Link, Content_Status, Content_Description)
values('Looking Forward', 'www.google.com', 1, 'Taking the plunge')
go

insert into Content(Content_Name, Content_Link, Content_Status, Content_Description)
values('Looking Backward', 'www.google.com', 1, 'Taking the plunge together')
go

insert into Content(Content_Name, Content_Link, Content_Status, Content_Description)
values('Strike a pose', 'www.google.com', 1, 'Making everything better for you')
go

/*
insert into AspNetUsers(Email,EmailConfirmed ,PasswordHash,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnabled,AccessFailedCount,UserName)
values('me@live.co.za',1,'454ttr##',1,1,1,1,'bob')
go*/

insert into AspNetUsers(Email, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled , LockoutEndDateUtc,LockoutEnabled, AccessFailedCount, UserName)
Values('u15213626@tuks.co.za', 1, '12sdsa', 'asdasd', '0834074027', 1, 1, '2017/10/12',1, 0, 'Rootsms4')
go

insert into Student(StudentNumber, Graduate, Degree, YearOfStudy, Student_Name, Student_Surname, Student_Phone, Student_DoB, ActiveStatus, Id, ResID, UserTypeID, StudentTypeID)
Values(15213626, 1, 'Bcom Informatics', '2017', 'Christine','Oakes', '0834074027', '1996/10/18', 1, 2, 8, 2, 1)
go