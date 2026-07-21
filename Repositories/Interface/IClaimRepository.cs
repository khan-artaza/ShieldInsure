using Final_Insure.Models;

namespace Final_Insure.Repositories.Interfaces
{
    public interface IClaimRepository
    {
        Task<IEnumerable<Claim>> GetAllAsync();
        Task<Claim?> GetByIdAsync(int id);
        Task AddAsync(Claim claim);
        Task UpdateAsync(Claim claim);
        Task DeleteAsync(int id);

        Task<IEnumerable<Claim>> GetClaimsByAssignedOfficerAsync(int officerId);
        Task<IEnumerable<Claim>> GetClaimsByStatusAsync(ClaimStatus status);
        Task<IEnumerable<Claim>> GetClaimsByCustomerAsync(int customerId);
    }
}