using System.ComponentModel.DataAnnotations;

namespace CMCSP3.Models
{
   
    public class User
    {
        [Key]
        [Required]
        public int LecturerId { get; set; } 

        [Required]
        [StringLength(100)]
        [Display(Name = "Username")]
        public string Username { get; set; } = string.Empty;

       
        [Required]
        [StringLength(255)]
        [Display(Name = "Hashed Password")]
        public string HashedPassword { get; set; } = string.Empty;
     
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
       
        public string Role { get; set; } = "Lecturer";

        [Required, Display(Name = "Hourly Rate (ZAR)")]
        [Range(0, 2000, ErrorMessage = "Hourly rate must be between 0 and 2000 ZAR.")]
        public decimal HourlyRate { get; set; }

        [Display(Name = "Department")]
        [StringLength(100)]
        public string? Department { get; set; } 

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

            }
}