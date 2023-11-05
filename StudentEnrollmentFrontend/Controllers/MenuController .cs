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
        const string API_URL = "https://localhost:7240/api/Menu/";
        private readonly IHttpClientFactory _clientHandler;
        public MenuController(IHttpClientFactory clientHandler)
        {
            this._clientHandler = clientHandler;
        }

        public async Task<IActionResult> Index()
        {
            var httpClient = _clientHandler.CreateClient("MenuAPI");

            var response = await httpClient.GetAsync("");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var productList = JsonConvert.DeserializeObject<List<Student>>(content);

                return View(productList);
            }

            else
                return Problem("Error in Api response");
        }


        public IActionResult Upsert(int id = 0)
        {
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
                viewModel.MealTypeId = view.MealTypeId;
                
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
            if (!ModelState.IsValid) return View(menuvm);

            MenuCreateDTO menu = new()
            {
                Starch = menuvm.Starch,
                Beverage = menuvm.Beverage,
                Meat = menuvm.Meat,
                Vegetable = menuvm.Vegetable,
                MealTypeId = menuvm.MealTypeId,
                MenuIdImageFile = menuvm.MenuIdImageFile,
            };

            if (menuvm.Id == 0)
            {
                var response = await SendStudentToApi(menu);

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
                var response = await SendStudentToApi(menu);

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

        private async Task<HttpResponseMessage> SendStudentToApi(MenuCreateDTO menuCreateDTO)
        {
            using(var httpClient = new HttpClient())
            {
                using (var formData = new MultipartFormDataContent())
                {
                    formData.Headers.ContentType.MediaType = "multipart/form-data";

                    formData.Add(new StringContent(menuCreateDTO.Starch), "Starch");
                    formData.Add(new StringContent(menuCreateDTO.Beverage), "Beverage");
                    formData.Add(new StringContent(menuCreateDTO.Meat), "Meat");
                    formData.Add(new StringContent(menuCreateDTO.Vegetable), "Vegetable");
                    formData.Add(new StringContent(menuCreateDTO.MealTypeId.ToString()), "MealTypeId");

                    if(menuCreateDTO.MenuIdImageFile != null && menuCreateDTO.MenuIdImageFile.Length > 0)
                    {
                        formData.Add(new StreamContent(menuCreateDTO.MenuIdImageFile.OpenReadStream())
                        {
                            Headers = {
                                ContentLength = menuCreateDTO.MenuIdImageFile.Length,
                                ContentType = new MediaTypeHeaderValue(menuCreateDTO.MenuIdImageFile.ContentType)
                            }
                        }, "MenuImageFile", menuCreateDTO.MenuIdImageFile.FileName);
                    }

                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("Multipart/form-data"));

                    //var response = _clientHandler.CreateClient("StudentAPI").PostAsync("FileUpload", formData).Result;

                    return await httpClient.PostAsync(API_URL + "FileUpload", formData);
                }
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var httpClient = _clientHandler.CreateClient("StudentAPI");
            var response = await httpClient.DeleteAsync($"{id}");
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
