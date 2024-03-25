using apidenemesi.Models;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace apidenemesi.Services
{
    public class UserService
    {
        private readonly IConfiguration _configuration;

        public UserService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<User> GetAllUsers()
        {
            var users = new List<User>();
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Users", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            username = reader.GetString(reader.GetOrdinal("username")),
                            password = reader.GetString(reader.GetOrdinal("password")),
                        });
                    }
                }
            }

            return users;
        }

        public User GetUserById(int id)
        {
            User user = null;
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Users WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new User
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            username = reader.GetString(reader.GetOrdinal("username")),
                            password = reader.GetString(reader.GetOrdinal("password")),
                            // Diğer alanlarınızı burada doldurun
                        };
                    }
                }
            }

            return user;
        }


        public bool IsUsernameTaken(string username)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT COUNT(*) FROM Users WHERE username = @Username", connection);
                command.Parameters.AddWithValue("@Username", username);
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }
        public void AddUser(User user)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("INSERT INTO Users (Name,username, password) VALUES (@Name, @username, @password)", connection);
                command.Parameters.AddWithValue("@Name", user.Name);
                command.Parameters.AddWithValue("@username", user.username);
                command.Parameters.AddWithValue("@password", user.password);


                command.ExecuteNonQuery();
            }
        }

        public void UpdateUser(int id, User user)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("UPDATE Users SET Name = @Name WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Name", user.Name);
                command.Parameters.AddWithValue("@Id", user.Id);

                command.ExecuteNonQuery();
            }
        }

        public void DeleteUser(int id)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("DELETE FROM Users WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();
            }
        }



        public User Authenticate(string username, string password)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Users WHERE username = @Username AND password = @Password", connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new User
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            username = reader.GetString(reader.GetOrdinal("username")),
                            password = reader.GetString(reader.GetOrdinal("password"))
                        };
                    }
                    else
                    {
                        return null; // Kullanıcı doğrulanamadıysa null döndür
                    }
                }
            }
        }
    }
}
