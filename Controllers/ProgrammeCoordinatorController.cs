using CMCSPART3.Controllers;
using CMCSPART3.Models;
using Microsoft.AspNetCore.Mvc;

namespace CMCSPART3.Controllers
{
    public class ProgrammeCoordinatorController : Controller
    {
        private static List<Claim> _claims = ClaimsController.GetClaimsList();

        public IActionResult Index()
        {
            // Show pending claims
            var pendingClaims = _claims.Where(c => c.Status == "Pending").ToList();
            return View(pendingClaims);
        }

        [HttpPost]
        public IActionResult Verification(int id)
        {
            var claim = _claims.FirstOrDefault(c => c.ClaimId == id);
            if (claim != null)
            {
                claim.Status = "Verified";
                claim.ApprovedBy = "Programme Coordinator";
                claim.ApproverRole = "Programme Coordinator";
            }

            TempData["Message"] = "✅ Claim successfully verified.";
            TempData["Status"] = "Verified";
            return RedirectToAction("Confirmation");
        }

        [HttpPost]
        public IActionResult Reject(int id)
        {
            var claim = _claims.FirstOrDefault(c => c.ClaimId == id);
            if (claim != null)
            {
                claim.Status = "Rejected";
                claim.ApprovedBy = "Programme Coordinator";
                claim.ApproverRole = "Programme Coordinator";
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