using Final_Insure.Models;

namespace Final_Insure.Repositories.Interfaces
{
    public interface IFraudCheckRepository
    {
        Task<IEnumerable<FraudCheck>> GetAllAsync();
        Task<FraudCheck?> GetByIdAsync(int id);
        Task AddAsync(FraudCheck fraudCheck);
        Task UpdateAsync(FraudCheck fraudCheck);
        Task DeleteAsync(int id);

        Task<FraudCheck?> GetFraudCheckByClaimIdAsync(int claimId);
    }
}