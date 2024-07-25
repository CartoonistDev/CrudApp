using CrudApp.IServices;
using CrudApp.Models;
using Microsoft.Data.SqlClient;

public class UserService : IUserService
{
    private readonly string connectionString; 
    private readonly IConfiguration configuration;

    public UserService(IConfiguration configuration)
    {
        this.configuration = configuration;

        this.connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public void CreateUser(string name, int age)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or empty", nameof(name));

        if (age < 0 || age > 150)
            throw new ArgumentOutOfRangeException(nameof(age), "Age must be between 0 and 150");

        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Users (Name, Age) VALUES (@Name, @Age)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Age", age);
                    command.ExecuteNonQuery();
                }
            }
        }
        catch (SqlException ex)
        {
            throw new Exception("Database error occurred while creating user", ex);
        }
    }

    public List<User> GetAllUsers()
    {
        List<User> users = new List<User>();
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Users";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new User
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Age = reader.GetInt32(2)
                            });
                        }
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            throw new Exception("Database error occurred while fetching users", ex);
        }
        return users;
    }

    public void UpdateUser(int id, string name, int age)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid user ID", nameof(id));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or empty", nameof(name));
        if (age < 0 || age > 150)
            throw new ArgumentOutOfRangeException(nameof(age), "Age must be between 0 and 150");

        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Users SET Name = @Name, Age = @Age WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Age", age);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                        throw new KeyNotFoundException($"User with ID {id} not found");
                }
            }
        }
        catch (SqlException ex)
        {
            throw new Exception("Database error occurred while updating user", ex);
        }
    }

    public void DeleteUser(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid user ID", nameof(id));

        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Users WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                        throw new KeyNotFoundException($"User with ID {id} not found");
                }
            }
        }
        catch (SqlException ex)
        {
            throw new Exception("Database error occurred while deleting user", ex);
        }
    }
}