using System;

namespace mvc_webapp.Models
{
    public class Login
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime? LastLogin { get; set; }
        public Int16 LoginCounter { get; set; }
        public string SessionKey { get; set; }
        public DateTime? SessionStart { get; set; }
        public DateTime? SessionEnd { get; set; }
    }
}
