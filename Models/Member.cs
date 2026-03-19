using System;

namespace SmartLibrary.Models
{
    public class Member
    {
        public int MemberID { get; set; }
        public string StudentNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Department { get; set; }
        public DateTime MembershipDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int MaxBooks { get; set; }
        public bool IsActive { get; set; }

        public Member()
        {
            StudentNumber = ""; FirstName = ""; LastName = "";
            Email = ""; Phone = ""; Department = "";
            MembershipDate = DateTime.Now; MaxBooks = 3; IsActive = true;
        }

        public string FullName { get { return FirstName + " " + LastName; } }
        public override string ToString() { return StudentNumber + " - " + FullName; }
    }
}
