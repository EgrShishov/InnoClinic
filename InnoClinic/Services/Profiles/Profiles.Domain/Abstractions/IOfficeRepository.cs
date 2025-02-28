using System.Linq.Expressions;

public interface IOfficeRepository
{
    public Task<Office> AddOfficeAsync(Office office, CancellationToken cancellationToken = default);
    public Task<Office> UpdateOfficeAsync(Office office, CancellationToken cancellationToken = default);
    public Task DeleteOfficeAsync(string id, CancellationToken cancellationToken = default);
    public Task<Office> GetOfficeByIdAsync(string id, CancellationToken cancellationToken = default);
    public Task<List<Office>> ListOfficiesAsync(Expression<Func<Office, bool>> filter, CancellationToken cancellationToken = default);
    public Task<List<Office>> GetListOfficesAsync(CancellationToken cancellationToken = default);
}