namespace Final_Insure.DTOs
{
    public class ClaimViewDTO
    {
        public int ClaimId { get; set; }
        public string PolicyNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string ClaimType { get; set; } = string.Empty;
        public decimal ClaimAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime SubmittedAt { get; set; }
        public string? AssignedOfficerName { get; set; }
    }
}