using System.ComponentModel.DataAnnotations;

namespace Final_Insure.Models
{
    public class SettlementLog
    {
        [Key]
        public int SettlementId { get; set; }

        [Required]
        public int ClaimId { get; set; }

        [Required(ErrorMessage = "Settlement Amount is required.")]
        public decimal SettlementAmount { get; set; }

        [Required(ErrorMessage = "Settlement Status is required.")]
        [StringLength(50)]
        public required string SettlementStatus { get; set; }

        [StringLength(255)]
        public string? Remarks { get; set; }

        public DateTime ProcessedAt { get; set; } = DateTime.UtcNow;

        public Claim? Claim { get; set; }
    }
}