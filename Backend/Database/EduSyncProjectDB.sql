create database EduSyncProjectDB;
use EduSyncProjectDB;

create table UserModel(
UserId UNIQUEIDENTIFIER PRIMARY KEY,
Name VARCHAR(100) NOT NULL,
Email VARCHAR(100) NOT NULL UNIQUE,
Role VARCHAR(20) CHECK (Role IN ('Student', 'Instructor')),
PasswordHash VARCHAR(225) NOT NULL
);

create table Course(
CourseId UNIQUEIDENTIFIER PRIMARY KEY,
Title VARCHAR(100) NOT NULL,
Description VARCHAR(100),
InstructorId UNIQUEIDENTIFIER,
MediaUrl VARCHAR(500),
FOREIGN KEY (InstructorId) REFERENCES UserModel(UserId)
);

CREATE TABLE Assessment (
    AssessmentId UNIQUEIDENTIFIER PRIMARY KEY,
    CourseId UNIQUEIDENTIFIER,
    Title VARCHAR(200) NOT NULL,
    Questions NVARCHAR(MAX),
    MaxScore INT,
    FOREIGN KEY (CourseId) REFERENCES Course(CourseId)
);

CREATE TABLE Result (
    ResultId UNIQUEIDENTIFIER PRIMARY KEY,
    AssessmentId UNIQUEIDENTIFIER,
    UserId UNIQUEIDENTIFIER,
    Score INT,
    AttemptDate DATETIME,
    FOREIGN KEY (AssessmentId) REFERENCES Assessment(AssessmentId),
    FOREIGN KEY (UserId) REFERENCES UserModel(UserId)
);