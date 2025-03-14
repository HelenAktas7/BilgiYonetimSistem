﻿using BilgiYonetimSistem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace BilgiYonetimSistem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentCourseSelectionsController : ControllerBase
    {
        private readonly DataContext _context;

        public StudentCourseSelectionsController(DataContext context)
        {
            _context = context;
        }

       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentCourseSelection>>> GetStudentCourseSelections()
        {
            var studentsWithCourses = await _context.Students
                .Include(s => s.StudentCourseSelections)
                    .ThenInclude(sc => sc.Course) 
                .Select(s => new
                {
                    s.StudentID,
                    s.FirstName,
                    s.LastName,
                    Courses = s.StudentCourseSelections.Select(sc => new
                    {
                        sc.CourseID,
                        sc.SelectionDate,
                        CourseName = sc.Course.CourseName
                    }).ToList()
                })
                .ToListAsync();


            if (studentsWithCourses == null || !studentsWithCourses.Any())
                return NotFound(new { Message = "Hiçbir öğrenci veya ders seçimi bulunamadı." });

            return Ok(studentsWithCourses);
        }

       
        [HttpGet("{studentId}")]
        public IActionResult GetStudentCourseSelections(int studentId)
        {
            
            var selections = _context.StudentCourseSelections
               .Where(s => s.StudentID == studentId)
               .Select(s => new
               {
                   s.Student.StudentID,
                   Course = new
                   {
                       s.Course.CourseCode,
                       s.Course.CourseName,
                       s.Course.Department,
                       s.Course.Credit
                   }
               })
               .ToList();


           
            if (selections == null || !selections.Any())
            {
                return NotFound("Bu öğrenci için ders seçimi bulunamadı.");
            }

           
            return Ok(selections);
        }

      
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudentCourseSelections(int id, StudentCourseSelection studentCourseSelections)
        {
            if (id != studentCourseSelections.SelectionID)
            {
                return BadRequest();
            }

            _context.Entry(studentCourseSelections).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentCourseSelectionsExists(id))
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
        public async Task<ActionResult<StudentCourseSelection>> PostStudentCourseSelections(StudentCourseSelection studentCourseSelections)
        {
            _context.StudentCourseSelections.Add(studentCourseSelections);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudentCourseSelections", new { id = studentCourseSelections.SelectionID }, studentCourseSelections);
        }

       
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudentCourseSelections(int id)
        {
            var studentCourseSelections = await _context.StudentCourseSelections.FindAsync(id);
            if (studentCourseSelections == null)
            {
                return NotFound();
            }

            _context.StudentCourseSelections.Remove(studentCourseSelections);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentCourseSelectionsExists(int id)
        {
            return _context.StudentCourseSelections.Any(e => e.SelectionID == id);
        }
    }
}
