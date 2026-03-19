-- ============================================
-- SMART LIBRARY DATABASE
-- University Library Management System
-- ============================================

CREATE DATABASE SmartLibraryDB;
GO

USE SmartLibraryDB;
GO

-- ============================================
-- 1. USERS (System Login)
-- ============================================
CREATE TABLE Users (
    UserID          INT IDENTITY(1,1) PRIMARY KEY,
    Username        NVARCHAR(50)  NOT NULL UNIQUE,
    PasswordHash    NVARCHAR(256) NOT NULL,
    FullName        NVARCHAR(100) NOT NULL,
    Role            NVARCHAR(20)  NOT NULL DEFAULT 'Librarian',  -- Admin, Librarian
    Email           NVARCHAR(100),
    IsActive        BIT NOT NULL DEFAULT 1,
    CreatedDate     DATETIME NOT NULL DEFAULT GETDATE()
);

-- ============================================
-- 2. CATEGORIES
-- ============================================
CREATE TABLE Categories (
    CategoryID      INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName    NVARCHAR(100) NOT NULL UNIQUE,
    Description     NVARCHAR(255)
);

-- ============================================
-- 3. BOOKS
-- ============================================
CREATE TABLE Books (
    BookID          INT IDENTITY(1,1) PRIMARY KEY,
    ISBN            NVARCHAR(20)  UNIQUE,
    Title           NVARCHAR(200) NOT NULL,
    Author          NVARCHAR(150) NOT NULL,
    Publisher       NVARCHAR(150),
    PublishYear     INT,
    CategoryID      INT FOREIGN KEY REFERENCES Categories(CategoryID),
    PageCount       INT,
    ShelfLocation   NVARCHAR(50),
    TotalCopies     INT NOT NULL DEFAULT 1,
    AvailableCopies INT NOT NULL DEFAULT 1,
    CoverImage      NVARCHAR(500),
    AddedDate       DATETIME NOT NULL DEFAULT GETDATE(),
    IsActive        BIT NOT NULL DEFAULT 1
);

-- ============================================
-- 4. MEMBERS
-- ============================================
CREATE TABLE Members (
    MemberID        INT IDENTITY(1,1) PRIMARY KEY,
    StudentNumber   NVARCHAR(20)  UNIQUE,
    FirstName       NVARCHAR(50)  NOT NULL,
    LastName        NVARCHAR(50)  NOT NULL,
    Email           NVARCHAR(100),
    Phone           NVARCHAR(20),
    Department      NVARCHAR(100),
    MembershipDate  DATETIME NOT NULL DEFAULT GETDATE(),
    ExpiryDate      DATETIME,
    MaxBooks        INT NOT NULL DEFAULT 3,
    IsActive        BIT NOT NULL DEFAULT 1
);

-- ============================================
-- 5. BORROW RECORDS
-- ============================================
CREATE TABLE BorrowRecords (
    RecordID        INT IDENTITY(1,1) PRIMARY KEY,
    BookID          INT NOT NULL FOREIGN KEY REFERENCES Books(BookID),
    MemberID        INT NOT NULL FOREIGN KEY REFERENCES Members(MemberID),
    BorrowDate      DATETIME NOT NULL DEFAULT GETDATE(),
    DueDate         DATETIME NOT NULL,
    ReturnDate      DATETIME NULL,
    Status          NVARCHAR(20) NOT NULL DEFAULT 'Borrowed', -- Borrowed, Returned, Overdue
    Fine            DECIMAL(10,2) DEFAULT 0,
    Notes           NVARCHAR(500),
    ProcessedBy     INT FOREIGN KEY REFERENCES Users(UserID)
);

-- ============================================
-- 6. FINES / LATE FEES
-- ============================================
CREATE TABLE Fines (
    FineID          INT IDENTITY(1,1) PRIMARY KEY,
    RecordID        INT NOT NULL FOREIGN KEY REFERENCES BorrowRecords(RecordID),
    MemberID        INT NOT NULL FOREIGN KEY REFERENCES Members(MemberID),
    Amount          DECIMAL(10,2) NOT NULL,
    Reason          NVARCHAR(200),
    IsPaid          BIT NOT NULL DEFAULT 0,
    FineDate        DATETIME NOT NULL DEFAULT GETDATE(),
    PaidDate        DATETIME NULL
);

-- ============================================
-- INDEXES (Performance)
-- ============================================
CREATE INDEX IX_Books_Title ON Books(Title);
CREATE INDEX IX_Books_Author ON Books(Author);
CREATE INDEX IX_Books_ISBN ON Books(ISBN);
CREATE INDEX IX_Members_StudentNumber ON Members(StudentNumber);
CREATE INDEX IX_Members_Name ON Members(LastName, FirstName);
CREATE INDEX IX_BorrowRecords_Status ON BorrowRecords(Status);
CREATE INDEX IX_BorrowRecords_Dates ON BorrowRecords(BorrowDate, DueDate, ReturnDate);

-- ============================================
-- SAMPLE DATA
-- ============================================

-- Default Admin (Password: admin123)
INSERT INTO Users (Username, PasswordHash, FullName, Role, Email)
VALUES ('admin', 'admin123', 'System Admin', 'Admin', 'admin@smartlibrary.com');

INSERT INTO Users (Username, PasswordHash, FullName, Role, Email)
VALUES ('librarian', 'lib123', 'Ayşe Yılmaz', 'Librarian', 'ayse@smartlibrary.com');

-- Categories
INSERT INTO Categories (CategoryName, Description) VALUES
(N'Computer Science',  N'Programming, algorithms, AI'),
(N'Mathematics',          N'Analysis, algebra, statistics'),
(N'Physics',              N'Mechanics, electromagnetics, quantum'),
(N'Literature',           N'Novels, poetry, stories'),
(N'History',              N'World history, civilizations'),
(N'Engineering',        N'Mechanical, electrical, civil'),
(N'Economics',            N'Micro, macro economics, finance'),
(N'Psychology',          N'Clinical, social, developmental'),
(N'Philosophy',            N'Logic, ethics, epistemology'),
(N'Medicine',               N'Anatomy, physiology, pharmacology');

-- Books
INSERT INTO Books (ISBN, Title, Author, Publisher, PublishYear, CategoryID, PageCount, ShelfLocation, TotalCopies, AvailableCopies) VALUES
('978-0132350884', N'Clean Code',                    N'Robert C. Martin',    N'Prentice Hall',  2008, 1, 464, 'A-101', 3, 3),
('978-0201633610', N'Design Patterns',               N'Gang of Four',        N'Addison-Wesley', 1994, 1, 395, 'A-102', 2, 2),
('978-0596007126', N'Head First Design Patterns',     N'Eric Freeman',        N'O''Reilly',      2004, 1, 694, 'A-103', 2, 2),
('978-0134685991', N'Effective Java',                 N'Joshua Bloch',        N'Addison-Wesley', 2018, 1, 416, 'A-104', 2, 2),
('978-0262033848', N'Introduction to Algorithms',     N'Thomas H. Cormen',    N'MIT Press',      2009, 1, 1312,'A-105', 3, 3),
('978-0321125217', N'Domain-Driven Design',           N'Eric Evans',          N'Addison-Wesley', 2003, 1, 560, 'A-106', 1, 1),
('978-9750719387', N'Suç ve Ceza',                   N'Fyodor Dostoyevski',  N'İş Bankası',     2020, 4, 687, 'D-101', 4, 4),
('978-9750726439', N'Sefiller',                      N'Victor Hugo',         N'İş Bankası',     2021, 4, 1432,'D-102', 3, 3),
('978-9750738609', N'1984',                          N'George Orwell',       N'Can Yayınları',   2021, 4, 352, 'D-103', 5, 5),
('978-0521676519', N'Probability and Statistics',     N'Morris DeGroot',      N'Pearson',        2012, 2, 816, 'B-101', 2, 2),
('978-0134093413', N'Campbell Biology',               N'Lisa Urry',           N'Pearson',        2016, 10, 1488,'J-101', 2, 2),
('978-0393603101', N'Western Civilization',           N'Joshua Cole',         N'W.W. Norton',    2019, 5, 1100,'E-101', 1, 1);

-- Members
INSERT INTO Members (StudentNumber, FirstName, LastName, Email, Phone, Department, ExpiryDate, MaxBooks) VALUES
('2024001', N'Ahmet',    N'Kaya',     'ahmet.kaya@uni.edu.tr',    '0532-111-2233', N'Computer Engineering', '2026-09-01', 5),
('2024002', N'Elif',     N'Demir',    'elif.demir@uni.edu.tr',    '0533-222-3344', N'Electrical Engineering',   '2026-09-01', 5),
('2024003', N'Mehmet',   N'Çelik',    'mehmet.celik@uni.edu.tr',  '0534-333-4455', N'Business',                 '2026-09-01', 3),
('2024004', N'Zeynep',   N'Arslan',   'zeynep.arslan@uni.edu.tr', '0535-444-5566', N'Medicine',                     '2026-09-01', 5),
('2024005', N'Can',      N'Yıldız',   'can.yildiz@uni.edu.tr',    '0536-555-6677', N'Law',                   '2026-09-01', 3),
('2024006', N'Selin',    N'Öztürk',   'selin.ozturk@uni.edu.tr',  '0537-666-7788', N'Psychology',               '2026-09-01', 3),
('2024007', N'Burak',    N'Şahin',    'burak.sahin@uni.edu.tr',   '0538-777-8899', N'Physics',                   '2026-09-01', 5),
('2024008', N'Ayşe',     N'Koç',      'ayse.koc@uni.edu.tr',      '0539-888-9900', N'Mathematics',               '2026-09-01', 3);

-- Borrow Records (Sample)
INSERT INTO BorrowRecords (BookID, MemberID, BorrowDate, DueDate, ReturnDate, Status, ProcessedBy) VALUES
(1, 1, '2025-03-01', '2025-03-15', '2025-03-14', 'Returned', 2),
(7, 3, '2025-03-05', '2025-03-19', NULL,          'Borrowed', 2),
(9, 1, '2025-03-10', '2025-03-24', NULL,          'Borrowed', 2),
(5, 2, '2025-02-20', '2025-03-06', '2025-03-08',  'Returned', 1),
(3, 4, '2025-03-12', '2025-03-26', NULL,          'Borrowed', 2);

-- Stored Procedures
GO

-- Search Books
CREATE PROCEDURE sp_SearchBooks
    @SearchTerm NVARCHAR(200)
AS
BEGIN
    SELECT b.*, c.CategoryName
    FROM Books b
    LEFT JOIN Categories c ON b.CategoryID = c.CategoryID
    WHERE b.IsActive = 1
      AND (b.Title    LIKE '%' + @SearchTerm + '%'
        OR b.Author   LIKE '%' + @SearchTerm + '%'
        OR b.ISBN     LIKE '%' + @SearchTerm + '%')
    ORDER BY b.Title;
END
GO

-- Dashboard Statistics
CREATE PROCEDURE sp_GetDashboardStats
AS
BEGIN
    SELECT
        (SELECT COUNT(*) FROM Books WHERE IsActive = 1) AS TotalBooks,
        (SELECT COUNT(*) FROM Members WHERE IsActive = 1) AS TotalMembers,
        (SELECT COUNT(*) FROM BorrowRecords WHERE Status = 'Borrowed') AS ActiveBorrows,
        (SELECT COUNT(*) FROM BorrowRecords WHERE Status = 'Borrowed' AND DueDate < GETDATE()) AS OverdueBooks,
        (SELECT SUM(Amount) FROM Fines WHERE IsPaid = 0) AS UnpaidFines;
END
GO

-- Borrow Book
CREATE PROCEDURE sp_BorrowBook
    @BookID INT,
    @MemberID INT,
    @DueDate DATETIME,
    @ProcessedBy INT
AS
BEGIN
    BEGIN TRANSACTION;
    BEGIN TRY
        -- Check book availability
        IF (SELECT AvailableCopies FROM Books WHERE BookID = @BookID) <= 0
        BEGIN
            RAISERROR('Bu kitabın müsait kopyası bulunmamaktadır.', 16, 1);
            RETURN;
        END

        -- Check member limit
        DECLARE @CurrentBorrows INT, @MaxBooks INT;
        SELECT @MaxBooks = MaxBooks FROM Members WHERE MemberID = @MemberID;
        SELECT @CurrentBorrows = COUNT(*) FROM BorrowRecords
            WHERE MemberID = @MemberID AND Status = 'Borrowed';

        IF @CurrentBorrows >= @MaxBooks
        BEGIN
            RAISERROR('Üye maksimum kitap limitine ulaşmıştır.', 16, 1);
            RETURN;
        END

        -- Create borrow record
        INSERT INTO BorrowRecords (BookID, MemberID, DueDate, ProcessedBy)
        VALUES (@BookID, @MemberID, @DueDate, @ProcessedBy);

        -- Update book stock
        UPDATE Books SET AvailableCopies = AvailableCopies - 1
        WHERE BookID = @BookID;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- Return Book
CREATE PROCEDURE sp_ReturnBook
    @RecordID INT
AS
BEGIN
    BEGIN TRANSACTION;
    BEGIN TRY
        DECLARE @BookID INT, @MemberID INT, @DueDate DATETIME;

        SELECT @BookID = BookID, @MemberID = MemberID, @DueDate = DueDate
        FROM BorrowRecords WHERE RecordID = @RecordID;

        -- Update record
        UPDATE BorrowRecords
        SET ReturnDate = GETDATE(),
            Status = 'Returned'
        WHERE RecordID = @RecordID;

        -- Update book stock
        UPDATE Books SET AvailableCopies = AvailableCopies + 1
        WHERE BookID = @BookID;

        -- Check late fee
        IF GETDATE() > @DueDate
        BEGIN
            DECLARE @DaysLate INT = DATEDIFF(DAY, @DueDate, GETDATE());
            DECLARE @FineAmount DECIMAL(10,2) = @DaysLate * 2.00; -- Günlük 2 TL

            INSERT INTO Fines (RecordID, MemberID, Amount, Reason)
            VALUES (@RecordID, @MemberID, @FineAmount,
                    CAST(@DaysLate AS NVARCHAR) + N' days late');

            UPDATE BorrowRecords SET Fine = @FineAmount
            WHERE RecordID = @RecordID;
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

PRINT '=== SmartLibrary Database Created Successfully ===';
