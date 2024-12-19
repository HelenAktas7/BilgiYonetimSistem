using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BilgiYonetimSistem.Models;

namespace BilgiYonetimSistem.Controllers
{
    public class StudentCourseHistoryController : Controller
    {
        private readonly DataContext _context;

        public StudentCourseHistoryController(DataContext context)
        {
            _context = context;
        }

        // GET: StudentCourseHistory/Index/{studentId}
        [HttpGet("StudentCourseHistory/Index/{studentId}")]
        public async Task<IActionResult> Index(int studentId)
        {
            // Öğrenciye ait geçmiş ders seçimlerini çek
            var student = await _context.Students
                .Include(s => s.StudentCourseSelections)
                    .ThenInclude(sc => sc.Course)
                .FirstOrDefaultAsync(s => s.StudentID == studentId);

            if (student == null)
            {
                ViewBag.Message = "Student not found.";
                return View();
            }

            return View(student);
        }
    }
}
