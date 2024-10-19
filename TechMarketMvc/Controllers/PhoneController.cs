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
            Console.WriteLine("Fetched phones from the database.");
            return View(phones);
        }

        // GET: /Phones/Manage
        public async Task<IActionResult> Manage(string searchString)
        {
            var phones = from p in _context.Phones
                         select p;

            if (!string.IsNullOrEmpty(searchString))
            {
                phones = phones.Where(s => s.Name.Contains(searchString) || s.Brand.Contains(searchString));
                Console.WriteLine($"Filtered phones with search string: {searchString}");
            }
            else
            {
                Console.WriteLine("No search string provided; fetching all phones.");
            }

            var result = await phones.ToListAsync();
            Console.WriteLine($"Total phones retrieved: {result.Count}");
            return View(result);
        }

        // GET: /Phones/Add
        public IActionResult Add()
        {
            Console.WriteLine("Navigated to Add Phone page.");
            return View();
        }

        // POST: /Phones/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Phone phone, IFormFile imageFile)
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
                    phone.ImagePath = $"/images/{imageFile.FileName}";
                }

                // Existing logic...
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
                Console.WriteLine($"Phone with ID {id} not found.");
                return NotFound();
            }

            Console.WriteLine($"Navigated to Edit page for phone: {phone.Name}");
            return View(phone); 
        }

        // POST: /Phones/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Phone phone)
        {
            if (id != phone.Id)
            {
                Console.WriteLine("ID mismatch in Edit method.");
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                Console.WriteLine($"Updating phone: {phone.Name}");
                _context.Entry(phone).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Phone updated successfully!";
                Console.WriteLine("Phone updated successfully. Redirecting to Manage page.");
                return RedirectToAction("Manage");
            }

            Console.WriteLine("Model is invalid, returning to Edit page.");
            return View(phone);
        }

        // POST: /Phones/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var phone = await _context.Phones.FindAsync(id);
            if (phone != null)
            {
                Console.WriteLine($"Deleting phone: {phone.Name}");
                _context.Phones.Remove(phone);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Phone deleted successfully!";
                Console.WriteLine("Phone deleted successfully. Redirecting to Manage page.");
            }
            else
            {
                Console.WriteLine($"Phone with ID {id} not found for deletion.");
            }

            return RedirectToAction("Manage");
        }
    }
}
