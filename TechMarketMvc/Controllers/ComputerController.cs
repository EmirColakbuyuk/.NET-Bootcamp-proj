using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMarketMvc.Data;
using TechMarketMvc.Models;
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

        // GET: /Computers/Add
        public IActionResult Add()
        {
            return View(); 
        }

        // POST: /Computers/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Computer computer)
        {
            if (ModelState.IsValid)
            {
                _context.Computers.Add(computer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var computer = await _context.Computers.FindAsync(id);
            if (computer != null)
            {
                _context.Computers.Remove(computer);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
