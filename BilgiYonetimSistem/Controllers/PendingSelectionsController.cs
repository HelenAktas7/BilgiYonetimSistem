using BilgiYonetimSistem.DataTransfer;
using BilgiYonetimSistem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace BilgiYonetimSistem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PendingSelectionsController : ControllerBase
    {
       
        private readonly DataContext _context;

        public PendingSelectionsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PendingSelectionsDTO>>> GetPendingSelections()
        {
            var PendingSelections = await _context.PendingSelections
                .Include(ns => ns.Student)
                .Include(ns => ns.Course)
                .Select(ns => new PendingSelectionsDTO
                {
                    Id = ns.Id,
                    StudentId = ns.Student.StudentID,
                    StudentFirstName = ns.Student.FirstName,
                    StudentLastName = ns.Student.LastName,
                    CourseName = ns.Course.CourseName,
                    CourseId = ns.Course.CourseId
                })
                .ToListAsync();

            return Ok(PendingSelections);
        }





       
        [HttpGet("{id}")]
        public async Task<ActionResult<PendingSelection>> GetPendingSelections(int id)
        {
            var PendingSelections = await _context.PendingSelections.FindAsync(id);

            if (PendingSelections == null)
            {
                return NotFound();
            }

            return PendingSelections;
        }

      
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PendingSelections(int id, PendingSelection PendingSelections)
        {
            if (id != PendingSelections.Id)
            {
                return BadRequest();
            }

            _context.Entry(PendingSelections).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PendingSelectionsExists(id))
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

      
        [HttpPost]
        public async Task<ActionResult<PendingSelection>> PostPendingSelections(PendingSelection PendingSelections)
        {
            _context.PendingSelections.Add(PendingSelections);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPendingSelections", new { id = PendingSelections.Id }, PendingSelections);
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePendingSelections([FromQuery] int studentId, [FromQuery] int courseId)
        {
       
            var PendingSelections = await _context.PendingSelections
                .Where(n => n.StudentId == studentId && n.CourseId == courseId)
                .ToListAsync();

         
            if (PendingSelections == null || !PendingSelections.Any())
            {
                return NotFound(new { Message = "Eşleşen kayıt bulunamadı." });
            }

         
            _context.PendingSelections.RemoveRange(PendingSelections);
            await _context.SaveChangesAsync();

         
            return NoContent();
        }




        private bool PendingSelectionsExists(int id)
        {
            return _context.PendingSelections.Any(e => e.Id == id);
        }
    }
}
