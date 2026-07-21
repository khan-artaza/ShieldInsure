using Final_Insure.DTOs;
using Final_Insure.Models;

namespace Final_Insure.Services.Interfaces
{
    public interface IPolicyService
    {
        Task<Policy> CreatePolicyAsync(CreatePolicyDTO dto);
    }
}