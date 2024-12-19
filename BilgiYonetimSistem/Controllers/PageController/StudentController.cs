using BilgiYonetimSistem.DataTransfer;
using BilgiYonetimSistem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BilgiYonetimSistem.Controllers.PageController
{
    public class StudentController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly DataContext AppContext;

        public StudentController(IHttpClientFactory httpClientFactory, DataContext _context)
        {
            _httpClient = httpClientFactory.CreateClient();
            AppContext = _context;
        }

        public async Task<IActionResult> Index(int id)
        {
            var userId = HttpContext.Session.GetString("UserID");

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("LoginUser", "Account");
            }

            var apiUrl = $"https://localhost:7262/api/students/{id}";

            try
            {
                var response = await _httpClient.GetAsync(apiUrl);
                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.ErrorMessage = "Error fetching student data. Status Code: " + response.StatusCode;
                    return View("Error");
                }
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var student = JsonConvert.DeserializeObject<Student>(jsonResponse);
                return View(student);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while fetching data: " + ex.Message;
                return View("Error");
            }
        }

        public async Task<IActionResult> CoursePreference()
        {
            var apiUrl = $"https://localhost:7262/api/courses";
            try
            {
                var response = await _httpClient.GetAsync(apiUrl);
                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.ErrorMessage = "Error fetching course data. Status Code: " + response.StatusCode;
                    return View("Error");
                }
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var courses = JsonConvert.DeserializeObject<List<Course>>(jsonResponse);
                return View(courses);

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while fetching data: " + ex.Message;
                return View("Error");
            }
        }

        public async Task<IActionResult> CourseSelection(List<int> selectedCourseIds)
        {
            var userId = HttpContext.Session.GetString("UserID");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("LoginUser", "Account");
            }

            int studentId = int.Parse(HttpContext.Session.GetString("RelatedID"));

            var apiUrl = $"https://localhost:7262/api/PendingCourseSelections";
            try
            {
                var pendingSelections = selectedCourseIds.Select(courseId => new CourseSelectionDTO
                {
                    studentId = studentId,
                    courseId = courseId,
                    selectionDate = DateTime.UtcNow,
                    isConfirmed = false
                }).ToList();

                var content = new StringContent(JsonConvert.SerializeObject(pendingSelections), System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(apiUrl, content);
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseContent); // Hata mesajını burada kontrol edin


                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Failed to save course selections.";
                    return RedirectToAction("CoursePreference");
                }

                ViewBag.Message = "Courses successfully added to pending selections.";
                return RedirectToAction("CoursePreference");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while submitting course selections: " + ex.Message;
                return View("Error");
            }
        }
    }
}
