using Microsoft.AspNetCore.Mvc;
using TechMarketMvc.Data;
using TechMarketMvc.Models;
using TechMarketMvc.ViewModels;
using System.Linq;

namespace TechMarketMvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly TechMarketContext _context;

        public AccountController(TechMarketContext context)
        {
            _context = context;
        }

        // GET: /Admin/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View("~/Views/Admin/Login.cshtml");  // Pointing to Admin folder login view
        }

        // POST: /Admin/Login
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.SingleOrDefault(u => u.Username == model.Username && u.Password == model.Password);
                if (user != null)
                {
                    if (user.Role == "Admin")
                    {
                        // Set authentication cookie or session (basic login for now)
                        TempData["Username"] = user.Username;
                        TempData["Role"] = user.Role;

                        return RedirectToAction("Index", "Admin");  // Redirect to Admin Dashboard
                    }
                    ModelState.AddModelError("", "Only admins can log in.");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                }
            }

            return View("~/Views/Admin/Login.cshtml", model);  // Return to admin login page on failure
        }

        // GET: /Account/Logout
        public IActionResult Logout()
        {
            TempData.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}
