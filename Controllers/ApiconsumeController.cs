

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;
using apiconsume.Models;
using Newtonsoft.Json;

namespace apiconsume.Controllers
{
    public class ApiconsumeController : Controller
    {
        private static readonly HttpClient client = new HttpClient { BaseAddress = new Uri("https://reqres.in/api/") };

      
        public ActionResult CreateUser()
        {
            return View();
        }

       
        [HttpPost]
       
        public async Task<ActionResult> CreateUser(Register newUser)
        {
            if (!ModelState.IsValid)
            {
                // Return validation errors as JSON
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, message = string.Join(", ", errors) });
            }

            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(newUser), System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync("users", content);
                response.EnsureSuccessStatusCode();

                // Return success message as JSON
                return Json(new { success = true, message = "User created successfully!" });
            }
            catch (HttpRequestException ex)
            {
                // Return error message as JSON
                return Json(new { success = false, message = $"Error creating user: {ex.Message}" });
            }
        }


        //public async Task<ActionResult> CreateUser(Register newUser)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid data");
        //    }

        //    try
        //    {
        //        var content = new StringContent(JsonConvert.SerializeObject(newUser), System.Text.Encoding.UTF8, "application/json");
        //        var response = await client.PostAsync("users", content);
        //        response.EnsureSuccessStatusCode();

        //        // Return success message as JSON
        //        return Json(new { success = true, message = "User created successfully!" });
        //    }
        //    catch (HttpRequestException ex)
        //    {
        //        // Return error message as JSON
        //        return Json(new { success = false, message = $"Error creating user: {ex.Message}" });
        //    }
        //}

        ////public async Task<ActionResult> CreateUser(Register newUser)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(newUser);
        //    }

        //    try
        //    {
        //        var content = new StringContent(JsonConvert.SerializeObject(newUser), System.Text.Encoding.UTF8, "application/json");
        //        var response = await client.PostAsync("users", content);
        //        response.EnsureSuccessStatusCode();
        //        TempData["msg"] = "Registration success";
        //        return RedirectToAction("Index");

        //    }
        //    catch (HttpRequestException ex)
        //    {

        //        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, $"Error creating user: {ex.Message}");
        //    }
    }
    public async Task<ActionResult> Index()
        {
            var users = await GetUsersAsync();
            return View(users);
        }

        private async Task<List<Register>> GetUsersAsync()
        {

            var response = await client.GetAsync("users");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(json);
            return apiResponse.Data;
        }
        //public async Task<ActionResult> GetUserDetails()
        //{
        //    var response = await client.GetAsync("users")
        //    response.EnsureSuccessStatusCode();

        //    var json = await response.Content.ReadAsStringAsync();
        //    var user = JsonConvert.DeserializeObject<Register>(json);
        //    return View(user);
        //}

        public async Task<ActionResult> GetUserDetails(int id)
        {
            var response = await client.GetAsync($"users/{id}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<Register>(json);
            return View(user);
        }
        [HttpGet]
        public async Task<ActionResult> UpdateUser(int id)
        {
           
            if (id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid User ID");
            }

            try
            {
               
                var response = await client.GetAsync($"users/{id}");
                response.EnsureSuccessStatusCode();

               
                var json = await response.Content.ReadAsStringAsync();
                var userResponse = JsonConvert.DeserializeObject<UserResponse>(json);

                // Extract the user data from the response
                var userData = userResponse.Data;

                // Pass the user object to the view
                return View(userData);
            }
            catch (HttpRequestException ex)
            {
                // Log the exception or handle it accordingly
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, $"Error retrieving user: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult> UpdateUser(Register user)
        {
            // Ensure the user.Id is valid
            if (user.Id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid User ID");
            }

            if (!ModelState.IsValid)
            {
                return View(user);
            }

            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(user), System.Text.Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"users/{user.Id}", content);

                response.EnsureSuccessStatusCode();

                return RedirectToAction("Index");
            }
            catch (HttpRequestException ex)
            {
                // Log the exception or handle it accordingly
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, $"Error updating user: {ex.Message}");
            }
        }



        

        [HttpPost]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var response = await client.DeleteAsync($"users/{id}");
            response.EnsureSuccessStatusCode();

            return RedirectToAction("Index");
        }

        public class ApiResponse
        {
            public List<Register> Data { get; set; }
        }
    }
}

