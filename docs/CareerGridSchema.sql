IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = N'CareerGrid')
BEGIN
    EXEC('CREATE DATABASE CareerGrid;');
END
GO

USE CareerGrid;
GO

-- DROP EXISTING TABLES (IF RE-RUNNING FRESH)
IF OBJECT_ID('dbo.UserSessions', 'U') IS NOT NULL DROP TABLE dbo.UserSessions;
IF OBJECT_ID('dbo.AuditLogs', 'U') IS NOT NULL DROP TABLE dbo.AuditLogs;
IF OBJECT_ID('dbo.ApplicationStatusLogs', 'U') IS NOT NULL DROP TABLE dbo.ApplicationStatusLogs;
IF OBJECT_ID('dbo.Applications', 'U') IS NOT NULL DROP TABLE dbo.Applications;
IF OBJECT_ID('dbo.JobPostingSkills', 'U') IS NOT NULL DROP TABLE dbo.JobPostingSkills;
IF OBJECT_ID('dbo.JobPostings', 'U') IS NOT NULL DROP TABLE dbo.JobPostings;
IF OBJECT_ID('dbo.ApplicantSkills', 'U') IS NOT NULL DROP TABLE dbo.ApplicantSkills;
IF OBJECT_ID('dbo.Skills', 'U') IS NOT NULL DROP TABLE dbo.Skills;
IF OBJECT_ID('dbo.Resumes', 'U') IS NOT NULL DROP TABLE dbo.Resumes;
IF OBJECT_ID('dbo.WorkExperiences', 'U') IS NOT NULL DROP TABLE dbo.WorkExperiences;
IF OBJECT_ID('dbo.ApplicantProfiles', 'U') IS NOT NULL DROP TABLE dbo.ApplicantProfiles;
IF OBJECT_ID('dbo.EmployerProfiles', 'U') IS NOT NULL DROP TABLE dbo.EmployerProfiles;
IF OBJECT_ID('dbo.Companies', 'U') IS NOT NULL DROP TABLE dbo.Companies;
IF OBJECT_ID('dbo.UserAccounts', 'U') IS NOT NULL DROP TABLE dbo.UserAccounts;
IF OBJECT_ID('dbo.JobSubClassifications', 'U') IS NOT NULL DROP TABLE dbo.JobSubClassifications;
IF OBJECT_ID('dbo.JobClassifications', 'U') IS NOT NULL DROP TABLE dbo.JobClassifications;
GO

-- CLASSIFICATIONS & SKILLS
IF OBJECT_ID('dbo.JobClassifications', 'U') IS NULL
BEGIN
    CREATE TABLE JobClassifications (
        ClassificationID INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL UNIQUE,
        Description NVARCHAR(255) NULL,
        CreatedAt DATETIMEOFFSET DEFAULT SYSDATETIMEOFFSET() NOT NULL
    );
END

IF OBJECT_ID('dbo.JobSubClassifications', 'U') IS NULL
BEGIN
    CREATE TABLE JobSubClassifications (
        SubClassificationID INT IDENTITY(1,1) PRIMARY KEY,
        ClassificationID INT NOT NULL,
        Name NVARCHAR(100) NOT NULL,
        
        CONSTRAINT FK_SubClassifications_Classifications 
            FOREIGN KEY (ClassificationID) REFERENCES JobClassifications(ClassificationID) ON DELETE CASCADE,
        CONSTRAINT UQ_Classification_SubClassification UNIQUE (ClassificationID, Name)
    );
END

IF OBJECT_ID('dbo.Skills', 'U') IS NULL
BEGIN
    CREATE TABLE Skills (
        SkillID INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL UNIQUE,
        ClassificationID INT NULL,
        SubClassificationID INT NULL,
        CreatedAt DATETIMEOFFSET DEFAULT SYSDATETIMEOFFSET() NOT NULL,

        CONSTRAINT FK_Skills_JobClassifications 
            FOREIGN KEY (ClassificationID) REFERENCES JobClassifications(ClassificationID) 
            ON DELETE SET NULL,

        CONSTRAINT FK_Skills_JobSubClassifications 
            FOREIGN KEY (SubClassificationID) REFERENCES JobSubClassifications(SubClassificationID) 
            ON DELETE NO ACTION
    );

    CREATE NONCLUSTERED INDEX IX_Skills_Name ON Skills(Name);
    CREATE NONCLUSTERED INDEX IX_Skills_ClassificationID ON Skills(ClassificationID);
    CREATE NONCLUSTERED INDEX IX_Skills_SubClassificationID ON Skills(SubClassificationID);
END

-- USER ACCOUNTS & IDENTITY
IF OBJECT_ID('dbo.UserAccounts', 'U') IS NULL
BEGIN
    CREATE TABLE UserAccounts (
        UserID INT IDENTITY(1,1) PRIMARY KEY,
        Email NVARCHAR(256) NOT NULL,
        PasswordHash NVARCHAR(256) NOT NULL,
        FirstName NVARCHAR(100) NOT NULL,
        LastName NVARCHAR(100) NOT NULL,
        Role NVARCHAR(20) NOT NULL, -- 'Applicant', 'Employer', 'Admin'
        IsVerified BIT NOT NULL DEFAULT 0,
        VerificationCode NVARCHAR(6) NULL,
        VerificationExpiry DATETIMEOFFSET NULL,
        CreatedAt DATETIMEOFFSET DEFAULT SYSDATETIMEOFFSET() NOT NULL,
        UpdatedAt DATETIMEOFFSET DEFAULT SYSDATETIMEOFFSET() NOT NULL,

        CONSTRAINT CK_UserAccounts_Role CHECK (Role IN ('Applicant', 'Employer', 'Admin')),
        CONSTRAINT CK_UserAccounts_EmailFormat CHECK (Email LIKE '%_@__%.__%')
    );

    CREATE UNIQUE NONCLUSTERED INDEX UX_UserAccounts_Email ON UserAccounts(Email);
END

-- USER SESSIONS
IF OBJECT_ID('dbo.UserSessions', 'U') IS NULL
BEGIN
    CREATE TABLE UserSessions (
        SessionID INT IDENTITY(1,1) PRIMARY KEY,
        UserID INT NOT NULL,
        RefreshToken NVARCHAR(500) NOT NULL,
        IpAddress NVARCHAR(45) NULL,
        UserAgent NVARCHAR(500) NULL,
        IsRevoked BIT NOT NULL DEFAULT 0,
        ExpiresAt DATETIMEOFFSET NOT NULL,
        CreatedAt DATETIMEOFFSET DEFAULT SYSDATETIMEOFFSET() NOT NULL,

        CONSTRAINT FK_UserSessions_UserAccounts FOREIGN KEY (UserID) 
            REFERENCES UserAccounts(UserID) ON DELETE CASCADE
    );

    CREATE NONCLUSTERED INDEX IX_UserSessions_UserID ON UserSessions(UserID);
    CREATE UNIQUE NONCLUSTERED INDEX UX_UserSessions_RefreshToken ON UserSessions(RefreshToken);
END

-- COMPANIES & EMPLOYER PROFILES
IF OBJECT_ID('dbo.Companies', 'U') IS NULL
BEGIN
    CREATE TABLE Companies (
        CompanyID INT IDENTITY(1,1) PRIMARY KEY,
        CompanyName NVARCHAR(150) NOT NULL,
        CompanyDescription NVARCHAR(MAX) NULL,
        WebsiteUrl NVARCHAR(2083) NULL,
        LogoUrl NVARCHAR(2083) NULL,
        CompanyCode NVARCHAR(10) NOT NULL UNIQUE,
        
        -- Verification by Admin
        VerificationStatus NVARCHAR(20) NOT NULL DEFAULT 'Pending', -- 'Pending', 'Approved', 'Rejected'
        ApprovedByAdminID INT NULL,
        ApprovedAt DATETIMEOFFSET NULL,
        
        CreatedAt DATETIMEOFFSET DEFAULT SYSDATETIMEOFFSET() NOT NULL,

        CONSTRAINT FK_Companies_Admin FOREIGN KEY (ApprovedByAdminID) REFERENCES UserAccounts(UserID),
        CONSTRAINT CK_Companies_Status CHECK (VerificationStatus IN ('Pending', 'Approved', 'Rejected'))
    );
END

IF OBJECT_ID('dbo.EmployerProfiles', 'U') IS NULL
BEGIN
    CREATE TABLE EmployerProfiles (
        EmployerProfileID INT IDENTITY(1,1) PRIMARY KEY,
        UserID INT NOT NULL UNIQUE,
        CompanyID INT NOT NULL,
        CompanyRole NVARCHAR(50) NOT NULL DEFAULT 'Recruiter', -- 'Owner', 'Admin', 'Recruiter', 'HiringManager'
        
        -- Approval when joining via company code
        MembershipStatus NVARCHAR(20) NOT NULL DEFAULT 'Pending', -- 'Pending', 'Approved', 'Rejected'
        ApprovedByUserID INT NULL,
        JoinedAt DATETIMEOFFSET DEFAULT SYSDATETIMEOFFSET() NOT NULL,

        CONSTRAINT FK_EmployerProfiles_UserAccounts FOREIGN KEY (UserID) REFERENCES UserAccounts(UserID) ON DELETE CASCADE,
        CONSTRAINT FK_EmployerProfiles_Companies FOREIGN KEY (CompanyID) REFERENCES Companies(CompanyID),
        CONSTRAINT FK_EmployerProfiles_ApprovedByUser FOREIGN KEY (ApprovedByUserID) REFERENCES UserAccounts(UserID),
        CONSTRAINT CK_EmployerProfiles_Status CHECK (MembershipStatus IN ('Pending', 'Approved', 'Rejected'))
    );

    CREATE NONCLUSTERED INDEX IX_EmployerProfiles_CompanyID ON EmployerProfiles(CompanyID);
END

-- APPLICANT PROFILES, EXPERIENCE & RESUMES
IF OBJECT_ID('dbo.ApplicantProfiles', 'U') IS NULL
BEGIN
    CREATE TABLE ApplicantProfiles (
        ApplicantProfileID INT IDENTITY(1,1) PRIMARY KEY,
        UserID INT NOT NULL UNIQUE,
        Headline NVARCHAR(150) NULL,
        HomeLocation NVARCHAR(150) NULL,
        Bio NVARCHAR(MAX) NULL,
        CreatedAt DATETIMEOFFSET DEFAULT SYSDATETIMEOFFSET() NOT NULL,

        CONSTRAINT FK_ApplicantProfiles_UserAccounts FOREIGN KEY (UserID) REFERENCES UserAccounts(UserID) ON DELETE CASCADE
    );
END

IF OBJECT_ID('dbo.WorkExperiences', 'U') IS NULL
BEGIN
    CREATE TABLE WorkExperiences (
        WorkExperienceID INT IDENTITY(1,1) PRIMARY KEY,
        ApplicantProfileID INT NOT NULL,
        JobTitle NVARCHAR(100) NOT NULL,
        Company NVARCHAR(150) NOT NULL,
        Location NVARCHAR(150) NULL,
        StartDate DATE NOT NULL,
        EndDate DATE NULL,
        IsCurrentRole BIT NOT NULL DEFAULT 0,
        Description NVARCHAR(MAX) NULL,

        CONSTRAINT FK_WorkExperiences_ApplicantProfiles 
            FOREIGN KEY (ApplicantProfileID) REFERENCES ApplicantProfiles(ApplicantProfileID) ON DELETE CASCADE,
        CONSTRAINT CK_WorkExperiences_Dates CHECK (EndDate IS NULL OR EndDate >= StartDate),
        CONSTRAINT CK_WorkExperiences_CurrentRole CHECK (
            (IsCurrentRole = 1 AND EndDate IS NULL) OR (IsCurrentRole = 0)
        )
    );

    CREATE NONCLUSTERED INDEX IX_WorkExperiences_ApplicantProfileID ON WorkExperiences(ApplicantProfileID);
END

IF OBJECT_ID('dbo.Resumes', 'U') IS NULL
BEGIN
    CREATE TABLE Resumes (
        ResumeID INT IDENTITY(1,1) PRIMARY KEY,
        ApplicantProfileID INT NOT NULL,
        OriginalFileName NVARCHAR(255) NOT NULL,
        StoredFileName NVARCHAR(255) NOT NULL,
        FilePath NVARCHAR(500) NOT NULL,
        FileExtension NVARCHAR(10) NOT NULL,
        ContentType NVARCHAR(100) NOT NULL,
        FileSizeInBytes BIGINT NOT NULL,
        IsPrimary BIT NOT NULL DEFAULT 0,
        UploadedAt DATETIMEOFFSET DEFAULT SYSDATETIMEOFFSET() NOT NULL,

        CONSTRAINT FK_Resumes_ApplicantProfiles 
            FOREIGN KEY (ApplicantProfileID) REFERENCES ApplicantProfiles(ApplicantProfileID) ON DELETE CASCADE
    );

    CREATE NONCLUSTERED INDEX IX_Resumes_ApplicantProfileID ON Resumes(ApplicantProfileID);

    -- Enforces strictly ONE primary resume per applicant
    CREATE UNIQUE NONCLUSTERED INDEX UX_Resumes_PrimaryPerApplicant 
    ON Resumes(ApplicantProfileID) 
    WHERE IsPrimary = 1;
END

IF OBJECT_ID('dbo.ApplicantSkills', 'U') IS NULL
BEGIN
    CREATE TABLE ApplicantSkills (
        ApplicantSkillID INT IDENTITY(1,1) PRIMARY KEY,
        ApplicantProfileID INT NOT NULL,
        SkillID INT NOT NULL,
        ProficiencyLevel NVARCHAR(30) NULL, -- 'Beginner', 'Intermediate', 'Advanced', 'Expert'
        YearsOfExperience INT NULL,

        CONSTRAINT FK_ApplicantSkills_ApplicantProfiles 
            FOREIGN KEY (ApplicantProfileID) REFERENCES ApplicantProfiles(ApplicantProfileID) ON DELETE CASCADE,
        CONSTRAINT FK_ApplicantSkills_Skills 
            FOREIGN KEY (SkillID) REFERENCES Skills(SkillID) ON DELETE CASCADE,
        CONSTRAINT CK_ApplicantSkills_Proficiency 
            CHECK (ProficiencyLevel IS NULL OR ProficiencyLevel IN ('Beginner', 'Intermediate', 'Advanced', 'Expert')),
        CONSTRAINT UQ_Applicant_Skill UNIQUE (ApplicantProfileID, SkillID)
    );

    CREATE NONCLUSTERED INDEX IX_ApplicantSkills_SkillID ON ApplicantSkills(SkillID);
END

-- JOB POSTINGS & REQUIRED SKILLS
IF OBJECT_ID('dbo.JobPostings', 'U') IS NULL
BEGIN
    CREATE TABLE JobPostings (
        JobPostingID INT IDENTITY(1,1) PRIMARY KEY,
        CompanyID INT NOT NULL,
        CreatedByUserID INT NOT NULL,
        
        -- Fields are NULLABLE to allow saving drafts
        ClassificationID INT NULL,
        SubClassificationID INT NULL,
        Title NVARCHAR(150) NULL,
        RequirementsMarkdown NVARCHAR(MAX) NULL,
        JobType NVARCHAR(50) NULL,
        WorkEnvironment NVARCHAR(50) NULL,
        Location NVARCHAR(150) NULL,
        MinSalary DECIMAL(12,2) NULL,
        MaxSalary DECIMAL(12,2) NULL,
        
        -- Internal Company Approval Workflow
        ApprovalStatus NVARCHAR(20) NOT NULL DEFAULT 'Draft', -- 'Draft', 'PendingApproval', 'Published', 'Rejected'
        ApprovedByUserID INT NULL,
        ApprovedAt DATETIMEOFFSET NULL,
        
        IsActive BIT NOT NULL DEFAULT 1,
        PostedAt DATETIMEOFFSET DEFAULT SYSDATETIMEOFFSET() NOT NULL,
        UpdatedAt DATETIMEOFFSET DEFAULT SYSDATETIMEOFFSET() NOT NULL,

        CONSTRAINT FK_JobPostings_Companies FOREIGN KEY (CompanyID) REFERENCES Companies(CompanyID),
        CONSTRAINT FK_JobPostings_CreatedByUser FOREIGN KEY (CreatedByUserID) REFERENCES UserAccounts(UserID),
        CONSTRAINT FK_JobPostings_ApprovedByUser FOREIGN KEY (ApprovedByUserID) REFERENCES UserAccounts(UserID),
        CONSTRAINT FK_JobPostings_Classification FOREIGN KEY (ClassificationID) REFERENCES JobClassifications(ClassificationID),
        CONSTRAINT FK_JobPostings_SubClassification FOREIGN KEY (SubClassificationID) REFERENCES JobSubClassifications(SubClassificationID),
        CONSTRAINT CK_JobPostings_ApprovalStatus CHECK (ApprovalStatus IN ('Draft', 'PendingApproval', 'Published', 'Rejected')),
        CONSTRAINT CK_JobPostings_JobType CHECK (JobType IS NULL OR JobType IN ('Full Time', 'Part Time', 'Contractual/Temporary')),
        CONSTRAINT CK_JobPostings_WorkEnvironment CHECK (WorkEnvironment IS NULL OR WorkEnvironment IN ('On-site', 'Hybrid', 'Remote')),
        CONSTRAINT CK_JobPostings_SalaryRange CHECK (MaxSalary IS NULL OR MinSalary IS NULL OR MaxSalary >= MinSalary)
    );

    CREATE NONCLUSTERED INDEX IX_JobPostings_CompanyID ON JobPostings(CompanyID);
    CREATE NONCLUSTERED INDEX IX_JobPostings_ClassificationID ON JobPostings(ClassificationID);
    CREATE NONCLUSTERED INDEX IX_JobPostings_SubClassificationID ON JobPostings(SubClassificationID);
END

IF OBJECT_ID('dbo.JobPostingSkills', 'U') IS NULL
BEGIN
    CREATE TABLE JobPostingSkills (
        JobPostingSkillID INT IDENTITY(1,1) PRIMARY KEY,
        JobPostingID INT NOT NULL,
        SkillID INT NOT NULL,
        IsRequired BIT NOT NULL DEFAULT 1,

        CONSTRAINT FK_JobPostingSkills_JobPostings 
            FOREIGN KEY (JobPostingID) REFERENCES JobPostings(JobPostingID) ON DELETE CASCADE,
        CONSTRAINT FK_JobPostingSkills_Skills 
            FOREIGN KEY (SkillID) REFERENCES Skills(SkillID) ON DELETE CASCADE,
        CONSTRAINT UQ_JobPosting_Skill UNIQUE (JobPostingID, SkillID)
    );

    CREATE NONCLUSTERED INDEX IX_JobPostingSkills_SkillID ON JobPostingSkills(SkillID);
END

-- APPLICATIONS & ATS WORKFLOW
IF OBJECT_ID('dbo.Applications', 'U') IS NULL
BEGIN
    CREATE TABLE Applications (
        ApplicationID INT IDENTITY(1,1) PRIMARY KEY,
        JobPostingID INT NOT NULL,
        ApplicantProfileID INT NOT NULL,
        ResumeID INT NULL, -- NULL while draft, required on submission
        
        -- Candidate Application Draft State
        IsSubmitted BIT NOT NULL DEFAULT 0,
        DraftStep INT NOT NULL DEFAULT 1,
        CoverLetter NVARCHAR(MAX) NULL,
        
        -- ATS Status (Includes 'Draft' state)
        Status NVARCHAR(30) NOT NULL DEFAULT 'Draft', 
        SubmittedAt DATETIMEOFFSET NULL,
        UpdatedAt DATETIMEOFFSET DEFAULT SYSDATETIMEOFFSET() NOT NULL,

        CONSTRAINT FK_Applications_JobPostings FOREIGN KEY (JobPostingID) REFERENCES JobPostings(JobPostingID),
        CONSTRAINT FK_Applications_ApplicantProfiles FOREIGN KEY (ApplicantProfileID) REFERENCES ApplicantProfiles(ApplicantProfileID),
        CONSTRAINT FK_Applications_Resumes FOREIGN KEY (ResumeID) REFERENCES Resumes(ResumeID),
        CONSTRAINT CK_Applications_Status CHECK (Status IN ('Draft', 'Applied', 'Screening', 'Interviewing', 'Hired', 'Rejected')),
        CONSTRAINT UQ_Applicant_JobPosting UNIQUE (JobPostingID, ApplicantProfileID)
    );

    CREATE NONCLUSTERED INDEX IX_Applications_JobPostingID ON Applications(JobPostingID);
    CREATE NONCLUSTERED INDEX IX_Applications_ApplicantProfileID ON Applications(ApplicantProfileID);
END

IF OBJECT_ID('dbo.ApplicationStatusLogs', 'U') IS NULL
BEGIN
    CREATE TABLE ApplicationStatusLogs (
        StatusLogID INT IDENTITY(1,1) PRIMARY KEY,
        ApplicationID INT NOT NULL,
        OldStatus NVARCHAR(30) NULL,
        NewStatus NVARCHAR(30) NOT NULL,
        ChangedByUserID INT NOT NULL,
        Notes NVARCHAR(MAX) NULL,
        ChangedAt DATETIMEOFFSET DEFAULT SYSDATETIMEOFFSET() NOT NULL,

        CONSTRAINT FK_StatusLogs_Applications FOREIGN KEY (ApplicationID) REFERENCES Applications(ApplicationID) ON DELETE CASCADE,
        CONSTRAINT FK_StatusLogs_UserAccounts FOREIGN KEY (ChangedByUserID) REFERENCES UserAccounts(UserID)
    );

    CREATE NONCLUSTERED INDEX IX_ApplicationStatusLogs_ApplicationID ON ApplicationStatusLogs(ApplicationID);
END
