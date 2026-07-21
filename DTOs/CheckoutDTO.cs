using System.ComponentModel.DataAnnotations;

namespace Final_Insure.DTOs
{
    public class CheckoutDTO
    {
        [Required]
        public int PlanId { get; set; }

        public string? PlanName { get; set; }

        public decimal PremiumAmount { get; set; }

        public decimal CoverageAmount { get; set; }

        [Required(ErrorMessage = "Vehicle Registration Certificate (RC) is required.")]
        public string VehicleRC { get; set; } = string.Empty;

        [Required(ErrorMessage = "Chassis Number is required.")]
        public string ChassisNumber { get; set; } = string.Empty;
    }
}