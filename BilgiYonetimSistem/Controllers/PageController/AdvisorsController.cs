using BilgiYonetimSistem.DataTransfer;
using BilgiYonetimSistem.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Text;

namespace BilgiYonetimSistem.Controllers.PageController
{
    public class AdvisorsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly DataContext AppContext;

        public AdvisorsController(IHttpClientFactory httpClientFactory, DataContext _context)
        {
            _httpClient = httpClientFactory.CreateClient();
            AppContext = _context;
        }
        public async Task<IActionResult> IndexAsync(int id)
        {
            var userId = HttpContext.Session.GetString("UserID");

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("LoginUser", "Account");
            }
            var apiUrl = $"https://localhost:7262/api/advisors/{id}";

            try
            {
                var response = await _httpClient.GetAsync(apiUrl);
                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.ErrorMessage = "Öğrenci verileri alınırken hata oluştu. Durum Kodu:" + response.StatusCode;
                    return View("Error");
                }
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var advisor = JsonConvert.DeserializeObject<Advisor>(jsonResponse);
                return View(advisor);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Veriler alınırken bir hata oluştu:" + ex.Message;
                return View("Error");
            }
        }

        public async Task<IActionResult> ApprovedCourseDetails()
        {

            return View();

        }


        public async Task<IActionResult> DisplayStudents(int id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7262/api/students");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
                var allSelections = JsonConvert.DeserializeObject<List<Student>>(content);
                if (allSelections != null)
                {
                    var students = allSelections.Where(s => s.AdvisorID == id).ToList();

                    return View(students);
                }
            }
            return null;

        }

        public async Task<IActionResult> AcceptCourse(int id)
        {
            var responseCourse = await _httpClient.GetAsync($"https://localhost:7262/api/pendingSelections");
            if (responseCourse.IsSuccessStatusCode)
            {
                var contentCourse = await responseCourse.Content.ReadAsStringAsync();
                Console.WriteLine("Course Selections: " + contentCourse);

                var allSelectionsCourse = JsonConvert.DeserializeObject<List<PendingSelectionsDTO>>(contentCourse);

                var response = await _httpClient.GetAsync($"https://localhost:7262/api/students");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Students: " + content);

                    var allSelectionsStudent = JsonConvert.DeserializeObject<List<Student>>(content);
                    if (allSelectionsStudent != null && allSelectionsCourse != null)
                    {
                        var students = allSelectionsStudent.Where(s => s.AdvisorID == id).ToList();

                        var matchedCourses = allSelectionsCourse
     .Where(course => students.Any(student => student.StudentID == course.StudentId))
     .ToList();

                        return View(matchedCourses);
                    }
                }
            }
            return null;


        }

        public async Task<IActionResult> Routing()
        {
            var id = HttpContext.Session.GetString("RelatedID");
            return RedirectToAction("Index", new { id });


        }

        public async Task<IActionResult> ApproveRegistration(List<string> selectedCourses)
        {
            if (selectedCourses == null || !selectedCourses.Any())
            {
                ViewBag.ErrorMessage = "Seçilen dersler listesi boş.";
                return View();
            }

            var apiURL = "https://localhost:7262/api/StudentCourseSelections";

            foreach (var course in selectedCourses)
            {
                var parts = course.Split('-');
                if (parts.Length == 2 && int.TryParse(parts[0], out int studentId) && int.TryParse(parts[1], out int courseId))
                {
                    Console.WriteLine($"StudentId: {studentId}, CourseId: {courseId}");

                    var selection = new StudentCourseSelection
                    {
                        StudentID = studentId,
                        CourseID = courseId,
                        SelectionDate = DateTime.Now,
                        IsApproved = true
                    };

                    var jsonData = JsonConvert.SerializeObject(selection);
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PostAsync(apiURL, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var apiConfirmUrl = $"https://localhost:7262/api/PendingSelections?studentId={studentId}&courseId={courseId}";
                        var deleteResponse = await _httpClient.DeleteAsync(apiConfirmUrl);

                        if (deleteResponse.IsSuccessStatusCode)
                        {
                            Console.WriteLine($"PendingSelections tablosundan silindi: StudentID={studentId}, CourseID={courseId}");
                        }
                        else
                        {
                            Console.WriteLine("PendingSelections tablosundan kayıt silinemedi.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Ders seçimi onaylanamadı: StudentID={studentId}, CourseID={courseId}");
                    }
                }
                else
                {
                    Console.WriteLine("Geçersiz ders formatı.");
                    ViewBag.ErrorMessage = "Geçersiz ders formatı.";
                }
            }

            return RedirectToAction("ApprovedCourseDetails", "Advisors");
        }
    }
}
