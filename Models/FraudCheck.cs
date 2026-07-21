using System.ComponentModel.DataAnnotations;

namespace Final_Insure.Models
{
    public class FraudCheck
    {
        [Key]
        public int FraudId { get; set; }

        [Required]
        public int ClaimId { get; set; }

        [Required]
        public int FraudScore { get; set; }

        [Required]
        public RiskFlag RiskFlag { get; set; }

        public DateTime CheckedAt { get; set; } = DateTime.UtcNow;

        public Claim? Claim { get; set; }
        public int? ComplianceOfficerId { get; set; }
        public User? ComplianceOfficer { get; set; }

        [StringLength(500)]
        public string? InvestigatorRemarks { get; set; }

        public DateTime CheckDate { get; set; }

        public bool IsFraudulent { get; set; }

        public string? Remarks { get; set; }
    }
}