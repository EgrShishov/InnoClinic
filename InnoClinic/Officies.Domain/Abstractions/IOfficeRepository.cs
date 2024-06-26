using Officies.Domain.Entities;
using System.Linq.Expressions;

namespace Officies.Domain.Abstractions
{
    public interface IOfficeRepository
    {
        Task<List<Office>> GetOfficesAsync();
        Task<List<Office>> ListOfficesAsync(Expression<Func<Office, bool>> filter);
        Task<Office> GetOfficeByIdAsync(string id);
        Task AddOfficeAsync(Office office);
        Task UpdateOfficeAsync(Office office);
        Task DeleteOfficeAsync(string id);
    }
}
