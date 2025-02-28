public class DatabaseInitializer
{
    private readonly ISqlConnectionFactory _factory;
    public DatabaseInitializer(ISqlConnectionFactory factory)
    {
        _factory = factory;
    }
    public void Initialize()
    {
        var _dbConnection = _factory.CreateConnection();

        _dbConnection.Open();

        using var dbTransaction = _dbConnection.BeginTransaction();

        try
        {
            _dbConnection.Execute(SqlScripts.CreateSpecializationsTables, transaction: dbTransaction);
            _dbConnection.Execute(SqlScripts.CreateServicesTable, transaction: dbTransaction);

            dbTransaction.Commit();

            _dbConnection.Close();
        }
        catch (Exception)
        {
            dbTransaction.Rollback();
            throw;
        }
    }
}
