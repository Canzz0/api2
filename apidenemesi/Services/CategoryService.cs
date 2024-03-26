using apidenemesi.Models;
using System.Data.SqlClient;

namespace apidenemesi.Services
{
    public class CategoryService
    {
        private readonly IConfiguration _configuration;

        public CategoryService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<Category> GetAllCategories()
        {
            var categories = new List<Category>();
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("Select * FROM Category", connection);
                using ( var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categories.Add(new Category
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                        });
                    }
                }
            }
            return categories;
        }
        public void AddCategory(Category category)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("INSERT INTO Category (Name) VALUES (@Name)", connection);
                command.Parameters.AddWithValue("@Name", category.Name);
                command.ExecuteNonQuery();
            }
        }

        public void UpdateCategory(Category category)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("UPDATE Category SET Name = @Name WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Name", category.Name);
                command.Parameters.AddWithValue("@Id", category.Id); // Burada @Id parametresini ekledik
                command.ExecuteNonQuery();
            }

        }
        public void DeleteCategory(int id)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("DELETE FROM Category WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();
            }
        }

    }
}
