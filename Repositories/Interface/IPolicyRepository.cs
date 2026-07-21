using Final_Insure.Models;

namespace Final_Insure.Repositories.Interfaces
{
    public interface IPolicyRepository
    {
        Task<IEnumerable<Policy>> GetAllAsync();
        Task<Policy?> GetByIdAsync(int id);
        Task AddAsync(Policy policy);
        Task UpdateAsync(Policy policy);
        Task DeleteAsync(int id);

        Task<IEnumerable<Policy>> GetPoliciesByCustomerIdAsync(int customerId);
        Task<IEnumerable<Policy>> GetActivePoliciesAsync();
    }
}