------------------------------------------------------------
-- 0) MEVCUT TABLOLARI TEMÝZLE (Varsa siler, hata vermez)
------------------------------------------------------------
IF OBJECT_ID('CourseSessionPhotos', 'U') IS NOT NULL DROP TABLE CourseSessionPhotos;
IF OBJECT_ID('MemberNotifications', 'U') IS NOT NULL DROP TABLE MemberNotifications;
IF OBJECT_ID('EquipmentMaintenance', 'U') IS NOT NULL DROP TABLE EquipmentMaintenance;
IF OBJECT_ID('EquipmentOrders', 'U') IS NOT NULL DROP TABLE EquipmentOrders;
IF OBJECT_ID('CheckIns', 'U') IS NOT NULL DROP TABLE CheckIns;
IF OBJECT_ID('CourseRegistrations', 'U') IS NOT NULL DROP TABLE CourseRegistrations;
IF OBJECT_ID('CourseSessions', 'U') IS NOT NULL DROP TABLE CourseSessions;
IF OBJECT_ID('StaffWorkSchedules', 'U') IS NOT NULL DROP TABLE StaffWorkSchedules;
IF OBJECT_ID('UserRoles', 'U') IS NOT NULL DROP TABLE UserRoles;
IF OBJECT_ID('Payments', 'U') IS NOT NULL DROP TABLE Payments;
IF OBJECT_ID('Memberships', 'U') IS NOT NULL DROP TABLE Memberships;
IF OBJECT_ID('Equipment', 'U') IS NOT NULL DROP TABLE Equipment;
IF OBJECT_ID('EquipmentTypes', 'U') IS NOT NULL DROP TABLE EquipmentTypes;
IF OBJECT_ID('Classes', 'U') IS NOT NULL DROP TABLE Classes;
IF OBJECT_ID('Rooms', 'U') IS NOT NULL DROP TABLE Rooms;
IF OBJECT_ID('Staff', 'U') IS NOT NULL DROP TABLE Staff;
IF OBJECT_ID('MembershipTypes', 'U') IS NOT NULL DROP TABLE MembershipTypes;
IF OBJECT_ID('Members', 'U') IS NOT NULL DROP TABLE Members;
IF OBJECT_ID('Roles', 'U') IS NOT NULL DROP TABLE Roles;
IF OBJECT_ID('Users', 'U') IS NOT NULL DROP TABLE Users;
GO

------------------------------------------------------------
-- 1) TEMEL TABLOLAR
------------------------------------------------------------
CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    Email NVARCHAR(100) NULL,
    FullName NVARCHAR(100) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
);

CREATE TABLE Roles (
    RoleId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL UNIQUE,
    Description NVARCHAR(200) NULL
);

CREATE TABLE UserRoles (
    UserRoleId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    RoleId INT NOT NULL,
    CONSTRAINT FK_UserRoles_Users FOREIGN KEY (UserId) REFERENCES Users(UserId),
    CONSTRAINT FK_UserRoles_Roles FOREIGN KEY (RoleId) REFERENCES Roles(RoleId),
    CONSTRAINT UQ_UserRoles_User_Role UNIQUE (UserId, RoleId)
);

CREATE TABLE Members (
    MemberId INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Gender CHAR(1) NULL, -- 'M' / 'F'
    BirthDate DATE NULL,
    Phone NVARCHAR(20) NULL,
    Email NVARCHAR(100) NULL,
    Address NVARCHAR(250) NULL,
    RegisterDate DATETIME NOT NULL DEFAULT GETDATE(),
    IsActive BIT NOT NULL DEFAULT 1
);

------------------------------------------------------------
-- 2) ÜYELÝK VE FÝNANS SÝSTEMÝ
------------------------------------------------------------
CREATE TABLE MembershipTypes (
    MembershipTypeId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(250) NULL,
    DurationInDays INT NOT NULL,
    Price DECIMAL(18,2) NOT NULL DEFAULT 0,
    IsActive BIT NOT NULL DEFAULT 1
);

CREATE TABLE Memberships (
    MembershipId INT IDENTITY(1,1) PRIMARY KEY,
    MemberId INT NOT NULL,
    MembershipTypeId INT NOT NULL,
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    TotalAmount DECIMAL(18,2) NOT NULL, -- Satýþ anýndaki net fiyat
    PaidAmount DECIMAL(18,2) DEFAULT 0,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    CreatedByUserId INT NULL,
    CONSTRAINT FK_Memberships_Members FOREIGN KEY (MemberId) REFERENCES Members(MemberId),
    CONSTRAINT FK_Memberships_MembershipTypes FOREIGN KEY (MembershipTypeId) REFERENCES MembershipTypes(MembershipTypeId),
    CONSTRAINT FK_Memberships_Users FOREIGN KEY (CreatedByUserId) REFERENCES Users(UserId)
);

CREATE TABLE Payments (
    PaymentId INT IDENTITY(1,1) PRIMARY KEY,
    MembershipId INT NOT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    PaymentDate DATETIME NOT NULL DEFAULT GETDATE(),
    PaymentMethod NVARCHAR(50), -- Nakit, Kredi Kartý vb.
    ProcessedByUserId INT NULL,
    CONSTRAINT FK_Payments_Memberships FOREIGN KEY (MembershipId) REFERENCES Memberships(MembershipId),
    CONSTRAINT FK_Payments_Users FOREIGN KEY (ProcessedByUserId) REFERENCES Users(UserId)
);

------------------------------------------------------------
-- 3) PERSONEL VE PLANLAMA
------------------------------------------------------------
CREATE TABLE Staff (
    StaffId INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Phone NVARCHAR(20) NULL,
    Position NVARCHAR(50) NOT NULL,
    HireDate DATE NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    UserId INT NULL,
    CONSTRAINT FK_Staff_Users FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

CREATE TABLE StaffWorkSchedules (
    StaffWorkScheduleId INT IDENTITY(1,1) PRIMARY KEY,
    StaffId INT NOT NULL,
    DayOfWeek TINYINT NOT NULL, -- 1=Pzt, 7=Paz
    StartTime TIME NOT NULL,
    EndTime TIME NOT NULL,
    CONSTRAINT FK_StaffWorkSchedules_Staff FOREIGN KEY (StaffId) REFERENCES Staff(StaffId)
);

------------------------------------------------------------
-- 4) SALON VE DERS YÖNETÝMÝ
------------------------------------------------------------
CREATE TABLE Rooms (
    RoomId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Capacity INT NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1
);

CREATE TABLE Classes (
    ClassId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    GenderRestriction NVARCHAR(20) NOT NULL, -- Female, Male, Hybrid
    IsActive BIT NOT NULL DEFAULT 1
);

CREATE TABLE CourseSessions (
    CourseSessionId INT IDENTITY(1,1) PRIMARY KEY,
    ClassId INT NOT NULL,
    RoomId INT NOT NULL,
    TrainerId INT NOT NULL,
    StartTime DATETIME NOT NULL,
    EndTime DATETIME NOT NULL,
    Capacity INT NOT NULL,
    IsCanceled BIT NOT NULL DEFAULT 0,
    CONSTRAINT FK_CourseSessions_Classes FOREIGN KEY (ClassId) REFERENCES Classes(ClassId),
    CONSTRAINT FK_CourseSessions_Rooms FOREIGN KEY (RoomId) REFERENCES Rooms(RoomId),
    CONSTRAINT FK_CourseSessions_Staff FOREIGN KEY (TrainerId) REFERENCES Staff(StaffId)
);

CREATE TABLE CourseRegistrations (
    CourseRegistrationId INT IDENTITY(1,1) PRIMARY KEY,
    CourseSessionId INT NOT NULL,
    MemberId INT NOT NULL,
    RegistrationDate DATETIME NOT NULL DEFAULT GETDATE(),
    Status NVARCHAR(20) DEFAULT 'Registered',
    CONSTRAINT FK_CourseRegistrations_CourseSessions FOREIGN KEY (CourseSessionId) REFERENCES CourseSessions(CourseSessionId),
    CONSTRAINT FK_CourseRegistrations_Members FOREIGN KEY (MemberId) REFERENCES Members(MemberId),
    CONSTRAINT UQ_CourseRegistrations UNIQUE (CourseSessionId, MemberId)
);

------------------------------------------------------------
-- 5) EKÝPMAN VE ENVANTER
------------------------------------------------------------
CREATE TABLE EquipmentTypes (
    EquipmentTypeId INT IDENTITY(1,1) PRIMARY KEY,
    TypeName NVARCHAR(100) NOT NULL,
    Category NVARCHAR(50) NULL
);

CREATE TABLE Equipment (
    EquipmentId INT IDENTITY(1,1) PRIMARY KEY,
    EquipmentTypeId INT NOT NULL,
    Brand NVARCHAR(100) NULL,
    SerialNumber NVARCHAR(100) NULL,
    Status NVARCHAR(50) DEFAULT 'Available',
    IsActive BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_Equipment_Types FOREIGN KEY (EquipmentTypeId) REFERENCES EquipmentTypes(EquipmentTypeId)
);

CREATE TABLE EquipmentMaintenance (
    MaintenanceId INT IDENTITY(1,1) PRIMARY KEY,
    EquipmentId INT NOT NULL,
    RecordDate DATE NOT NULL,
    IssueType NVARCHAR(50) NOT NULL,
    IsResolved BIT NOT NULL DEFAULT 0,
    CONSTRAINT FK_EquipmentMaintenance_Equipment FOREIGN KEY (EquipmentId) REFERENCES Equipment(EquipmentId)
);

------------------------------------------------------------
-- 6) TAKÝP VE BÝLDÝRÝM
------------------------------------------------------------
CREATE TABLE CheckIns (
    CheckInId INT IDENTITY(1,1) PRIMARY KEY,
    MemberId INT NOT NULL,
    CheckInTime DATETIME NOT NULL DEFAULT GETDATE(),
    CheckOutTime DATETIME NULL,
    CONSTRAINT FK_CheckIns_Members FOREIGN KEY (MemberId) REFERENCES Members(MemberId)
);

CREATE TABLE MemberNotifications (
    NotificationId INT IDENTITY(1,1) PRIMARY KEY,
    MemberId INT NOT NULL,
    Message NVARCHAR(500) NOT NULL,
    SentAt DATETIME NOT NULL DEFAULT GETDATE(),
    IsRead BIT NOT NULL DEFAULT 0,
    CONSTRAINT FK_MemberNotifications_Members FOREIGN KEY (MemberId) REFERENCES Members(MemberId)
);
-- Eðer tablo varsa önce siler
IF OBJECT_ID('Payments', 'U') IS NOT NULL
    DROP TABLE Payments;
GO

-- Þimdi temiz ve uyumlu tabloyu oluþtur
CREATE TABLE Payments (
    PaymentId INT PRIMARY KEY IDENTITY(1,1),
    MembershipId INT NOT NULL,
    Amount DECIMAL(18, 2) NOT NULL,
    PaymentDate DATETIME NOT NULL DEFAULT GETDATE(),
    PaymentMethod NVARCHAR(50) NULL,
    CONSTRAINT FK_Payments_Memberships FOREIGN KEY (MembershipId) 
    REFERENCES Memberships(MembershipId)
);
GO