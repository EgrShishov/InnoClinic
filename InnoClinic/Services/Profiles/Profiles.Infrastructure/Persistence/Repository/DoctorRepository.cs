using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

public class DoctorRepository : IDoctorRepository
{
    private readonly ProfilesDbContext _dbContext;
    public DoctorRepository(ProfilesDbContext dbContext) 
    {
        _dbContext = dbContext;
    }

    public async Task<Doctor> AddDoctorAsync(Doctor doctor, CancellationToken cancellationToken = default)
    {
        var newDoctor = await _dbContext.Doctors.AddAsync(doctor, cancellationToken);
        return newDoctor.Entity;
    }

    public async Task DeleteDoctorAsync(int id, CancellationToken cancellationToken = default)
    {
        var doctor = await _dbContext.Doctors.FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        
        if (doctor != null)
        {
            _dbContext.Entry(doctor).State = EntityState.Deleted;
        }
    }
    public async Task<Doctor> GetDoctorByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Doctors.FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }
    public async Task<List<Doctor>> ListDoctorsAsync(Expression<Func<Doctor, bool>> filter, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Doctors.AsQueryable();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<List<Doctor>> ListDoctorsAsync(Expression<Func<Doctor, bool>> filter, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Doctors.AsQueryable();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        query = query.Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);

        return await query.ToListAsync(cancellationToken);
    } 
    public async Task<List<Doctor>> GetListDoctorsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Doctors
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public Task<Doctor> UpdateDoctorAsync(Doctor doctor, CancellationToken cancellationToken = default)
    {
        _dbContext.Entry(doctor).State = EntityState.Modified;
        return Task.FromResult(doctor);
    }

    public async Task UpdateStatusAsync(int doctorId, ProfileStatus status, CancellationToken cancellationToken = default)
    {
        var doctor = await _dbContext.Doctors.FirstOrDefaultAsync(d => d.Id == doctorId, cancellationToken);
        
        if (doctor != null)
        {
            doctor.Status = status;
        }

        _dbContext.Entry(doctor).State = EntityState.Modified;
    }
}
