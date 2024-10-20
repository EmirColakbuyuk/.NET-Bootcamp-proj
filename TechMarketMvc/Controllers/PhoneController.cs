using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMarketMvc.Data;
using TechMarketMvc.Models;
using System.IO;
using System.Linq;
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
            Console.WriteLine("Fetched phones from the database.");
            return View(phones);
        }

        // GET: /Phones/Manage
        public async Task<IActionResult> Manage(string searchString)
        {
            var phones = from p in _context.Phones select p;

            if (!string.IsNullOrEmpty(searchString))
            {
                phones = phones.Where(s => s.Name.Contains(searchString) || s.Brand.Contains(searchString));
                Console.WriteLine($"Filtered phones with search string: {searchString}");
            }

            var result = await phones.ToListAsync();
            Console.WriteLine($"Total phones retrieved: {result.Count}");
            return View(result);
        }

        // GET: /Phones/Add
        public IActionResult Add()
        {
            return View();
        }

        // POST: /Phones/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Phone phone, IFormFile imageFile)
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
                    phone.ImagePath = $"/images/{randomFileName}";
                }
            }
            else
            {
                ModelState.AddModelError("", "Please select an image!");
            }

            if (ModelState.IsValid)
            {
                _context.Phones.Add(phone);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Phone added successfully!";
                return RedirectToAction("Manage");
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

            // Fetch the existing phone entry from the database
            var existingPhone = await _context.Phones.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

            if (existingPhone == null)
            {
                return NotFound();
            }

            try
            {
                // If the ImagePath is not set (null or empty), retain the existing image
                if (string.IsNullOrEmpty(phone.ImagePath))
                {
                    phone.ImagePath = existingPhone.ImagePath;
                }

                if (ModelState.IsValid)
                {
                    _context.Entry(phone).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Phone updated successfully!";
                    return RedirectToAction(nameof(Manage));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                ModelState.AddModelError("", "An error occurred while updating the phone. Please try again.");
            }

            return View(phone);
        }


        // POST: /Phones/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var phone = await _context.Phones.FindAsync(id);
            if (phone != null)
            {
                _context.Phones.Remove(phone);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Phone deleted successfully!";
            }
            return RedirectToAction("Manage");
        }
    }
}
