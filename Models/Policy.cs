using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Final_Insure.Models
{
    public class Policy
    {
        [Key]
        public int PolicyId { get; set; }

        [Required(ErrorMessage = "Policy Number is required.")]
        [StringLength(50)]
        public required string PolicyNumber { get; set; }

        [Required(ErrorMessage = "Policy Type is required.")]
        [StringLength(50)]
        public required string PolicyType { get; set; }

        [Required(ErrorMessage = "Coverage Amount is required.")]
        public decimal CoverageAmount { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        
        public int? CustomerId { get; set; }

        public User? Customer { get; set; }
        
        [Required]
        public decimal PremiumAmount { get; set; }

        [Required]
        public PolicyStatus Status { get; set; } = PolicyStatus.Active;
        public ICollection<Claim> Claims { get; set; } = new List<Claim>();


        // Vehicle-specific details (nullable because Health/Property policies wouldn't need them)
        public string? VehicleRC { get; set; }
        public string? ChassisNumber { get; set; }
    }
}