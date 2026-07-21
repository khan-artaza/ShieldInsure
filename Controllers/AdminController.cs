using Final_Insure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Insure.Controllers
{
    [Authorize(Roles = "Admin")] // Strictly locked down!
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // 1. GET: Admin Dashboard (View all staff)
        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            // Fetch all users who are NOT customers so the Admin can manage staff
            var staffList = await _context.Users
                .Where(u => u.Role != UserRole.Customer)
                .OrderBy(u => u.IsApproved) // Shows pending approvals at the top
                .ToListAsync();

            return View(staffList);
        }

        // 2. POST: Approve a staff member
        [HttpPost]
        public async Task<IActionResult> ApproveStaff(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null && !user.IsApproved)
            {
                user.IsApproved = true;
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"{user.Username} has been officially approved and can now log in!";
            }
            return RedirectToAction("Dashboard");
        }

        // 3. POST: Reject/Delete a staff application
        [HttpPost]
        public async Task<IActionResult> RejectStaff(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null && !user.IsApproved)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Staff application for {user.Username} was rejected and removed.";
            }
            return RedirectToAction("Dashboard");
        }

        // GET: Show the form to create a new policy
        [HttpGet]
        public IActionResult CreatePolicy()
        {
            return View();
        }

        // POST: Save the new policy to the database
        [HttpPost]
        public async Task<IActionResult> CreatePolicy(Policy newPolicy)
        {
            if (ModelState.IsValid)
            {
                // Add to database and save
                _context.Policies.Add(newPolicy);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"The new policy '{newPolicy.PolicyType}' has been successfully published!";
                return RedirectToAction("Dashboard");
            }

            // If validation fails, return the form with errors
            return View(newPolicy);
        }
    }
}