﻿namespace FruitShop.Areas.Admin.DTOs.request
{
    public class LoginDTO
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
