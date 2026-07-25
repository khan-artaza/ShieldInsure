using Final_Insure.DTOs;
using Final_Insure.Models;
using Final_Insure.Repositories.Interfaces;
using Final_Insure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Final_Insure.Controllers
{
    // Locks this controller down so ONLY Claim Officers can access it
    [Authorize(Roles = "ClaimOfficer")]
    public class ClaimOfficerController : Controller
    {
        private readonly IClaimRepository _claimRepo;
        private readonly IUserRepository _userRepo;
        private readonly IClaimDocumentRepository _docRepo;
        private readonly IClaimService _claimService;
        private readonly IAssessmentRepository _assessmentRepo;

        public ClaimOfficerController(
            IClaimRepository claimRepo,
            IUserRepository userRepo,
            IClaimDocumentRepository docRepo,
            IClaimService claimService,
            IAssessmentRepository assessmentRepo)
        {
            _claimRepo = claimRepo;
            _userRepo = userRepo;
            _docRepo = docRepo;
            _claimService = claimService;
            _assessmentRepo = assessmentRepo;
        }

        // 1. GET: Show claims needing attention
        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            // Fetch ALL claims so our new Dashboard UI can sort them into the 4 buckets
            // (New, In Investigation, Ready for Decision, History)
            var allClaims = await _claimRepo.GetAllAsync();

            return View(allClaims);
        }

        // 2. GET: Show the full details of a specific claim AND the Assignment Dropdown
        [HttpGet]
        public async Task<IActionResult> ClaimDetails(int claimId)
        {
            // Fetch the claim using your Repository
            var claim = await _claimRepo.GetByIdAsync(claimId);
            if (claim == null) return NotFound();

            // Fetch a list of all Surveyors using your Repository
            var allSurveyors = await _userRepo.GetUsersByRoleAsync(UserRole.Surveyor);

            // Only pass APPROVED Surveyors to the view
            ViewBag.AvailableSurveyors = allSurveyors.Where(u => u.IsApproved).ToList();

            return View(claim);
        }

        // 3. POST: Assign the claim to the chosen Surveyor
        [HttpPost]
        public async Task<IActionResult> AssignSurveyor(int claimId, int surveyorId)
        {
            // We map the incoming data directly into your existing DTO
            var dto = new AssignSurveyorDTO
            {
                ClaimId = claimId,
                SurveyorId = surveyorId
            };

            // We use your existing Service to handle the database save and status update!
            await _claimService.MoveToSurveyorAsync(dto);

            TempData["SuccessMessage"] = $"Claim #{claimId} has been successfully assigned to the Surveyor.";
            return RedirectToAction("Dashboard");
        }

        // =========================================================================
        // NEW: 3A. GET: View Documents before assignment
        // =========================================================================
        [HttpGet]
        public async Task<IActionResult> ViewDocuments(int claimId)
        {
            var claim = await _claimRepo.GetByIdAsync(claimId);
            if (claim == null) return NotFound();

            // Return the claim to a view so the officer can see the attached docs.
            // Note: Make sure your GetByIdAsync method Includes the ClaimDocuments.
            return View(claim);
        }

        // =========================================================================
        // NEW: 3B. POST: Reject claim due to bad documents
        // =========================================================================
        [HttpPost]
        public async Task<IActionResult> RejectInvalidDocs(int ClaimId, string Reason)
        {
            if (ClaimId <= 0 || string.IsNullOrEmpty(Reason))
            {
                TempData["ErrorMessage"] = "Invalid request.";
                return RedirectToAction("Dashboard");
            }

            var claim = await _claimRepo.GetByIdAsync(ClaimId);
            if (claim == null) return NotFound();

            // Set the remarks explaining why it was rejected
            claim.OfficerRemarks = Reason + " - Docs are inappropriate, please file a new claim with correct docs.";

            // Completely reject the claim instead of waiting for re-upload
            claim.ClaimStatus = ClaimStatus.Rejected;

            await _claimRepo.UpdateAsync(claim);

            TempData["SuccessMessage"] = $"Claim #{ClaimId} rejected due to inappropriate documents. The customer must file a new claim.";
            return RedirectToAction("Dashboard");
        }

        // 4. GET: Form for final approval/rejection
        [HttpGet]
        public async Task<IActionResult> ProcessSettlement(int claimId)
        {
            var claim = await _claimRepo.GetByIdAsync(claimId);
            if (claim == null) return NotFound();

            // 1. Fetch the Surveyor's Assessment from the database
            // (If you don't have _assessmentRepo injected here, you can inject it in the constructor, 
            // or use _context.Assessments.FirstOrDefaultAsync...)
            var assessment = await _assessmentRepo.GetAssessmentByClaimIdAsync(claimId);

            // 2. Pass BOTH to the view so the UI can display them side-by-side
            ViewBag.ClaimData = claim;
            ViewBag.AssessmentData = assessment;

            // 3. Set the default Settlement Amount to the SURVEYOR'S assessed amount!
            decimal suggestedAmount = assessment != null ? assessment.AssessedAmount : claim.ClaimAmount;

            var dto = new ProcessSettlementDTO
            {
                ClaimId = claimId,
                SettlementAmount = suggestedAmount // <-- THIS FIXES BUG 3 (Right Side Form)
            };

            return View(dto);
        }

        // 5. POST: Finalize the claim
        [HttpPost]
        public async Task<IActionResult> ProcessSettlement(ProcessSettlementDTO dto)
        {
            if (!ModelState.IsValid) return View(dto);

            await _claimService.ProcessFinalSettlementAsync(dto);

            TempData["SuccessMessage"] = $"Claim has been {dto.FinalStatus}!";
            return RedirectToAction("Dashboard");
        }

        // 6. POST: Update a specific document's verification status (Verified / Rejected)
        [HttpPost]
        public async Task<IActionResult> UpdateDocumentStatus(int documentId, VerificationStatus status)
        {
            var doc = await _docRepo.GetByIdAsync(documentId);
            if (doc == null) return NotFound();

            doc.VerificationStatus = status;
            await _docRepo.UpdateAsync(doc);

            TempData["SuccessMessage"] = "Document status updated.";
            return RedirectToAction("ProcessSettlement", new { claimId = doc.ClaimId });
        }

        // 7. POST: Reject a claim immediately (used when documents are invalid)
        [HttpPost]
        public async Task<IActionResult> RejectClaim(int claimId, string? reason)
        {
            var claim = await _claimRepo.GetByIdAsync(claimId);
            if (claim == null) return NotFound();

            claim.ClaimStatus = ClaimStatus.Rejected;
            await _claimRepo.UpdateAsync(claim);

            TempData["ErrorMessage"] = "Claim has been rejected due to invalid documentation.";
            return RedirectToAction("Dashboard");
        }

        // GET: View Final Settlement/Payment Receipt
        [HttpGet]
        public async Task<IActionResult> PaymentReceipt(int claimId)
        {
            var claim = await _claimRepo.GetByIdAsync(claimId);

            // If the claim doesn't exist in the database, this triggers the "Not Found" error!
            if (claim == null) return NotFound();

            return View(claim);
        }
    }
}