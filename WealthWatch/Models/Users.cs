using System.ComponentModel.DataAnnotations;

namespace WealthWatch.Models
{
    public class Users
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Full Name is required.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Currency selection is required.")]
        public string Currency { get; set; } = string.Empty;
        
    }
}
