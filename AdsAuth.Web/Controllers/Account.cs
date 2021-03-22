using AdsAuth.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace AdsAuth.Web.Controllers
{
    public class Account : Controller
    {
        private string _connectionString =
         @"Data Source=.\sqlexpress; Initial Catalog=AdsDB;Integrated Security=true;";
        public IActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Signup(User u,string password)
        {
            var db = new AdsDB(_connectionString);
            db.AddUser(u, password);
            return Redirect("/Account/Login");
        }
        public IActionResult Login()
        {
            if (TempData["message"] != null)
            {
                ViewBag.Message = TempData["message"];
            }
            return View();
        }
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var db = new AdsDB(_connectionString);
            User u= db.Login(email, password);
            if (u == null)
            {
                TempData["message"] = "Invalid password/email combination. Try again";
                return Redirect("/Account/Login");
            }
            var claims = new List<Claim>
            {
                new Claim("user",email)
            };
            HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity("claims", "cookies", "role"))).Wait();
            return Redirect("/");
        }
        [Authorize]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return Redirect("/");
        }
    }
}
