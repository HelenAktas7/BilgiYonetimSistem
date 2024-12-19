using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BilgiYonetimSistem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BilgiYonetimSistem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PendingCourseSelectionsController : ControllerBase
    {
        private readonly DataContext _context;

        public PendingCourseSelectionsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/PendingCourseSelections
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetPendingCourseSelections()
        {
            var pendingSelections = await _context.PendingCourseSelections
                .Include(p => p.Student)
                .Include(p => p.Course)
                .Select(p => new
                {
                    p.Id,
                    p.StudentId,
                    StudentName = p.Student.FirstName + " " + p.Student.LastName,
                    p.CourseId,
                    CourseName = p.Course.CourseName,
                    p.SelectionDate,
                    p.IsConfirmed
                })
                .ToListAsync();

            return Ok(pendingSelections);
        }

        // GET: api/PendingCourseSelections/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPendingCourseSelection(int id)
        {
            var pendingSelection = await _context.PendingCourseSelections
                .Include(p => p.Student)
                .Include(p => p.Course)
                .Where(p => p.Id == id)
                .Select(p => new
                {
                    p.Id,
                    p.StudentId,
                    StudentName = p.Student.FirstName + " " + p.Student.LastName,
                    p.CourseId,
                    CourseName = p.Course.CourseName,
                    p.SelectionDate,
                    p.IsConfirmed
                })
                .FirstOrDefaultAsync();

            if (pendingSelection == null)
            {
                return NotFound();
            }

            return Ok(pendingSelection);
        }

        // PUT: api/PendingCourseSelections/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPendingCourseSelection(int id, PendingCourseSelection pendingCourseSelection)
        {
            if (id != pendingCourseSelection.Id)
            {
                return BadRequest();
            }

            _context.Entry(pendingCourseSelection).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PendingCourseSelectionExists(id))
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

        // POST: api/PendingCourseSelections
        [HttpPost]
        public async Task<ActionResult<PendingCourseSelection>> PostPendingCourseSelection(PendingCourseSelection pendingCourseSelection)
        {
            _context.PendingCourseSelections.Add(pendingCourseSelection);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPendingCourseSelection", new { id = pendingCourseSelection.Id }, pendingCourseSelection);
        }

        // DELETE: api/PendingCourseSelections/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePendingCourseSelection(int id)
        {
            var pendingCourseSelection = await _context.PendingCourseSelections.FindAsync(id);
            if (pendingCourseSelection == null)
            {
                return NotFound();
            }

            _context.PendingCourseSelections.Remove(pendingCourseSelection);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PendingCourseSelectionExists(int id)
        {
            return _context.PendingCourseSelections.Any(e => e.Id == id);
        }
    }
}
