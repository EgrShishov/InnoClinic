public interface ISqlConnectionFactory
{
    public NpgsqlConnection CreateConnection();
}