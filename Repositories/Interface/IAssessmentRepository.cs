using Final_Insure.Models;

namespace Final_Insure.Repositories.Interfaces
{
    public interface IAssessmentRepository
    {
        Task<IEnumerable<Assessment>> GetAllAsync();
        Task<Assessment?> GetByIdAsync(int id);
        Task AddAsync(Assessment assessment);
        Task UpdateAsync(Assessment assessment);
        Task DeleteAsync(int id);

        Task<Assessment?> GetAssessmentByClaimIdAsync(int claimId);
        Task<IEnumerable<Assessment>> GetAssessmentsBySurveyorIdAsync(int surveyorId);
    }
}