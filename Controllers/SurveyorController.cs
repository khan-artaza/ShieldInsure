using Final_Insure.DTOs;
using Final_Insure.Models;
using Final_Insure.Repositories.Interfaces;
using Final_Insure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Final_Insure.Controllers
{
    // Locks this controller down so ONLY Surveyors can access it
    [Authorize(Roles = "Surveyor")]
    public class SurveyorController : Controller
    {
        private readonly IAssessmentRepository _assessmentRepo;
        private readonly IClaimRepository _claimRepo;
        private readonly IAssessmentService _assessmentService;

        public SurveyorController(
            IAssessmentRepository assessmentRepo,
            IClaimRepository claimRepo,
            IAssessmentService assessmentService)
        {
            _assessmentRepo = assessmentRepo;
            _claimRepo = claimRepo;
            _assessmentService = assessmentService;
        }

        // Securely get the ID of the logged-in Surveyor
        private int GetCurrentUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
        }

        // 1. GET: Show claims assigned to the surveyors
        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var claims = await _claimRepo.GetAllAsync();

            // Only show claims that are currently under assessment
            var assignedClaims = claims.Where(c => c.ClaimStatus == ClaimStatus.UnderAssessment).ToList();

            return View(assignedClaims);
        }

        // 2. GET: Show the Assessment Form
        [HttpGet]
        public IActionResult SubmitAssessment(int claimId)
        {
            var dto = new SubmitAssessmentDTO { ClaimId = claimId };
            return View(dto);
        }

        // 3. POST: Save the Assessment and push the claim to Fraud Check
        [HttpPost]
        public async Task<IActionResult> SubmitAssessment(SubmitAssessmentDTO dto)
        {
            if (!ModelState.IsValid) return View(dto);

            int surveyorId = GetCurrentUserId();

            // Push the data to the service layer to save and update the status
            await _assessmentService.SubmitAssessmentAsync(dto, surveyorId);

            TempData["SuccessMessage"] = "Assessment successfully submitted. The claim has been sent for a Fraud Check!";
            return RedirectToAction("Dashboard");
        }

        // 2. GET: Show the Assessment Form
        [HttpGet]
        public async Task<IActionResult> InspectVehicle(int id)
        {
            // 1. Fetch the full claim from the database
            var claim = await _claimRepo.GetByIdAsync(id);
            if (claim == null) return NotFound();

            // 2. Pass the full claim to the View. 
            // This allows the left side of our split-screen UI to show the customer's actual incident details!
            return View(claim);
        }

    }
}