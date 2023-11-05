using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using StudentEnrollmentFrontend.Models;
using System.Text;
using System.Net.Http.Headers;

namespace StudentEnrollmentFrontend.Controllers
{
    public class StudentsController : Controller
    {
        const string API_URL = "https://localhost:7240/api/Student/";
        private readonly IHttpClientFactory _clientHandler;
        public StudentsController(IHttpClientFactory clientHandler)
        {
            this._clientHandler = clientHandler;
        }

        public async Task<IActionResult> Index()
        {
            var httpClient = _clientHandler.CreateClient("StudentAPI");

            var response = await httpClient.GetAsync("");

            List<Student> productList = new();

            if(response != null){

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                productList = JsonConvert.DeserializeObject<List<Student>>(content)!;
            }
            else
                return Problem("Error in Api response");

            return View(productList);
            }
            else 
                return View(productList);
        }


        public IActionResult Upsert(int id = 0)
        {
            var CourseResponse = _clientHandler.CreateClient("StudentAPI").GetAsync("course").Result;

            var ParishResponse = _clientHandler.CreateClient("StudentAPI").GetAsync("parish").Result;

            var SizeResponse = _clientHandler.CreateClient("StudentAPI").GetAsync("size").Result;

            var Courses = CourseResponse.Content.ReadAsStringAsync().Result;

            var Parishes = ParishResponse.Content.ReadAsStringAsync().Result;

            var Sizes = SizeResponse.Content.ReadAsStringAsync().Result;

            List<Course> CourseList = JsonConvert.DeserializeObject<List<Course>>(Courses)!;
            List<Parish> ParishList = JsonConvert.DeserializeObject<List<Parish>>(Parishes)!;
            List<Size> SizeList = JsonConvert.DeserializeObject<List<Size>>(Sizes)!;

            StudentVM viewModel = new()
            {
                ProgramList = CourseList.Select(data => new SelectListItem
                {
                    Text = data.CourseName,
                    Value = data.Id.ToString()
                }).ToList(),

                ParishList = ParishList.Select(data => new SelectListItem
                {
                    Text = data.ParishName,
                    Value = data.Id.ToString()
                }).ToList(),

                SizeList = SizeList.Select(data => new SelectListItem
                {
                    Text = data.SizeName,
                    Value = data.Id.ToString()
                }).ToList()
            };

            if(id == 0)
                return View(viewModel);
            else
            {
                var studentResponse = _clientHandler.CreateClient("StudentAPI").GetAsync($"{id}").Result;
                var student = studentResponse.Content.ReadAsStringAsync().Result;

                StudentVM view = JsonConvert.DeserializeObject<StudentVM>(student)!;

                viewModel.StudentName = view.StudentName;
                viewModel.EmailAddress = view.EmailAddress;
                viewModel.PhoneNumber = view.PhoneNumber;
                viewModel.SelectedParishId = view.SelectedParishId;
                viewModel.SelectedProgramId = view.SelectedProgramId;
                viewModel.SelectedSizeId = view.SelectedSizeId;
                
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

        public async Task<IActionResult> Upsert(StudentVM studentvm)
        {
            if (!ModelState.IsValid) return View(studentvm);

            var student = new StudentCreateDTO
            {
                StudentName = studentvm.StudentName,
                EmailAddress = studentvm.EmailAddress,
                PhoneNumber = studentvm.PhoneNumber,
                ParishId = studentvm.SelectedParishId,
                ProgramId = studentvm.SelectedProgramId,
                SizeId = studentvm.SelectedSizeId,
                StudntIdImageFile = studentvm.StudntIdImageFile
            };

            if (studentvm.Id == 0)
            {
                var response = await SendStudentToApi(student);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Product creation failed");
                    return View(studentvm);
                }
            }
            else
            {
                var response = await SendStudentToApi(student);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Product creation failed");
                    return View(studentvm);
                }
            }
        }

        private async Task<HttpResponseMessage> SendStudentToApi(StudentCreateDTO studentCreateDTO)
        {
            using(var httpClient = new HttpClient())
            {
                using (var formData = new MultipartFormDataContent())
                {
                    formData.Headers.ContentType.MediaType = "multipart/form-data";

                    formData.Add(new StringContent(studentCreateDTO.StudentName), "StudentName");
                    formData.Add(new StringContent(studentCreateDTO.EmailAddress), "EmailAddress");
                    formData.Add(new StringContent(studentCreateDTO.PhoneNumber), "PhoneNumber");
                    formData.Add(new StringContent(studentCreateDTO.SizeId.ToString()), "SizeId");
                    formData.Add(new StringContent(studentCreateDTO.ParishId.ToString()), "ParishId");
                    formData.Add(new StringContent(studentCreateDTO.ProgramId.ToString()), "ProgramId");

                    if(studentCreateDTO.StudntIdImageFile != null && studentCreateDTO.StudntIdImageFile.Length > 0)
                    {
                        formData.Add(new StreamContent(studentCreateDTO.StudntIdImageFile.OpenReadStream())
                        {
                            Headers = {
                                ContentLength = studentCreateDTO.StudntIdImageFile.Length,
                                ContentType = new MediaTypeHeaderValue(studentCreateDTO.StudntIdImageFile.ContentType)
                            }
                        }, "StudentImageFile", studentCreateDTO.StudntIdImageFile.FileName);
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
