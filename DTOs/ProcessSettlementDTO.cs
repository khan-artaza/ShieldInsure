using System.ComponentModel.DataAnnotations;
using Final_Insure.Models;

namespace Final_Insure.DTOs
{
    public class ProcessSettlementDTO
    {
        [Required]
        public int ClaimId { get; set; }

        [Required(ErrorMessage = "You must select a final status (Approved or Rejected).")]
        public ClaimStatus FinalStatus { get; set; }

        [Required]
        public decimal SettlementAmount { get; set; }

        [StringLength(255)]
        public string? Remarks { get; set; }
        public string? Comments { get; set; }
    }
}