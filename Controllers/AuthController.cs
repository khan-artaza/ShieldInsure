using BCrypt.Net;
using Final_Insure.DTOs;
using Final_Insure.Models; // Added to access UserRole and User
using Final_Insure.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore; // Added for database checks
using System.Security.Claims;

namespace Final_Insure.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly AppDbContext _context; // Added database context
       

        public AuthController(IAuthService authService, AppDbContext context)
        {
            _authService = authService;
            _context = context;
        }

        // 1. GET: Show the Login Page
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // 2. POST: Process the Login Form
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDTO loginDto)
        {
            if (!ModelState.IsValid) return View(loginDto);

            var user = await _authService.AuthenticateAsync(loginDto);

            if (user == null)
            {
                ViewBag.ErrorMessage = "Invalid email or password";
                return View(loginDto);
            }

            if (!user.IsApproved)
            {
                ViewBag.ErrorMessage = "Your staff account is pending Admin approval.";
                return View(loginDto);
            }

            // Create the secure cookie card for the user
            var claims = new List<System.Security.Claims.Claim>
            {
                new System.Security.Claims.Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new System.Security.Claims.Claim(ClaimTypes.Name, user.Username),
                new System.Security.Claims.Claim(ClaimTypes.Email, user.Email),
                new System.Security.Claims.Claim(ClaimTypes.Role, user.Role.ToString())
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            // Smart Routing: Send the user to their specific department dashboard!
            return user.Role switch
            {
                UserRole.Customer => RedirectToAction("Dashboard", "Customer"),
                UserRole.ClaimOfficer => RedirectToAction("Dashboard", "ClaimOfficer"),
                UserRole.Surveyor => RedirectToAction("Dashboard", "Surveyor"),
                UserRole.ComplianceOfficer => RedirectToAction("Dashboard", "ComplianceOfficer"),
                UserRole.Admin => RedirectToAction("Dashboard", "Admin"),
                _ => RedirectToAction("Index", "Home")
            };
        }

        // 3. GET: Show the Register Page (For Customers)
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // 4. POST: Process the Register Form (For Customers)
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterDTO registerDto)
        {
            if (!ModelState.IsValid) return View(registerDto);

            await _authService.RegisterAsync(registerDto);

            TempData["SuccessMessage"] = "Registration successful! Please log in.";
            return RedirectToAction("Login");
        }

        // 5. GET: Show the Staff Registration form
        [HttpGet]
        public IActionResult RegisterStaff()
        {
            // Dynamically grab all role names from your UserRole Enum, excluding "Customer"
            var staffRoles = Enum.GetNames(typeof(UserRole))
                                 .Where(roleName => roleName != "Customer")
                                 .ToList();

            // Pass the filtered list to the View
            ViewBag.Roles = new SelectList(staffRoles);

            return View();
        }

        // 6. POST: Process the Staff Registration
        [HttpPost]
        public async Task<IActionResult> RegisterStaff(RegisterStaffDTO dto)
        {
            if (!ModelState.IsValid) return View(dto);

            // Check if email already exists
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            {
                ModelState.AddModelError("Email", "Email is already registered.");
                return View(dto);
            }

            // Create the new staff user
            // Create the new staff user
            var staffUser = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = dto.Role,
                IsApproved = false, // Critical: Lock the account until Admin approves!
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(staffUser);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Staff account for {dto.Role} created successfully! You can now log in.";
            return RedirectToAction("Login");
        }

        //// 7. POST: Logout
        //[HttpPost]
        //public async Task<IActionResult> Logout()
        //{
        //    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //    return RedirectToAction("Login");
        //}


        // 1. GET: Show User Profile
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            // Get the logged-in user's ID from the secure cookie
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

            // Fetch their details from the database
            var user = await _context.Users.FindAsync(userId);

            if (user == null) return NotFound();

            return View(user);
        }



        // GET: Show Staff Login Page
        [HttpGet]
        public IActionResult StaffLogin()
        {
            return View();
        }

        // 2. POST: Securely Log Out
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // This completely destroys the authentication cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect back to the login page
            return RedirectToAction("StaffLogin", "Auth");
        }
    }
}