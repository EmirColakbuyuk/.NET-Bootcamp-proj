using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMarketMvc.Data;
using TechMarketMvc.Models;
using System.IO;
using System.Linq;
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
            var smartwatches = from s in _context.Smartwatches select s;

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
            var allowedExtensions = new[] { ".jpg", ".png", ".jpeg" };

            if (imageFile != null && imageFile.Length > 0)
            {
                var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("", "Please select a valid image type (JPG, PNG, JPEG).");
                }
                else
                {
                    var randomFileName = Guid.NewGuid().ToString() + extension;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", randomFileName);

                    if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images")))
                    {
                        Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images"));
                    }

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    smartwatch.ImagePath = $"/images/{randomFileName}";
                }
            }
            else
            {
                ModelState.AddModelError("", "Please select an image!");
            }

            if (ModelState.IsValid)
            {
                _context.Smartwatches.Add(smartwatch);
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

            // Fetch the existing smartwatch entry from the database
            var existingSmartwatch = await _context.Smartwatches.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);

            if (existingSmartwatch == null)
            {
                return NotFound();
            }

            try
            {
                // If the ImagePath is not set (null or empty), retain the existing image
                if (string.IsNullOrEmpty(smartwatch.ImagePath))
                {
                    smartwatch.ImagePath = existingSmartwatch.ImagePath;
                }

                if (ModelState.IsValid)
                {
                    _context.Entry(smartwatch).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Smartwatch updated successfully!";
                    return RedirectToAction(nameof(Manage));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                ModelState.AddModelError("", "An error occurred while updating the smartwatch. Please try again.");
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
