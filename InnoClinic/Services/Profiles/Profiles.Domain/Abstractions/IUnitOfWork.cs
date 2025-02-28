public interface IUnitOfWork : IDisposable
{
    public IDoctorRepository DoctorsRepository { get; }
    public IPatientRepository PatientsRepository { get; }
    public IReceptionistRepository ReceptionistsRepository { get; }
    public IOfficeRepository OfficeRepository { get; }

    public Task DeleteDataBaseAsync(CancellationToken cancellationToken = default);
    public Task CreateDataBaseAsync(CancellationToken cancellationToken = default);
    public Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    public Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    public Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    public Task<int> CompleteAsync(CancellationToken cancellationToken = default);
}