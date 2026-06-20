using Microsoft.AspNetCore.Mvc;
using GymManagement.DAL.Models;
using System.Diagnostics;

namespace MVC01_MVC02.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
