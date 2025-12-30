USE IT_Roots_Task_Db;
GO


CREATE TABLE Users (
    UserId NVARCHAR(450) PRIMARY KEY DEFAULT NEWID(),
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Role INT NOT NULL DEFAULT 1 CHECK (Role IN (1, 2)), 
    IsEmailVerified BIT NOT NULL DEFAULT 0,
    EmailVerificationToken NVARCHAR(255) NULL,
    EmailVerificationTokenExpiry DATETIME2 NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL,
    UpdatedAt DATETIME2 NULL
);

CREATE INDEX IX_Users_Username ON Users(Username);
CREATE INDEX IX_Users_Email ON Users(Email);
CREATE INDEX IX_Users_Role ON Users(Role);
CREATE INDEX IX_Users_IsActive ON Users(IsActive);



CREATE TABLE Students (
    StudentId NVARCHAR(450) PRIMARY KEY DEFAULT NEWID(),
    UserId NVARCHAR(450) NOT NULL UNIQUE,
    FullName NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(20) NULL,
    AcademicYear INT NOT NULL CHECK (AcademicYear BETWEEN 1 AND 5),
    EnrollmentDate DATETIME2 NOT NULL ,
    CreatedAt DATETIME2 NOT NULL,
    UpdatedAt DATETIME2 NULL,
    CONSTRAINT FK_Students_Users FOREIGN KEY (UserId) 
        REFERENCES Users(UserId) ON DELETE CASCADE
);

CREATE INDEX IX_Students_UserId ON Students(UserId);
CREATE INDEX IX_Students_AcademicYear ON Students(AcademicYear);


CREATE TABLE Courses (
    CourseId NVARCHAR(450) PRIMARY KEY DEFAULT NEWID(),
    CourseCode NVARCHAR(20) NOT NULL UNIQUE,
    CourseName NVARCHAR(100) NOT NULL,
    CourseNameAr NVARCHAR(100) NULL,
    Description NVARCHAR(500) NULL,
    DescriptionAr NVARCHAR(500) NULL,
    Credits INT NOT NULL,
    Semester INT NOT NULL CHECK (Semester IN (1, 2, 3)),
    SemesterYear INT NOT NULL,
    SemesterStartDate DATETIME2 NULL,
    MaxCapacity INT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL,
    UpdatedAt DATETIME2 NULL
);

CREATE INDEX IX_Courses_Semester ON Courses(Semester, SemesterYear);
CREATE INDEX IX_Courses_IsActive ON Courses(IsActive);
CREATE INDEX IX_Courses_CourseCode ON Courses(CourseCode);
CREATE INDEX IX_Courses_SemesterStartDate ON Courses(SemesterStartDate);


CREATE TABLE CourseRegistrations (
    RegistrationId NVARCHAR(450) PRIMARY KEY DEFAULT NEWID(),
    StudentId NVARCHAR(450) NOT NULL,
    CourseId NVARCHAR(450) NOT NULL,
    RegistrationDate DATETIME2 NOT NULL ,
    Status NVARCHAR(20) NOT NULL DEFAULT 'Registered',
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL,
    UpdatedAt DATETIME2 NULL,
    
    CONSTRAINT FK_CourseRegistrations_Students FOREIGN KEY (StudentId) 
        REFERENCES Students(StudentId) ON DELETE CASCADE,
    CONSTRAINT FK_CourseRegistrations_Courses FOREIGN KEY (CourseId) 
        REFERENCES Courses(CourseId) ON DELETE NO ACTION,
    
    CONSTRAINT UQ_StudentCourse UNIQUE (StudentId, CourseId),
    
    CONSTRAINT CK_CourseRegistrations_Status CHECK (
        Status IN ('Registered', 'Dropped', 'Withdrawn')
    )
);

CREATE INDEX IX_CourseRegistrations_StudentId ON CourseRegistrations(StudentId);
CREATE INDEX IX_CourseRegistrations_CourseId ON CourseRegistrations(CourseId);
CREATE INDEX IX_CourseRegistrations_Status ON CourseRegistrations(Status);
CREATE INDEX IX_CourseRegistrations_IsActive ON CourseRegistrations(IsActive);

CREATE TABLE PasswordResetTokens (
    TokenId NVARCHAR(450) PRIMARY KEY DEFAULT NEWID(),
    UserId NVARCHAR(450) NOT NULL,
    Token NVARCHAR(255) NOT NULL UNIQUE,
    ExpiresAt DATETIME2 NOT NULL,
    IsUsed BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL,
    CONSTRAINT FK_PasswordResetTokens_Users FOREIGN KEY (UserId) 
        REFERENCES Users(UserId) ON DELETE CASCADE
);

CREATE INDEX IX_PasswordResetTokens_Token ON PasswordResetTokens(Token);
CREATE INDEX IX_PasswordResetTokens_UserId ON PasswordResetTokens(UserId);
CREATE INDEX IX_PasswordResetTokens_ExpiresAt ON PasswordResetTokens(ExpiresAt);
CREATE INDEX IX_PasswordResetTokens_IsUsed ON PasswordResetTokens(IsUsed);
