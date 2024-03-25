using System.Data.SqlClient;

public interface IDatabaseService
{
    SqlConnection GetConnection();
}

public class DatabaseService : IDatabaseService
{
    private readonly IConfiguration _configuration;

    public DatabaseService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public SqlConnection GetConnection()
    {
        var connectionString = _configuration.GetConnectionString("ApiDatabase");
        return new SqlConnection(connectionString);
    }
}
