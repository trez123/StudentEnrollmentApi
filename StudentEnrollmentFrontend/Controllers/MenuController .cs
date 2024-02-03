using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using StudentEnrollmentFrontend.Models;
using System.Text;
using System.Net.Http.Headers;

namespace StudentEnrollmentFrontend.Controllers
{
    public class MenuController : Controller
    {
        const string SESSION_AUTH = "Student";
        private readonly IHttpClientFactory _clientHandler;
        public MenuController(IHttpClientFactory clientHandler)
        {
            this._clientHandler = clientHandler;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TextColor = "black";
            ViewBag.ButtonColor = "#ED6468";
            string token = HttpContext.Session.GetString(SESSION_AUTH)!;
            if(string.IsNullOrEmpty(token)) return RedirectToAction("Login","Auth");
            HttpClient studentClient = _clientHandler.CreateClient("MenuAPI");
            studentClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

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

                return View(ImageList.Take(3));
            }

            else
                return Problem("Error in Api response");
        }


        public IActionResult Upsert(int id = 0)
        {
            ViewBag.TextColor = "black";
            ViewBag.ButtonColor = "#ED6468";
            var mealTypeResponse = _clientHandler.CreateClient("MenuAPI").GetAsync("mealType").Result;

            var MealTypes = mealTypeResponse.Content.ReadAsStringAsync().Result;

            List<MealType> MealTypeList = JsonConvert.DeserializeObject<List<MealType>>(MealTypes)!;

            MenuVM viewModel = new()
            {
                MealTypeList = MealTypeList.Select(data => new SelectListItem
                {
                    Text = data.MealTypeName,
                    Value = data.Id.ToString()
                }).ToList()
            };

            if(id == 0)
                return View(viewModel);
            else
            {
                var MenuResponse = _clientHandler.CreateClient("StudentAPI").GetAsync($"{id}").Result;
                var Menu = MenuResponse.Content.ReadAsStringAsync().Result;

                MenuVM view = JsonConvert.DeserializeObject<MenuVM>(Menu)!;

                viewModel.Starch = view.Starch;
                viewModel.Beverage = view.Beverage;
                viewModel.Meat = view.Meat;
                viewModel.Vegetable = view.Vegetable;
                viewModel.SelectedMealTypeId = view.SelectedMealTypeId;
                
                return View(viewModel);
            }
        }


        //[HttpPost]

        //public IActionResult Upsert(StudentVM studentvm)
        //{
        //    if (!ModelState.IsValid) return View(studentvm);

        //    var student = new Student
        //    {
        //        StudentName = studentvm.StudentName,
        //        EmailAddress = studentvm.EmailAddress,
        //        PhoneNumber = studentvm.PhoneNumber,
        //        ParishId = studentvm.SelectedParishId,
        //        ProgramId = studentvm.SelectedProgramId,
        //        SizeId = studentvm.SelectedSizeId,
        //        //StudntIdImageFile = studentvm.StudntIdImageFile
        //    };

        //    var json = JsonConvert.SerializeObject(student);

        //    var data = new StringContent(json, Encoding.UTF8, "application/json");

        //    if (student.Id == 0)
        //    {
        //        var response = _clientHandler.CreateClient("StudentAPI").PostAsync("", data).Result;

        //        if (response.IsSuccessStatusCode)
        //        {
        //            return RedirectToAction("Index");
        //        }
        //        else
        //        {
        //            ModelState.AddModelError(string.Empty, "Product creation failed");
        //            return View(studentvm);
        //        }
        //    }
        //    else
        //    {
        //        var response = _clientHandler.CreateClient("StudentAPI").PutAsync($"{student.Id}", data).Result;

        //        if (response.IsSuccessStatusCode)
        //        {
        //            return RedirectToAction("Index");
        //        }
        //        else
        //        {
        //            ModelState.AddModelError(string.Empty, "Product creation failed");
        //            return View(studentvm);
        //        }
        //    }
        //}

        [HttpPost]

        public async Task<IActionResult> Upsert(MenuVM menuvm)
        {
            ViewBag.TextColor = "black";
            ViewBag.ButtonColor = "#ED6468";

            if (!ModelState.IsValid) return View(menuvm);

            MenuCreateDTO menu = new()
            {
                Starch = menuvm.Starch,
                Beverage = menuvm.Beverage,
                Meat = menuvm.Meat,
                Vegetable = menuvm.Vegetable,
                MealTypeId = menuvm.SelectedMealTypeId,
                MenuIdImageFile = menuvm.MenuIdImageFile,
            };

            if (menuvm.Id == 0)
            {
                var response = await SendMenuToApi(menu);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Product creation failed");
                    return View(menuvm);
                }
            }
            else
            {
                var response = await SendMenuToApi(menu);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Product creation failed");
                    return View(menuvm);
                }
            }
        }

        private async Task<HttpResponseMessage> SendMenuToApi(MenuCreateDTO menuCreateDTO)
        {
            HttpClient client = _clientHandler.CreateClient("MenuAPI");

            MultipartFormDataContent formData = new();
            formData.Headers.ContentType!.MediaType = "multipart/form-data";
            formData.Add(new StringContent(menuCreateDTO.Starch!), "Starch");
            formData.Add(new StringContent(menuCreateDTO.Beverage!), "Beverage");
            formData.Add(new StringContent(menuCreateDTO.Meat!), "Meat");
            formData.Add(new StringContent(menuCreateDTO.Vegetable!), "Vegetable");
            formData.Add(new StringContent(menuCreateDTO.MealTypeId.ToString()), "MealTypeId");

            if (menuCreateDTO.MenuIdImageFile != null && menuCreateDTO.MenuIdImageFile.Length > 0)
            {
                formData.Add(new StreamContent(menuCreateDTO.MenuIdImageFile.OpenReadStream())
                {
                    Headers = {
                                ContentLength = menuCreateDTO.MenuIdImageFile.Length,
                                ContentType = new MediaTypeHeaderValue(menuCreateDTO.MenuIdImageFile.ContentType)
                            }
                }, "MenuIdImageFile", menuCreateDTO.MenuIdImageFile.FileName);
            }
            return await client.PostAsync("FileUpload", formData);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string token = HttpContext.Session.GetString(SESSION_AUTH)!;
            if(string.IsNullOrEmpty(token)) return RedirectToAction("Login","Auth");
            HttpClient studentClient = _clientHandler.CreateClient("MenuAPI");
            studentClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await studentClient.DeleteAsync($"{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Student Delete failed");
                return RedirectToAction("Index");
            }
        }
    }
}
