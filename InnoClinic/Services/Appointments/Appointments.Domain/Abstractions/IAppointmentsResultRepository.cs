using System.Linq.Expressions;

public interface IAppointmentsResultRepository
{
    public Task<Results> GetResultsByIdAsync(int id, CancellationToken cancellationToken = default);
    public Task<IEnumerable<Results>> GetAllResultsAsync(CancellationToken cancellationToken = default);
    public Task<IEnumerable<Results>> ListAsync(Expression<Func<Results, bool>> filter, CancellationToken cancellationToken = default);
    public Task<Results> AddResultsAsync(Results results, CancellationToken cancellationToken = default);
    public Task<Results> UpdateResultsAsync(Results results, CancellationToken cancellationToken = default);
    public Task DeleteResultsAsync(int id, CancellationToken cancellationToken = default);
}
