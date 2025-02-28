using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

public class OfficeRepository : IOfficeRepository
{
    private readonly ProfilesDbContext _dbContext;
    public OfficeRepository(ProfilesDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Office> AddOfficeAsync(Office office, CancellationToken cancellationToken = default)
    {
        var newOffice = await _dbContext.Office.AddAsync(office, cancellationToken);

        return newOffice.Entity;
    }
    
    public async Task DeleteOfficeAsync(string id, CancellationToken cancellationToken = default)
    {
        var office = await _dbContext.Office.FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        
        if (office != null)
        {
            _dbContext.Entry(office).State = EntityState.Deleted;
        }
    }

    public async Task<List<Office>> GetListOfficesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Office.ToListAsync(cancellationToken);
    }

    public async Task<Office> GetOfficeByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Office.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<List<Office>> ListOfficiesAsync(Expression<Func<Office, bool>> filter, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Office.AsQueryable();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public Task<Office> UpdateOfficeAsync(Office office, CancellationToken cancellationToken = default)
    {
        _dbContext.Entry(office).State = EntityState.Modified;
        return Task.FromResult(office);
    }
}