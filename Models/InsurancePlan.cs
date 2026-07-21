using System.ComponentModel.DataAnnotations;

namespace Final_Insure.Models
{
    public class InsurancePlan
    {
        [Key]
        public int PlanId { get; set; }

        [Required]
        public required string PlanName { get; set; }

        [Required]
        public required string PlanType { get; set; }

        public required string Description { get; set; }

        [Required]
        public decimal BasePremium { get; set; }

        [Required]
        public decimal CoverageAmount { get; set; }

        public bool IsActive { get; set; } = true;
    }
}