using System.Linq.Expressions;
public interface IOfficeRepository
{
    public Task<List<Office>> GetOfficesAsync(CancellationToken cancellationToken = default);
    public Task<List<Office>> ListOfficesAsync(Expression<Func<Office, bool>> filter, CancellationToken cancellationToken = default);
    public Task<Office> GetOfficeByIdAsync(string id, CancellationToken cancellationToken = default);
    public Task<string> AddOfficeAsync(Office office, CancellationToken cancellationToken = default);
    public Task UpdateOfficeAsync(Office office, CancellationToken cancellationToken = default);
    public Task DeleteOfficeAsync(string id, CancellationToken cancellationToken = default);
}
