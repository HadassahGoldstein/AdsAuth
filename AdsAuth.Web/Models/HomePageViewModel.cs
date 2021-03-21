using AdsAuth.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdsAuth.Web.Models
{
    public class HomePageViewModel
    {
        public List<Ad> Ads { get; set; }
        public int UserId { get; set; }
    }
}
