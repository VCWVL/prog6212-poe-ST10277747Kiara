using CMCSP3.Data;
using CMCSP3.Models;
using CMCSP3.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CMCSP3.Controllers
{
    public class AccountController : Controller
    {
        private readonly CMCSDbContext _context;

        public AccountController(CMCSDbContext context)
        {
            _context = context;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password, string role, int? lecturerId)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(role))
            {
                ViewBag.Error = "Please fill in all required fields.";
                return View();
            }

            // -------- HR Login (hardcoded) --------
            if (role == "HR" && username == "hr1" && password == "HR.Company@1004")
            {
                HttpContext.Session.SetString("Username", username);
                HttpContext.Session.SetString("Role", role);
                HttpContext.Session.SetInt32("LecturerId", 0);
                return RedirectToAction("Index", "HR");
            }

            // -------- Lecturer login (no password verification, any username/id) --------
            if (role == "Lecturer")
            {
                HttpContext.Session.SetString("Username", username);
                HttpContext.Session.SetString("Role", role);
                HttpContext.Session.SetInt32("LecturerId", lecturerId ?? 0);
                return RedirectToAction("Create", "Claims");
            }

            // -------- Database login for other roles (ProgrammeCoordinator, AcademicManager) --------
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username && u.Role == role && u.IsActive);
            if (user == null || !PasswordHasher.Verify(password, user.HashedPassword))
            {
                ViewBag.Error = "Invalid username, password, or role.";
                return View();
            }

            // Set session
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("Role", user.Role);
            HttpContext.Session.SetInt32("LecturerId", lecturerId ?? 0);

            // Role-based redirect
            return role switch
            {
                "ProgrammeCoordinator" => RedirectToAction("Index", "ProgrammeCoordinator"),
                "AcademicManager" => RedirectToAction("Index", "AcademicManager"),
                _ => RedirectToAction("Login")
            };
        }


        // GET: /Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult ForgotPassword() => View();

        [HttpPost]
        public IActionResult ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ViewBag.Message = "Please enter your registered email.";
                return View();
            }

            ViewBag.Message = $"A password reset link has been sent to {email}.";
            return View();
        }
    }
}
