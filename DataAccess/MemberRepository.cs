using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SmartLibrary.Models;

namespace SmartLibrary.DataAccess
{
    public class MemberRepository
    {
        private Member ReadMember(SqlDataReader r)
        {
            var m = new Member();
            m.MemberID = r.GetInt32(r.GetOrdinal("MemberID"));
            m.StudentNumber = r.IsDBNull(r.GetOrdinal("StudentNumber")) ? "" : r.GetString(r.GetOrdinal("StudentNumber"));
            m.FirstName = r.GetString(r.GetOrdinal("FirstName"));
            m.LastName = r.GetString(r.GetOrdinal("LastName"));
            m.Email = r.IsDBNull(r.GetOrdinal("Email")) ? "" : r.GetString(r.GetOrdinal("Email"));
            m.Phone = r.IsDBNull(r.GetOrdinal("Phone")) ? "" : r.GetString(r.GetOrdinal("Phone"));
            m.Department = r.IsDBNull(r.GetOrdinal("Department")) ? "" : r.GetString(r.GetOrdinal("Department"));
            m.MaxBooks = r.GetInt32(r.GetOrdinal("MaxBooks"));
            return m;
        }

        public List<Member> GetAll()
        {
            var list = new List<Member>();
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT * FROM Members WHERE IsActive=1 ORDER BY LastName,FirstName", conn))
                using (var r = cmd.ExecuteReader())
                    while (r.Read()) list.Add(ReadMember(r));
            }
            return list;
        }

        public Member GetById(int id)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT * FROM Members WHERE MemberID=@ID", conn))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    using (var r = cmd.ExecuteReader())
                        if (r.Read()) return ReadMember(r);
                }
            }
            return null;
        }

        public List<Member> Search(string term)
        {
            var list = new List<Member>();
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    @"SELECT * FROM Members WHERE IsActive=1 
                      AND (FirstName LIKE @T OR LastName LIKE @T OR StudentNumber LIKE @T)
                      ORDER BY LastName,FirstName", conn))
                {
                    cmd.Parameters.AddWithValue("@T", "%" + term + "%");
                    using (var r = cmd.ExecuteReader())
                        while (r.Read()) list.Add(ReadMember(r));
                }
            }
            return list;
        }

        public int Add(Member m)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    @"INSERT INTO Members (StudentNumber,FirstName,LastName,Email,Phone,Department,ExpiryDate,MaxBooks)
                      VALUES (@SN,@FN,@LN,@Em,@Ph,@Dep,@Exp,@Max);
                      SELECT CAST(SCOPE_IDENTITY() AS INT);", conn))
                {
                    cmd.Parameters.AddWithValue("@SN", m.StudentNumber);
                    cmd.Parameters.AddWithValue("@FN", m.FirstName);
                    cmd.Parameters.AddWithValue("@LN", m.LastName);
                    cmd.Parameters.AddWithValue("@Em", (object)m.Email ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Ph", (object)m.Phone ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Dep", (object)m.Department ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Exp", m.ExpiryDate.HasValue ? (object)m.ExpiryDate.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Max", m.MaxBooks);
                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        public bool Update(Member m)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    @"UPDATE Members SET StudentNumber=@SN,FirstName=@FN,LastName=@LN,
                      Email=@Em,Phone=@Ph,Department=@Dep,ExpiryDate=@Exp,MaxBooks=@Max
                      WHERE MemberID=@ID", conn))
                {
                    cmd.Parameters.AddWithValue("@ID", m.MemberID);
                    cmd.Parameters.AddWithValue("@SN", m.StudentNumber);
                    cmd.Parameters.AddWithValue("@FN", m.FirstName);
                    cmd.Parameters.AddWithValue("@LN", m.LastName);
                    cmd.Parameters.AddWithValue("@Em", (object)m.Email ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Ph", (object)m.Phone ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Dep", (object)m.Department ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Exp", m.ExpiryDate.HasValue ? (object)m.ExpiryDate.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Max", m.MaxBooks);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Delete(int memberId)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand("UPDATE Members SET IsActive=0 WHERE MemberID=@ID", conn))
                {
                    cmd.Parameters.AddWithValue("@ID", memberId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
