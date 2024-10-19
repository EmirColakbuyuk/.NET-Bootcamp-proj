using Microsoft.AspNetCore.Mvc;

namespace TechMarketMvc.Controllers
{
    public class AdminController : Controller
    {
       
        public IActionResult Index()
        {
           
            if (TempData["Username"] == null || TempData["Role"]?.ToString() != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            return View("AdminDashboard"); 
        }

        
        public IActionResult Dashboard()
        {
            if (TempData["Username"] == null || TempData["Role"]?.ToString() != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            return View("AdminDashboard"); 
        }
    }
}
