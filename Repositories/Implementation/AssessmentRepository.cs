using Final_Insure.Models;
using Final_Insure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Final_Insure.Repositories.Implementations
{
    public class AssessmentRepository : IAssessmentRepository
    {
        private readonly AppDbContext _context;
        public AssessmentRepository(AppDbContext context) { _context = context; }

        public async Task<IEnumerable<Assessment>> GetAllAsync() => await _context.Assessments.ToListAsync();
        public async Task<Assessment?> GetByIdAsync(int id) => await _context.Assessments.FindAsync(id);

        public async Task AddAsync(Assessment assessment)
        {
            await _context.Assessments.AddAsync(assessment);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Assessment assessment)
        {
            _context.Assessments.Update(assessment);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var assessment = await _context.Assessments.FindAsync(id);
            if (assessment != null)
            {
                _context.Assessments.Remove(assessment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Assessment?> GetAssessmentByClaimIdAsync(int claimId) =>
            await _context.Assessments.FirstOrDefaultAsync(a => a.ClaimId == claimId);

        public async Task<IEnumerable<Assessment>> GetAssessmentsBySurveyorIdAsync(int surveyorId) =>
            await _context.Assessments
                .Include(a => a.Claim)
                .Where(a => a.SurveyorId == surveyorId)
                .ToListAsync();
    }
}