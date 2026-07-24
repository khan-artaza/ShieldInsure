using Final_Insure.DTOs;
using Final_Insure.Models;
using Final_Insure.Repositories.Interfaces;
using Final_Insure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Final_Insure.Controllers
{

    [Authorize(Roles = "Customer")]
    public class CustomerController : Controller
    {
        private readonly IPolicyRepository _policyRepo;
        private readonly IClaimRepository _claimRepo;
        private readonly IClaimService _claimService;
        private readonly AppDbContext _context;

        public CustomerController(
            IPolicyRepository policyRepo,
            IClaimRepository claimRepo,
            IClaimService claimService,
            AppDbContext context)
        {
            _policyRepo = policyRepo;
            _claimRepo = claimRepo;
            _claimService = claimService;
            _context = context;
        }



        // Helper method to get the logged-in user's ID securely from the cookie
        private int GetCurrentUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
        }


        // GET: Customer Dashboard
        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            // 1. Securely get the logged-in customer's ID
            int currentUserId = int.Parse(User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier) ?? "0");

            // 2. RESTORED: Fetch all policies for this customer
            var myPolicies = await _context.Policies
                .Where(p => p.CustomerId == currentUserId)
                .ToListAsync();

            // Pass policies to the view (Adjust this if you used a ViewModel instead of ViewBag)
            ViewBag.Policies = myPolicies;

            // 3. Fetch all claims for this specific customer
            var myClaims = await _context.Claims
                .Where(c => c.CustomerId == currentUserId)
                .ToListAsync();

            // 4. Fetch all assessments linked to this customer's claims
            var claimIds = myClaims.Select(c => c.ClaimId).ToList();
            var myAssessments = await _context.Assessments
                .Where(a => claimIds.Contains(a.ClaimId))
                .ToListAsync();

            // Pass the assessments to the View
            ViewBag.Assessments = myAssessments;

            return View(myClaims);
        }

        // 2. GET: Show the Form to File a Claim
        [HttpGet]
        public async Task<IActionResult> FileClaim()
        {
            int customerId = GetCurrentUserId();

            // We need to pass the policies to the view so they can select one in a dropdown
            ViewBag.ActivePolicies = await _policyRepo.GetPoliciesByCustomerIdAsync(customerId);

            return View();
        }

        // 3. POST: Process the New Claim
        [HttpPost]
        public async Task<IActionResult> FileClaim(CreateClaimDTO dto)
        {
            int customerId = GetCurrentUserId();

            if (!ModelState.IsValid)
            {
                // If they made a mistake, reload the policies for the dropdown and show errors
                ViewBag.ActivePolicies = await _policyRepo.GetPoliciesByCustomerIdAsync(customerId);
                return View(dto);
            }

            // Hand the DTO to our Service layer to apply business rules and save it
            await _claimService.RegisterClaimAsync(dto, customerId);

            TempData["SuccessMessage"] = "Your claim has been successfully submitted!";
            return RedirectToAction("Dashboard");
        }

        // 4. GET: Show the Insurance Catalog
        [HttpGet]
        public async Task<IActionResult> BuyPolicy()
        {
            // CHANGED: Fetch from the Policies table where CustomerId is null (Admin Master Templates)
            var templates = await _context.Policies
                .Where(p => p.CustomerId == null && p.Status == PolicyStatus.Active)
                .ToListAsync();

            return View(templates);
        }

        // 5. GET: Show the Checkout/Vehicle Details Form
        [HttpGet]
        public async Task<IActionResult> Checkout(int planId)
        {
            // CHANGED: Fetch the chosen template from the Policies table
            var template = await _context.Policies.FindAsync(planId);
            if (template == null) return NotFound();

            // Pre-fill the DTO with the template details
            var dto = new CheckoutDTO
            {
                PlanId = template.PolicyId,
                PlanName = $"{template.PolicyType} Insurance ({template.PolicyNumber})",
                PremiumAmount = template.PremiumAmount,
                CoverageAmount = template.CoverageAmount
            };

            return View(dto);
        }

        // 6. POST: Process the Payment and Issue the Policy
        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutDTO dto)
        {
            if (!ModelState.IsValid) return View(dto);

            int customerId = GetCurrentUserId();

            // Use the selected policy template created by Admin so the issued policy preserves the Admin-defined PolicyNumber
            var template = await _context.Policies.FindAsync(dto.PlanId);
            if (template == null)
            {
                ModelState.AddModelError(string.Empty, "Selected policy template could not be found.");
                return View(dto);
            }

            // Create the actual policy receipt copying the admin-defined policy number and type from the template
            var policy = new Policy
            {
                CustomerId = customerId,
                PolicyNumber = template.PolicyNumber,
                PolicyType = template.PolicyType,
                CoverageAmount = dto.CoverageAmount,
                PremiumAmount = dto.PremiumAmount,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddYears(1), // 1 Year validity
                VehicleRC = dto.VehicleRC,
                ChassisNumber = dto.ChassisNumber,
                Status = PolicyStatus.Active
            };

            _context.Policies.Add(policy);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Payment successful! Your {dto.PlanName} is now active.";
            return RedirectToAction("Dashboard");
        }

        // GET: View a single policy's details
        [HttpGet]
        public async Task<IActionResult> ViewPolicy(int id)
        {
            int customerId = GetCurrentUserId();
            var policy = await _context.Policies
                .FirstOrDefaultAsync(p => p.PolicyId == id && p.CustomerId == customerId);

            if (policy == null) return NotFound();

            return View(policy);
        }

        // GET: View a single claim's details AND uploaded documents
        [HttpGet]
        public async Task<IActionResult> ViewClaim(int id)
        {
            int customerId = GetCurrentUserId();

            // Notice we use .Include() to grab the files and the linked policy!
            var claim = await _context.Claims
                .Include(c => c.ClaimDocuments)
                .Include(c => c.Policy)
                .FirstOrDefaultAsync(c => c.ClaimId == id && c.CustomerId == customerId);

            if (claim == null) return NotFound();

            return View(claim);
        }

        // GET: View Final Settlement/Payment Receipt for the Customer
        [HttpGet]
        public async Task<IActionResult> PaymentReceipt(int claimId)
        {
            // Securely get the logged-in customer's ID (assuming you use a helper method like this)
            int currentUserId = int.Parse(User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier) ?? "0");

            // Fetch the claim, ensuring it belongs to the logged-in user!
            // Note: Use _context or _claimRepo depending on how your CustomerController is set up
            var claim = await _context.Claims
                .FirstOrDefaultAsync(c => c.ClaimId == claimId && c.CustomerId == currentUserId);

            if (claim == null)
            {
                return NotFound("Receipt not found or you do not have permission to view it.");
            }

            return View(claim);
        }
    }
}