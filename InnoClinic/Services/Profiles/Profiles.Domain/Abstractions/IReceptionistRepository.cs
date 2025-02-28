using System.Linq.Expressions;

public interface IReceptionistRepository
{
    public Task<Receptionist> AddReceptionistAsync(Receptionist receptionist, CancellationToken cancellationToken = default);
    public Task<Receptionist> UpdateReceptionistAsync(Receptionist receptionist, CancellationToken cancellationToken = default);
    public Task DeleteReceptionistAsync(int id, CancellationToken cancellationToken = default);
    public Task<List<Receptionist>> ListReceptionistAsync(Expression<Func<Receptionist, bool>> filter, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    public Task<List<Receptionist>> GetListReceptionistAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    public Task<Receptionist> GetReceptionistByIdAsync(int id, CancellationToken cancellationToken = default);
}
