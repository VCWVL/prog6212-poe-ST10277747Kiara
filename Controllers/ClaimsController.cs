using Microsoft.AspNetCore.Mvc;
using CMCSPART3.Models;
using System.Security.Cryptography;
using System.Text;

namespace CMCSPART3.Controllers
{
    public class ClaimsController : Controller
    {
        private static List<Claim> _claims = new();
        private static int _nextClaimId = 1;

        // ===========================
        // Claims List Per Role
        // ===========================
        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("Role");
            var lecturerId = HttpContext.Session.GetInt32("LecturerId");

            if (role == null)
                return RedirectToAction("Login", "Account");

            List<Claim> claimsToShow = role switch
            {
                "Lecturer" => _claims.Where(c => c.LecturerId == lecturerId).ToList(),
                "ProgrammeCoordinator" => _claims.Where(c => c.Status == "Pending").ToList(),
                "AcademicManager" => _claims.Where(c => c.Status == "Verified").ToList(),
                _ => _claims
            };

            return View(claimsToShow);
        }

        // ===========================
        // GET: Submit Claim
        // ===========================
        public IActionResult Create()
        {

            var role = HttpContext.Session.GetString("Role");

            if (role == null || role != "Lecturer")
                return RedirectToAction("Login", "Account");

            ViewBag.LecturerId = HttpContext.Session.GetInt32("LecturerId");
            ViewBag.MaxFileSize = "Maximum file size: 5 MB | Allowed: .pdf, .docx, .xlsx.";

            return View();
        }

        // ===========================
        // POST: Submit Claim
        // ===========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Claim claim, IFormFile? SupportingDocument)
        {
            var lecturerId = HttpContext.Session.GetInt32("LecturerId");

            if (!lecturerId.HasValue)
                return RedirectToAction("Login", "Account");
            claim.ClaimDate = DateTime.Now;

            claim.LecturerId = lecturerId.Value;
            claim.ClaimId = _nextClaimId++;
            claim.Status = "Pending";
            claim.CalculateClaimAmount();

            // ------------------------
            // FILE UPLOAD
            // ------------------------
            if (SupportingDocument != null && SupportingDocument.Length > 0)
            {
                var allowedExtensions = new[] { ".pdf", ".docx", ".xlsx" };
                var ext = Path.GetExtension(SupportingDocument.FileName).ToLower();

                if (!allowedExtensions.Contains(ext))
                {
                    TempData["Error"] = "Invalid file type.";
                    return View(claim);
                }

                if (SupportingDocument.Length > 5 * 1024 * 1024)
                {
                    TempData["Error"] = "File too large.";
                    return View(claim);
                }

                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Documents");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                string encryptedFileName = EncryptFileName(Guid.NewGuid() + "_" + SupportingDocument.FileName);
                string filePath = Path.Combine(uploadsFolder, encryptedFileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                SupportingDocument.CopyTo(stream);

                claim.SupportingDocumentPath = "/Documents/" + encryptedFileName;
            }

            _claims.Add(claim);
            TempData["Success"] = "Claim Submitted Successfully!";

            return RedirectToAction(nameof(Index));
        }

        // ===========================
        // CLAIM DETAILS
        // ===========================
        public IActionResult Details(int id)
        {
            var role = HttpContext.Session.GetString("Role");
            var lecturerId = HttpContext.Session.GetInt32("LecturerId");

            if (role == null) return RedirectToAction("Login", "Account");

            var claim = _claims.FirstOrDefault(c => c.ClaimId == id);
            if (claim == null) return NotFound();

            if (role == "Lecturer" && claim.LecturerId != lecturerId)
                return RedirectToAction("Login", "Account");

            return View(claim);
        }

        // =============================
        // PROGRAMME COORDINATOR VERIFY
        // =============================
        [HttpPost]
        public IActionResult VerifyClaim(int id)
        {
            if (HttpContext.Session.GetString("Role") != "ProgrammeCoordinator")
                return RedirectToAction("Login", "Account");

            var claim = _claims.FirstOrDefault(c => c.ClaimId == id);
            if (claim == null) return NotFound();

            claim.Status = "Verified";
            claim.ApproverRole = "Programme Coordinator";
            claim.ApprovedBy = "Coordinator John";

            TempData["Success"] = "Claim Verified!";
            return RedirectToAction(nameof(Index));
        }

        // =============================
        // ACADEMIC MANAGER APPROVE
        // =============================
        [HttpPost]
        public IActionResult ApproveClaim(int id)
        {
            if (HttpContext.Session.GetString("Role") != "AcademicManager")
                return RedirectToAction("Login", "Account");

            var claim = _claims.FirstOrDefault(c => c.ClaimId == id);
            if (claim == null) return NotFound();

            if (claim.Status != "Verified")
            {
                TempData["Error"] = "Only verified claims can be approved.";
                return RedirectToAction(nameof(Index));
            }

            claim.Status = "Approved";
            claim.ApproverRole = "Academic Manager";
            claim.ApprovedBy = "Academic Manager";

            TempData["Success"] = "Claim Approved!";
            return RedirectToAction(nameof(Index));
        }

        private string EncryptFileName(string input)
        {
            using var sha = SHA256.Create();
            byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            return BitConverter.ToString(bytes).Replace("-", "") + Path.GetExtension(input);
        }

        public static List<Claim> GetClaimsList() => _claims;
    }
}
