﻿namespace OnlineShopping.WebApp.Models.Dto
{
    public class RegisterationRequestDTO
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string? Role { get; set; }
        public string? PhoneNumber { get; set; }

    }
}
