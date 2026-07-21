using System.ComponentModel.DataAnnotations;
using Final_Insure.Models;

namespace Final_Insure.DTOs
{
    public class SubmitFraudCheckDTO
    {
        [Required]
        public int ClaimId { get; set; }

        [Required(ErrorMessage = "Fraud Score is required.")]
        [Range(1, 100, ErrorMessage = "Score must be between 1 and 100.")]
        public int FraudScore { get; set; }

        [Required]
        public RiskFlag RiskFlag { get; set; }

        [StringLength(500)]
        public string? InvestigatorRemarks { get; set; }
    }
}