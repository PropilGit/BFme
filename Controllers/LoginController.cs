using BFme.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BFme.Controllers
{
    public class LoginController : Controller
    {
        InvestContext db;

        public LoginController(InvestContext dbContext)
        {
            db = dbContext;
        }

        [HttpGet]
        public IActionResult Index(string Message = "")
        {
            ViewBag.Message = Message;
            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(User newUser)
        {
            User user = db.Users.FirstOrDefault(u => (u.Login == newUser.Login && u.Password == newUser.Password));
            if (user == null) return Index("Некорректные логин и(или) пароль");
            user.Role = db.Roles.FirstOrDefault(r => r.Id == user.RoleId);

            await Authenticate(user);
            return RedirectToAction("Index", "Home");
        }

        [NonAction]
        private async Task Authenticate(User user)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Index("Index");
        }
    }
}
