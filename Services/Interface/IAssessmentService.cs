using Final_Insure.DTOs;
using Final_Insure.Models;

namespace Final_Insure.Services.Interfaces
{
    public interface IAssessmentService
    {
        Task<Assessment> SubmitAssessmentAsync(SubmitAssessmentDTO dto, int surveyorId);
    }
}