using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TechMarketMvc.Models;
using TechMarketMvc.ViewModels;
using TechMarketMvc.Data;
using System.Linq;

namespace TechMarketMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TechMarketContext _context;  

        public HomeController(ILogger<HomeController> logger, TechMarketContext context)
        {
            _logger = logger;
            _context = context; 
        }

        // Home Page: Shows the latest 3 products from each category
        public IActionResult Index()
        {
            var model = new ProductViewModel
            {
                Computers = _context.Computers
                    .OrderByDescending(c => c.Id) 
                    .Take(3)  
                    .ToList(),
                
                Phones = _context.Phones
                    .OrderByDescending(p => p.Id)
                    .Take(3)
                    .ToList(),
                
                Smartwatches = _context.Smartwatches
                    .OrderByDescending(s => s.Id)
                    .Take(3)
                    .ToList()
            };

            return View(model);  // Pass the ProductViewModel to the view
        }

        // Product Details for Computers
        public IActionResult ComputerDetails(int id)
        {
            var computer = _context.Computers.FirstOrDefault(c => c.Id == id);
            if (computer == null)
            {
                return NotFound();
            }
            return View(computer);
        }

        // Product Details for Phones
        public IActionResult PhoneDetails(int id)
        {
            var phone = _context.Phones.FirstOrDefault(p => p.Id == id);
            if (phone == null)
            {
                return NotFound();
            }
            return View(phone);
        }

        // Product Details for Smartwatches
        public IActionResult SmartwatchDetails(int id)
        {
            var smartwatch = _context.Smartwatches.FirstOrDefault(s => s.Id == id);
            if (smartwatch == null)
            {
                return NotFound();
            }
            return View(smartwatch);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
