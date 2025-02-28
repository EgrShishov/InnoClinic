using System.Linq.Expressions;

public interface IDoctorRepository
{
    public Task<Doctor> AddDoctorAsync(Doctor doctor, CancellationToken cancellationToken = default);
    public Task<Doctor> UpdateDoctorAsync(Doctor doctor, CancellationToken cancellationToken = default);
    public Task DeleteDoctorAsync(int id, CancellationToken cancellationToken = default);
    public Task<Doctor> GetDoctorByIdAsync(int id, CancellationToken cancellationToken = default);
    public Task<List<Doctor>> ListDoctorsAsync(Expression<Func<Doctor, bool>> filter, CancellationToken cancellationToken = default);
    public Task<List<Doctor>> ListDoctorsAsync(Expression<Func<Doctor, bool>> filter, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    public Task<List<Doctor>> GetListDoctorsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
}
