using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMarketApi.Data;
using TechMarketApi.Models;

namespace TechMarketApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhoneController : ControllerBase
    {
        private readonly TechMarketContext _context;

        public PhoneController(TechMarketContext context)
        {
            _context = context;
        }

        // GET: api/Phone/getAll
        // Fetches all phones from the database
        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<Phone>>> GetPhones()
        {
            var phones = await _context.Phones.ToListAsync();
            if (phones == null || phones.Count == 0)
            {
                return NotFound(new { message = "No phones found." });
            }
            return Ok(new { message = "Phones retrieved successfully.", data = phones });
        }

        // GET: api/Phone/{id}
        // Fetches a single phone by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Phone>> GetPhone(int id)
        {
            var phone = await _context.Phones.FindAsync(id);

            if (phone == null)
            {
                return NotFound(new { message = $"Phone with ID {id} not found." });
            }

            return Ok(new { message = $"Phone with ID {id} retrieved successfully.", data = phone });
        }

        // PUT: api/Phone/update/{id}
        // Updates a phone's details based on the provided ID
        [HttpPut("update/{id}")]
        public async Task<IActionResult> PutPhone(int id, Phone phone)
        {
            if (id != phone.Id)
            {
                return BadRequest(new { message = "Phone ID mismatch." });
            }

            _context.Entry(phone).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhoneExists(id))
                {
                    return NotFound(new { message = $"Phone with ID {id} not found." });
                }
                else
                {
                    return StatusCode(500, new { message = "An error occurred while updating the phone." });
                }
            }

            return Ok(new { message = $"Phone with ID {id} updated successfully." });
        }

        // POST: api/Phone/add
        // Adds a new phone to the database
        [HttpPost("add")]
        public async Task<ActionResult<Phone>> PostPhone(Phone phone)
        {
            _context.Phones.Add(phone);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPhone), new { id = phone.Id }, 
                new { message = "Phone added successfully.", data = phone });
        }

        // DELETE: api/Phone/delete/{id}
        // Deletes a phone based on the provided ID
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeletePhone(int id)
        {
            var phone = await _context.Phones.FindAsync(id);
            if (phone == null)
            {
                return NotFound(new { message = $"Phone with ID {id} not found." });
            }

            _context.Phones.Remove(phone);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Phone with ID {id} deleted successfully." });
        }

        // Helper method to check if a phone exists by ID
        private bool PhoneExists(int id)
        {
            return _context.Phones.Any(e => e.Id == id);
        }
    }
}
