﻿namespace YourVitebskApp.Models
{
    public class User
    {
        public int? UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public UserDatum UserDatum { get; set; }
    }
}
