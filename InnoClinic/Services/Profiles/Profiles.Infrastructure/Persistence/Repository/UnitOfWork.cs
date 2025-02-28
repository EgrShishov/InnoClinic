using Microsoft.EntityFrameworkCore.Storage;

public class UnitOfWork : IUnitOfWork
{
    private readonly ProfilesDbContext _dbContext;
    private readonly Lazy<IDoctorRepository> _doctorsRepository;
    private readonly Lazy<IPatientRepository> _patientsRepository;
    private readonly Lazy<IReceptionistRepository> _receptionistsRepository;
    private readonly Lazy<IOfficeRepository> _officeRepository;
    private IDbContextTransaction _transaction;

    public UnitOfWork(ProfilesDbContext dbContext) 
    {
        _dbContext = dbContext;

        _doctorsRepository = new(() => new DoctorRepository(_dbContext));
        _patientsRepository = new(() => new PatientRepository(_dbContext));
        _receptionistsRepository = new(() => new ReceptionistRepository(_dbContext));
        _officeRepository = new(() => new OfficeRepository(_dbContext));
    }

    public IDoctorRepository DoctorsRepository => _doctorsRepository.Value;
    public IPatientRepository PatientsRepository => _patientsRepository.Value;
    public IReceptionistRepository ReceptionistsRepository => _receptionistsRepository.Value;
    public IOfficeRepository OfficeRepository => _officeRepository.Value;

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _transaction.CommitAsync(cancellationToken);
    }

    public async Task<int> CompleteAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task CreateDataBaseAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.Database.EnsureCreatedAsync(cancellationToken);
    }

    public async Task DeleteDataBaseAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.Database.EnsureDeletedAsync(cancellationToken);
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _dbContext.Dispose();
        GC.SuppressFinalize(this);
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _transaction.RollbackAsync(cancellationToken);
    }
}
