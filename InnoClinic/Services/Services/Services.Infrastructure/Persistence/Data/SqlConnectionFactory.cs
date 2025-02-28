using Microsoft.Extensions.Configuration;
public class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly IConfiguration _configuration;

    public SqlConnectionFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public NpgsqlConnection CreateConnection()
    {
        return new NpgsqlConnection(_configuration.GetConnectionString("ServicesDb"));
    }
}
