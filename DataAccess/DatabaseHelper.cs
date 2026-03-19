using System;
using System.Data.SqlClient;

namespace SmartLibrary.DataAccess
{
    public static class DatabaseHelper
    {
        // LocalDB - comes with Visual Studio, no extra install needed
        private static string _connectionString =
            @"Server=(localdb)\MSSQLLocalDB;Database=SmartLibraryDB;Trusted_Connection=True;";

        private static string _masterConnection =
            @"Server=(localdb)\MSSQLLocalDB;Database=master;Trusted_Connection=True;";

        public static string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public static bool TestConnection(out string errorMessage)
        {
            errorMessage = "";
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Checks if database exists, creates it if not. Call this at app startup.
        /// </summary>
        public static void EnsureDatabase()
        {
            if (!DatabaseExists())
            {
                CreateDatabase();
            }
        }

        private static bool DatabaseExists()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_masterConnection))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(
                        "SELECT COUNT(*) FROM sys.databases WHERE name = 'SmartLibraryDB'", conn))
                    {
                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        private static void CreateDatabase()
        {
            // Step 1: Create the database
            using (SqlConnection conn = new SqlConnection(_masterConnection))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("CREATE DATABASE SmartLibraryDB", conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }

            // Step 2: Create tables and seed data
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                string[] commands = GetCreateScript().Split(new string[] { "\nGO\n", "\nGO\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string sql in commands)
                {
                    string trimmed = sql.Trim();
                    if (string.IsNullOrEmpty(trimmed) || trimmed == "GO") continue;
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(trimmed, conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("SQL Error: " + ex.Message);
                    }
                }
            }
        }

        private static string GetCreateScript()
        {
            return @"
CREATE TABLE Users (
    UserID          INT IDENTITY(1,1) PRIMARY KEY,
    Username        NVARCHAR(50)  NOT NULL UNIQUE,
    PasswordHash    NVARCHAR(256) NOT NULL,
    FullName        NVARCHAR(100) NOT NULL,
    Role            NVARCHAR(20)  NOT NULL DEFAULT 'Librarian',
    Email           NVARCHAR(100),
    IsActive        BIT NOT NULL DEFAULT 1,
    CreatedDate     DATETIME NOT NULL DEFAULT GETDATE()
);
GO
CREATE TABLE Categories (
    CategoryID      INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName    NVARCHAR(100) NOT NULL UNIQUE,
    Description     NVARCHAR(255)
);
GO
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
GO
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
GO
CREATE TABLE BorrowRecords (
    RecordID        INT IDENTITY(1,1) PRIMARY KEY,
    BookID          INT NOT NULL FOREIGN KEY REFERENCES Books(BookID),
    MemberID        INT NOT NULL FOREIGN KEY REFERENCES Members(MemberID),
    BorrowDate      DATETIME NOT NULL DEFAULT GETDATE(),
    DueDate         DATETIME NOT NULL,
    ReturnDate      DATETIME NULL,
    Status          NVARCHAR(20) NOT NULL DEFAULT 'Borrowed',
    Fine            DECIMAL(10,2) DEFAULT 0,
    Notes           NVARCHAR(500),
    ProcessedBy     INT FOREIGN KEY REFERENCES Users(UserID)
);
GO
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
GO
CREATE INDEX IX_Books_Title ON Books(Title);
GO
CREATE INDEX IX_Books_Author ON Books(Author);
GO
CREATE INDEX IX_Members_StudentNumber ON Members(StudentNumber);
GO
CREATE INDEX IX_BorrowRecords_Status ON BorrowRecords(Status);
GO
INSERT INTO Users (Username, PasswordHash, FullName, Role, Email)
VALUES ('admin', 'admin123', 'System Admin', 'Admin', 'admin@smartlibrary.com');
GO
INSERT INTO Users (Username, PasswordHash, FullName, Role, Email)
VALUES ('librarian', 'lib123', 'Jane Smith', 'Librarian', 'jane@smartlibrary.com');
GO
INSERT INTO Categories (CategoryName, Description) VALUES
('Computer Science', 'Programming, algorithms, AI');
GO
INSERT INTO Categories (CategoryName, Description) VALUES
('Mathematics', 'Analysis, algebra, statistics');
GO
INSERT INTO Categories (CategoryName, Description) VALUES
('Physics', 'Mechanics, electromagnetics, quantum');
GO
INSERT INTO Categories (CategoryName, Description) VALUES
('Literature', 'Novels, poetry, stories');
GO
INSERT INTO Categories (CategoryName, Description) VALUES
('History', 'World history, civilizations');
GO
INSERT INTO Categories (CategoryName, Description) VALUES
('Engineering', 'Mechanical, electrical, civil');
GO
INSERT INTO Categories (CategoryName, Description) VALUES
('Economics', 'Micro, macro economics, finance');
GO
INSERT INTO Categories (CategoryName, Description) VALUES
('Psychology', 'Clinical, social, developmental');
GO
INSERT INTO Categories (CategoryName, Description) VALUES
('Philosophy', 'Logic, ethics, epistemology');
GO
INSERT INTO Categories (CategoryName, Description) VALUES
('Medicine', 'Anatomy, physiology, pharmacology');
GO
INSERT INTO Books (ISBN, Title, Author, Publisher, PublishYear, CategoryID, PageCount, ShelfLocation, TotalCopies, AvailableCopies) VALUES
('978-0132350884', 'Clean Code', 'Robert C. Martin', 'Prentice Hall', 2008, 1, 464, 'A-101', 3, 3);
GO
INSERT INTO Books (ISBN, Title, Author, Publisher, PublishYear, CategoryID, PageCount, ShelfLocation, TotalCopies, AvailableCopies) VALUES
('978-0201633610', 'Design Patterns', 'Gang of Four', 'Addison-Wesley', 1994, 1, 395, 'A-102', 2, 2);
GO
INSERT INTO Books (ISBN, Title, Author, Publisher, PublishYear, CategoryID, PageCount, ShelfLocation, TotalCopies, AvailableCopies) VALUES
('978-0596007126', 'Head First Design Patterns', 'Eric Freeman', 'OReilly', 2004, 1, 694, 'A-103', 2, 2);
GO
INSERT INTO Books (ISBN, Title, Author, Publisher, PublishYear, CategoryID, PageCount, ShelfLocation, TotalCopies, AvailableCopies) VALUES
('978-0134685991', 'Effective Java', 'Joshua Bloch', 'Addison-Wesley', 2018, 1, 416, 'A-104', 2, 2);
GO
INSERT INTO Books (ISBN, Title, Author, Publisher, PublishYear, CategoryID, PageCount, ShelfLocation, TotalCopies, AvailableCopies) VALUES
('978-0262033848', 'Introduction to Algorithms', 'Thomas H. Cormen', 'MIT Press', 2009, 1, 1312, 'A-105', 3, 3);
GO
INSERT INTO Books (ISBN, Title, Author, Publisher, PublishYear, CategoryID, PageCount, ShelfLocation, TotalCopies, AvailableCopies) VALUES
('978-9750719387', 'Crime and Punishment', 'Fyodor Dostoevsky', 'Penguin', 2003, 4, 687, 'D-101', 4, 4);
GO
INSERT INTO Books (ISBN, Title, Author, Publisher, PublishYear, CategoryID, PageCount, ShelfLocation, TotalCopies, AvailableCopies) VALUES
('978-9750738609', '1984', 'George Orwell', 'Penguin', 2021, 4, 352, 'D-103', 5, 5);
GO
INSERT INTO Books (ISBN, Title, Author, Publisher, PublishYear, CategoryID, PageCount, ShelfLocation, TotalCopies, AvailableCopies) VALUES
('978-0521676519', 'Probability and Statistics', 'Morris DeGroot', 'Pearson', 2012, 2, 816, 'B-101', 2, 2);
GO
INSERT INTO Members (StudentNumber, FirstName, LastName, Email, Phone, Department, ExpiryDate, MaxBooks) VALUES
('2024001', 'John', 'Davis', 'john.davis@uni.edu', '555-111-2233', 'Computer Engineering', '2026-09-01', 5);
GO
INSERT INTO Members (StudentNumber, FirstName, LastName, Email, Phone, Department, ExpiryDate, MaxBooks) VALUES
('2024002', 'Emily', 'Wilson', 'emily.wilson@uni.edu', '555-222-3344', 'Electrical Engineering', '2026-09-01', 5);
GO
INSERT INTO Members (StudentNumber, FirstName, LastName, Email, Phone, Department, ExpiryDate, MaxBooks) VALUES
('2024003', 'Michael', 'Brown', 'michael.brown@uni.edu', '555-333-4455', 'Business', '2026-09-01', 3);
GO
INSERT INTO Members (StudentNumber, FirstName, LastName, Email, Phone, Department, ExpiryDate, MaxBooks) VALUES
('2024004', 'Sarah', 'Johnson', 'sarah.johnson@uni.edu', '555-444-5566', 'Medicine', '2026-09-01', 5);
GO
INSERT INTO Members (StudentNumber, FirstName, LastName, Email, Phone, Department, ExpiryDate, MaxBooks) VALUES
('2024005', 'David', 'Miller', 'david.miller@uni.edu', '555-555-6677', 'Law', '2026-09-01', 3);
GO
INSERT INTO BorrowRecords (BookID, MemberID, BorrowDate, DueDate, ReturnDate, Status, ProcessedBy) VALUES
(1, 1, '2025-03-01', '2025-03-15', '2025-03-14', 'Returned', 2);
GO
INSERT INTO BorrowRecords (BookID, MemberID, BorrowDate, DueDate, ReturnDate, Status, ProcessedBy) VALUES
(6, 3, '2025-03-05', '2025-03-19', NULL, 'Borrowed', 2);
GO
INSERT INTO BorrowRecords (BookID, MemberID, BorrowDate, DueDate, ReturnDate, Status, ProcessedBy) VALUES
(7, 1, '2025-03-10', '2025-03-24', NULL, 'Borrowed', 2);
GO
INSERT INTO BorrowRecords (BookID, MemberID, BorrowDate, DueDate, ReturnDate, Status, ProcessedBy) VALUES
(5, 2, '2025-02-20', '2025-03-06', '2025-03-08', 'Returned', 1);
GO
INSERT INTO BorrowRecords (BookID, MemberID, BorrowDate, DueDate, ReturnDate, Status, ProcessedBy) VALUES
(3, 4, '2025-03-12', '2025-03-26', NULL, 'Borrowed', 2);
GO
CREATE PROCEDURE sp_SearchBooks
    @SearchTerm NVARCHAR(200)
AS
BEGIN
    SELECT b.*, c.CategoryName
    FROM Books b
    LEFT JOIN Categories c ON b.CategoryID = c.CategoryID
    WHERE b.IsActive = 1
      AND (b.Title LIKE '%' + @SearchTerm + '%'
        OR b.Author LIKE '%' + @SearchTerm + '%'
        OR b.ISBN LIKE '%' + @SearchTerm + '%')
    ORDER BY b.Title;
END
GO
CREATE PROCEDURE sp_GetDashboardStats
AS
BEGIN
    SELECT
        (SELECT COUNT(*) FROM Books WHERE IsActive = 1) AS TotalBooks,
        (SELECT COUNT(*) FROM Members WHERE IsActive = 1) AS TotalMembers,
        (SELECT COUNT(*) FROM BorrowRecords WHERE Status = 'Borrowed') AS ActiveBorrows,
        (SELECT COUNT(*) FROM BorrowRecords WHERE Status = 'Borrowed' AND DueDate < GETDATE()) AS OverdueBooks;
END
GO
CREATE PROCEDURE sp_BorrowBook
    @BookID INT,
    @MemberID INT,
    @DueDate DATETIME,
    @ProcessedBy INT
AS
BEGIN
    BEGIN TRANSACTION;
    BEGIN TRY
        IF (SELECT AvailableCopies FROM Books WHERE BookID = @BookID) <= 0
        BEGIN
            RAISERROR('No available copies of this book.', 16, 1);
            RETURN;
        END

        DECLARE @CurrentBorrows INT, @MaxBooks INT;
        SELECT @MaxBooks = MaxBooks FROM Members WHERE MemberID = @MemberID;
        SELECT @CurrentBorrows = COUNT(*) FROM BorrowRecords
            WHERE MemberID = @MemberID AND Status = 'Borrowed';

        IF @CurrentBorrows >= @MaxBooks
        BEGIN
            RAISERROR('Member has reached maximum book limit.', 16, 1);
            RETURN;
        END

        INSERT INTO BorrowRecords (BookID, MemberID, DueDate, ProcessedBy)
        VALUES (@BookID, @MemberID, @DueDate, @ProcessedBy);

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
CREATE PROCEDURE sp_ReturnBook
    @RecordID INT
AS
BEGIN
    BEGIN TRANSACTION;
    BEGIN TRY
        DECLARE @BookID INT, @MemberID INT, @DueDate DATETIME;

        SELECT @BookID = BookID, @MemberID = MemberID, @DueDate = DueDate
        FROM BorrowRecords WHERE RecordID = @RecordID;

        UPDATE BorrowRecords
        SET ReturnDate = GETDATE(), Status = 'Returned'
        WHERE RecordID = @RecordID;

        UPDATE Books SET AvailableCopies = AvailableCopies + 1
        WHERE BookID = @BookID;

        IF GETDATE() > @DueDate
        BEGIN
            DECLARE @DaysLate INT = DATEDIFF(DAY, @DueDate, GETDATE());
            DECLARE @FineAmount DECIMAL(10,2) = @DaysLate * 2.00;

            INSERT INTO Fines (RecordID, MemberID, Amount, Reason)
            VALUES (@RecordID, @MemberID, @FineAmount,
                    CAST(@DaysLate AS NVARCHAR) + ' days late');

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
";
        }
    }
}
