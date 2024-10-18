using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMarketMvc.Data;
using TechMarketMvc.Models;
using System.Threading.Tasks;

namespace TechMarketMvc.Controllers
{
    public class PhonesController : Controller
    {
        private readonly TechMarketContext _context;

        public PhonesController(TechMarketContext context)
        {
            _context = context;
        }

        // GET: /Phones
        public async Task<IActionResult> Index()
        {
            var phones = await _context.Phones.ToListAsync();
            return View(phones);
        }

        // GET: /Phones/Add
        public IActionResult Add()
        {
            return View();
        }

        // POST: /Phones/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Phone phone)
        {
            if (ModelState.IsValid)
            {
                // Check if a phone with the same name, brand, OS, RAM, and storage exists
                var existingPhone = await _context.Phones
                    .FirstOrDefaultAsync(p => p.Name == phone.Name && p.Brand == phone.Brand 
                                              && p.OperatingSystem == phone.OperatingSystem 
                                              && p.RAM == phone.RAM 
                                              && p.Storage == phone.Storage);

                if (existingPhone != null)
                {
                    // If it exists, update the stock
                    existingPhone.Stock += phone.Stock;
                    _context.Entry(existingPhone).State = EntityState.Modified;
                }
                else
                {
                    // If it does not exist, add it to the database
                    _context.Phones.Add(phone);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(phone);
        }

        // GET: /Phones/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var phone = await _context.Phones.FindAsync(id);
            if (phone == null)
            {
                return NotFound();
            }
            return View(phone); 
        }

        // POST: /Phones/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Phone phone)
        {
            if (id != phone.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _context.Entry(phone).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(phone);
        }

        // GET: /Phones/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var phone = await _context.Phones.FindAsync(id);
            if (phone == null)
            {
                return NotFound();
            }

            return View(phone); 
        }

        // POST: /Phones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var phone = await _context.Phones.FindAsync(id);
            if (phone != null)
            {
                // Decrease the stock by 1
                phone.Stock--;

                if (phone.Stock <= 0)
                {
                    // If stock is 0 or less, remove the phone from the database
                    _context.Phones.Remove(phone);
                }
                else
                {
                    // If stock is still above 0, just update it
                    _context.Entry(phone).State = EntityState.Modified;
                }

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
