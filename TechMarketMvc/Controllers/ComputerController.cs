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
        public async Task<IActionResult> Add(Computer computer, IFormFile imageFile)
        {
            var allowedExtensions = new[] { ".jpg", ".png", ".jpeg" };

            try
            {
                // Validate image file
                if (imageFile != null && imageFile.Length > 0)
                {
                    var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("", "Please select a valid image type (JPG, PNG, JPEG).");
                    }
                    else
                    {
                        var randomFileName = Guid.NewGuid().ToString() + extension; // Generate a random file name
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", randomFileName);

                        // Ensure the images directory exists
                        if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images")))
                        {
                            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images"));
                        }

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream); // Save the image to the specified path
                        }
                        computer.ImagePath = $"/images/{randomFileName}"; // Set the image path
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Please select an image!"); // Add error if no image selected
                }

                // Check model state
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
                        existingComputer.ImagePath = computer.ImagePath; // Update the image path in the database
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
                else
                {
                    // Log model state errors for debugging
                    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                ModelState.AddModelError("", "An error occurred while adding the computer. Please try again.");
            }

            // Return the view with the current model if there are validation errors or an exception occurred
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

            // Fetch the existing computer entry from the database
            var existingComputer = await _context.Computers.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

            if (existingComputer == null)
            {
                return NotFound();
            }

            try
            {
                // If the ImagePath is not set (null), retain the existing image
                if (string.IsNullOrEmpty(computer.ImagePath))
                {
                    computer.ImagePath = existingComputer.ImagePath;
                }

                if (ModelState.IsValid)
                {
                    _context.Entry(computer).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Computer updated successfully!";
                    return RedirectToAction(nameof(Manage));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                ModelState.AddModelError("", "An error occurred while updating the computer. Please try again.");
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
