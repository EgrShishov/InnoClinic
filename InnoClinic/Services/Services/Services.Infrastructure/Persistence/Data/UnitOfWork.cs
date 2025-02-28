using System.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly ISqlConnectionFactory _factory;
    private IDbConnection _dbConnection;
    private IDbTransaction _dbTransaction;
    public UnitOfWork(ISqlConnectionFactory factory)
    {
        _factory = factory;

        _dbConnection = factory.CreateConnection();
        _dbConnection.Open();

        _dbTransaction = _dbConnection.BeginTransaction();

        Services = new ServicesRepository(_factory);
        Specializations = new SpecializationsRepository(_factory);
    }

    public ISpecializationsRepository Specializations { get; }
    public IServicesRepository Services { get; }

    public Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_dbTransaction is null)
        {
            _dbTransaction = _dbConnection.BeginTransaction(); 
        }

        return Task.CompletedTask;
    }

    public Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_dbTransaction is null)
        {
            return Task.CompletedTask;
        }

        _dbTransaction.Commit();

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _dbTransaction.Dispose();
        _dbConnection.Dispose();
        GC.SuppressFinalize(this);
    }

    public Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_dbTransaction is null)
        {
            return Task.CompletedTask;
        }

        _dbTransaction.Rollback();

        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _dbTransaction.Commit();
        }
        catch
        {
            _dbTransaction.Rollback();
            throw;
        }
        finally
        {
            _dbConnection.Close();
        }

        return Task.CompletedTask;
    }
}
