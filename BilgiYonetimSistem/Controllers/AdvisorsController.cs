using BilgiYonetimSistem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BilgiYonetimSistem.Controllers
{
    public class Advisors
    {
        [Route("api/AdvisorController")]
        [ApiController]
        public class AdvisorsController : Controller
        {
            private readonly DataContext _context;

            public AdvisorsController(DataContext context)
            {
                _context = context;
            }

            [HttpGet("ApproveCourses")]
            public IActionResult ApproveCourses(int id)
            {

                var advisor = _context.Advisors
                    .Include(a => a.Students)
                    .FirstOrDefault(a => a.AdvisorId == id);

                if (advisor == null)
                {
                    return NotFound(new { Message = "Advisor not found." });
                }
                return View(advisor);
            }

            [HttpGet("ViewStudentCourses/{studentId}")]
            public IActionResult ViewStudentCourses(int studentId)
            {

                var student = _context.Students
                    .Include(s => s.StudentCourseSelections)
                        .ThenInclude(sc => sc.Course)
                    .FirstOrDefault(s => s.StudentID == studentId);

                if (student == null)
                {
                    return NotFound(new { Message = "Student not found." });
                }

                return View(student);
            }


            [HttpGet("getAdvisorList")]
            public async Task<ActionResult<IEnumerable<Advisor>>> GetAdvisors()
            {
                return await _context.Advisors.ToListAsync();
            }


            [HttpGet("getById")]
            public async Task<ActionResult<Advisor>> GetAdvisor(int id)
            {
                var advisor = await _context.Advisors.FindAsync(id);

                if (advisor == null)
                {
                    return NotFound(new { Message = "Advisor not found." });
                }

                return advisor;
            }


            [HttpPost("CreateAdvisor")]
            public async Task<ActionResult<Advisor>> CreateAdvisor(Advisor advisor)
            {
                _context.Advisors.Add(advisor);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetAdvisor", new { id = advisor.AdvisorId }, advisor);
            }


            [HttpPut("{id}")]
            public async Task<IActionResult> UpdateAdvisor(int id, Advisor advisor)
            {
                if (id != advisor.AdvisorId)
                {
                    return BadRequest(new { Message = "Advisor ID mismatch." });
                }

                _context.Entry(advisor).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdvisorExists(id))
                    {
                        return NotFound(new { Message = "Advisor not found." });
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent();
            }


            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteAdvisor(int id)
            {
                var advisor = await _context.Advisors.FindAsync(id);
                if (advisor == null)
                {
                    return NotFound(new { Message = "Advisor not found." });
                }

                _context.Advisors.Remove(advisor);
                await _context.SaveChangesAsync();

                return NoContent();
            }

            private bool AdvisorExists(int id)
            {
                return _context.Advisors.Any(e => e.AdvisorId == id);
            }
        }
    }
}