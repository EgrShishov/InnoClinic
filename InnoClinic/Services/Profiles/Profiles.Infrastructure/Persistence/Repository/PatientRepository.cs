using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

public class PatientRepository : IPatientRepository
{
    private readonly ProfilesDbContext _dbContext;
    public PatientRepository(ProfilesDbContext dbContext) 
    {
        _dbContext = dbContext;
    }
    public async Task<Patient> AddPatientAsync(Patient patient, CancellationToken cancellationToken = default)
    {
        var newPatient = await _dbContext.Patients.AddAsync(patient, cancellationToken);
        return newPatient.Entity;
    }
    public async Task DeletePatientAsync(int id, CancellationToken cancellationToken = default)
    {
        var patient = await _dbContext.Patients.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        
        if (patient != null)
        {
            _dbContext.Entry(patient).State = EntityState.Deleted;
        }
    }
    public async Task<Patient> GetPatientByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Patients.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }
    public async Task<List<Patient>> ListPatientsAsync(Expression<Func<Patient, bool>> filter, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Patients.AsQueryable();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query.ToListAsync(cancellationToken);
    }
    public async Task<List<Patient>> ListPatientsAsync(Expression<Func<Patient, bool>> filter, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Patients.AsQueryable();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        query = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

        return await query.ToListAsync(cancellationToken);
    }
    public async Task<List<Patient>> GetListPatientsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Patients.ToListAsync(cancellationToken);
    }
    public async Task<List<Patient>> GetListPatientsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Patients
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }
    public Task<Patient> UpdatePatientAsync(Patient patient, CancellationToken cancellationToken = default)
    {
        _dbContext.Entry(patient).State = EntityState.Modified;
        return Task.FromResult(patient);
    }
}
