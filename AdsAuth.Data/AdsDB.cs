using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace AdsAuth.Data
{
    public class AdsDB
    {
        private readonly string _connectionString;
        public AdsDB(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<Ad> GetAds(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = @"SELECT a.*,u.Name FROM Ads a JOIN Users u ON u.Id=a.UserId";
            if (id > 0)
            {
                command.CommandText += @"WHERE UserId = @personId";
                command.Parameters.AddWithValue("@personId", id);
            }
            connection.Open();
            var reader = command.ExecuteReader();
            var ads = new List<Ad>();
            while (reader.Read())
            {
                ads.Add(new Ad
                {
                    Id = (int)reader["id"],
                    Name = (string)reader["Name"],
                    Date = (DateTime)reader["date"],
                    Cell = (string)reader["Cell"],
                    Text = (string)reader["text"],
                    UserId = (int)reader["UserId"]
                });
            }
            return ads;
        }
        public void AddUser(User u, string password)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO Users (Name,Email,PasswordHash)
                                    VALUES (@name,@email,@password)";
            u.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            command.Parameters.AddWithValue("@name", u.Name);
            command.Parameters.AddWithValue("@email", u.Email);
            command.Parameters.AddWithValue("@password", u.PasswordHash);
            connection.Open();
            command.ExecuteNonQuery();
        }
        public User Login(string email, string password)
        {
            User u = GetByEmail(email);
            if (u == null)
            {
                return null;
            }
            bool isValid = BCrypt.Net.BCrypt.Verify(password, u.PasswordHash);
            return isValid ? u : null;
        }
        public User GetByEmail(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT top 1 * FROM Users WHERE Email=@email";
            cmd.Parameters.AddWithValue("@email", email);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }
            return new User
            {
                Id = (int)reader["Id"],
                Name = (string)reader["Name"],
                PasswordHash = (string)reader["PasswordHash"],
                Email = (string)reader["email"]
            };

        }
        public void AddAd(Ad a, User u)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO Ads(UserId,Text,Date,Cell)
                                    VALUES (@userId,@text,@date,@cell)";
            command.Parameters.AddWithValue("@userId", u.Id);
            command.Parameters.AddWithValue("@text", a.Text);
            command.Parameters.AddWithValue("@date", a.Date);
            command.Parameters.AddWithValue("@cell", a.Cell);
            connection.Open();
            command.ExecuteNonQuery();
        }
        public void DeleteAd(int userId, int adId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = @"DELETE FROM Ads WHERE UserId=@userId AND Id=@adId";
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@adId", adId);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}
