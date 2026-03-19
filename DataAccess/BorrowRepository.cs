using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SmartLibrary.Models;

namespace SmartLibrary.DataAccess
{
    public class BorrowRepository
    {
        private const string BaseQuery =
            @"SELECT br.*, b.Title AS BookTitle, b.Author AS BookAuthor,
                     m.FirstName + ' ' + m.LastName AS MemberName, m.StudentNumber
              FROM BorrowRecords br
              INNER JOIN Books b ON br.BookID = b.BookID
              INNER JOIN Members m ON br.MemberID = m.MemberID";

        private BorrowRecord ReadRecord(SqlDataReader r)
        {
            var br = new BorrowRecord();
            br.RecordID = r.GetInt32(r.GetOrdinal("RecordID"));
            br.BookID = r.GetInt32(r.GetOrdinal("BookID"));
            br.MemberID = r.GetInt32(r.GetOrdinal("MemberID"));
            br.BorrowDate = r.GetDateTime(r.GetOrdinal("BorrowDate"));
            br.DueDate = r.GetDateTime(r.GetOrdinal("DueDate"));
            int retIdx = r.GetOrdinal("ReturnDate");
            br.ReturnDate = r.IsDBNull(retIdx) ? (DateTime?)null : r.GetDateTime(retIdx);
            br.Status = r.GetString(r.GetOrdinal("Status"));
            br.BookTitle = r.GetString(r.GetOrdinal("BookTitle"));
            br.BookAuthor = r.GetString(r.GetOrdinal("BookAuthor"));
            br.MemberName = r.GetString(r.GetOrdinal("MemberName"));
            br.StudentNumber = r.GetString(r.GetOrdinal("StudentNumber"));
            return br;
        }

        public List<BorrowRecord> GetAll()
        {
            var list = new List<BorrowRecord>();
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand(BaseQuery + " ORDER BY br.BorrowDate DESC", conn))
                using (var r = cmd.ExecuteReader())
                    while (r.Read()) list.Add(ReadRecord(r));
            }
            return list;
        }

        public List<BorrowRecord> GetActive()
        {
            var list = new List<BorrowRecord>();
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand(BaseQuery + " WHERE br.Status='Borrowed' ORDER BY br.DueDate ASC", conn))
                using (var r = cmd.ExecuteReader())
                    while (r.Read()) list.Add(ReadRecord(r));
            }
            return list;
        }

        public List<BorrowRecord> GetOverdue()
        {
            var list = new List<BorrowRecord>();
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand(BaseQuery + " WHERE br.Status='Borrowed' AND br.DueDate < GETDATE() ORDER BY br.DueDate ASC", conn))
                using (var r = cmd.ExecuteReader())
                    while (r.Read()) list.Add(ReadRecord(r));
            }
            return list;
        }

        public void BorrowBook(int bookId, int memberId, DateTime dueDate, int processedBy)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand("sp_BorrowBook", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BookID", bookId);
                    cmd.Parameters.AddWithValue("@MemberID", memberId);
                    cmd.Parameters.AddWithValue("@DueDate", dueDate);
                    cmd.Parameters.AddWithValue("@ProcessedBy", processedBy);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void ReturnBook(int recordId)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand("sp_ReturnBook", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RecordID", recordId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public DashboardStats GetDashboardStats()
        {
            var stats = new DashboardStats();
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand("sp_GetDashboardStats", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            stats.TotalBooks = r.IsDBNull(0) ? 0 : r.GetInt32(0);
                            stats.TotalMembers = r.IsDBNull(1) ? 0 : r.GetInt32(1);
                            stats.ActiveBorrows = r.IsDBNull(2) ? 0 : r.GetInt32(2);
                            stats.OverdueBooks = r.IsDBNull(3) ? 0 : r.GetInt32(3);
                        }
                    }
                }
            }
            return stats;
        }

        public List<BorrowRecord> GetByDateRange(DateTime start, DateTime end)
        {
            var list = new List<BorrowRecord>();
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand(BaseQuery + " WHERE br.BorrowDate BETWEEN @S AND @E ORDER BY br.BorrowDate DESC", conn))
                {
                    cmd.Parameters.AddWithValue("@S", start);
                    cmd.Parameters.AddWithValue("@E", end);
                    using (var r = cmd.ExecuteReader())
                        while (r.Read()) list.Add(ReadRecord(r));
                }
            }
            return list;
        }
    }
}
