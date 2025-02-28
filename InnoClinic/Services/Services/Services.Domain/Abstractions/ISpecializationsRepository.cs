public interface ISpecializationsRepository
{
    public Task<Specialization> GetSpecializationByIdAsync(int id, CancellationToken cancellationToken = default);
    public Task<IEnumerable<Specialization>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<Specialization> AddSpecializationAsync(Specialization specialization, CancellationToken cancellationToken = default);
    public Task UpdateSpecializationAsync(Specialization specialization, CancellationToken cancellationToken = default);
    public Task DeleteSpecializationAsync(int id, CancellationToken cancellationToken = default);
}
