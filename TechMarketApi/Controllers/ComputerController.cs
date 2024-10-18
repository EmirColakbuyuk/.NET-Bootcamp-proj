using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMarketApi.Data;
using TechMarketApi.Models;

namespace TechMarketApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComputerController : ControllerBase
    {
        private readonly TechMarketContext _context;

        public ComputerController(TechMarketContext context)
        {
            _context = context;
        }

        // GET: api/Computer/getAll
        // Fetches all computers from the database
        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<Computer>>> GetComputers()
        {
            var computers = await _context.Computers.ToListAsync();
            if (computers == null || computers.Count == 0)
            {
                return NotFound(new { message = "No computers found." });
            }
            return Ok(new { message = "Computers retrieved successfully.", data = computers });
        }

        // GET: api/Computer/{id}
        // Fetches a single computer by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Computer>> GetComputer(int id)
        {
            var computer = await _context.Computers.FindAsync(id);

            if (computer == null)
            {
                return NotFound(new { message = $"Computer with ID {id} not found." });
            }

            return Ok(new { message = $"Computer with ID {id} retrieved successfully.", data = computer });
        }

        // PUT: api/Computer/update/{id}
        // Updates a computer's details based on the provided ID
        [HttpPut("update/{id}")]
        public async Task<IActionResult> PutComputer(int id, Computer computer)
        {
            if (id != computer.Id)
            {
                return BadRequest(new { message = "Computer ID mismatch." });
            }

            _context.Entry(computer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComputerExists(id))
                {
                    return NotFound(new { message = $"Computer with ID {id} not found." });
                }
                else
                {
                    return StatusCode(500, new { message = "An error occurred while updating the computer." });
                }
            }

            return Ok(new { message = $"Computer with ID {id} updated successfully." });
        }

        // POST: api/Computer/add
        // Adds a new computer to the database
        [HttpPost("add")]
        public async Task<ActionResult<Computer>> PostComputer(Computer computer)
        {
            _context.Computers.Add(computer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetComputer), new { id = computer.Id }, 
                new { message = "Computer added successfully.", data = computer });
        }

        // DELETE: api/Computer/delete/{id}
        // Deletes a computer based on the provided ID
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteComputer(int id)
        {
            var computer = await _context.Computers.FindAsync(id);
            if (computer == null)
            {
                return NotFound(new { message = $"Computer with ID {id} not found." });
            }

            _context.Computers.Remove(computer);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Computer with ID {id} deleted successfully." });
        }

        // Helper method to check if a computer exists by ID
        private bool ComputerExists(int id)
        {
            return _context.Computers.Any(e => e.Id == id);
        }
    }
}
