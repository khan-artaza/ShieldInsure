using Final_Insure.DTOs;
using Final_Insure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Final_Insure.Controllers
{
    // Locks this down so ONLY Compliance Officers can access it
    [Authorize(Roles = "ComplianceOfficer")]
    public class ComplianceOfficerController : Controller
    {
        private readonly AppDbContext _context;

        public ComplianceOfficerController(AppDbContext context)
        {
            _context = context;
        }

        // Helper method to securely get the logged-in user's ID
        private int GetCurrentUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
        }

        // 1. GET: Show claims waiting for fraud check
        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var claims = await _context.Claims
                .Where(c => c.ClaimStatus == ClaimStatus.PendingCompliance)
                .ToListAsync();

            return View(claims);
        }

        // 2. GET: Show the Fraud Check Form
        // 2. GET: Show the Fraud Check Form and Auto-Calculate Score
        [HttpGet]
        public async Task<IActionResult> SubmitFraudCheck(int claimId)
        {
            // Fetch claim with Policy, and fetch the Surveyor's Assessment
            var claim = await _context.Claims
                .Include(c => c.Policy) // Assuming you have a Policy navigation property
                .FirstOrDefaultAsync(c => c.ClaimId == claimId);

            var assessment = await _context.Assessments
                .FirstOrDefaultAsync(a => a.ClaimId == claimId);

            if (claim == null) return NotFound();

            // --- 🤖 AUTOMATED 3-RULE FRAUD SCORE MATH ---
            int autoScore = 1; // Start at 1 (since your DTO requires 1-100)

            // Rule 1: Policy is Inactive (+50 Points)
            // Adjust 'PolicyStatus.Active' to match your actual enum
            if (claim.Policy != null && claim.Policy.Status != PolicyStatus.Active) autoScore += 50;

            // Rule 2: Surveyor Marked Suspicious (+40 Points)
            if (assessment != null && assessment.AccidentMatchesDescription == false) autoScore += 40;

            // Rule 3: Claim within 30 days of Policy Purchase (+25 Points)
            if (claim.Policy != null)
            {
                var daysSincePurchase = (claim.IncidentDate - claim.Policy.StartDate).TotalDays;
                if (daysSincePurchase <= 30 && daysSincePurchase >= 0) autoScore += 25;
            }

            if (autoScore > 100) autoScore = 100;

            // Auto-Assign the RiskFlag based on the score
            RiskFlag calculatedFlag = RiskFlag.Low;
            if (autoScore >= 80) calculatedFlag = RiskFlag.Critical;
            else if (autoScore >= 50) calculatedFlag = RiskFlag.High;
            else if (autoScore >= 25) calculatedFlag = RiskFlag.Medium;

            // Pass the raw data to ViewBag so the HTML UI can display the details on the left side
            ViewBag.ClaimData = claim;
            ViewBag.AssessmentData = assessment;

            // Pre-fill your exact DTO with the calculated data!
            var dto = new SubmitFraudCheckDTO
            {
                ClaimId = claimId,
                FraudScore = autoScore,
                RiskFlag = calculatedFlag
            };

            return View(dto);
        }

        // 3. POST: Process the Fraud Check results
        [HttpPost]
        public async Task<IActionResult> SubmitFraudCheck(SubmitFraudCheckDTO dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var claim = await _context.Claims.FindAsync(dto.ClaimId);
            if (claim == null) return NotFound();

            // Business Logic: If Risk is High/Critical OR Score is 80+, it is considered fraudulent
            bool isFraudDetected = dto.RiskFlag == RiskFlag.High ||
                                   dto.RiskFlag == RiskFlag.Critical ||
                                   dto.FraudScore >= 80;

            // Create the Fraud Check database record
            var fraudCheck = new FraudCheck
            {
                ClaimId = dto.ClaimId,
                ComplianceOfficerId = GetCurrentUserId(),
                CheckDate = DateTime.UtcNow,
                IsFraudulent = isFraudDetected,

                // Combine your detailed DTO fields into the existing Remarks column for the database
                Remarks = $"Score: {dto.FraudScore}/100 | Flag: {dto.RiskFlag} | Notes: {dto.InvestigatorRemarks ?? "None"}"
            };

            _context.FraudChecks.Add(fraudCheck);

            // Route the claim based on the strict evaluation
            if (isFraudDetected)
            {
                claim.ClaimStatus = ClaimStatus.Rejected;
                TempData["SuccessMessage"] = $"Fraud Detected (Score: {dto.FraudScore})! The claim has been automatically rejected.";
            }
            else
            {
                claim.ClaimStatus = ClaimStatus.PendingFinalApproval;
                TempData["SuccessMessage"] = "Fraud check cleared. Claim sent to Claim Officer for final payout approval.";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Dashboard");
        }
    }
}