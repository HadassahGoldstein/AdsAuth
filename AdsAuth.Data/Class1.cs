using System;

namespace AdsAuth.Data
{
    public class Ad
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Cell { get; set; }
        public DateTime Date { get; set; }
        public string Text { get; set; }
    }
}
