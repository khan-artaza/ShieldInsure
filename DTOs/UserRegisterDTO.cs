using System.ComponentModel.DataAnnotations;
using Final_Insure.Models;

namespace Final_Insure.DTOs
{
    public class UserRegisterDTO
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50)]
        public required string Username { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public required string Password { get; set; }

        // Defaulting new registrations to Customer role
        public UserRole Role { get; set; } = UserRole.Customer;
    }
}