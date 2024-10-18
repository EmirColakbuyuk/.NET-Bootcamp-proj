using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMarketMvc.Data;
using TechMarketMvc.Models;
using System.Linq;
using System.Threading.Tasks;

namespace TechMarketMvc.Controllers
{
    public class ComputersController : Controller
    {
        private readonly TechMarketContext _context;

        public ComputersController(TechMarketContext context)
        {
            _context = context;
        }

        // GET: /Computers
        public async Task<IActionResult> Index()
        {
            var computers = await _context.Computers.ToListAsync();
            return View(computers);
        }

        // GET: /Computers/Manage
        // Allows filtering by name or brand
        public async Task<IActionResult> Manage(string searchString)
        {
            var computers = from c in _context.Computers
                            select c;

            if (!string.IsNullOrEmpty(searchString))
            {
                computers = computers.Where(s => s.Name.Contains(searchString) || s.Brand.Contains(searchString));
            }

            return View(await computers.ToListAsync());
        }

        // GET: /Computers/Add
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Computer computer)
        {
            if (ModelState.IsValid)
            {
                // Check if a computer with the same name, brand, processor, RAM, and storage exists
                var existingComputer = await _context.Computers
                    .FirstOrDefaultAsync(c => c.Name == computer.Name && c.Brand == computer.Brand 
                                            && c.Processor == computer.Processor 
                                            && c.RAM == computer.RAM 
                                            && c.Storage == computer.Storage);

                if (existingComputer != null)
                {
                    // If it exists, update the stock
                    existingComputer.Stock += computer.Stock;
                    _context.Entry(existingComputer).State = EntityState.Modified;
                }
                else
                {
                    // If it does not exist, add it to the database
                    _context.Computers.Add(computer);
                }

                await _context.SaveChangesAsync();

                // Set a success message in TempData
                TempData["SuccessMessage"] = "Computer added successfully!";

                // Redirect to the Manage page
                return RedirectToAction("Manage");
            }
            return View(computer);
        }

        // GET: /Computers/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var computer = await _context.Computers.FindAsync(id);
            if (computer == null)
            {
                return NotFound();
            }
            return View(computer);
        }

        // POST: /Computers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Computer computer)
        {
            if (id != computer.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _context.Entry(computer).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(computer);
        }

        // GET: /Computers/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var computer = await _context.Computers.FindAsync(id);
            if (computer == null)
            {
                return NotFound();
            }

            return View(computer);
        }

        // POST: /Computers/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var computer = await _context.Computers.FindAsync(id);
            if (computer != null)
            {
                // Decrease the stock by 1
                computer.Stock--;

                if (computer.Stock <= 0)
                {
                    // If stock is 0 or less, remove the computer from the database
                    _context.Computers.Remove(computer);
                }
                else
                {
                    // If stock is still above 0, just update it
                    _context.Entry(computer).State = EntityState.Modified;
                }

                await _context.SaveChangesAsync();
            }

            // Return a response but do not redirect
            return NoContent();
        }

    }
}
