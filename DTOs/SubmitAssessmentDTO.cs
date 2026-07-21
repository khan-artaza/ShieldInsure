using System.ComponentModel.DataAnnotations;

namespace Final_Insure.DTOs
{
    public class SubmitAssessmentDTO
    {
        [Required]
        public int ClaimId { get; set; }

        [Required]
        public decimal AssessedAmount { get; set; }

        [StringLength(255)]
        public string? AssessmentRemarks { get; set; }
        public string? SurveyorNotes { get; set; }

        [Required]
        public bool AccidentMatchesDescription { get; set; }
    }
}