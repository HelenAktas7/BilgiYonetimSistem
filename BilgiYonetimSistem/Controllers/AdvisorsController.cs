using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BilgiYonetimSistem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseSelectionApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvisorsController : ControllerBase
    {
        private readonly DataContext _context;

        public AdvisorsController(DataContext context)
        {
            _context = context;
        }

      
        [HttpGet]
        public async Task<IActionResult> GetAdvisor()
        {
            var advisors = await _context.Advisors
                .Select(a => new
                {
                    a.AdvisorId,
                    a.FullName,
                    a.Title,
                    a.Department,
                    a.Email,
                    Students = a.Students.Select(s => new
                    {
                        s.StudentID,
                        s.FirstName,
                        s.LastName
                    }).ToList()
                })
                .ToListAsync();

            return Ok(advisors);
        }


       
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAdvisors(int id)
        {
            var advisor = await _context.Advisors
                .Where(a => a.AdvisorId == id)
                .Select(a => new
                {
                    a.AdvisorId,
                    a.FullName,
                    a.Title,
                    a.Department,
                    a.Email,
                    Students = a.Students.Select(s => new
                    {
                        s.StudentID,
                        s.FirstName,
                        s.LastName
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (advisor == null)
            {
                return NotFound(); 
            }

            return Ok(advisor);
        }


       
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdvisors(int id, Advisor advisors)
        {
            if (id != advisors.AdvisorId)
            {
                return BadRequest();
            }

            _context.Entry(advisors).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdvisorsExists(id))
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
        public async Task<ActionResult<Advisor>> PostAdvisors(Advisor advisors)
        {
            _context.Advisors.Add(advisors);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAdvisors", new { id = advisors.AdvisorId }, advisors);
        }

     
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdvisors(int id)
        {
            var advisors = await _context.Advisors.FindAsync(id);
            if (advisors == null)
            {
                return NotFound();
            }

            _context.Advisors.Remove(advisors);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AdvisorsExists(int id)
        {
            return _context.Advisors.Any(e => e.AdvisorId == id);
        }
    }
}