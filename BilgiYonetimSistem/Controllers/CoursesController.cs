using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BilgiYonetimSistem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BilgiYonetimSistem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly DataContext _context;

        public CoursesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetCourse()
        {
            var courses = await _context.Courses
                .Include(c => c.StudentCourseSelections)  // StudentCourseSelections ilişkisini dahil ediyoruz
                .Select(c => new
                {
                    c.CourseId,
                    c.CourseCode,
                    c.CourseName,
                    c.IsMandatory,
                    c.Credit,
                    c.Department,
                    StudentCourseSelections = c.StudentCourseSelections.Select(sc => new
                    {
                        sc.StudentID,
                        sc.SelectionDate
                    }).ToList()  // Sadece StudentID ve SelectionDate bilgilerini alıyoruz
                })
                .ToListAsync();

            return Ok(courses);
        }


        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourse(int id)
        {
            var course = await _context.Courses
                .Include(c => c.StudentCourseSelections)  // StudentCourseSelections ilişkisini dahil ediyoruz
                .Where(c => c.CourseId== id)
                .Select(c => new
                {
                    c.CourseId,
                    c.CourseCode,
                    c.CourseName,
                    c.IsMandatory,
                    c.Credit,
                    c.Department,
                    StudentCourseSelections = c.StudentCourseSelections.Select(sc => new
                    {
                        sc.StudentID,
                        sc.SelectionDate
                    }).ToList()  // Sadece StudentID ve SelectionDate bilgilerini alıyoruz
                })
                .FirstOrDefaultAsync();

            if (course == null)
            {
                return NotFound();
            }

            return Ok(course);
        }


        // PUT: api/Courses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, Course course)
        {
            if (id != course.CourseId)
            {
                return BadRequest();
            }

            _context.Entry(course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Courses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCourse", new { id = course.CourseId }, course);
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
        }
    }
}