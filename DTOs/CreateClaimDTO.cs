using System.ComponentModel.DataAnnotations;

namespace Final_Insure.DTOs
{
    public class CreateClaimDTO
    {
        [Required(ErrorMessage = "Please select a policy.")]
        public int PolicyId { get; set; }

        [Required(ErrorMessage = "Claim Type is required.")]
        [StringLength(50)]
        public required string ClaimType { get; set; }

        [Required(ErrorMessage = "Claim Amount is required.")]
        [Range(1, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal ClaimAmount { get; set; }

        [Required(ErrorMessage = "Incident Date is required.")]
        public DateTime IncidentDate { get; set; }

        [Required(ErrorMessage = "Please provide a description.")]
        [StringLength(1000)]
        public required string Description { get; set; }

        // NEW: Accident Address
        [Required(ErrorMessage = "Accident address is required.")]
        public string AccidentAddress { get; set; }

        // NEW: File Uploads (IFormFile is how ASP.NET catches uploaded files)
        [Required(ErrorMessage = "Please upload at least one photo of the damage.")]
        public IFormFile DamagePhoto { get; set; }

        [Required(ErrorMessage = "Please upload the FIR copy.")]
        public IFormFile FIRDocument { get; set; }

        // Optional Garage Receipt
        public IFormFile? GarageReceipt { get; set; }
    }
}