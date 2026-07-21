namespace Final_Insure.Models
{
    public enum UserRole
    {
        Customer,
        ClaimOfficer,
        Surveyor,
        ComplianceOfficer,
        Admin
    }

    public enum ClaimStatus
    {
        Registered,            // Brand new claim
        Assigned,
        UnderAssessment,
        PendingSurvey,         // Waiting on Surveyor
        PendingCompliance,     // Waiting on Compliance Officer
        PendingFinalApproval,  // Back on Claim Officer's desk
        Approved,
        Rejected,
        Settled
    }

    public enum VerificationStatus
    {
        Pending,
        Verified,
        Rejected
    }

    public enum RiskFlag
    {
        Low,
        Medium,
        High,
        Critical
    }
    public enum PolicyStatus
    {
        Active,
        Expired,
        Cancelled
    }

    
}