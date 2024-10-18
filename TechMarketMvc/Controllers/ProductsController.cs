using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TechMarketMvc.Data;
using TechMarketMvc.Models;
using TechMarketMvc.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace TechMarketMvc.Controllers
{
    public class ProductsController : Controller
    {
        private readonly TechMarketContext _context;

        public ProductsController(TechMarketContext context)
        {
            _context = context;
        }

        // GET: /Products
        public async Task<IActionResult> Index()
        {
            // Fetch all products (computers, phones, and smartwatches)
            var computers = await _context.Computers.ToListAsync();
            var phones = await _context.Phones.ToListAsync();
            var smartwatches = await _context.Smartwatches.ToListAsync();

            var products = new ProductViewModel
            {
                Computers = computers,
                Phones = phones,
                Smartwatches = smartwatches
            };

            return View(products);
        }
    }
}
