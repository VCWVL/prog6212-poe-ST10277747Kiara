using System.ComponentModel.DataAnnotations;

namespace CMCSP3.Models
{
    // Used for HR forms (Add/Edit)
    public class UserViewModel
    {
        public int LecturerId { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Range(0.01, 1000.00, ErrorMessage = "Hourly Rate must be a positive value.")]
        [Display(Name = "Hourly Rate")]
        public decimal HourlyRate { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; } = "Lecturer";

       
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password (Required for New User)")]
        public string InitialPassword { get; set; } = string.Empty;
    }
}