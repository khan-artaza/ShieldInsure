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
        Registered,            // 0: Brand new claim
        Assigned,              // 1
        UnderAssessment,       // 2
        PendingSurvey,         // 3: Waiting on Surveyor
        PendingCompliance,     // 4: Waiting on Compliance Officer
        PendingFinalApproval,  // 5: Back on Claim Officer's desk
        Approved,              // 6
        Rejected,              // 7
        Settled,               // 8

        // ADD NEW STATUSES AT THE END TO PREVENT DATABASE CORRUPTION
        AwaitingDocuments      // 9: Waiting on customer to re-upload docs
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