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

        // GET: api/Smartwatch
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Smartwatch>>> GetSmartwatches()
        {
            return await _context.Smartwatches.ToListAsync();
        }

        // GET: api/Smartwatch/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Smartwatch>> GetSmartwatch(int id)
        {
            var smartwatch = await _context.Smartwatches.FindAsync(id);

            if (smartwatch == null)
            {
                return NotFound();
            }

            return smartwatch;
        }

        // PUT: api/Smartwatch/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSmartwatch(int id, Smartwatch smartwatch)
        {
            if (id != smartwatch.Id)
            {
                return BadRequest();
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
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Smartwatch
        [HttpPost]
        public async Task<ActionResult<Smartwatch>> PostSmartwatch(Smartwatch smartwatch)
        {
            _context.Smartwatches.Add(smartwatch);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSmartwatch), new { id = smartwatch.Id }, smartwatch);
        }

        // DELETE: api/Smartwatch/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSmartwatch(int id)
        {
            var smartwatch = await _context.Smartwatches.FindAsync(id);
            if (smartwatch == null)
            {
                return NotFound();
            }

            _context.Smartwatches.Remove(smartwatch);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SmartwatchExists(int id)
        {
            return _context.Smartwatches.Any(e => e.Id == id);
        }
    }
}
