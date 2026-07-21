using Final_Insure.DTOs;
using Final_Insure.Models;
using Final_Insure.Repositories.Interfaces;
using Final_Insure.Services.Interfaces;

namespace Final_Insure.Services.Implementations
{
    public class FraudCheckService : IFraudCheckService
    {
        private readonly IFraudCheckRepository _fraudCheckRepo;
        private readonly IClaimRepository _claimRepo;

        public FraudCheckService(IFraudCheckRepository fraudCheckRepo, IClaimRepository claimRepo)
        {
            _fraudCheckRepo = fraudCheckRepo;
            _claimRepo = claimRepo;
        }

        public async Task<FraudCheck> SubmitFraudCheckAsync(SubmitFraudCheckDTO dto, int complianceOfficerId)
        {
            // 1. Save the fraud check results
            var fraudCheck = new FraudCheck
            {
                ClaimId = dto.ClaimId,
                ComplianceOfficerId = complianceOfficerId,
                FraudScore = dto.FraudScore,
                RiskFlag = dto.RiskFlag,
                InvestigatorRemarks = dto.InvestigatorRemarks,
                CheckedAt = DateTime.UtcNow
            };
            await _fraudCheckRepo.AddAsync(fraudCheck);

            // 2. Send the claim back to the Claim Officer for final approval
            var claim = await _claimRepo.GetByIdAsync(dto.ClaimId);
            if (claim != null)
            {
                claim.ClaimStatus = ClaimStatus.PendingFinalApproval;
                await _claimRepo.UpdateAsync(claim);
            }

            return fraudCheck;
        }
    }
}