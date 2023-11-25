using StudentEnrollmentFrontend.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Text.Encodings.Web;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace IdentityDemoFrontend.Controllers;

public class AuthController : Controller
{
    private readonly IHttpClientFactory _clientHandler;
    const string SESSION_AUTH = "Student";
    const string ROLES = "Roles";
    public AuthController(IHttpClientFactory clienthandler)
    {
        this._clientHandler = clienthandler;
    }

    [HttpGet]
    public IActionResult Register()
    {
        ViewBag.TextColor = "black";
        ViewBag.ButtonColor = "#ED6468";
        return View();
    }


    [HttpPost]
    public IActionResult Register(User user)
    {
        if(ModelState.IsValid)
        {
            HttpClient httpClient = _clientHandler.CreateClient("AuthAPI");
            string json = JsonConvert.SerializeObject(user);
            StringContent data = new(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = httpClient.PostAsync("register", data).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Register");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "register failed");
                return View(user);
            }
        }
        return View(user);
    }

    [HttpGet]
    public IActionResult Login()
    {
        ViewBag.TextColor = "black";
        ViewBag.ButtonColor = "#ED6468";
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(User user)
    {
        if(ModelState.IsValid)
        {
            HttpClient httpClient = _clientHandler.CreateClient("AuthAPI");
            string json = JsonConvert.SerializeObject(user);
            StringContent data = new(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = httpClient.PostAsync("login", data).Result;

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                Dictionary<string, object> responseData = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseContent)!;

                if(responseData.ContainsKey("status") && responseData["status"].ToString() == "Success")
                {
                    if(responseData.ContainsKey("data") && responseData["data"] is JObject jObject)
                    {
                        // string token = responseData["data"].ToString()!;
                        string token = jObject.GetValue("result")!.ToString();
                        HttpContext.Session.SetString(SESSION_AUTH, token);
                        if(responseData.ContainsKey("roles") && responseData["roles"] is JArray jArray)
                        {
                            HttpContext.Session.SetString(ROLES, jArray.ToString());
                            Console.WriteLine($"roles exist");

                        } else
                        {
                        Console.WriteLine($"roles dont exist");
                        }
                    }

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid Username or Password");
                    return RedirectToAction("Login");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "login failed, Invalid Username or Password");
                return RedirectToAction("Login");
            }
        }
        return View(user);
    }

    public IActionResult LogOut()
    {
        if(ModelState.IsValid)
        {
            HttpContext.Session.Remove(SESSION_AUTH);
            HttpContext.Session.Remove(ROLES);
            return RedirectToAction("Index", "Home");
        }

        return View();
    }
}