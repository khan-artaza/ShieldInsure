using System.ComponentModel.DataAnnotations;

namespace Final_Insure.Models
{
    public class Assessment
    {
        [Key]
        public int AssessmentId { get; set; }

        [Required]
        public int ClaimId { get; set; }

        [Required]
        public int SurveyorId { get; set; }

        [Required(ErrorMessage = "Assessed Amount is required.")]
        public decimal AssessedAmount { get; set; }

        [StringLength(255)]
        public string? AssessmentRemarks { get; set; }

        // --- NEW FIELDS ADDED FOR SURVEYOR ---
        [Required(ErrorMessage = "Vehicle registration number is required.")]
        [StringLength(50)]
        public string CarNumber { get; set; } = string.Empty;

        [Required]
        public bool AccidentMatchesDescription { get; set; } // Fraud Check (Yes/No)

        [StringLength(255)]
        public string? GarageDetails { get; set; } // Where is the car located/fixed?

        [StringLength(500)]
        public string? EvidencePhotoPath { get; set; } // Path to save the uploaded inspection photo
        // -------------------------------------

        public DateTime AssessedAt { get; set; } = DateTime.UtcNow;

        public Claim? Claim { get; set; }
        public User? Surveyor { get; set; }
    }
}