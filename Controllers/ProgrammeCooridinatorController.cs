using CMCSP3.Data;
using CMCSP3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CMCSP3.Controllers
{
    public class ProgrammeCoordinatorController : Controller
    {
        private readonly CMCSDbContext _context;

        public ProgrammeCoordinatorController(CMCSDbContext context)
        {
            _context = context;
        }

       
        public async Task<IActionResult> Index()
        {
          
            var pendingClaims = await _context.Claims
                .Where(c => c.Status == "Pending")
                .OrderByDescending(c => c.SubmittedDate)
                .ToListAsync();

            return View(pendingClaims);
        }

        
        [HttpPost]
        public async Task<IActionResult> Verification(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim != null)
            {
                claim.Status = "Verified";
                claim.ApprovedBy = "Programme Coordinator";
                claim.ApproverRole = "Programme Coordinator";

                _context.Claims.Update(claim);
                await _context.SaveChangesAsync();
            }

            TempData["Message"] = "✅ Claim successfully verified.";
            TempData["Status"] = "Verified";
            return RedirectToAction("Confirmation");
        }

      
        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim != null)
            {
                claim.Status = "Rejected";
                claim.ApprovedBy = "Programme Coordinator";
                claim.ApproverRole = "Programme Coordinator";

                _context.Claims.Update(claim);
                await _context.SaveChangesAsync();
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