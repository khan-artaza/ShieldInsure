using System.ComponentModel.DataAnnotations;

namespace Final_Insure.Models
{
    public class ClaimDocument
    {
        [Key]
        public int DocumentId { get; set; }

        [Required]
        public int ClaimId { get; set; }

        [Required(ErrorMessage = "Document Name is required.")]
        [StringLength(100)]
        public required string DocumentName { get; set; }

        [Required]
        [StringLength(255)]
        public required string FilePath { get; set; }

        [Required]
        public VerificationStatus VerificationStatus { get; set; } = VerificationStatus.Pending;

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        public Claim? Claim { get; set; }   
    }
}