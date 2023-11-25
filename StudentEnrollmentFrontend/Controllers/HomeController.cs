using Microsoft.AspNetCore.Mvc;
using StudentEnrollmentFrontend.Models;
using System.Diagnostics;

namespace StudentEnrollmentFrontend.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            ViewBag.TextColor = "black";
            ViewBag.ButtonColor = "#ED6468";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            ViewBag.TextColor = "black";
            ViewBag.ButtonColor = "#ED6468";
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}