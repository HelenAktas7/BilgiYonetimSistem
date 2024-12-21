using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BilgiYonetimSistem.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
namespace BilgiYonetimSistem.Controllers.PageController
{
    public class AccountController : Controller
    {
        private readonly DataContext _context;

        public AccountController(DataContext context)
        {
            _context = context;
        }


        public IActionResult LoginUser()
        {
            return View();
        }


        [HttpPost]

        public async Task<ActionResult> LoginUser(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == model.Username && u.PasswordHash == model.Password && u.Role == model.Role);
                if (user != null)
                {
                    HttpContext.Session.SetString("UserID", user.UserID.ToString());
                    HttpContext.Session.SetString("Username", user.Username);
                    HttpContext.Session.SetString("Role", user.Role);
                    HttpContext.Session.SetString("RelatedID", user.RelatedID.ToString());

                    if (user.Role == "Student")
                    {
                        HttpContext.Session.SetString("StudentID", user.RelatedID.ToString());
                        return RedirectToAction("Index", "Student", new { id = user.RelatedID });
                    }
                    else if (user.Role == "Advisor")
                    {
                        return RedirectToAction("Index", "Advisors", new { id = user.RelatedID });
                    }
                }
                else
                {
                    ViewBag.Message = "Geçersiz kullanıcı adı, şifre veya rol.";
                }
            }
            return View(model);
        }
     
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            // Anasayfaya veya giriş ekranına yönlendirme
            return RedirectToAction("LoginUser", "Account");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

                if (user != null)
                {

                    string resetLink = Url.Action("ForgotPassword", "Account", new { email = model.Email }, Request.Scheme);



                    ViewBag.Message = "E-posta adresinize şifre sıfırlama bağlantısı gönderildi.";
                }
                else
                {
                    ViewBag.Message = "E-posta adresi bulunamadı.";
                }
            }

            return View(model);
        }

    }
}