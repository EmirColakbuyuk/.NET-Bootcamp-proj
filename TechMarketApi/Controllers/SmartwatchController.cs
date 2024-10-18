using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMarketApi.Data;
using TechMarketApi.Models;

namespace TechMarketApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SmartwatchController : ControllerBase
    {
        private readonly TechMarketContext _context;

        public SmartwatchController(TechMarketContext context)
        {
            _context = context;
        }

        // GET: api/Smartwatch/getAll
        // Fetches all smartwatches from the database
        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<Smartwatch>>> GetSmartwatches()
        {
            var smartwatches = await _context.Smartwatches.ToListAsync();
            if (smartwatches == null || smartwatches.Count == 0)
            {
                return NotFound(new { message = "No smartwatches found." });
            }
            return Ok(new { message = "Smartwatches retrieved successfully.", data = smartwatches });
        }

        // GET: api/Smartwatch/{id}
        // Fetches a single smartwatch by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Smartwatch>> GetSmartwatch(int id)
        {
            var smartwatch = await _context.Smartwatches.FindAsync(id);

            if (smartwatch == null)
            {
                return NotFound(new { message = $"Smartwatch with ID {id} not found." });
            }

            return Ok(new { message = $"Smartwatch with ID {id} retrieved successfully.", data = smartwatch });
        }

        // PUT: api/Smartwatch/update/{id}
        // Updates a smartwatch's details based on the provided ID
        [HttpPut("update/{id}")]
        public async Task<IActionResult> PutSmartwatch(int id, Smartwatch smartwatch)
        {
            if (id != smartwatch.Id)
            {
                return BadRequest(new { message = "Smartwatch ID mismatch." });
            }

            _context.Entry(smartwatch).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SmartwatchExists(id))
                {
                    return NotFound(new { message = $"Smartwatch with ID {id} not found." });
                }
                else
                {
                    return StatusCode(500, new { message = "An error occurred while updating the smartwatch." });
                }
            }

            return Ok(new { message = $"Smartwatch with ID {id} updated successfully." });
        }

        // POST: api/Smartwatch/add
        // Adds a new smartwatch to the database
        [HttpPost("add")]
        public async Task<ActionResult<Smartwatch>> PostSmartwatch(Smartwatch smartwatch)
        {
            _context.Smartwatches.Add(smartwatch);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSmartwatch), new { id = smartwatch.Id }, 
                new { message = "Smartwatch added successfully.", data = smartwatch });
        }

        // DELETE: api/Smartwatch/delete/{id}
        // Deletes a smartwatch based on the provided ID
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteSmartwatch(int id)
        {
            var smartwatch = await _context.Smartwatches.FindAsync(id);
            if (smartwatch == null)
            {
                return NotFound(new { message = $"Smartwatch with ID {id} not found." });
            }

            _context.Smartwatches.Remove(smartwatch);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Smartwatch with ID {id} deleted successfully." });
        }

        // Helper method to check if a smartwatch exists by ID
        private bool SmartwatchExists(int id)
        {
            return _context.Smartwatches.Any(e => e.Id == id);
        }
    }
}
