using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMCSP3.Models
{
    public class Claim
    {
        [Key]
        public int ClaimId { get; set; }

        [Required]
        [Display(Name = "Lecturer ID")]
        public int LecturerId { get; set; }

        [Required]
        [Range(0, 180, ErrorMessage = "You cannot claim more than 180 hours in a month.")]
        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "Hours Worked")]
        public decimal HoursWorked { get; set; }
       
        public string TaskDescription { get; set; }
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "Hourly Rate")]
        public decimal HourlyRate { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "Claim Amount")]
        public decimal ClaimAmount { get; set; }

        [Display(Name = "Additional Notes")]
        public string? Notes { get; set; }

        [Display(Name = "Supporting Document")]
        public string? SupportingDocumentPath { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; } = "Draft";  // changed default from Pending

        [Display(Name = "Verified By")]
        public string? VerifiedBy { get; set; }

        [Display(Name = "Approved By")]
        public string? ApprovedBy { get; set; }

        [Display(Name = "Approved Date")]
        public DateTime? ApprovedDate { get; set; }

        [Display(Name = "Approver Role")]
        public string? ApproverRole { get; set; }

        [Display(Name = "Submitted Date")]
        public DateTime? SubmittedDate { get; set; }
      

        [Display(Name = "HR Processed Date")]
        public DateTime? ProcessedDate { get; set; }
        public string? OriginalDocumentName { get; set; }

        public DateTime ClaimDate { get; set; } = DateTime.Now;

        // Method for auto calculation
        public void CalculateClaimAmount()
        {
            ClaimAmount = Math.Round(HoursWorked * HourlyRate, 2);
        }
    }
}
