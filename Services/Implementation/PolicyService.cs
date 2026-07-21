using Final_Insure.DTOs;
using Final_Insure.Models;
using Final_Insure.Repositories.Interfaces;
using Final_Insure.Services.Interfaces;

namespace Final_Insure.Services.Implementations
{
    public class PolicyService : IPolicyService
    {
        private readonly IPolicyRepository _policyRepo;

        public PolicyService(IPolicyRepository policyRepo)
        {
            _policyRepo = policyRepo;
        }

        public async Task<Policy> CreatePolicyAsync(CreatePolicyDTO dto)
        {
            var policy = new Policy
            {
                CustomerId = dto.CustomerId,
                PolicyNumber = dto.PolicyNumber,
                PolicyType = dto.PolicyType,
                CoverageAmount = dto.CoverageAmount,
                PremiumAmount = dto.PremiumAmount,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Status = PolicyStatus.Active // Default to active when created
            };

            await _policyRepo.AddAsync(policy);
            return policy;
        }
    }
}