using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;

namespace CMCSP3.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly string _rootFolder = Path.Combine(Directory.GetCurrentDirectory(), "Documents");

        public DocumentsController()
        {
           
            if (!Directory.Exists(_rootFolder))
                Directory.CreateDirectory(_rootFolder);
        }

        
        public IActionResult Index()
        {
            var lecturerId = HttpContext.Session.GetInt32("LecturerId");
            if (!lecturerId.HasValue) return Forbid();

            string lecturerFolder = Path.Combine(_rootFolder, lecturerId.Value.ToString());

            if (!Directory.Exists(lecturerFolder))
                Directory.CreateDirectory(lecturerFolder);

           
            var fileList = Directory.GetFiles(lecturerFolder)
                .Select(f => Path.GetFileName(f))
                .ToList();

            ViewBag.LecturerId = lecturerId.Value;
            return View(fileList);
        }

     
        public IActionResult Details(string fileName)
        {
            var lecturerId = HttpContext.Session.GetInt32("LecturerId");
            if (!lecturerId.HasValue) return Forbid();

            if (string.IsNullOrEmpty(fileName)) return NotFound();

            string lecturerFolder = Path.Combine(_rootFolder, lecturerId.Value.ToString());
            string fullPath = Path.Combine(lecturerFolder, fileName);

            if (!System.IO.File.Exists(fullPath)) return NotFound();

            var info = new FileInfo(fullPath);

            var model = new
            {
                FileName = info.Name,
                SizeKB = info.Length / 1024,
                Extension = info.Extension,
                Created = info.CreationTime
            };

            return View(model);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upload(IFormFile file)
        {
            var lecturerId = HttpContext.Session.GetInt32("LecturerId");
            if (!lecturerId.HasValue) return Forbid();

            if (file == null || file.Length == 0)
            {
                TempData["Error"] = "Please select a file.";
                return RedirectToAction(nameof(Index));
            }

            var allowed = new[] { ".pdf", ".docx", ".xlsx" };
            var ext = Path.GetExtension(file.FileName).ToLower();

            if (!allowed.Contains(ext))
            {
                TempData["Error"] = "Invalid file type. Only PDF, DOCX, XLSX allowed.";
                return RedirectToAction(nameof(Index));
            }

            if (file.Length > 5 * 1024 * 1024)
            {
                TempData["Error"] = "File too large (max 5MB).";
                return RedirectToAction(nameof(Index));
            }

            string lecturerFolder = Path.Combine(_rootFolder, lecturerId.Value.ToString());
            if (!Directory.Exists(lecturerFolder))
                Directory.CreateDirectory(lecturerFolder);

          
            string originalName = Path.GetFileName(file.FileName);
            string finalPath = Path.Combine(lecturerFolder, originalName);

           
            int counter = 1;
            while (System.IO.File.Exists(finalPath))
            {
                string nameOnly = Path.GetFileNameWithoutExtension(originalName);
                string newName = $"{nameOnly}_{counter}{ext}";
                finalPath = Path.Combine(lecturerFolder, newName);
                counter++;
            }

            
            using (var stream = new FileStream(finalPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            TempData["Success"] = "File uploaded successfully!";
            return RedirectToAction(nameof(Index));
        }

        
        public IActionResult Download(string fileName)
        {
            var lecturerId = HttpContext.Session.GetInt32("LecturerId");
            if (!lecturerId.HasValue) return Forbid();

            string path = Path.Combine(_rootFolder, lecturerId.Value.ToString(), fileName);

            if (!System.IO.File.Exists(path))
                return NotFound();

            var fileBytes = System.IO.File.ReadAllBytes(path);
            return File(fileBytes, "application/octet-stream", fileName);
        }

       
        public IActionResult Delete(string fileName)
        {
            var lecturerId = HttpContext.Session.GetInt32("LecturerId");
            if (!lecturerId.HasValue) return Forbid();

            string path = Path.Combine(_rootFolder, lecturerId.Value.ToString(), fileName);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            TempData["Success"] = "File deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}
