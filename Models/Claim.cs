using System.ComponentModel.DataAnnotations;

namespace Final_Insure.Models
{
    public class Claim
    {
        [Key]
        public int ClaimId { get; set; }

        [Required]
        public int PolicyId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Claim Type is required.")]
        [StringLength(50)]
        public required string ClaimType { get; set; }

        [Required(ErrorMessage = "Claim Amount is required.")]
        public decimal ClaimAmount { get; set; }

        [Required]
        public ClaimStatus ClaimStatus { get; set; } = ClaimStatus.Registered;

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public Policy? Policy { get; set; }
        public User? Customer { get; set; }
        
        [Required(ErrorMessage = "Incident Date is required.")]
        public DateTime IncidentDate { get; set; }

        [Required(ErrorMessage = "Please provide a description of the incident.")]
        [StringLength(1000)]
        public required string Description { get; set; }

        [Required(ErrorMessage = "Please provide the address where the incident occurred.")]
        [StringLength(500)]
        public string AccidentAddress { get; set; } = string.Empty;

        // Tracks which Claim Officer is currently handling the file
        public int? AssignedOfficerId { get; set; }
        public User? AssignedOfficer { get; set; }
        public ICollection<ClaimDocument> ClaimDocuments { get; set; } = new List<ClaimDocument>();
        public ICollection<Assessment> Assessments { get; set; } = new List<Assessment>();
        public ICollection<FraudCheck> FraudChecks { get; set; } = new List<FraudCheck>();
        public ICollection<SettlementLog> SettlementLogs { get; set; } = new List<SettlementLog>();
    }
}