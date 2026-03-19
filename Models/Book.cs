using System;

namespace SmartLibrary.Models
{
    public class Book
    {
        public int BookID { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public int PublishYear { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public int PageCount { get; set; }
        public string ShelfLocation { get; set; }
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
        public DateTime AddedDate { get; set; }
        public bool IsActive { get; set; }

        public Book()
        {
            ISBN = ""; Title = ""; Author = ""; Publisher = "";
            CategoryName = ""; ShelfLocation = "";
            TotalCopies = 1; AvailableCopies = 1; IsActive = true;
            AddedDate = DateTime.Now;
        }

        public override string ToString() { return Title + " - " + Author; }
    }
}
