using System.ComponentModel.DataAnnotations;

namespace Final_Insure.DTOs
{
    public class AssignSurveyorDTO
    {
        [Required]
        public int ClaimId { get; set; }

        [Required(ErrorMessage = "You must select a surveyor.")]
        public int SurveyorId { get; set; }

        public string? OfficerNotes { get; set; }
    }
}