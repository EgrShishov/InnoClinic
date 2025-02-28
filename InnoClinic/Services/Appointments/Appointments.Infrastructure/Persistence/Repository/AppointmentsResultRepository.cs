using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

public class AppointmentsResultRepository : IAppointmentsResultRepository
{
    private readonly AppointmentsDbContext _context;

    public AppointmentsResultRepository(AppointmentsDbContext context)
    {
        _context = context;
    }

    public async Task<Results> AddResultsAsync(Results results, CancellationToken cancellationToken = default)
    {
        var newAppontmentsResult = await _context.Results.AddAsync(results, cancellationToken);
        return newAppontmentsResult.Entity;
    }

    public async Task DeleteResultsAsync(int id, CancellationToken cancellationToken = default)
    {
        var appointmentResult = await _context.Results.FirstOrDefaultAsync(x => x.Id == id);
        if (appointmentResult != null)
        {
            _context.Entry(appointmentResult).State = EntityState.Deleted;
        }
    }

    public async Task<IEnumerable<Results>> GetAllResultsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Results.ToListAsync(cancellationToken);
    }

    public async Task<Results> GetResultsByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Results.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<Results>> ListAsync(Expression<Func<Results, bool>> filter, CancellationToken cancellationToken = default)
    {
        var query = _context.Results.AsQueryable();

        if (filter != null)
        {
            query = query.Where(filter);
        }
        return await query.ToListAsync();
    }

    public Task<Results> UpdateResultsAsync(Results results, CancellationToken cancellationToken = default)
    {
        _context.Entry(results).State = EntityState.Modified;
        return Task.FromResult(results);
    }
}
