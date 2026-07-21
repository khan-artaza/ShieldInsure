using System.ComponentModel.DataAnnotations;
using Final_Insure.Models;

namespace Final_Insure.DTOs
{
    public class RegisterStaffDTO
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a staff role")]
        public UserRole Role { get; set; }
    }
}