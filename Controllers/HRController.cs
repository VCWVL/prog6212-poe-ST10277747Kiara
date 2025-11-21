using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CMCSP3.Data;
using CMCSP3.Models;
using CMCSP3.Services;
using CMCSP3.Reports;        // Needed for InvoiceDocument class
using QuestPDF.Fluent;       // <<< FIX: Required for the GeneratePdf() extension method
using System.Linq;
using System.Threading.Tasks;

namespace CMCSP3.Controllers
{
    public class HRController : Controller
    {
        private readonly CMCSDbContext _context;

        public HRController(CMCSDbContext context)
        {
            _context = context;
        }

        private bool IsHR()
        {
           
            return HttpContext.Session.GetString("Role") == "HR";
        }

       

        public IActionResult Index()
        {
            if (!IsHR()) return RedirectToAction("Login", "Account");

            

            return View();
        }

        
        public async Task<IActionResult> ManageUsers()
        {
            if (!IsHR()) return RedirectToAction("Login", "Account");

            var users = await _context.Users.ToListAsync();
            return View(users);
        }

        
        [HttpGet]
        public IActionResult AddUser()
        {
            if (!IsHR()) return RedirectToAction("Login", "Account");
            return View(new UserViewModel());
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(UserViewModel model)
        {
            if (!IsHR()) return RedirectToAction("Login", "Account");

            
            if (string.IsNullOrEmpty(model.InitialPassword))
                ModelState.AddModelError("InitialPassword", "An initial password is required for a new user.");

            if (ModelState.IsValid)
            {
                if (await _context.Users.AnyAsync(u => u.Username == model.Username))
                {
                    ModelState.AddModelError("Username", "This Username already exists.");
                    return View(model);
                }

                var user = new User
                {
                    Username = model.Username,
                   
                    HashedPassword = PasswordHasher.Hash(model.InitialPassword),
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Role = model.Role,
                    HourlyRate = model.HourlyRate
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"User **{user.Username}** created successfully. Login ID is **{user.LecturerId}**.";
                return RedirectToAction("ManageUsers");
            }
            return View(model);
        }

      
        [HttpGet]
        public async Task<IActionResult> EditUser(int id)
        {
            if (!IsHR()) return RedirectToAction("Login", "Account");

            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            var model = new UserViewModel
            {
                LecturerId = user.LecturerId,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                HourlyRate = user.HourlyRate,
                Role = user.Role,
                InitialPassword = string.Empty 
            };

            return View(model);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(UserViewModel model)
        {
            if (!IsHR()) return RedirectToAction("Login", "Account");

            
            if (string.IsNullOrEmpty(model.InitialPassword))
                ModelState.Remove("InitialPassword");

            if (ModelState.IsValid)
            {
                var user = await _context.Users.FindAsync(model.LecturerId);
                if (user == null) return NotFound();

                user.Username = model.Username;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.Role = model.Role;
                user.HourlyRate = model.HourlyRate;

                
                if (!string.IsNullOrEmpty(model.InitialPassword))
                {
                    user.HashedPassword = PasswordHasher.Hash(model.InitialPassword);
                }

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"User **{user.Username}** updated successfully.";
                return RedirectToAction("ManageUsers");
            }
            return View(model);
        }

        
        public async Task<IActionResult> ViewReports()
        {
            if (!IsHR()) return RedirectToAction("Login", "Account");

            
            var readyClaims = await _context.Claims
                .Where(c => c.Status == "Approved")
                .OrderByDescending(c => c.SubmittedDate)
                .Select(c => new
                {
                    Claim = c,
                    Lecturer = _context.Users.FirstOrDefault(u => u.LecturerId == c.LecturerId)
                })
                .ToListAsync();

            
            var model = readyClaims.Select(x => (x.Claim, x.Lecturer)).ToList();
            return View(model); 
        }

       
        public async Task<IActionResult> DownloadInvoice(int id)
        {
            if (!IsHR()) return RedirectToAction("Login", "Account");

            var claim = await _context.Claims.FirstOrDefaultAsync(c => c.ClaimId == id);
            if (claim == null || claim.Status != "Approved")
                return NotFound("Claim not found or not approved.");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.LecturerId == claim.LecturerId);
            if (user == null)
                return NotFound("Associated lecturer not found.");

           
            claim.Status = "Processed";
            claim.ProcessedDate = DateTime.Now;
            await _context.SaveChangesAsync();

           
            var invoice = new InvoiceDocument(claim, user);
            var reportsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Reports");
            if (!Directory.Exists(reportsFolder))
                Directory.CreateDirectory(reportsFolder);

            var fileName = $"Invoice_{claim.LecturerId}_{claim.ClaimId}.pdf";
            var filePath = Path.Combine(reportsFolder, fileName);
            invoice.GeneratePdf(filePath);

            var pdfBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(pdfBytes, "application/pdf", fileName);
        }

    }
}