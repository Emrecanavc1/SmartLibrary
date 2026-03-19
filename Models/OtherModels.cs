using System;

namespace SmartLibrary.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }

        public User()
        {
            Username = ""; PasswordHash = ""; FullName = "";
            Role = "Librarian"; Email = ""; IsActive = true;
        }
    }

    public class Category
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }

        public Category() { CategoryName = ""; Description = ""; }
        public override string ToString() { return CategoryName; }
    }

    public class DashboardStats
    {
        public int TotalBooks { get; set; }
        public int TotalMembers { get; set; }
        public int ActiveBorrows { get; set; }
        public int OverdueBooks { get; set; }
    }

    public class FineRecord
    {
        public int FineID { get; set; }
        public string MemberName { get; set; }
        public string BookTitle { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; }
        public DateTime FineDate { get; set; }

        public FineRecord() { MemberName = ""; BookTitle = ""; Reason = ""; }
    }
}
