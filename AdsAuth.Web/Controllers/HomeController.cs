using AdsAuth.Data;
using AdsAuth.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;


namespace AdsAuth.Web.Controllers
{
    
    public class HomeController : Controller
    {
        private string _connectionString =
         @"Data Source=.\sqlexpress; Initial Catalog=AdsDB;Integrated Security=true;";        
        public IActionResult Index(int id)
        {
            var db = new AdsDB(_connectionString);
            var vm = new HomePageViewModel()
            {
                Ads= db.GetAds(id)
            };
            if (User.Identity.IsAuthenticated)
            {                               
                vm.UserId = db.GetByEmail(User.Identity.Name).Id;
            }
            return View(vm);
        }
        [Authorize]
        public IActionResult NewAd()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult NewAd(Ad a)
        {
            var db = new AdsDB(_connectionString);            
            User u = db.GetByEmail(User.Identity.Name);
            db.AddAd(a, u);
            return Redirect("/");
        }
        [Authorize]
        public IActionResult MyAcount()
        {
            var db = new AdsDB(_connectionString);
            int currentId = db.GetByEmail(User.Identity.Name).Id;
            return Redirect($"/Home/index?id={currentId}");
        }
        [Authorize]
        [HttpPost]
        public IActionResult Delete(int adId)
        {
            var db = new AdsDB(_connectionString);
            db.DeleteAd(adId);
            return Redirect("/");
        }


    }
}
