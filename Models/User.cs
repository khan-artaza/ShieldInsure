using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Final_Insure.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50)]
        public required string Username { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        [StringLength(100)]
        public required string Email { get; set; }

        [Required]
        public required string PasswordHash { get; set; }

        [Required]
        public UserRole Role { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsApproved { get; set; } = true;

        public ICollection<Policy> Policies { get; set; } = new List<Policy>();
        public ICollection<Claim> Claims { get; set; } = new List<Claim>();
        public ICollection<Assessment> Assessments { get; set; } = new List<Assessment>();
    }
}