public interface IUnitOfWork : IDisposable
{
    public IAppointmentsRepository AppointmentsRepository { get; }
    public IAppointmentsResultRepository ResultsRepository { get; }
    public IServiceRepository ServiceRepository { get; }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default);
    public Task DeleteDataBaseAsync(CancellationToken cancellationToken = default);
    public Task CreateDataBaseAsync(CancellationToken cancellationToken = default);
    public Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    public Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    public Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
