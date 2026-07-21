using Final_Insure.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims; // Required for reading the secure cookie

namespace Final_Insure.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // SMART ROUTER: If the user is logged in, hide the landing page and push them to their Dashboard
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var userRole = User.FindFirstValue(ClaimTypes.Role);

                return userRole switch
                {
                    "Customer" => RedirectToAction("Dashboard", "Customer"),
                    "ClaimOfficer" => RedirectToAction("Dashboard", "ClaimOfficer"),
                    "Surveyor" => RedirectToAction("Dashboard", "Surveyor"),
                    "ComplianceOfficer" => RedirectToAction("Dashboard", "ComplianceOfficer"),
                    "Admin" => RedirectToAction("Dashboard", "Admin"),
                    _ => View()
                };
            }

            // If they are a guest (not logged in), show the normal beautiful landing page
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}