using Final_Insure.Models;

namespace Final_Insure.Repositories.Interfaces
{
    public interface IClaimDocumentRepository
    {
        Task<IEnumerable<ClaimDocument>> GetAllAsync();
        Task<ClaimDocument?> GetByIdAsync(int id);
        Task AddAsync(ClaimDocument document);
        Task UpdateAsync(ClaimDocument document);
        Task DeleteAsync(int id);

        Task<IEnumerable<ClaimDocument>> GetDocumentsByClaimIdAsync(int claimId);
    }
}