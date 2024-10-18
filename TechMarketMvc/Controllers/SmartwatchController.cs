using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMarketMvc.Data;
using TechMarketMvc.Models;
using System.Threading.Tasks;

namespace TechMarketMvc.Controllers
{
    public class SmartwatchesController : Controller
    {
        private readonly TechMarketContext _context;

        public SmartwatchesController(TechMarketContext context)
        {
            _context = context;
        }

        // GET: /Smartwatches
        public async Task<IActionResult> Index()
        {
            var smartwatches = await _context.Smartwatches.ToListAsync();
            return View(smartwatches);
        }

        // GET: /Smartwatches/Add
        public IActionResult Add()
        {
            return View();
        }

        // POST: /Smartwatches/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Smartwatch smartwatch)
        {
            if (ModelState.IsValid)
            {
                // Check if a smartwatch with the same name, brand, and HasGPS exists
                var existingSmartwatch = await _context.Smartwatches
                    .FirstOrDefaultAsync(s => s.Name == smartwatch.Name && s.Brand == smartwatch.Brand 
                                              && s.HasGPS == smartwatch.HasGPS);

                if (existingSmartwatch != null)
                {
                    // If it exists, update the stock
                    existingSmartwatch.Stock += smartwatch.Stock;
                    _context.Entry(existingSmartwatch).State = EntityState.Modified;
                }
                else
                {
                    // If it does not exist, add it to the database
                    _context.Smartwatches.Add(smartwatch);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(smartwatch);
        }

        // GET: /Smartwatches/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var smartwatch = await _context.Smartwatches.FindAsync(id);
            if (smartwatch == null)
            {
                return NotFound();
            }
            return View(smartwatch); 
        }

        // POST: /Smartwatches/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Smartwatch smartwatch)
        {
            if (id != smartwatch.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _context.Entry(smartwatch).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(smartwatch);
        }

        // GET: /Smartwatches/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var smartwatch = await _context.Smartwatches.FindAsync(id);
            if (smartwatch == null)
            {
                return NotFound();
            }

            return View(smartwatch); 
        }

        // POST: /Smartwatches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var smartwatch = await _context.Smartwatches.FindAsync(id);
            if (smartwatch != null)
            {
                _context.Smartwatches.Remove(smartwatch);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
