using Final_Insure.Models;

namespace Final_Insure.Repositories.Interfaces
{
    public interface ISettlementLogRepository
    {
        Task<IEnumerable<SettlementLog>> GetAllAsync();
        Task<SettlementLog?> GetByIdAsync(int id);
        Task AddAsync(SettlementLog settlementLog);
        Task UpdateAsync(SettlementLog settlementLog);
        Task DeleteAsync(int id);

        Task<SettlementLog?> GetSettlementByClaimIdAsync(int claimId);
    }
}