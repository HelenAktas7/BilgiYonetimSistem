using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BilgiYonetimSistem.Models;
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


        [HttpPost("LoginUser")]

        public async Task<ActionResult> LoginUser(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == model.Username && u.PasswordHash == model.Password && u.Role == model.Role);
                HttpContext.Session.SetString("UserID", user.UserID.ToString());
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Role", user.Role);
                HttpContext.Session.SetString("RelatedID", user.RelatedID.ToString());
                if (user != null)
                {

                    if (user.Role == "Student")
                    {
                        return RedirectToAction("Index", "Student", new { id = user.RelatedID });
                    }
                    else if (user.Role == "Advisor")
                    {
                        return RedirectToAction("ApproveCourses", "Advisors", new { id = user.RelatedID });
                    }
                }
                else
                {
                    ViewBag.Message = "Invalid username, password, or role.";
                }
            }
            return View(model);
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



                    ViewBag.Message = "A password reset link has been sent to your email.";
                }
                else
                {
                    ViewBag.Message = "Email address not found.";
                }
            }

            return View(model);
        }

    }
}