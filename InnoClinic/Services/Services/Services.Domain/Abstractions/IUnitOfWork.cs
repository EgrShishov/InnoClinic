public interface IUnitOfWork : IDisposable
{
    public ISpecializationsRepository Specializations { get; }
    public IServicesRepository Services { get; }
    public Task SaveChangesAsync(CancellationToken cancellationToken = default);
    public Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    public Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    public Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
