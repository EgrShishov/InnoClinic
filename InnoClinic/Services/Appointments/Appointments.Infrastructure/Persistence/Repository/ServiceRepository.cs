using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

public class ServiceRepository : IServiceRepository
{
    private readonly AppointmentsDbContext _dbContext;
    public ServiceRepository(AppointmentsDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Service> AddServiceAsync(Service service, CancellationToken cancellationToken = default)
    {
        var addedService = await _dbContext.Services.AddAsync(service, cancellationToken);

        return addedService.Entity;
    }

    public async Task<IEnumerable<Service>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Services.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Service>> GetListAsync(Expression<Func<Service, bool>> filter, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Services.AsQueryable();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<Service> GetServiceByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Services.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public Task UpdateServiceAsync(Service service, CancellationToken cancellationToken = default)
    {
        _dbContext.Entry(service).State = EntityState.Modified;
        return Task.CompletedTask;
    }    
    public async Task DeleteServiceAsync(int id, CancellationToken cancellationToken = default)
    {
        var service = await _dbContext.Services.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        if (service is not null)
        {
            _dbContext.Entry(service).State = EntityState.Deleted;
        }
    }
}