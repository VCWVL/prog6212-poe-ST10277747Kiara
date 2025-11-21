using System.ComponentModel.DataAnnotations;

namespace CMCSP3.Models
{
    // This is the primary database entity for all system users (Lecturers, HR, etc.)
    public class User
    {
        [Key]
        [Required]
        public int LecturerId { get; set; } // Primary Key

        [Required]
        [StringLength(100)]
        [Display(Name = "Username")]
        public string Username { get; set; } = string.Empty;

        // --- SECURITY FIX ---
        // CRITICAL: We MUST store the HASHED password, not the plain text "Password".
        // The PasswordHasher service uses this field.
        [Required]
        [StringLength(255)] // Hash strings are longer than a typical password, use 255 for safety
        [Display(Name = "Hashed Password")]
        public string HashedPassword { get; set; } = string.Empty;
        // --------------------

        [Required, Display(Name = "First Name")]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required, Display(Name = "Last Name")]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required, EmailAddress, Display(Name = "Email Address")]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required, Display(Name = "Role")]
        [StringLength(50)]
        // Stores the user's role: "Lecturer", "HR", etc.
        public string Role { get; set; } = "Lecturer";

        [Required, Display(Name = "Hourly Rate (ZAR)")]
        [Range(0, 2000, ErrorMessage = "Hourly rate must be between 0 and 2000 ZAR.")]
        public decimal HourlyRate { get; set; }

        [Display(Name = "Department")]
        [StringLength(100)]
        public string? Department { get; set; } // Can be null

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        // Navigation property for Claims (if used in DB context)
        // public ICollection<Claim> Claims { get; set; } 
    }
}