using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SmartLibrary.Models;

namespace SmartLibrary.DataAccess
{
    public class UserRepository
    {
        public User Login(string username, string password)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    "SELECT * FROM Users WHERE Username=@U AND PasswordHash=@P AND IsActive=1", conn))
                {
                    cmd.Parameters.AddWithValue("@U", username);
                    cmd.Parameters.AddWithValue("@P", password);
                    using (var r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            var u = new User();
                            u.UserID = r.GetInt32(r.GetOrdinal("UserID"));
                            u.Username = r.GetString(r.GetOrdinal("Username"));
                            u.FullName = r.GetString(r.GetOrdinal("FullName"));
                            u.Role = r.GetString(r.GetOrdinal("Role"));
                            return u;
                        }
                    }
                }
            }
            return null;
        }
    }

    public class CategoryRepository
    {
        public List<Category> GetAll()
        {
            var list = new List<Category>();
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT * FROM Categories ORDER BY CategoryName", conn))
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        var c = new Category();
                        c.CategoryID = r.GetInt32(r.GetOrdinal("CategoryID"));
                        c.CategoryName = r.GetString(r.GetOrdinal("CategoryName"));
                        c.Description = r.IsDBNull(r.GetOrdinal("Description")) ? "" : r.GetString(r.GetOrdinal("Description"));
                        list.Add(c);
                    }
                }
            }
            return list;
        }
    }

    public class FineRepository
    {
        public List<FineRecord> GetUnpaid()
        {
            var list = new List<FineRecord>();
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    @"SELECT f.FineID, f.Amount, f.Reason, f.FineDate,
                             m.FirstName + ' ' + m.LastName AS MemberName, b.Title AS BookTitle
                      FROM Fines f
                      INNER JOIN Members m ON f.MemberID = m.MemberID
                      INNER JOIN BorrowRecords br ON f.RecordID = br.RecordID
                      INNER JOIN Books b ON br.BookID = b.BookID
                      WHERE f.IsPaid = 0 ORDER BY f.FineDate DESC", conn))
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        var f = new FineRecord();
                        f.FineID = r.GetInt32(r.GetOrdinal("FineID"));
                        f.Amount = r.GetDecimal(r.GetOrdinal("Amount"));
                        f.Reason = r.IsDBNull(r.GetOrdinal("Reason")) ? "" : r.GetString(r.GetOrdinal("Reason"));
                        f.FineDate = r.GetDateTime(r.GetOrdinal("FineDate"));
                        f.MemberName = r.GetString(r.GetOrdinal("MemberName"));
                        f.BookTitle = r.GetString(r.GetOrdinal("BookTitle"));
                        list.Add(f);
                    }
                }
            }
            return list;
        }
    }
}
