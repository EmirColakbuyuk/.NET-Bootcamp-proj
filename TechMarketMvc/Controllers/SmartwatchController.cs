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

        // GET: /Smartwatches/Manage
        public async Task<IActionResult> Manage(string searchString)
        {
            var smartwatches = from s in _context.Smartwatches
                               select s;

            if (!string.IsNullOrEmpty(searchString))
            {
                smartwatches = smartwatches.Where(s => s.Name.Contains(searchString) || s.Brand.Contains(searchString));
            }

            return View(await smartwatches.ToListAsync());
        }

        // GET: /Smartwatches/Add
        public IActionResult Add()
        {
            return View();
        }

        // POST: /Smartwatches/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Smartwatch smartwatch, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imageFile.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    smartwatch.ImagePath = $"/images/{imageFile.FileName}";
                }

                // Existing logic...
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Smartwatch added successfully!";
                return RedirectToAction("Manage");
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
                TempData["SuccessMessage"] = "Smartwatch updated successfully!";
                return RedirectToAction("Manage");
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
                TempData["SuccessMessage"] = "Smartwatch deleted successfully!";
            }
            return RedirectToAction("Manage");
        }
    }
}
