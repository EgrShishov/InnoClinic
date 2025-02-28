using System.Linq.Expressions;

public interface IServiceRepository
{
    public Task<Service> GetServiceByIdAsync(int id, CancellationToken cancellationToken = default);
    public Task<IEnumerable<Service>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<IEnumerable<Service>> GetListAsync(Expression<Func<Service, bool>> filter, CancellationToken cancellationToken = default);
    public Task<Service> AddServiceAsync(Service service, CancellationToken cancellationToken = default);
    public Task UpdateServiceAsync(Service service, CancellationToken cancellationToken = default);
    public Task DeleteServiceAsync(int id, CancellationToken cancellationToken = default);
}
