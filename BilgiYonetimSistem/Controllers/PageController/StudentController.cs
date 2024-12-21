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
                    ViewBag.ErrorMessage = "Öğrenci verileri alınırken hata oluştu. Durum Kodu: " + response.StatusCode;
                    return View("Error");
                }
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var student = JsonConvert.DeserializeObject<Student>(jsonResponse);
                return View(student);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Veriler alınırken bir hata oluştu: " + ex.Message;
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
                    ViewBag.ErrorMessage = "Kurs verileri alınırken hata oluştu. Durum Kodu:" + response.StatusCode;
                    return View("Error");
                }
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var courses = JsonConvert.DeserializeObject<List<Course>>(jsonResponse);
                return View(courses);

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Veriler alınırken bir hata oluştu:" + ex.Message;
                return View("Error");
            }
        }
        public async Task<IActionResult> ConfirmedList()
        {
            return View();

        
        }
        public async Task<IActionResult> Routing()
        {
            var id = HttpContext.Session.GetString("StudentID");
            return RedirectToAction("Index", new { id });


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
                Console.WriteLine(responseContent); 


                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Ders seçimleri kaydedilemedi.";
                    return RedirectToAction("CoursePreference");
                }

                ViewBag.Message = "Dersler bekleyen seçimlere başarıyla eklendi.";
                return RedirectToAction("CoursePreference");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ders seçimleri gönderilirken bir hata oluştu:" + ex.Message;
                return View("Error");
            }
        }
        [HttpPost]
        public async Task<IActionResult> SaveCourseSelection(List<int> selectedCourseIds)
        {
            var studentIdString = HttpContext.Session.GetString("StudentID");

            if (string.IsNullOrEmpty(studentIdString))
            {
                TempData["ErrorMessage"] = "Öğrenci oturumu bulunamadı. Lütfen yeniden giriş yapın.";
                return RedirectToAction("LoginUser", "Account");
            }

            int studentId = int.Parse(studentIdString);

            foreach (var courseId in selectedCourseIds)
            {
                var pendingSelections = new PendingSelection
                {
                    StudentId = studentId,
                    CourseId = courseId,
                    SelectedAt = DateTime.Now
                };

                var response = await _httpClient.PostAsJsonAsync("https://localhost:7262/api/pendingSelections", pendingSelections);

                if (!response.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Seçiminiz kaydedilemedi, tekrar deneyin.";
                    return RedirectToAction("CoursePreference", "Student");
                }
            }

            TempData["SuccessMessage"] = "Ders seçiminiz başarıyla kaydedildi!";
            return RedirectToAction("ConfirmedList", "Student");
        }

    }
}
