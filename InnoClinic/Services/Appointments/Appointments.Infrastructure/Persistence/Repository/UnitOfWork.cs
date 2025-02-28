using Microsoft.EntityFrameworkCore.Storage;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppointmentsDbContext _context;
    private readonly Lazy<IAppointmentsRepository> _appointmentsRepository;
    private readonly Lazy<IAppointmentsResultRepository> _appointmentsResultRepository;
    private readonly Lazy<IServiceRepository> _serviceRepository;
    private IDbContextTransaction _transaction;

    public UnitOfWork(AppointmentsDbContext context)
    {
        _context = context;

        _appointmentsRepository = new(() => new AppointmentsRepository(_context));
        _appointmentsResultRepository = new(() => new AppointmentsResultRepository(_context));
        _serviceRepository = new(() => new ServiceRepository(_context));
    }
    public IAppointmentsRepository AppointmentsRepository => _appointmentsRepository.Value;
    public IAppointmentsResultRepository ResultsRepository => _appointmentsResultRepository.Value;
    public IServiceRepository ServiceRepository => _serviceRepository.Value;

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync(cancellationToken);
        }
    }

    public async Task CreateDataBaseAsync(CancellationToken cancellationToken = default)
    {
        await _context.Database.EnsureCreatedAsync(cancellationToken);
    }

    public async Task DeleteDataBaseAsync(CancellationToken cancellationToken = default)
    {
        await _context.Database.EnsureDeletedAsync(cancellationToken);
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
        }
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
