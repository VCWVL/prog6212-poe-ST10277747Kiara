using Microsoft.AspNetCore.Mvc;
using CMCSPART3.Data;
using Microsoft.AspNetCore.Http;

namespace CMCSPART3.Controllers
{
    public class AccountController : Controller
    {
        private readonly CMCSPART3DbContext _context;

        public AccountController(CMCSPART3DbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string username, string password, int? lecturerId, string role)
        {
            if (string.IsNullOrWhiteSpace(username)
                || string.IsNullOrWhiteSpace(password)
                || string.IsNullOrWhiteSpace(role))
            {
                ViewBag.Error = "Please enter all fields and select a role.";
                return View();
            }

            // ==============================
            // SPECIAL CASE: HR AUTO LOGIN
            // ==============================
            if (role == "HR")
            {
                if (lecturerId == 9999 && username == "KiaraIsrael" && password == "Kiara@1004")
                {
                    HttpContext.Session.SetString("Username", username);
                    HttpContext.Session.SetString("Role", "HR");
                    HttpContext.Session.SetInt32("LecturerId", lecturerId.Value);

                    return RedirectToAction("Index", "HR");
                }

                ViewBag.Error = "Invalid HR login details.";
                return View();
            }

            // ==============================
            // NORMAL USER LOGIN
            // ==============================
            if (!lecturerId.HasValue)
            {
                ViewBag.Error = "Please enter Lecturer ID.";
                return View();
            }

            HttpContext.Session.SetString("Username", username);
            HttpContext.Session.SetInt32("LecturerId", lecturerId.Value);
            HttpContext.Session.SetString("Role", role);

            switch (role)
            {
                case "Lecturer":
                    return RedirectToAction("Create", "Claims");

                case "ProgrammeCoordinator":
                    return RedirectToAction("Index", "ProgrammeCoordinator");

                case "AcademicManager":
                    return RedirectToAction("Index", "AcademicManager");

                default:
                    ViewBag.Error = "Invalid role selected.";
                    return View();
            }
        }

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
