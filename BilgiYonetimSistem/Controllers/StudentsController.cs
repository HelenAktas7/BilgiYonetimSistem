using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BilgiYonetimSistem.DataTransfer;
using BilgiYonetimSistem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BilgiYonetimSistem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly DataContext _context;

        public StudentsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudent()
        {
            var studentsWithAdvisorAndCourses = await _context.Students
        .Select(s => new StudentDto
        {
            StudentID = s.StudentID,
            FirstName = s.FirstName,
            LastName = s.LastName,
            Email = s.Email,
            AdvisorID = s.AdvisorID, 
            Advisor = s.Advisor != null ? new AdvisorDto
            {
                FullName = s.Advisor.FullName,
                Title = s.Advisor.Title,
                Department = s.Advisor.Department
            } : null,
            Courses = s.StudentCourseSelections.Select(sc => new CourseDto
            {
                CourseID = sc.CourseID,
                CourseName = sc.Course.CourseName,
                SelectionDate = sc.SelectionDate
            }).ToList()
        })
        .ToListAsync();

            return Ok(studentsWithAdvisorAndCourses);
        }

   
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var studentWithAdvisorAndCourses = await _context.Students
                .Where(s => s.StudentID == id)
                .Select(s => new
                {
                    s.StudentID,
                    s.FirstName,
                    s.LastName,
                    s.Email,
                    s.AdvisorID,
                    Advisor = new
                    {
                        s.Advisor.FullName, 
                        s.Advisor.Title,
                        s.Advisor.Department, 
                        s.Advisor.AdvisorId
                    },
                    Courses = s.StudentCourseSelections.Select(sc => new
                    {
                        sc.CourseID,
                        sc.Course.CourseName,
                        sc.SelectionDate
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (studentWithAdvisorAndCourses == null)
            {
                return NotFound(); 
            }

            return Ok(studentWithAdvisorAndCourses); 
        }


        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, Student student)
        {
            if (id != student.StudentID)
            {
                return BadRequest();
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
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

    
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudent", new { id = student.StudentID }, student);
        }

       
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students
                .Include(s => s.StudentCourseSelections) 
                .FirstOrDefaultAsync(s => s.StudentID == id);

            if (student == null)
            {
                return NotFound();
            }

           
            _context.StudentCourseSelections.RemoveRange(student.StudentCourseSelections);

            
            _context.Students.Remove(student);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.StudentID == id);
        }
    }
}