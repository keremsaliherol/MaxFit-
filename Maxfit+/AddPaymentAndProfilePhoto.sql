-- Add PhotoUrl column to Members table
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
               WHERE TABLE_NAME = 'Members' AND COLUMN_NAME = 'PhotoUrl')
BEGIN
    ALTER TABLE Members
    ADD PhotoUrl NVARCHAR(MAX) NULL;
    PRINT 'PhotoUrl column added to Members table';
END
ELSE
BEGIN
    PRINT 'PhotoUrl column already exists';
END
GO

-- Create Payments table if not exists
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES 
               WHERE TABLE_NAME = 'Payments')
BEGIN
    CREATE TABLE Payments (
        PaymentId INT PRIMARY KEY IDENTITY(1,1),
        MemberId INT NOT NULL,
        MembershipId INT NOT NULL,
        Amount DECIMAL(10,2) NOT NULL,
        PaymentDate DATETIME2 NOT NULL DEFAULT GETDATE(),
        PaymentMethod NVARCHAR(50) NOT NULL, -- Cash, Credit Card, Bank Transfer, Online
        Status NVARCHAR(50) NOT NULL DEFAULT 'Completed', -- Completed, Pending, Failed, Refunded
        Notes NVARCHAR(500) NULL,
        TransactionId NVARCHAR(100) NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
        
        CONSTRAINT FK_Payments_Members FOREIGN KEY (MemberId) 
            REFERENCES Members(MemberId) ON DELETE CASCADE,
        CONSTRAINT FK_Payments_Memberships FOREIGN KEY (MembershipId) 
            REFERENCES Memberships(MembershipId)
    );
    
    PRINT 'Payments table created successfully';
END
ELSE
BEGIN
    PRINT 'Payments table already exists';
END
GO

-- Fix MembershipType Price column precision
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
           WHERE TABLE_NAME = 'MembershipTypes' AND COLUMN_NAME = 'Price' 
           AND NUMERIC_PRECISION != 10)
BEGIN
    ALTER TABLE MembershipTypes
    ALTER COLUMN Price DECIMAL(10,2) NOT NULL;
    PRINT 'MembershipType Price column precision updated';
END
GO

PRINT 'Database update completed successfully!';
