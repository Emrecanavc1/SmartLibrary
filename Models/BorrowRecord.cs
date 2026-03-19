using System;

namespace SmartLibrary.Models
{
    public class BorrowRecord
    {
        public int RecordID { get; set; }
        public int BookID { get; set; }
        public int MemberID { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Status { get; set; }
        public decimal Fine { get; set; }
        public int ProcessedBy { get; set; }
        public string BookTitle { get; set; }
        public string BookAuthor { get; set; }
        public string MemberName { get; set; }
        public string StudentNumber { get; set; }

        public BorrowRecord()
        {
            Status = "Borrowed"; BookTitle = ""; BookAuthor = "";
            MemberName = ""; StudentNumber = "";
        }

        public bool IsOverdue { get { return Status == "Borrowed" && DateTime.Now > DueDate; } }
        public int DaysRemaining { get { return Status == "Borrowed" ? (DueDate - DateTime.Now).Days : 0; } }
    }
}
