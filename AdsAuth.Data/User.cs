using System;
using System.Collections.Generic;
using System.Text;

namespace AdsAuth.Data
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
    }
}
