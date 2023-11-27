using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudentEnrollmentFrontend.Models;
using System.Diagnostics;

namespace StudentEnrollmentFrontend.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
         private readonly IHttpClientFactory _clientHandler;
        public HomeController(ILogger<HomeController> logger, IHttpClientFactory clientHandler)
        {
            _logger = logger;
            this._clientHandler = clientHandler;
        }

        public async Task<IActionResult> Index()
        {
            HttpClient studentClient = _clientHandler.CreateClient("MenuAPI");

            HttpResponseMessage response = await studentClient.GetAsync("");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();

                List<Menu> MenuList = JsonConvert.DeserializeObject<List<Menu>>(content)!;

                List<MenuImageViewModel> ImageList = new();

                foreach(Menu menu in MenuList)
                {
                    HttpResponseMessage MenuImage = await studentClient.GetAsync($"files/{menu.MenuImageFilePath}");
                    if (MenuImage.IsSuccessStatusCode)
                    {
                        byte[] imageBytes = await MenuImage.Content.ReadAsByteArrayAsync();
                        string contentType = MenuImage.Content.Headers.ContentType!.ToString();

                        MenuImageViewModel imageView = new()
                        {
                            Menu = menu, 
                            ImageBytes = imageBytes, 
                            ContentType = contentType 
                        };

                        ImageList.Add(imageView);
                    }
                }

                return View(ImageList);
            }
            return View();
        }

        public IActionResult Privacy()
        {
            ViewBag.TextColor = "black";
            ViewBag.ButtonColor = "#ED6468";
            return View();
        }

        public IActionResult EnrollmentComplete()
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