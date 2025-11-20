using System.ComponentModel.DataAnnotations;

namespace CMCSPART3.Models
{
    public class User
    {
        [Key]   // EF needs a primary key to scaffold
        [Required]
        public int LecturerId { get; set; }

        [Required]
        [StringLength(100)]   // prevents scaffolder crashes on NULL/size
        public string Username { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100)]
        public string Password { get; set; } = string.Empty;
        // NEW FIELDS
        // HR FIELDS
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        public decimal HourlyRate { get; set; }

        public string Role { get; set; } = "Lecturer";
    }
}