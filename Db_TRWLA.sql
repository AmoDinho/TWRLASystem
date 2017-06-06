--Worries
--Milestone 1-1 relationship
--Difference between message and notification?
--Need more attributes for Lecture and ComEngage(Otherwise may as well stay as event)
--More attributes for Schdeule

use Master
Go
if Exists (Select * from sys.databases where name = 'Db_TRWLA')
DROP DATABASE Db_TRWLA
Go
Create Database Db_TRWLA
Go

use Db_TRWLA
go

create table ResType
(ResTypeID int identity(1,1) primary key,
Description varchar(25) not null
)
go

create table Residence
(ResID int identity(1,1) primary key,
ResName varchar(100) not null,
ResType int references ResType(ResTypeID)
)
go

create table SecurityAnswer
 (SecurityAnswerID int identity(1,1) primary key,
 Question varchar(150) not null,
 Answer varchar(25) not null
 )
 go
 
create table UserType
(
UserTypeID int identity(1,1) primary key,
Description varchar(25) not null,
AccessRight int not null
)

create table Person
(PersonID int identity(1,1) primary key,
FirstName varchar(50) not null,
LastName varchar(50) not null,
Phone int not null,
EmailAddress varchar(255) not null,
DateOfBirth datetime not null,
LoginPassword varchar(35) not null,
SecurityAnswer int references SecurityAnswer(SecurityAnswerID),
UserTypeID int references UserType(UserTypeID)
)
go

create table AuditLog
(
AuditID int identity(1,1),
PersonID int references Person(PersonID),
LoginDate datetime not null,
LoginTime time not null,
LoginDuration dec not null
)

create table StudentType
(StudentTypeID int identity(1,1) primary key,
Description varchar(25) not null
)
go

create table Student
(
PersonID int primary key references Person(PersonID),
StudentNumber int not null,
Graduate bit not null,
Residence int references Residence(ResID),
StudentType int references StudentType(StudentTypeID)
)
go

create table VolunteerType
(VolunteerTypeID int identity(1,1) primary key,
Description varchar(25) not null
)
go

create table Volunteer
(
PersonID int primary key references Person(PersonID),
HireDate datetime not null,
VolunteerType int references VolunteerType(VolunteerTypeID)
)

create table UniqueCode
(
UniqueCodeID int identity(1,1) primary key,
Code char(5) not null,
Status bit not null,
--TimeActive not sure about type
PersonID int references Volunteer(PersonID)
)

create table VolunteerFeedback
(
VolunteerFeedbackID int identity(1,1),
PersonID int references Volunteer(PersonID),
primary key (VolunteerFeedbackID, PersonID),
Date datetime not null,
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
ExperienceImpact varchar(250) not null
)

create table LectureSummary
(
LectureSummaryID int identity(1,1),
PersonID int references Volunteer(PersonID),
primary key (LectureSummaryID, PersonID),
LectureDate datetime not null,
Description varchar(250) not null,
Suggestions varchar(250) not null,
Breakdown varchar(250) not null
)

create table Address
(
AddressID int identity(1,1) primary key,
StreetNumber varchar (35) not null,
StreetName varchar(35) not null,
Suburb varchar(150) not null,
City varchar(35) not null,
Province varchar(35) null,
PostCode varchar(9) null,
)

create table Schedule
(
ScheduleID int identity(1,1) primary key
)

create table VenueType
(
VenueTypeID int identity(1,1) primary key,
Description varchar(25) not null
)

create table Venue
(
VenueID int identity(1,1) primary key,
Name varchar(35) not null,
Capacity int null,
Accessibility varchar(20) null,
AddressID int references Address(AddressID),
VenueTypeID int references VenueType(VenueTypeID)
)

create table Content 
(
ContentID int identity(1,1) primary key,
ContentType varchar(35) not null,
Name varchar(35) not null,
Author varchar (70) not null,
DatePublished datetime not null,
Link varchar(100) not null,
Theme varchar(35) null,
Status binary not null,
Description varchar(300) not null
)

create table Event
(
EventID int identity(1,1) primary key,
Name varchar(35) not null,
Summary varchar(100) not null,
Description varchar(300) not null,
EventDate datetime not null,
EventStartTime time not null,
VenueID int references Venue(VenueID)
)

create table Theme
( 
ThemeID int identity(1,1) primary key,
ThemeName varchar(35) not null,
EventID int references Event(EventID)--Not sure this should be here
)

create table EventSchedule
(
ScheduleID int references Schedule(ScheduleID),
EventID int references Event(EventID),
primary key(ScheduleID, EventID)
)

create table GuestSpeaker
(
GuestSpeakerID int identity(1,1) primary key,
FirstName varchar(50) not null,
LastName varchar(50) not null,
Phone int not null,
EmailAddress varchar(255) not null,
PictureLink varchar(100) null
)

create table FunctionEvent
(
EventID int primary key references Event(EventID)
)

create table FunctionSpeaker
(
GuestSpeakerID int references GuestSpeaker(GuestSpeakerID),
EventID int references FunctionEvent(EventID),
primary key(GuestSpeakerID, EventID)
)

create table CommEngage
(
EventID int primary key references Event(EventID),
)

create table Lecture
(
EventID int primary key references Event(EventID),
)

create table LectureReview
(
LectureReviewID int identity(1,1),
EventID int references Lecture(EventID),
primary key (LectureReviewID, EventID),
Comment varchar(250) not null,
Rating char(5) not null,

)

create table CommContent
(
ContentID int references Content(ContentID),
EventID int references CommEngage(EventID),
primary key(ContentID, EventID)
)

create table LectureContent
(
ContentID int references Content(ContentID),
EventID int references Lecture(EventID),
primary key(ContentID, EventID)
)

create table Attendance
(
PersonID int references Person(PersonID),
EventID int references Event(EventID),
primary key(PersonID, EventID)
)

create table RSVP_Event
(
PersonID int references Person(PersonID),
EventID int references Event(EventID),
primary key(PersonID, EventID)
)

create table Res_Event
(
ResID int references Residence(ResID),
EventID int references Event(EventID),
primary key(ResID, EventID)
)
