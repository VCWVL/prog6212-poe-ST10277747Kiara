using CMCSPART3.Controllers;
using CMCSPART3.Models;
using Microsoft.AspNetCore.Mvc;

namespace CMCSPART3.Controllers
{
    public class AcademicManagerController : Controller
    {
        private static List<Claim> _claims = ClaimsController.GetClaimsList();

        public IActionResult Index()
        {
            // Show verified claims only
            var verifiedClaims = _claims.Where(c => c.Status == "Verified").ToList();
            return View(verifiedClaims);
        }

        [HttpPost]
        public IActionResult Approve(int id)
        {
            var claim = _claims.FirstOrDefault(c => c.ClaimId == id);
            if (claim != null)
            {
                claim.Status = "Approved";
                claim.ApprovedBy = "Academic Manager";
                claim.ApproverRole = "Academic Manager";
            }

            TempData["Message"] = "✅ Claim successfully approved.";
            TempData["Status"] = "Approved";
            return RedirectToAction("Confirmation");
        }

        [HttpPost]
        public IActionResult Reject(int id)
        {
            var claim = _claims.FirstOrDefault(c => c.ClaimId == id);
            if (claim != null)
            {
                claim.Status = "Rejected";
                claim.ApprovedBy = "Academic Manager";
                claim.ApproverRole = "Academic Manager";
            }

            TempData["Message"] = "❌ Claim has been rejected.";
            TempData["Status"] = "Rejected";
            return RedirectToAction("Confirmation");
        }

        public IActionResult Confirmation()
        {
            ViewBag.Message = TempData["Message"];
            ViewBag.Status = TempData["Status"];
            return View();
        }
    }
}
//Caulfield, J. (2020) Reference a Website in Harvard Style | Templates & Examples. Available at: https://www.scribbr.co.uk/referencing/harvard-website-reference/ (Accessed: 17 September 2025).
//ST10277747 KIARA ISRAEL 