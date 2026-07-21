using System.ComponentModel.DataAnnotations;

namespace Final_Insure.DTOs
{
    public class CreatePolicyDTO
    {
        [Required]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Policy Number is required.")]
        [StringLength(50)]
        public required string PolicyNumber { get; set; }

        [Required(ErrorMessage = "Policy Type is required.")]
        [StringLength(50)]
        public required string PolicyType { get; set; }

        [Required]
        public decimal CoverageAmount { get; set; }

        [Required]
        public decimal PremiumAmount { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }
}