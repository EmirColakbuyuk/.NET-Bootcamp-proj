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
                var existingComputer = await _context.Computers
                    .FirstOrDefaultAsync(c => c.Name == computer.Name && c.Brand == computer.Brand 
                                            && c.Processor == computer.Processor 
                                            && c.RAM == computer.RAM 
                                            && c.Storage == computer.Storage);

                if (existingComputer != null)
                {
                    existingComputer.Stock += computer.Stock;
                    _context.Entry(existingComputer).State = EntityState.Modified;
                }
                else
                {
                    _context.Computers.Add(computer);
                }

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Computer added successfully!";
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
                TempData["SuccessMessage"] = "Computer updated successfully!";
                return RedirectToAction(nameof(Manage));
            }
            return View(computer);
        }

        
       // DELETE: /Computers/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var computer = await _context.Computers.FindAsync(id);
            if (computer != null)
            {
                // Remove the computer from the database
                _context.Computers.Remove(computer);
                await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = "Computer deleted successfully!";
            }
            
            // Redirect to the Manage page
            return RedirectToAction("Manage");
        }

    }
}
