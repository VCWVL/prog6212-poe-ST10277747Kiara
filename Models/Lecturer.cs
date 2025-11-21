using System.ComponentModel.DataAnnotations;



namespace CMCSP3.Models

{

    public class Lecturer

    {

        [Key]

        public int LecturerId { get; set; }



        [Required, Display(Name = "First Name")]

        public string FirstName { get; set; } = string.Empty; 



        [Required, Display(Name = "Last Name")]

        public string LastName { get; set; } = string.Empty;



        [Required, EmailAddress, Display(Name = "Email Address")]

        public string Email { get; set; } = string.Empty;



        [Display(Name = "Department")]

        public string? Department { get; set; }



        [Required, Display(Name = "Hourly Rate (ZAR)")]

        [Range(0, 2000, ErrorMessage = "Hourly rate must be between 0 and 2000 ZAR.")]

        public decimal HourlyRate { get; set; }



        [Display(Name = "Is Active")]

        public bool IsActive { get; set; } = true;

    }

}