using Final_Insure.DTOs;
using Final_Insure.Models;

namespace Final_Insure.Services.Interfaces
{
    public interface IClaimService
    {
        Task<Claim> RegisterClaimAsync(CreateClaimDTO dto, int customerId);
        Task MoveToSurveyorAsync(AssignSurveyorDTO dto);
        Task ProcessFinalSettlementAsync(ProcessSettlementDTO dto);
    }
}