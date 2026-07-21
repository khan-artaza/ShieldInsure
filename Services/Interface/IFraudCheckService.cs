using Final_Insure.DTOs;
using Final_Insure.Models;

namespace Final_Insure.Services.Interfaces
{
    public interface IFraudCheckService
    {
        Task<FraudCheck> SubmitFraudCheckAsync(SubmitFraudCheckDTO dto, int complianceOfficerId);
    }
}