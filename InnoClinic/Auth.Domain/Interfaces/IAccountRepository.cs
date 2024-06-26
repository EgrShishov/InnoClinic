using Auth.Domain.Entities;

namespace Auth.Domain.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account> AddAsync(Account account, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<Account> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Account> GetByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<Account> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
    }
}
