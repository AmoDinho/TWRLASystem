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

CREATE TABLE [dbo].[LOOKUPRole](
    [LOOKUPRoleID] [int] IDENTITY(1,1) NOT NULL,
    [RoleName] [varchar](100) DEFAULT '',
    [RoleDescription] [varchar](500) DEFAULT '',
    [RowCreatedSYSUserID] [int] NOT NULL,
    [RowCreatedDateTime] [datetime]  DEFAULT GETDATE(),
    [RowModifiedSYSUserID] [int] NOT NULL,
    [RowModifiedDateTime] [datetime] DEFAULT GETDATE(),
PRIMARY KEY (LOOKUPRoleID)
    )
GO





 ---- StudentType---

create table StudentType
(StudentTypeID int identity(1,1) primary key,
StudentTypeDescription varchar(25) not null
)
go




CREATE TABLE [dbo].[SYSUser](
    [SYSUserID] [int] IDENTITY(1,1) NOT NULL,
    [LoginName] [varchar](50) NOT NULL,
    [PasswordEncryptedText] [varchar](200) NOT NULL,
    [RowCreatedSYSUserID] [int] NOT NULL,
    [RowCreatedDateTime] [datetime] DEFAULT GETDATE(),
    [RowModifiedSYSUserID] [int] NOT NULL,
    [RowModifiedDateTime] [datetime] DEFAULT GETDATE(),
    PRIMARY KEY (SYSUserID)
)

GO


CREATE TABLE [dbo].[SYSUserProfile](
    [SYSUserProfileID] [int] IDENTITY(1,1) NOT NULL,
    [SYSUserID] [int] NOT NULL,
	[StudentNumber] [varchar](10) NULL,
    [FirstName] [varchar](35) NOT NULL,
    [LastName] [varchar](35) NOT NULL,
	UserTypeID int not null,
    [Email] [varchar](225) NOT NULL,
	[DoB] [datetime]  NOT NULL,
	[Phonenumber] [varchar](50) NOT NULL,

	
	Graduate varchar(10) null,
    Degree varchar(35) null,
    YearOfStudy datetime null,
	
    [RowCreatedSYSUserID] [int] NOT NULL,
    [RowCreatedDateTime] [datetime] DEFAULT GETDATE(),
    [RowModifiedSYSUserID] [int] NOT NULL,
    [RowModifiedDateTime] [datetime] DEFAULT GETDATE(),
    PRIMARY KEY (SYSUserProfileID),
	
	ResID int FOREIGN KEY REFERENCES Residence(ResID)  null,

	
    )


create table SecurityQuestion
(
	QuestionID int identity (1,1) primary key,
	Question varchar(100) not null
	)


---SecurityAnswer---
create table SecurityAnswer
 (SecurityAnswerID int identity(1,1) primary key,
 Security_Answer varchar(25) not null,
QuestionID INT NOT NULL,
SYSUserProfileID INT FOREIGN KEY REFERENCES SYSUserProfile(SYSUserProfileID) not null,
FOREIGN KEY (QuestionID) REFERENCES SecurityQuestion(QuestionID)
 )
 go

 Create table Progress
(
	ProgressID int identity (1,1) primary key,
	[SYSUserProfileID] [int] not null,
	ProgressCount int not null,

	FOREIGN KEY ([SYSUserProfileID]) REFERENCES [SYSUserProfile]([SYSUserProfileID])
)
go


ALTER TABLE [dbo].[SYSUserProfile]  WITH CHECK ADD FOREIGN KEY([SYSUserID])
REFERENCES [dbo].[SYSUser] ([SYSUserID])
GO

ALTER TABLE [dbo].[SYSUserProfile]  WITH CHECK ADD FOREIGN KEY([UserTypeID])
REFERENCES [dbo].[UserType] ([UserTypeID])
GO



CREATE TABLE [dbo].[SYSUserRole](
    [SYSUserRoleID] [int] IDENTITY(1,1) NOT NULL,
    [SYSUserID] [int] NOT NULL,
    [LOOKUPRoleID] [int] NOT NULL,
    [IsActive] [bit] DEFAULT (1),
    [RowCreatedSYSUserID] [int] NOT NULL,
    [RowCreatedDateTime] [datetime] DEFAULT GETDATE(),
    [RowModifiedSYSUserID] [int] NOT NULL,
    [RowModifiedDateTime] [datetime] DEFAULT GETDATE(),
    PRIMARY KEY (SYSUserRoleID)
)
GO

ALTER TABLE [dbo].[SYSUserRole]  WITH CHECK ADD FOREIGN KEY([LOOKUPRoleID])
REFERENCES [dbo].[LOOKUPRole] ([LOOKUPRoleID])
GO

ALTER TABLE [dbo].[SYSUserRole]  WITH CHECK ADD FOREIGN KEY([SYSUserID])
REFERENCES [dbo].[SYSUser] ([SYSUserID])
GO



create table UniqueCode
(
UniID int IDENTITY(1,1) PRIMARY KEY not null,
Code int not null,
stamptime datetime not null

)
go

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
 ResID int FOREIGN KEY REFERENCES Residence(ResID) not null,
 UserTypeID int FOREIGN KEY REFERENCES UserType(UserTypeID) not null,
StudentTypeID int FOREIGN KEY REFERENCES StudentType(StudentTypeID) not null,
SYSUserProfileID INT FOREIGN KEY REFERENCES SYSUserProfile(SYSUserProfileID) null
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
values('Student','Strict')
GO

insert into UserType(Description, AccessRight)
values('Volunteer','Flexiable')
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
  Values ('None')
GO

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
values('Klaradyn')
GO


insert into Residence(Res_Name)
values('Magritjie')
go

insert into Residence(Res_Name)
values('Madelief')
go


insert into Residence(Res_Name)
values('Vividus Ladies')
go









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

insert into Volunteer(Volunteer_Name,Volunteer_Surname,Volunteer_Phone,Volunteer_DoB,ActiveStatus,UserTypeID,VolunteerTypeID)
values('Vuyo','Renene','0741258963','1994/06/12','None',4,1,1)
GO

--Security question---

 insert into SecurityAnswer(Security_Question,Security_Answer)
 values('What is your mothers maden name','Kelebogile')
 go

				  
				  /* TEST RECORDS!!! - 
				  
				  PLEASE END YOUR INSERTS HERERERERERERERERE :D - LIFE IS ALWAYS BEUTIFUL 
				  
				  
				  */



	-------------------------=== Events Management ===---------------------
		  
		  



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
	StreeNumber varchar(10) null,
StreetName varchar(35) null,
Suburb varchar(35) null,
City varchar(35) null,
Province varchar(35) null,
PostCode varchar(9) null,
	VenueTypeID int not null,
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
	Type int not null,
	FOREIGN KEY (VenueID) REFERENCES Venue(VenueID),
	FOREIGN KEY (ResidenceID) REFERENCES Residence(ResID),
	FOREIGN KEY (ContentID) REFERENCES Content(ContentID)
)
GO

create table GenEvent
(
	GenID int identity(1,1) primary key,
	Gen_Name varchar(35) not null,
	Gen_Summary varchar(100) not null,
	Gen_Description varchar(300) not null,
	Gen_Date datetime not null,
	Gen_StartTime time not null,
	Gen_EndTime time not null,
	Gene_Theme varchar(25) null,
	Type int not null,
	VenueID int null,
	ResID int null,
	ContentID int null,
	GuestSpeakerID int null,
	FOREIGN KEY (VenueID) REFERENCES Venue(VenueID),
	FOREIGN KEY (ResID) REFERENCES Residence(ResID),
	FOREIGN KEY (ContentID ) REFERENCES Content(ContentID),
	FOREIGN KEY (GuestSpeakerID) REFERENCES GuestSpeaker(GuestSpeakerID)
)
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
	Type int not null,
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
	Type int not null,
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
	GenID int null,
	FOREIGN KEY (FunctionID) REFERENCES FunctionEvent(FunctionID),
	FOREIGN KEY (LectureID) REFERENCES Lecture(LectureID),
	FOREIGN KEY (ComEngID) REFERENCES ComEngEvent(ComEngID),
	FOREIGN KEY (GenID) REFERENCES GenEvent(GenID)
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
		GenID int null,
	SYSUserProfileID int null,
	FOREIGN KEY (SYSUserProfileID) REFERENCES SYSUserProfile(SYSUserProfileID),
		FOREIGN KEY (VolunteerID) REFERENCES Volunteer(VolunteerID),
		FOREIGN KEY (StudentID) REFERENCES Student(StudentID),
		FOREIGN KEY (FunctionID) REFERENCES FunctionEvent(FunctionID),
		FOREIGN KEY (LectureID) REFERENCES Lecture(LectureID),
		FOREIGN KEY (ComEngID) REFERENCES ComEngEvent(ComEngID),
		FOREIGN KEY (GenID) REFERENCES GenEvent(GenID)
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
		GenID int null,
		SYSUserProfileID int null,
		FOREIGN KEY (SYSUserProfileID) REFERENCES SYSUserProfile(SYSUserProfileID),
		FOREIGN KEY (VolunteerID) REFERENCES Volunteer(VolunteerID),
		FOREIGN KEY (StudentID) REFERENCES Student(StudentID),
		FOREIGN KEY (FunctionID) REFERENCES FunctionEvent(FunctionID),
		FOREIGN KEY (LectureID) REFERENCES Lecture(LectureID),
		FOREIGN KEY (ComEngID) REFERENCES ComEngEvent(ComEngID),
		FOREIGN KEY (GenID) REFERENCES GenEvent(GenID)
)	
GO	

create table RSVPSchedule
(
	RsvpScheduleID int identity (1,1) primary key,
	rsvpID int not null,
	ScheduleID int not null,
	SYSUserProfileID int null,
	FOREIGN KEY (SYSUserProfileID) REFERENCES SYSUserProfile(SYSUserProfileID),
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
	LectureID int not null,
	SYSUserProfileID int null,
	FOREIGN KEY (SYSUserProfileID) REFERENCES SYSUserProfile(SYSUserProfileID),
	FOREIGN KEY (LectureID) REFERENCES Lecture(LectureID),
	FOREIGN KEY (RatingID) REFERENCES RatingType(RatingID)
)

Create table Progress
(
	ProgressID int identity (1,1) primary key,
	StudentNumber varchar(1) not null,
	ProgressCount int not null
)
go

create table EventMessage
(
	MessID int identity (1,1) primary key,
	Msg varchar(500) not null,
	TimeMes time not null,
	NumberMess int not null,
	VolunteerID int null,
	SYSUserProfileID int null,
	FOREIGN KEY (SYSUserProfileID) REFERENCES SYSUserProfile(SYSUserProfileID),
	FOREIGN KEY (VolunteerID) REFERENCES Volunteer(VolunteerID)
)
go























				  /* TEST RECORDS!!! - 
				  
				  PLEASE START INSERTING INTO THE TABLES HERE :D :D :D
				  
				  
				  */
				  ---INSERT TEST RECORDS----

				  				  --rESIDENCES

	













			  
				  /* TEST RECORDS!!! - 
				  
				  PLEASE END YOUR INSERTS HERERERERERERERERE :D - LIFE IS ALWAYS BEUTIFUL 
				  
				  
				  */



	-------------------------=== Events Management ===---------------------
		  
		  



					
				  /* TEST RECORDS!!! - 
				  
				  PLEASE START INSERTING INTO THE TABLES HERE :D :D :D
				  
				  
				  */




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





				  /* TEST RECORDS!!! - 
				  
				  PLEASE END YOUR INSERTS HERERERERERERERERE :D - LIFE IS ALWAYS BEUTIFUL */
	

insert into SecurityAnswer(Security_Question, Security_Answer)
values('What is the Maiden Name of your Mother', 'Wallace')
go

insert into SecurityAnswer(Security_Question, Security_Answer)
values('What is first dogs name?', 'Cadbury')
go

insert into SecurityAnswer(Security_Question, Security_Answer)
values('What is the name of your street where you were born', 'Linksfield')
go

insert into SecurityAnswer(Security_Question, Security_Answer)
values('What is was your favourite sport in high school', 'Swimming')
go
/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP 1000 [ContentID]
      ,[Content_Name]
      ,[Content_Link]
      ,[Content_Status]
      ,[Content_Description]
  FROM [TWRLADB_Staging_V2].[dbo].[Content]

  
/*Look up Roles*/

INSERT INTO LOOKUPRole (RoleName,RoleDescription,RowCreatedSYSUserID,RowModifiedSYSUserID)
       VALUES ('Volunteer','Can Edit, Update, Delete',1,1)
INSERT INTO LOOKUPRole (RoleName,RoleDescription,RowCreatedSYSUserID,RowModifiedSYSUserID)
       VALUES ('Student','Read only',1,1)


/*Test Students & Volunteerss*/

--Volunteers--

--vOL 1--
INSERT INTO SYSUser (LoginName,PasswordEncryptedText, RowCreatedSYSUserID, RowModifiedSYSUserID)  
VALUES ('Admin','Admin',1,1)  
  
INSERT INTO SYSUserProfile (SYSUserID,FirstName,LastName,DoB,UserTypeID,Email,Phonenumber,RowCreatedSYSUserID, RowModifiedSYSUserID)  
VALUES (1,'Vinz','Durano','1994/02/03',2,'vinz@tuks.co.za','0741028963',1,1)  
 
  
INSERT INTO SYSUserRole (SYSUserID,LOOKUPRoleID,IsActive,RowCreatedSYSUserID, RowModifiedSYSUserID)  
VALUES (1,1,1,1,1)
 


--vOL 2---
INSERT INTO SYSUser (LoginName,PasswordEncryptedText, RowCreatedSYSUserID, RowModifiedSYSUserID)  
VALUES ('Admin2','Admin2',2,2)  

INSERT INTO SYSUserProfile (SYSUserID,FirstName,LastName,DoB,UserTypeID,Email,Phonenumber,RowCreatedSYSUserID, RowModifiedSYSUserID)  
VALUES (2,'Sarah','Swart','1995/02/08',2,'sarah@tuks.co.za','0821478961',2,2)  


INSERT INTO SYSUserRole (SYSUserID,LOOKUPRoleID,IsActive,RowCreatedSYSUserID, RowModifiedSYSUserID)  
VALUES (2,1,1,2,2)




---Students---


---Student1---
INSERT INTO SYSUser (LoginName,PasswordEncryptedText, RowCreatedSYSUserID, RowModifiedSYSUserID)  
VALUES ('Student1','Student2',3,3)  

INSERT INTO SYSUserProfile (SYSUserID,FirstName,LastName,DoB,UserTypeID,Email,Phonenumber,StudentNumber,Degree,YearOfStudy,ResID,RowCreatedSYSUserID, RowModifiedSYSUserID)  
VALUES (3,'Noma','Hear','1995/10/11',1,'noma@tuks.co.za','0893123456','14284783','BCom','2017/01/01',5,3,3)  

INSERT INTO SYSUserRole (SYSUserID,LOOKUPRoleID,IsActive,RowCreatedSYSUserID, RowModifiedSYSUserID)  
VALUES (3,2,1,3,3)


---Student2---

INSERT INTO SYSUser (LoginName,PasswordEncryptedText, RowCreatedSYSUserID, RowModifiedSYSUserID)  
VALUES ('Student2','Student3',4,4)  

INSERT INTO SYSUserProfile (SYSUserID,FirstName,LastName,DoB,UserTypeID,Email,Phonenumber,StudentNumber,Degree,YearOfStudy,ResID,RowCreatedSYSUserID, RowModifiedSYSUserID)  


VALUES (4,'Marche','De Waal','1994/06/14',1,'march17@tuks.co.za','0587966258','14847834','BCom','2017/01/01',4,4,4)  


INSERT INTO SYSUserRole (SYSUserID,LOOKUPRoleID,IsActive,RowCreatedSYSUserID, RowModifiedSYSUserID)  
VALUES (4,2,1,4,4)


---Student 3-----

INSERT INTO SYSUser (LoginName,PasswordEncryptedText, RowCreatedSYSUserID, RowModifiedSYSUserID)  
VALUES ('Student3','Student4',5,5)  

INSERT INTO SYSUserProfile (SYSUserID,FirstName,LastName,DoB,UserTypeID,Email,Graduate,Phonenumber,StudentNumber,Degree,YearOfStudy,ResID,RowCreatedSYSUserID, RowModifiedSYSUserID)  


VALUES (5,'Michelle','Swart','1992/10/20',1,'swart@tuks.co.za','Graduated','0741025896','17845796','Soil Science','2017/01/01',2,5,5)  


INSERT INTO SYSUserRole (SYSUserID,LOOKUPRoleID,IsActive,RowCreatedSYSUserID, RowModifiedSYSUserID)  
VALUES (5,2,1,5,5)

---Unique Code ----
INSERT INTO UniqueCode(Code,stamptime)
VALUES('12345','2017/09/20')


INSERT INTO UniqueCode(Code,stamptime)
VALUES('25864','2017/09/22')

INSERT INTO UniqueCode(Code,stamptime)
VALUES('89752','2017/09/26')



CREATE TABLE AuditLog
(
	AuditID int identity (1,1) Primary key,
	DateDone datetime not null,
	TypeTran varchar(50) not null,
	TableAff varchar(50) not null,
	SYSUserProfileID int not null,
	FOREIGN KEY (SYSUserProfileID) REFERENCES SYSUserProfile(SYSUserProfileID)
)

create table progressbar
(
	ProgBarID int identity (1,1) primary key,
	SYSUserProfileID int not null,
	LecProg int not null,
	FuncProg int not null,
	ComProg int not null,
	GenProg int not null,

	foreign key (SYSUserProfileID) references SYSUserProfile(SYSUserProfileID)
)

create table MasterData
(
	MasterID int identity (1,1) primary key,
	LecAttend int not null,
	FuncAttend int not null,
	ComAttend int not null,
	GenAttend int not null,
	RegDate date not null,
	CancelEvent int not null,
	LogAttendTime int not null,
	PassFactor varchar(5) null
)


Create table ClassAttendance
(
	CAPK int identity not null primary key,
	EventType varchar(50) not null,
	attend int not null
)

insert into ClassAttendance(EventType, attend)
values('Function', 0)
go
Fr
insert into ClassAttendance(EventType, attend)
values('Lecture', 0)
go

insert into ClassAttendance(EventType, attend)
values('Community Engagement', 0)
go

insert into ClassAttendance(EventType, attend)
values('General Event', 0)
go

ALTER TABLE  SYSUserProfile
ADD RowVersion timestamp not null;

INSERT INTO VenueType(VenueType_Description)
Values ('None')

INSERT INTO VenueType(VenueType_Description)
Values ('House')

INSERT INTO VenueType(VenueType_Description)
Values ('Hall')

INSERT INTO VenueType(VenueType_Description)
Values ('Lecture Hall')

INSERT INTO VenueType(VenueType_Description)

Values ('School')

INSERT INTO Venue (Venue_Name, StreeNumber, StreetName, Suburb, City, Province, PostCode, VenueTypeID)
VALUES ('None', 'NULL', 'NULL', 'NULL', 'NULL', 'NULL', 'NULL', 1)
go

INSERT INTO GuestSpeaker(GuestSpeaker_Name, GuestSpeaker_Surname, GuestSpeaker_Phone, GuestSpeaker_Email, GuestSpeaker_PictureLink)
VALUES ('None', 'NULL', 'NULL', 'NULL', 'NULL')

INSERT INTO Content(Content_Name, Content_Link, Content_Status, Content_Description)
VALUES ('None', 'NULL',1,'NULL')

insert into SecurityQuestion (Question)
  values ('What Is your favorite book?'),( 
  'What is the name of the road you grew up on?'),
('What was the first company that you worked for?'),
('Where did you meet your spouse?'),
('Where did you go to high school/college?'),
('What is your favorite food?'),
('Where is your favorite place to vacation?')

