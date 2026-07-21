using Final_Insure.DTOs;
using Final_Insure.Models;
using Final_Insure.Repositories.Interfaces;
using Final_Insure.Services.Interfaces;

namespace Final_Insure.Services.Implementations
{
    public class AssessmentService : IAssessmentService
    {
        private readonly IAssessmentRepository _assessmentRepo;
        private readonly IClaimRepository _claimRepo;

        public AssessmentService(IAssessmentRepository assessmentRepo, IClaimRepository claimRepo)
        {
            _assessmentRepo = assessmentRepo;
            _claimRepo = claimRepo;
        }

        public async Task<Assessment> SubmitAssessmentAsync(SubmitAssessmentDTO dto, int surveyorId)
        {
            // 1. Save the surveyor's assessment
            var assessment = new Assessment
            {
                ClaimId = dto.ClaimId,
                SurveyorId = surveyorId,
                AssessedAmount = dto.AssessedAmount,
                AssessmentRemarks = dto.AssessmentRemarks,
                AccidentMatchesDescription = dto.AccidentMatchesDescription,
                AssessedAt = DateTime.UtcNow
            };
            await _assessmentRepo.AddAsync(assessment);

            // 2. Move the claim to the Compliance Officer's desk automatically!
            var claim = await _claimRepo.GetByIdAsync(dto.ClaimId);
            if (claim != null)
            {
                claim.ClaimStatus = ClaimStatus.PendingCompliance;
                await _claimRepo.UpdateAsync(claim);
            }

            return assessment;
        }
    }
}