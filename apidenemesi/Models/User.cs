﻿using System.Security.Claims;

namespace apidenemesi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}
