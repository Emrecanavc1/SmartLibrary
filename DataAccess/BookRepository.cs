using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SmartLibrary.Models;

namespace SmartLibrary.DataAccess
{
    public class BookRepository
    {
        private Book ReadBook(SqlDataReader r)
        {
            var b = new Book();
            b.BookID = r.GetInt32(r.GetOrdinal("BookID"));
            b.ISBN = r.IsDBNull(r.GetOrdinal("ISBN")) ? "" : r.GetString(r.GetOrdinal("ISBN"));
            b.Title = r.GetString(r.GetOrdinal("Title"));
            b.Author = r.GetString(r.GetOrdinal("Author"));
            b.Publisher = r.IsDBNull(r.GetOrdinal("Publisher")) ? "" : r.GetString(r.GetOrdinal("Publisher"));
            b.PublishYear = r.IsDBNull(r.GetOrdinal("PublishYear")) ? 0 : r.GetInt32(r.GetOrdinal("PublishYear"));
            b.CategoryID = r.IsDBNull(r.GetOrdinal("CategoryID")) ? 0 : r.GetInt32(r.GetOrdinal("CategoryID"));
            b.PageCount = r.IsDBNull(r.GetOrdinal("PageCount")) ? 0 : r.GetInt32(r.GetOrdinal("PageCount"));
            b.ShelfLocation = r.IsDBNull(r.GetOrdinal("ShelfLocation")) ? "" : r.GetString(r.GetOrdinal("ShelfLocation"));
            b.TotalCopies = r.GetInt32(r.GetOrdinal("TotalCopies"));
            b.AvailableCopies = r.GetInt32(r.GetOrdinal("AvailableCopies"));
            try { b.CategoryName = r.GetString(r.GetOrdinal("CategoryName")); } catch { b.CategoryName = ""; }
            return b;
        }

        public List<Book> GetAll()
        {
            var list = new List<Book>();
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    @"SELECT b.*, c.CategoryName FROM Books b 
                      LEFT JOIN Categories c ON b.CategoryID = c.CategoryID 
                      WHERE b.IsActive = 1 ORDER BY b.Title", conn))
                using (var r = cmd.ExecuteReader())
                    while (r.Read()) list.Add(ReadBook(r));
            }
            return list;
        }

        public Book GetById(int id)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    @"SELECT b.*, c.CategoryName FROM Books b 
                      LEFT JOIN Categories c ON b.CategoryID = c.CategoryID 
                      WHERE b.BookID = @ID", conn))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    using (var r = cmd.ExecuteReader())
                        if (r.Read()) return ReadBook(r);
                }
            }
            return null;
        }

        public List<Book> Search(string term)
        {
            var list = new List<Book>();
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    @"SELECT b.*, c.CategoryName FROM Books b 
                      LEFT JOIN Categories c ON b.CategoryID = c.CategoryID 
                      WHERE b.IsActive = 1 
                        AND (b.Title LIKE @T OR b.Author LIKE @T OR b.ISBN LIKE @T)
                      ORDER BY b.Title", conn))
                {
                    cmd.Parameters.AddWithValue("@T", "%" + term + "%");
                    using (var r = cmd.ExecuteReader())
                        while (r.Read()) list.Add(ReadBook(r));
                }
            }
            return list;
        }

        public List<Book> GetAvailableBooks()
        {
            var list = new List<Book>();
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    @"SELECT b.*, c.CategoryName FROM Books b 
                      LEFT JOIN Categories c ON b.CategoryID = c.CategoryID 
                      WHERE b.IsActive = 1 AND b.AvailableCopies > 0 ORDER BY b.Title", conn))
                using (var r = cmd.ExecuteReader())
                    while (r.Read()) list.Add(ReadBook(r));
            }
            return list;
        }

        public int Add(Book b)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    @"INSERT INTO Books (ISBN,Title,Author,Publisher,PublishYear,CategoryID,
                      PageCount,ShelfLocation,TotalCopies,AvailableCopies)
                      VALUES (@ISBN,@Title,@Author,@Publisher,@Year,@CatID,
                      @Pages,@Shelf,@Total,@Avail);
                      SELECT CAST(SCOPE_IDENTITY() AS INT);", conn))
                {
                    cmd.Parameters.AddWithValue("@ISBN", (object)b.ISBN ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Title", b.Title);
                    cmd.Parameters.AddWithValue("@Author", b.Author);
                    cmd.Parameters.AddWithValue("@Publisher", (object)b.Publisher ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Year", b.PublishYear);
                    cmd.Parameters.AddWithValue("@CatID", b.CategoryID);
                    cmd.Parameters.AddWithValue("@Pages", b.PageCount);
                    cmd.Parameters.AddWithValue("@Shelf", (object)b.ShelfLocation ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Total", b.TotalCopies);
                    cmd.Parameters.AddWithValue("@Avail", b.AvailableCopies);
                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        public bool Update(Book b)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    @"UPDATE Books SET ISBN=@ISBN,Title=@Title,Author=@Author,
                      Publisher=@Publisher,PublishYear=@Year,CategoryID=@CatID,
                      PageCount=@Pages,ShelfLocation=@Shelf,TotalCopies=@Total,
                      AvailableCopies=@Avail WHERE BookID=@ID", conn))
                {
                    cmd.Parameters.AddWithValue("@ID", b.BookID);
                    cmd.Parameters.AddWithValue("@ISBN", (object)b.ISBN ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Title", b.Title);
                    cmd.Parameters.AddWithValue("@Author", b.Author);
                    cmd.Parameters.AddWithValue("@Publisher", (object)b.Publisher ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Year", b.PublishYear);
                    cmd.Parameters.AddWithValue("@CatID", b.CategoryID);
                    cmd.Parameters.AddWithValue("@Pages", b.PageCount);
                    cmd.Parameters.AddWithValue("@Shelf", (object)b.ShelfLocation ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Total", b.TotalCopies);
                    cmd.Parameters.AddWithValue("@Avail", b.AvailableCopies);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Delete(int bookId)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand("UPDATE Books SET IsActive=0 WHERE BookID=@ID", conn))
                {
                    cmd.Parameters.AddWithValue("@ID", bookId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
