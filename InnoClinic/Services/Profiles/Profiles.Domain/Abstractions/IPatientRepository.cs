using System.Linq.Expressions;

public interface IPatientRepository
{
    public Task<Patient> AddPatientAsync(Patient patient, CancellationToken cancellationToken = default);
    public Task<Patient> UpdatePatientAsync(Patient patient, CancellationToken cancellationToken = default);
    public Task DeletePatientAsync(int id, CancellationToken cancellationToken = default);
    public Task<List<Patient>> ListPatientsAsync(Expression<Func<Patient, bool>> filter, CancellationToken cancellationToken = default);
    public Task<List<Patient>> ListPatientsAsync(Expression<Func<Patient, bool>> filter, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    public Task<List<Patient>> GetListPatientsAsync(CancellationToken cancellationToken = default);
    public Task<List<Patient>> GetListPatientsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    public Task<Patient> GetPatientByIdAsync(int id, CancellationToken cancellationToken = default);
}
