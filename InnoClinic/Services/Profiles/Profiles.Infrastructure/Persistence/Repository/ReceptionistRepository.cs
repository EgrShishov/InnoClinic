using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

public class ReceptionistRepository : IReceptionistRepository
{
    private readonly ProfilesDbContext _dbContext;
    public ReceptionistRepository(ProfilesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Receptionist> AddReceptionistAsync(Receptionist receptionist, CancellationToken cancellationToken = default)
    {
        var newReceptionist = await _dbContext.Receptionists.AddAsync(receptionist);

        return newReceptionist.Entity;
    }

    public async Task DeleteReceptionistAsync(int id, CancellationToken cancellationToken = default)
    {
        var receptionist = await _dbContext.Receptionists.FirstOrDefaultAsync(r => r.Id == id);
        
        if(receptionist != null)
        {
            _dbContext.Entry(receptionist).State = EntityState.Deleted;
        }
    }

    public async Task<Receptionist> GetReceptionistByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Receptionists.FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<List<Receptionist>> ListReceptionistAsync(Expression<Func<Receptionist, bool>> filter, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Receptionists.AsQueryable();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        query = query.Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize);

        return await query.ToListAsync();
    }
    public async Task<List<Receptionist>> GetListReceptionistAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Receptionists
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();
    }

    public Task<Receptionist> UpdateReceptionistAsync(Receptionist receptionist, CancellationToken cancellationToken = default)
    {
        _dbContext.Receptionists.Entry(receptionist).State = EntityState.Modified;

        return Task.FromResult(receptionist);
    }
}
